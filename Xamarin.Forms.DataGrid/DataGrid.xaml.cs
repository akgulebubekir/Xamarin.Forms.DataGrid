using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using System.Collections;
using System.Windows.Input;
using System.Collections.Specialized;

namespace Xamarin.Forms.DataGrid
{
	[Xaml.XamlCompilation(Xaml.XamlCompilationOptions.Compile)]
	public partial class DataGrid : Grid
	{
		public event EventHandler Refreshing;
		public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

		#region Bindable properties
		public static readonly BindableProperty ActiveRowColorProperty =
			BindableProperty.Create(nameof(ActiveRowColor), typeof(Color), typeof(DataGrid), Color.FromRgb(128, 144, 160));

		public static readonly BindableProperty HeaderBackgroundProperty =
			BindableProperty.Create(nameof(HeaderBackground), typeof(Color), typeof(DataGrid), Color.White,
				propertyChanged: (b, o, n) =>
				{
					if ((b as DataGrid)._headerView == null)
						return;
					if (!(b as DataGrid).HeaderBordersVisible)
						(b as DataGrid)._headerView.BackgroundColor = (Color)n;
				});

		public static readonly BindableProperty HeaderTextColorProperty =
			BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(DataGrid), Color.Black);

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(DataGrid), Color.Black,
				propertyChanged: (b, o, n) => { (b as DataGrid)._listView.SeparatorColor = (Color)n; });

		public static readonly BindableProperty RowsBackgroundColorPaletteProperty =
			BindableProperty.Create(nameof(RowsBackgroundColorPalette), typeof(PaletteCollection), typeof(DataGrid), new PaletteCollection { Color.White });

		public static readonly BindableProperty RowsTextColorPaletteProperty =
			BindableProperty.Create(nameof(RowsTextColorPalette), typeof(PaletteCollection), typeof(DataGrid), new PaletteCollection { Color.Black });

		public static readonly BindableProperty ColumnsProperty =
			BindableProperty.Create(nameof(Columns), typeof(ColumnCollection), typeof(DataGrid),
				propertyChanged: (b, o, n) => (b as DataGrid).CreateUI(),
				defaultValueCreator: bindable => { return new ColumnCollection(); }
			);

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null,
				propertyChanged: (b, o, n) =>
				{
					//ObservableCollection Tracking 
					if (n is INotifyCollectionChanged)
						(n as INotifyCollectionChanged).CollectionChanged += (list, arg) =>
						{
							DataGrid self = b as DataGrid;
							if (list == (b as DataGrid).ItemsSource && self._listView.ItemsSource != list)
								self.SortItems(self.SortedColumnIndex, false);
						};
				});

		public static readonly BindableProperty RowHeightProperty =
			BindableProperty.Create(nameof(RowHeight), typeof(int), typeof(DataGrid), 40);

		public static readonly BindableProperty HeaderHeightProperty =
			BindableProperty.Create(nameof(HeaderHeight), typeof(int), typeof(DataGrid), 40);

		public static readonly BindableProperty IsSortableProperty =
			BindableProperty.Create(nameof(IsSortable), typeof(bool), typeof(DataGrid), true);

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSizeProperty), typeof(double), typeof(DataGrid), 13.0);

		public static readonly BindableProperty HeaderFontSizeProperty =
			BindableProperty.Create(nameof(HeaderFontSize), typeof(double), typeof(DataGrid), 13.0);

		public static readonly BindableProperty SelectedItemProperty =
			BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(DataGrid), null, BindingMode.TwoWay,
				propertyChanged: (b, o, n) =>
				{
					if ((b as DataGrid)._listView.SelectedItem != n)
						(b as DataGrid)._listView.SelectedItem = n;
				}
				);

		public static readonly BindableProperty SelectionEnabledProperty =
			BindableProperty.Create(nameof(SelectionEnabled), typeof(bool), typeof(DataGrid), false);

		public static readonly BindableProperty PullToRefreshCommandProperty =
			BindableProperty.Create(nameof(PullToRefreshCommand), typeof(ICommand), typeof(DataGrid), null,
				propertyChanged: (b, o, n) =>
				{
					if (n == null)
					{
						(b as DataGrid)._listView.IsPullToRefreshEnabled = false;
						(b as DataGrid)._listView.RefreshCommand = null;
					}
					else
					{
						(b as DataGrid)._listView.IsPullToRefreshEnabled = true;
						(b as DataGrid)._listView.RefreshCommand = n as ICommand;
					}
				});

		public static readonly BindableProperty IsRefreshingProperty =
			BindableProperty.Create(nameof(IsRefreshing), typeof(bool), typeof(DataGrid), false, BindingMode.TwoWay,
				propertyChanged: (b, o, n) =>
				{
					(b as DataGrid)._listView.IsRefreshing = (bool)n;
				});

		public static readonly BindableProperty BorderThicknessProperty =
			BindableProperty.Create(nameof(BorderThickness), typeof(Thickness), typeof(DataGrid), new Thickness(0.5));

		public static readonly BindableProperty HeaderBordersVisibleProperty =
			BindableProperty.Create(nameof(HeaderBordersVisible), typeof(bool), typeof(DataGrid), true,
				propertyChanged: (b, o, n) =>
				{
					if ((b as DataGrid)._headerView == null)
						return;
					if (!(bool)n)
						(b as DataGrid)._headerView.BackgroundColor = (b as DataGrid).HeaderBackground;
					else
						(b as DataGrid)._headerView.BackgroundColor = (b as DataGrid).BorderColor;
				});

		public static readonly BindableProperty SortedColumnIndexProperty =
			BindableProperty.Create(nameof(SortedColumnIndex), typeof(int), typeof(DataGrid), -1, BindingMode.TwoWay,
				propertyChanged: (b, o, n) =>
				{
					if (o != n && (int)n >= 0)
						(b as DataGrid).SortItems((int)n);
				});

		public static readonly BindableProperty HeaderLabelStyleProperty =
			BindableProperty.Create(nameof(HeaderLabelStyle), typeof(Style), typeof(DataGrid));

		public static readonly BindableProperty AscendingIconProperty =
			BindableProperty.Create(nameof(AscendingIcon), typeof(ImageSource), typeof(DataGrid), ImageSource.FromResource("Xamarin.Forms.DataGrid.up.png"));

		public static readonly BindableProperty DescendingIconProperty =
			BindableProperty.Create(nameof(DescendingIcon), typeof(ImageSource), typeof(DataGrid), ImageSource.FromResource("Xamarin.Forms.DataGrid.down.png"));

		public static readonly BindableProperty DescendingIconStyleProperty =
			BindableProperty.Create(nameof(DescendingIconStyle), typeof(Style), typeof(DataGrid), null);

		public static readonly BindableProperty AscendingIconStyleProperty =
			BindableProperty.Create(nameof(AscendingIconStyle), typeof(Style), typeof(DataGrid), null);
		#endregion

		#region properties
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

		public Color HeaderTextColor
		{
			get { return (Color)GetValue(HeaderTextColorProperty); }
			set { SetValue(HeaderTextColorProperty, value); }
		}

		public Color BorderColor
		{
			get { return (Color)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}

		public PaletteCollection RowsBackgroundColorPalette
		{
			get { return (PaletteCollection)GetValue(RowsBackgroundColorPaletteProperty); }
			set { SetValue(RowsBackgroundColorPaletteProperty, value); }
		}

		public PaletteCollection RowsTextColorPalette
		{
			get { return (PaletteCollection)GetValue(RowsTextColorPaletteProperty); }
			set { SetValue(RowsTextColorPaletteProperty, value); }
		}

		public IEnumerable ItemsSource
		{
			get { return (IEnumerable)GetValue(ItemsSourceProperty); }
			set { SetValue(ItemsSourceProperty, value); }
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

		public double HeaderFontSize
		{
			get { return (double)GetValue(HeaderFontSizeProperty); }
			set { SetValue(HeaderFontSizeProperty, value); }
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

		public int SortedColumnIndex
		{
			get { return (int)GetValue(SortedColumnIndexProperty); }
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
		#endregion

		#region fields

		Dictionary<int, SortingOrder> _sortingOrders;
		View _headerView;

		#endregion

		#region ctor
		public DataGrid()
		{
			InitializeComponent();

			_sortingOrders = new Dictionary<int, SortingOrder>();

			_listView.ItemSelected += (s, e) =>
			{
				if (SelectionEnabled)
					SelectedItem = _listView.SelectedItem;
				else
					_listView.SelectedItem = null;

				ItemSelected?.Invoke(this, e);
			};

			_listView.Refreshing += (s, e) =>
			{
				Refreshing?.Invoke(this, e);
			};
		}
		#endregion

		#region UI Methods
		protected override void OnParentSet()
		{
			base.OnParentSet();
			CreateUI();
		}

		private void CreateUI()
		{
			//if Columns changed
			if (_headerView != null)
				Children.Remove(_headerView);

			_headerView = GetHeader();
			Header.Content = _headerView;
		}
		#endregion

		#region header creation methods

		private View GetHeaderViewForColumn(DataGridColumn column)
		{
			Label text = new Label
			{
				Text = column.Title,
			};

			if (HeaderLabelStyle != null)
				text.Style = HeaderLabelStyle;

			Grid grid = new Grid
			{
				ColumnSpacing = 0,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

			if (IsSortable)
			{
				column.SortingIcon = new Image
				{
					Style = (Style)Header.Resources["ImageStyleBase"],
				};

				grid.Children.Add(column.SortingIcon);
				Grid.SetColumn(column.SortingIcon, 1);

				TapGestureRecognizer tgr = new TapGestureRecognizer();
				tgr.Tapped += (s, e) =>
				{
					int index = Columns.IndexOf(column);
					if (index == SortedColumnIndex)
						SortItems(index);
					else
						SortedColumnIndex = index;
				};
				grid.GestureRecognizers.Add(tgr);
			}

			grid.Children.Add(text);

			return grid;
		}

		private View GetHeader()
		{
			var header = new Grid
			{
				HeightRequest = HeaderHeight,
				Padding = (HeaderBordersVisible) ? BorderThickness.HorizontalThickness : 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,
				ColumnSpacing = (HeaderBordersVisible) ? BorderThickness.HorizontalThickness : 0,
				BackgroundColor = (HeaderBordersVisible) ? BorderColor : HeaderBackground,
			};

			foreach (var col in Columns)
			{
				header.ColumnDefinitions.Add(new ColumnDefinition() { Width = col.Width });

				var cell = GetHeaderViewForColumn(col);

				header.Children.Add(cell);
				Grid.SetColumn(cell, Columns.IndexOf(col));

				_sortingOrders.Add(Columns.IndexOf(col), SortingOrder.None);
			}

			return header;
		}

		#endregion

		#region Sorting methods
		private void SortItems(int propertyIndex, bool changeOrder = true)
		{
			if (!IsSortable)
				throw new InvalidOperationException("This DataGrid is not sortable");
			else if (Columns[propertyIndex].PropertyName == null)
				throw new InvalidOperationException("Please set the PropertyName property of Column");

			if (ItemsSource == null || !Columns[propertyIndex].SortingEnabled)
				return;

			var items = ItemsSource.Cast<object>();
			var column = Columns[propertyIndex];
			SortingOrder order = _sortingOrders[propertyIndex];

			if (changeOrder)
				order = _sortingOrders[propertyIndex] == SortingOrder.Descendant ? SortingOrder.Ascendant : SortingOrder.Descendant;

			//Sort
			if (order == SortingOrder.Descendant)
				items = items.OrderByDescending((x) => x.GetType().GetRuntimeProperty(column.PropertyName).GetValue(x)).ToList();
			else
				items = items.OrderBy((x) => x.GetType().GetRuntimeProperty(column.PropertyName).GetValue(x)).ToList();

			//Update sorting icon
			if (changeOrder)
			{
				column.SortingIcon.Style = (order == SortingOrder.Descendant) ?
					AscendingIconStyle ?? (Style)Header.Resources["DescendingIconStyle"] :
					DescendingIconStyle ?? (Style)Header.Resources["AscendingIconStyle"];

				//Support DescendingIcon property (if setted)
				if (!column.SortingIcon.Style.Setters.Any(x => x.Property == Image.SourceProperty))
				{
					if (order == SortingOrder.Descendant && DescendingIconProperty.DefaultValue != DescendingIcon)
						column.SortingIcon.Source = DescendingIcon;
					if (order == SortingOrder.Ascendant && AscendingIconProperty.DefaultValue != AscendingIcon)
						column.SortingIcon.Source = AscendingIcon;
				}
			}

			for (int i = 0; i < Columns.Count; i++)
			{
				if (i != propertyIndex)
				{
					Columns[i].SortingIcon.Source = null;
					_sortingOrders[i] = SortingOrder.None;
				}
			}

			_listView.ItemsSource = items;

			_sortingOrders[propertyIndex] = order;
			SortedColumnIndex = propertyIndex;
		}
		#endregion
	}
}
