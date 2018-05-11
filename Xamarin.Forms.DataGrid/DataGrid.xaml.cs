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
	public partial class DataGrid : Grid
	{
		public event EventHandler Refreshing;
		public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

		#region Bindable properties
		public static readonly BindableProperty ActiveRowColorProperty =
			BindableProperty.Create(nameof(ActiveRowColor), typeof(Color), typeof(DataGrid), Color.FromRgb(128, 144, 160),
				coerceValue: (b, v) => {
					if (!(b as DataGrid).SelectionEnabled)
						throw new InvalidOperationException("Datagrid must be SelectionEnabled=true to set ActiveRowColor");
					return v;
				});

		public static readonly BindableProperty HeaderBackgroundProperty =
			BindableProperty.Create(nameof(HeaderBackground), typeof(Color), typeof(DataGrid), Color.White,
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (self._headerView != null && !self.HeaderBordersVisible)
						self._headerView.BackgroundColor = (Color)n;
				});

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(DataGrid), Color.Black,
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (self.HeaderBordersVisible)
						self._headerView.BackgroundColor = (Color)n;

					if (self.Columns != null && self.ItemsSource != null)
						self.Reload();
				});

		public static readonly BindableProperty RowsBackgroundColorPaletteProperty =
			BindableProperty.Create(nameof(RowsBackgroundColorPalette), typeof(IColorProvider), typeof(DataGrid), new PaletteCollection { default(Color) },
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (self.Columns != null && self.ItemsSource != null)
						self.Reload();
				});

		public static readonly BindableProperty RowsTextColorPaletteProperty =
			BindableProperty.Create(nameof(RowsTextColorPalette), typeof(IColorProvider), typeof(DataGrid), new PaletteCollection { Color.Black },
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (self.Columns != null && self.ItemsSource != null)
						self.Reload();
				});

		public static readonly BindableProperty ColumnsProperty =
			BindableProperty.Create(nameof(Columns), typeof(ColumnCollection), typeof(DataGrid),
				propertyChanged: (b, o, n) => (b as DataGrid).InitHeaderView(),
				defaultValueCreator: b => { return new ColumnCollection(); }
			);

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null,
				propertyChanged: (b, o, n) => {
					DataGrid self = b as DataGrid;
					//ObservableCollection Tracking 
					if (o != null && o is INotifyCollectionChanged)
						(o as INotifyCollectionChanged).CollectionChanged -= self.HandleItemsSourceCollectionChanged;

					if (n != null)
					{
						if (n is INotifyCollectionChanged)
							(n as INotifyCollectionChanged).CollectionChanged += self.HandleItemsSourceCollectionChanged;

						self.InternalItems = new List<object>(((IEnumerable)n).Cast<object>());
					}

					if (self.SelectedItem != null && !self.InternalItems.Contains(self.SelectedItem))
						self.SelectedItem = null;

					if (self.NoDataView != null)
					{
						if (self.ItemsSource == null || self.InternalItems.Count() == 0)
							self._noDataView.IsVisible = true;
						else if (self._noDataView.IsVisible)
							self._noDataView.IsVisible = false;
					}
				});

		void HandleItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			InternalItems = new List<object>(((IEnumerable)sender).Cast<object>());
			if (SelectedItem != null && !InternalItems.Contains(SelectedItem))
				SelectedItem = null;
		}

		public static readonly BindableProperty RowHeightProperty =
			BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(DataGrid), 40,
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					self._listView.RowHeight = (int)n;
				});


		public static readonly BindableProperty HeaderHeightProperty =
			BindableProperty.Create(nameof(HeaderHeight), typeof(int), typeof(DataGrid), 40,
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
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
					var self = b as DataGrid;
					if (!self.SelectionEnabled && v != null)
						throw new InvalidOperationException("Datagrid must be SelectionEnabled=true to set SelectedItem");
					if (self.InternalItems != null && self.InternalItems.Contains(v))
						return v;
					else
						return null;
				},
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (self._listView.SelectedItem != n)
						self._listView.SelectedItem = n;
				}
			);

		public static readonly BindableProperty SelectionEnabledProperty =
			BindableProperty.Create(nameof(SelectionEnabled), typeof(bool), typeof(DataGrid), true,
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (!self.SelectionEnabled && self.SelectedItem != null)
						self.SelectedItem = null;
				});

		public static readonly BindableProperty PullToRefreshCommandProperty =
			BindableProperty.Create(nameof(PullToRefreshCommand), typeof(ICommand), typeof(DataGrid), null,
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if (n == null)
					{
						self._listView.IsPullToRefreshEnabled = false;
						self._listView.RefreshCommand = null;
					}
					else
					{
						self._listView.IsPullToRefreshEnabled = true;
						self._listView.RefreshCommand = n as ICommand;
					}
				});

		public static readonly BindableProperty IsRefreshingProperty =
			BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(DataGrid), false, BindingMode.TwoWay,
				propertyChanged: (b, o, n) => (b as DataGrid)._listView.IsRefreshing = (bool)n);

		public static readonly BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(BorderThickness), typeof(Thickness), typeof(DataGrid), new Thickness(1),
				propertyChanged: (b, o, n) => {
					(b as DataGrid)._headerView.ColumnSpacing = ((Thickness)n).HorizontalThickness / 2;
					(b as DataGrid)._headerView.Padding = ((Thickness)n).HorizontalThickness / 2;
				});

		public static readonly BindableProperty HeaderBordersVisibleProperty =
			BindableProperty.Create(nameof(HeaderBordersVisible), typeof(bool), typeof(DataGrid), true,
				propertyChanged: (b, o, n) => (b as DataGrid)._headerView.BackgroundColor = (bool)n ? (b as DataGrid).BorderColor : (b as DataGrid).HeaderBackground);

		public static readonly BindableProperty SortedColumnIndexProperty =
			BindableProperty.Create(nameof(SortedColumnIndex), typeof(SortData), typeof(DataGrid), null, BindingMode.TwoWay,
				coerceValue: (b, v) => {
					var self = b as DataGrid;
					if (((SortData)v).Index > self.Columns.Count)
						throw new InvalidOperationException("SortedColumnIndex cannot be greather than Columns count");
					return v;
				},
				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
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
					var self = b as DataGrid;
					var style = (n as Style).Setters.FirstOrDefault(x => x.Property == Image.SourceProperty);
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
				coerceValue: (b, v) => {
					var self = b as DataGrid;

					return v;
				},

				propertyChanged: (b, o, n) => {
					var self = b as DataGrid;
					if ((n as Style).Setters.Any(x => x.Property == Image.SourceProperty))
					{
						var style = (n as Style).Setters.FirstOrDefault(x => x.Property == Image.SourceProperty);
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
						(b as DataGrid)._noDataView.Content = n as View;
				});
		#endregion

		#region Properties
		public Color ActiveRowColor
		{
			get { return (Color)GetValue(ActiveRowColorProperty); }
			set { SetValue(ActiveRowColorProperty, value); }
		}

		public Color HeaderBackground
		{
			get { return (Color)GetValue(HeaderBackgroundProperty); }
			set { SetValue(HeaderBackgroundProperty, value); }
		}

		[Obsolete("Please use HeaderLabelStyle", true)]
		public Color HeaderTextColor
		{
			get; set;
		}

		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		public IColorProvider RowsBackgroundColorPalette
		{
			get { return (IColorProvider)GetValue(RowsBackgroundColorPaletteProperty); }
			set { SetValue(RowsBackgroundColorPaletteProperty, value); }
		}

		public IColorProvider RowsTextColorPalette
		{
			get { return (IColorProvider)GetValue(RowsTextColorPaletteProperty); }
			set { SetValue(RowsTextColorPaletteProperty, value); }
		}

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
		}

		IList<object> _internalItems;

		internal IList<object> InternalItems
		{
			get { return _internalItems; }
			set
			{
				_internalItems = value;

				if (IsSortable && SortedColumnIndex != null)
					SortItems(SortedColumnIndex);
				else
					_listView.ItemsSource = _internalItems;
			}
		}

		public ColumnCollection Columns
		{
			get { return (ColumnCollection)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		public double FontSize
		{
			get { return (double)GetValue(FontSizeProperty); }
			set { SetValue(FontSizeProperty, value); }
		}

		[Obsolete("Please use HeaderLabelStyle", true)]
		public double HeaderFontSize
		{
			get; set;
		}

		public string FontFamily
		{
			get { return (string)GetValue(FontFamilyProperty); }
			set { SetValue(FontFamilyProperty, value); }
		}

		public int RowHeight
		{
			get { return (int)GetValue(RowHeightProperty); }
			set { SetValue(RowHeightProperty, value); }
		}

		public int HeaderHeight
		{
			get { return (int)GetValue(HeaderHeightProperty); }
			set { SetValue(HeaderHeightProperty, value); }
		}

		public bool IsSortable
		{
			get { return (bool)GetValue(IsSortableProperty); }
			set { SetValue(IsSortableProperty, value); }
		}

		public bool SelectionEnabled
		{
			get { return (bool)GetValue(SelectionEnabledProperty); }
			set { SetValue(SelectionEnabledProperty, value); }
		}

		public object SelectedItem
		{
			get { return GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public ICommand PullToRefreshCommand
		{
			get { return (ICommand)GetValue(PullToRefreshCommandProperty); }
			set { SetValue(PullToRefreshCommandProperty, value); }
		}

		public bool IsRefreshing
		{
			get { return (bool)GetValue(IsRefreshingProperty); }
			set { SetValue(IsRefreshingProperty, value); }
		}

		public Thickness BorderThickness
		{
			get { return (Thickness)GetValue(BorderThicknessProperty); }
			set { SetValue(BorderThicknessProperty, value); }
		}

		public bool HeaderBordersVisible
		{
			get { return (bool)GetValue(HeaderBordersVisibleProperty); }
			set { SetValue(HeaderBordersVisibleProperty, value); }
		}

		public SortData SortedColumnIndex
		{
			get { return (SortData)GetValue(SortedColumnIndexProperty); }
			set { SetValue(SortedColumnIndexProperty, value); }
		}

		public Style HeaderLabelStyle
		{
			get { return (Style)GetValue(HeaderLabelStyleProperty); }
			set { SetValue(HeaderLabelStyleProperty, value); }
		}

		public ImageSource AscendingIcon
		{
			get { return (ImageSource)GetValue(AscendingIconProperty); }
			set { SetValue(AscendingIconProperty, value); }
		}

		public ImageSource DescendingIcon
		{
			get { return (ImageSource)GetValue(DescendingIconProperty); }
			set { SetValue(DescendingIconProperty, value); }
		}

		public Style AscendingIconStyle
		{
			get { return (Style)GetValue(AscendingIconStyleProperty); }
			set { SetValue(AscendingIconStyleProperty, value); }
		}

		public Style DescendingIconStyle
		{
			get { return (Style)GetValue(DescendingIconStyleProperty); }
			set { SetValue(DescendingIconStyleProperty, value); }
		}

		public View NoDataView
		{
			get { return (View)GetValue(NoDataViewProperty); }
			set { SetValue(NoDataViewProperty, value); }
		}
		#endregion

		#region Fields
		Dictionary<int, SortingOrder> _sortingOrders;
		ListView _listView;
		#endregion

		#region ctor

		public DataGrid() : this(ListViewCachingStrategy.RecycleElement)
		{
		}

		public DataGrid(ListViewCachingStrategy cachingStrategy)
		{
			InitializeComponent();

			_sortingOrders = new Dictionary<int, SortingOrder>();

			_listView = new ListView(cachingStrategy) {
				BackgroundColor = Color.Transparent,
				ItemTemplate = new DataGridRowTemplateSelector(),
				SeparatorVisibility = SeparatorVisibility.None,
			};

			_listView.ItemSelected += (s, e) => {
				if (SelectionEnabled)
					SelectedItem = _listView.SelectedItem;
				else
					_listView.SelectedItem = null;

				ItemSelected?.Invoke(this, e);
			};

			_listView.Refreshing += (s, e) => {
				Refreshing?.Invoke(this, e);
			};

			_listView.SetBinding(ListView.RowHeightProperty, new Binding("RowHeight", source: this));
			Grid.SetRow(_listView, 1);
			Children.Add(_listView);
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
			column.HeaderLabel.Style = column.HeaderLabelStyle ?? this.HeaderLabelStyle ?? (Style)_headerView.Resources["HeaderDefaultStyle"];

			Grid grid = new Grid {
				ColumnSpacing = 0,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

			if (IsSortable)
			{
				column.SortingIcon.Style = (Style)_headerView.Resources["ImageStyleBase"];

				grid.Children.Add(column.SortingIcon);
				Grid.SetColumn(column.SortingIcon, 1);

				TapGestureRecognizer tgr = new TapGestureRecognizer();
				tgr.Tapped += (s, e) => {
					int index = Columns.IndexOf(column);
					SortingOrder order = _sortingOrders[index] == SortingOrder.Ascendant ? SortingOrder.Descendant : SortingOrder.Ascendant;

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
			SortingOrder order = sData.Order;

			if (!IsSortable)
				throw new InvalidOperationException("This DataGrid is not sortable");
			else if (column.PropertyName == null)
				throw new InvalidOperationException("Please set the PropertyName property of Column");

			//Sort
			if (order == SortingOrder.Descendant)
				items = items.OrderByDescending(x => ReflectionUtils.GetValueByPath(x, column.PropertyName)).ToList();
			else
				items = items.OrderBy(x => ReflectionUtils.GetValueByPath(x, column.PropertyName)).ToList();

			column.SortingIcon.Style = (order == SortingOrder.Descendant) ?
				AscendingIconStyle ?? (Style)_headerView.Resources["DescendingIconStyle"] :
				DescendingIconStyle ?? (Style)_headerView.Resources["AscendingIconStyle"];

			//Support DescendingIcon property (if setted)
			if (!column.SortingIcon.Style.Setters.Any(x => x.Property == Image.SourceProperty))
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

			_listView.ItemsSource = _internalItems;
		}
		#endregion
	}
}
