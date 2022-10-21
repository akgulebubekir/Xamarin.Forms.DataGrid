using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms.DataGrid.Utils;

[assembly: InternalsVisibleTo("Xamarin.Forms.DataGrid.UnitTest")]
namespace Xamarin.Forms.DataGrid
{
	[Xaml.XamlCompilation(Xaml.XamlCompilationOptions.Compile)]
	public partial class DataGrid
	{
		public event EventHandler Refreshing;
		public event EventHandler<SelectionChangedEventArgs> ItemSelected;

		#region Bindable properties
		public static readonly BindableProperty ActiveRowColorProperty =
			BindableProperty.Create(nameof(ActiveRowColor), typeof(Color), typeof(DataGrid), Color.FromRgb(128, 144, 160),
				coerceValue: (b, v) => {
					if (!((DataGrid) b).SelectionEnabled)
						throw new InvalidOperationException("Datagrid must be SelectionEnabled=true to set ActiveRowColor");
					return v;
				});

		public static readonly BindableProperty HeaderBackgroundProperty =
			BindableProperty.Create(nameof(HeaderBackground), typeof(Color), typeof(DataGrid), Color.White,
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (self._headerView != null && !self.HeaderBordersVisible)
						self._headerView.BackgroundColor = (Color)n;
				});

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(DataGrid), Color.Black,
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (self.HeaderBordersVisible)
						self._headerView.BackgroundColor = (Color)n;

					if (self.Columns != null && self.ItemsSource != null)
						self.Reload();
				});

		public static readonly BindableProperty RowsBackgroundColorPaletteProperty =
			BindableProperty.Create(nameof(RowsBackgroundColorPalette), typeof(IColorProvider), typeof(DataGrid), new PaletteCollection { default(Color) },
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (self.Columns != null && self.ItemsSource != null)
						self.Reload();
				});

		public static readonly BindableProperty RowsTextColorPaletteProperty =
			BindableProperty.Create(nameof(RowsTextColorPalette), typeof(IColorProvider), typeof(DataGrid), new PaletteCollection { Color.Black },
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (self.Columns != null && self.ItemsSource != null)
						self.Reload();
				});

		public static readonly BindableProperty ColumnsProperty =
			BindableProperty.Create(nameof(Columns), typeof(ColumnCollection), typeof(DataGrid),
				propertyChanged: (b, o, n) => ((DataGrid) b).InitHeaderView(),
				defaultValueCreator: b => new ColumnCollection());

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null,
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					//ObservableCollection Tracking 
					if (o is INotifyCollectionChanged collectionChanged)
						collectionChanged.CollectionChanged -= self.HandleItemsSourceCollectionChanged;

					if (n != null)
					{
						if (n is INotifyCollectionChanged changed)
							changed.CollectionChanged += self.HandleItemsSourceCollectionChanged;

						self.InternalItems = new List<object>(((IEnumerable)n).Cast<object>());
					}

					if (self.SelectedItem != null && !self.InternalItems.Contains(self.SelectedItem))
						self.SelectedItem = null;

					//if (self.NoDataView != null)
					//{
					//	if (self.ItemsSource == null || !self.InternalItems.Any())
					//		self._noDataView.IsVisible = true;
					//	else if (self._noDataView.IsVisible)
					//		self._noDataView.IsVisible = false;
					//}
				});

		void HandleItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			InternalItems = new List<object>(((IEnumerable)sender).Cast<object>());
			if (SelectedItem != null && !InternalItems.Contains(SelectedItem))
				SelectedItem = null;
		}

		public static readonly BindableProperty RowHeightProperty =
			BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(DataGrid), 40);

		public static readonly BindableProperty HeaderHeightProperty =
			BindableProperty.Create(nameof(HeaderHeight), typeof(int), typeof(DataGrid), 40,
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					self._headerView.HeightRequest = (int)n;
				});

		public static readonly BindableProperty IsSortableProperty =
			BindableProperty.Create(nameof(IsSortable), typeof(bool), typeof(DataGrid), true);

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSize), typeof(double), typeof(DataGrid), 13.0);

		public static readonly BindableProperty FontFamilyProperty =
			BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(DataGrid), Font.Default.FontFamily);

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(DataGrid), null, BindingMode.TwoWay,
				coerceValue: (b, v) => {
					var self = (DataGrid)b;
					if (!self.SelectionEnabled && v != null)
						throw new InvalidOperationException("Datagrid must be SelectionEnabled=true to set SelectedItem");
					if (self.InternalItems != null && self.InternalItems.Contains(v))
						return v;
					return null;
				},
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (self._collectionView.SelectedItem != n)
						self._collectionView.SelectedItem = n;
				}
			);

		public static readonly BindableProperty SelectionEnabledProperty =
			BindableProperty.Create(nameof(SelectionEnabled), typeof(bool), typeof(DataGrid), true,
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;

					// set selectionMode of collectionView
                    if (self.SelectionEnabled)
                    {
                        if (self._collectionView.SelectionMode == SelectionMode.None)
                            self._collectionView.SelectionMode = SelectionMode.Single;
                    }
                    else if (self._collectionView.SelectionMode != SelectionMode.None)
                        self._collectionView.SelectionMode = SelectionMode.None;

					// handle selected Item
                    if (!self.SelectionEnabled && self.SelectedItem != null)
						self.SelectedItem = null;
				});

		public static readonly BindableProperty PullToRefreshCommandProperty =
			BindableProperty.Create(nameof(PullToRefreshCommand), typeof(ICommand), typeof(DataGrid), null,
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (n == null)
					{
						self._refreshView.IsEnabled = false;
						self._refreshView.Command = null;
					}
					else
					{
						self._refreshView.IsEnabled = true;
						self._refreshView.Command = n as ICommand;
					}
				});

		public static readonly BindableProperty IsRefreshingProperty =
			BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(DataGrid), false, BindingMode.TwoWay,
				propertyChanged: (b, o, n) => ((DataGrid) b)._refreshView.IsRefreshing = (bool)n);

		public static readonly BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(BorderThickness), typeof(Thickness), typeof(DataGrid), new Thickness(1),
				propertyChanged: (b, o, n) => {
					((DataGrid) b)._headerView.ColumnSpacing = ((Thickness)n).HorizontalThickness / 2;
					((DataGrid) b)._headerView.Padding = ((Thickness)n).HorizontalThickness / 2;
				});

		public static readonly BindableProperty HeaderBordersVisibleProperty =
			BindableProperty.Create(nameof(HeaderBordersVisible), typeof(bool), typeof(DataGrid), true,
				propertyChanged: (b, o, n) => ((DataGrid) b)._headerView.BackgroundColor = (bool)n ? ((DataGrid) b).BorderColor : ((DataGrid) b).HeaderBackground);

		public static readonly BindableProperty SortedColumnIndexProperty =
			BindableProperty.Create(nameof(SortedColumnIndex), typeof(SortData), typeof(DataGrid), null, BindingMode.TwoWay,
				validateValue: (b, v) => {
					var self = (DataGrid)b;
					var sData = (SortData)v;

					return
						sData == null || //setted to null
						self.Columns == null || // Columns binded but not setted
						self.Columns.Count == 0 || //columns not setted yet
						(sData.Index < self.Columns.Count && self.Columns.ElementAt(sData.Index).SortingEnabled);
				},
				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (o != n)
						self.SortItems((SortData)n);
				});


		public static readonly BindableProperty HeaderLabelStyleProperty =
			BindableProperty.Create(nameof(HeaderLabelStyle), typeof(Style), typeof(DataGrid));

		public static readonly BindableProperty AscendingIconProperty =
			BindableProperty.Create(nameof(AscendingIcon), typeof(ImageSource), typeof(DataGrid), ImageSource.FromResource("Xamarin.Forms.DataGrid.up.png", typeof(DataGrid).GetTypeInfo().Assembly));

		public static readonly BindableProperty DescendingIconProperty =
			BindableProperty.Create(nameof(DescendingIcon), typeof(ImageSource), typeof(DataGrid), ImageSource.FromResource("Xamarin.Forms.DataGrid.down.png", typeof(DataGrid).GetTypeInfo().Assembly));

		public static readonly BindableProperty DescendingIconStyleProperty =
			BindableProperty.Create(nameof(DescendingIconStyle), typeof(Style), typeof(DataGrid), null,

				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					var style = ((Style) n).Setters.FirstOrDefault(x => x.Property == Image.SourceProperty);
					if (style != null)
					{
						if (style.Value is string vs)
							self.DescendingIcon = ImageSource.FromFile(vs);
						else
							self.DescendingIcon = (ImageSource)style.Value;
					}
				});

		public static readonly BindableProperty AscendingIconStyleProperty =
			BindableProperty.Create(nameof(AscendingIconStyle), typeof(Style), typeof(DataGrid), null,
				coerceValue: (b, v) => v,

				propertyChanged: (b, o, n) => {
					var self = (DataGrid)b;
					if (((Style) n).Setters.Any(x => x.Property == Image.SourceProperty))
					{
						var style = ((Style) n).Setters.FirstOrDefault(x => x.Property == Image.SourceProperty);
						if (style != null)
						{
							if (style.Value is string vs)
								self.AscendingIcon = ImageSource.FromFile(vs);
							else
								self.AscendingIcon = (ImageSource)style.Value;
						}
					}
				});

		public static readonly BindableProperty NoDataViewProperty =
			BindableProperty.Create(nameof(NoDataView), typeof(View), typeof(DataGrid),
				propertyChanged: (b, o, n) => {
					if (o != n)
						((DataGrid) b)._collectionView.EmptyView = n as View;
				});
		#endregion

		#region Properties
		public Color ActiveRowColor
		{
			get => (Color)GetValue(ActiveRowColorProperty);
			set => SetValue(ActiveRowColorProperty, value);
		}

		public Color HeaderBackground
		{
			get => (Color)GetValue(HeaderBackgroundProperty);
			set => SetValue(HeaderBackgroundProperty, value);
		}

		public Color BorderColor
		{
			get => (Color)GetValue(BorderColorProperty);
			set => SetValue(BorderColorProperty, value);
		}

		public IColorProvider RowsBackgroundColorPalette
		{
			get => (IColorProvider)GetValue(RowsBackgroundColorPaletteProperty);
			set => SetValue(RowsBackgroundColorPaletteProperty, value);
		}

		public IColorProvider RowsTextColorPalette
		{
			get => (IColorProvider)GetValue(RowsTextColorPaletteProperty);
			set => SetValue(RowsTextColorPaletteProperty, value);
		}

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		IList<object> _internalItems;

		internal IList<object> InternalItems
		{
			get => _internalItems;
			set
			{
				_internalItems = value;

				if (IsSortable && SortedColumnIndex != null)
					SortItems(SortedColumnIndex);
				else
					_collectionView.ItemsSource = _internalItems;
			}
		}

		public ColumnCollection Columns
		{
			get => (ColumnCollection)GetValue(ColumnsProperty);
			set => SetValue(ColumnsProperty, value);
		}

		public double FontSize
		{
			get => (double)GetValue(FontSizeProperty);
			set => SetValue(FontSizeProperty, value);
		}

		public string FontFamily
		{
			get => (string)GetValue(FontFamilyProperty);
			set => SetValue(FontFamilyProperty, value);
		}

		public int RowHeight
		{
			get => (int)GetValue(RowHeightProperty);
			set => SetValue(RowHeightProperty, value);
		}

		public int HeaderHeight
		{
			get => (int)GetValue(HeaderHeightProperty);
			set => SetValue(HeaderHeightProperty, value);
		}

		public bool IsSortable
		{
			get => (bool)GetValue(IsSortableProperty);
			set => SetValue(IsSortableProperty, value);
		}

		public bool SelectionEnabled
		{
			get => (bool) GetValue(SelectionEnabledProperty);
			set => SetValue(SelectionEnabledProperty, value);
		}

		public object SelectedItem
		{
			get => GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		public ICommand PullToRefreshCommand
		{
			get => (ICommand) GetValue(PullToRefreshCommandProperty);
			set => SetValue(PullToRefreshCommandProperty, value);
		}

		public bool IsRefreshing
		{
			get => (bool) GetValue(IsRefreshingProperty);
			set => SetValue(IsRefreshingProperty, value);
		}

		public Thickness BorderThickness
		{
			get => (Thickness) GetValue(BorderThicknessProperty);
			set => SetValue(BorderThicknessProperty, value);
		}

		public bool HeaderBordersVisible
		{
			get => (bool) GetValue(HeaderBordersVisibleProperty);
			set => SetValue(HeaderBordersVisibleProperty, value);
		}

		public SortData SortedColumnIndex
		{
			get => (SortData) GetValue(SortedColumnIndexProperty);
			set => SetValue(SortedColumnIndexProperty, value);
		}

		public Style HeaderLabelStyle
		{
			get => (Style) GetValue(HeaderLabelStyleProperty);
			set => SetValue(HeaderLabelStyleProperty, value);
		}

		public ImageSource AscendingIcon
		{
			get => (ImageSource) GetValue(AscendingIconProperty);
			set => SetValue(AscendingIconProperty, value);
		}

		public ImageSource DescendingIcon
		{
			get => (ImageSource) GetValue(DescendingIconProperty);
			set => SetValue(DescendingIconProperty, value);
		}

		public Style AscendingIconStyle
		{
			get => (Style) GetValue(AscendingIconStyleProperty);
			set => SetValue(AscendingIconStyleProperty, value);
		}

		public Style DescendingIconStyle
		{
			get => (Style) GetValue(DescendingIconStyleProperty);
			set => SetValue(DescendingIconStyleProperty, value);
		}

		public View NoDataView
		{
			get => (View) GetValue(NoDataViewProperty);
			set => SetValue(NoDataViewProperty, value);
		}

		#endregion

		#region Fields

		readonly Dictionary<int, SortingOrder> _sortingOrders;
		#endregion

		#region ctor

		public DataGrid()
		{
			InitializeComponent();

			_sortingOrders = new Dictionary<int, SortingOrder>();
			
			_collectionView.SelectionChanged += (s, e) => {
				if (SelectionEnabled)
					SelectedItem = _collectionView.SelectedItem;
				else
					_collectionView.SelectedItem = null;

				ItemSelected?.Invoke(this, e);
			};

			_refreshView.Refreshing += (s, e) => {
				Refreshing?.Invoke(this, e);
			};
			
		}
		#endregion

		#region UI Methods
		protected override void OnParentSet()
		{
			base.OnParentSet();
			InitHeaderView();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			SetColumnsBindingContext();
		}

		private void Reload()
		{
			InternalItems = new List<object>(_internalItems);
		}
		#endregion

		#region Header Creation Methods

		private View GetHeaderViewForColumn(DataGridColumn column)
		{
			column.HeaderLabel.Style = column.HeaderLabelStyle ?? HeaderLabelStyle ?? (Style)_headerView.Resources["HeaderDefaultStyle"];

			var grid = new Grid {
				ColumnSpacing = 0,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

			if (IsSortable)
			{
				column.SortingIcon.Style = (Style)_headerView.Resources["ImageStyleBase"];

				grid.Children.Add(column.SortingIcon);
				Grid.SetColumn(column.SortingIcon, 1);

				var tgr = new TapGestureRecognizer();
				tgr.Tapped += (s, e) => {
					var index = Columns.IndexOf(column);
					var order = _sortingOrders[index] == SortingOrder.Ascendant ? SortingOrder.Descendant : SortingOrder.Ascendant;

					if (Columns.ElementAt(index).SortingEnabled)
						SortedColumnIndex = new SortData(index, order);
				};
				grid.GestureRecognizers.Add(tgr);
			}

			grid.Children.Add(column.HeaderLabel);

			return grid;
		}

		private void InitHeaderView()
		{
			SetColumnsBindingContext();
			_headerView.Children.Clear();
			_headerView.ColumnDefinitions.Clear();
			_sortingOrders.Clear();

			_headerView.Padding = new Thickness(BorderThickness.Left, BorderThickness.Top, BorderThickness.Right, 0);
			_headerView.ColumnSpacing = BorderThickness.HorizontalThickness / 2;

			if (Columns != null)
			{
				foreach (var col in Columns)
				{
					_headerView.ColumnDefinitions.Add(new ColumnDefinition { Width = col.Width });

					var cell = GetHeaderViewForColumn(col);

					_headerView.Children.Add(cell);
					Grid.SetColumn(cell, Columns.IndexOf(col));

					_sortingOrders.Add(Columns.IndexOf(col), SortingOrder.None);
				}
			}
		}

		private void SetColumnsBindingContext()
		{
			if (Columns != null)
				foreach (var c in Columns)
					c.BindingContext = BindingContext;
		}
		#endregion

		#region Sorting methods
		internal void SortItems(SortData sData)
		{
			if (InternalItems == null || sData.Index >= Columns.Count || !Columns[sData.Index].SortingEnabled)
				return;

			var items = InternalItems;
			var column = Columns[sData.Index];
			var order = sData.Order;

			if (!IsSortable)
				throw new InvalidOperationException("This DataGrid is not sortable");
			else if (column.PropertyName == null)
				throw new InvalidOperationException("Please set the PropertyName property of Column");

			//Sort
			items = order == SortingOrder.Descendant ? items.OrderByDescending(x => ReflectionUtils.GetValueByPath(x, column.PropertyName)).ToList() : items.OrderBy(x => ReflectionUtils.GetValueByPath(x, column.PropertyName)).ToList();

			column.SortingIcon.Style = (order == SortingOrder.Descendant) ?
				AscendingIconStyle ?? (Style)_headerView.Resources["DescendingIconStyle"] :
				DescendingIconStyle ?? (Style)_headerView.Resources["AscendingIconStyle"];

			//Support DescendingIcon property (if setted)
			if (column.SortingIcon.Style.Setters.All(x => x.Property != Image.SourceProperty))
			{
				if (order == SortingOrder.Descendant && DescendingIconProperty.DefaultValue != DescendingIcon)
					column.SortingIcon.Source = DescendingIcon;
				if (order == SortingOrder.Ascendant && AscendingIconProperty.DefaultValue != AscendingIcon)
					column.SortingIcon.Source = AscendingIcon;
			}

			for (int i = 0; i < Columns.Count; i++)
			{
				if (i != sData.Index)
				{
					if (Columns[i].SortingIcon.Style != null)
						Columns[i].SortingIcon.Style = null;
					if (Columns[i].SortingIcon.Source != null)
						Columns[i].SortingIcon.Source = null;
					_sortingOrders[i] = SortingOrder.None;
				}
			}

			_internalItems = items;

			_sortingOrders[sData.Index] = order;
			SortedColumnIndex = sData;

			_collectionView.ItemsSource = _internalItems;
		}
        #endregion

        #region Methods
        public void ScrollTo(object item, ScrollToPosition position, bool animated = true)
        {
			_collectionView.ScrollTo(item, position:position, animate:animated);
        }
        #endregion
    }
}
