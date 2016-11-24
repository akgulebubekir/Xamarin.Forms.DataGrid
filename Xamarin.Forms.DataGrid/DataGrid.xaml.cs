using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using System.Collections;
using System.Windows.Input;

namespace Xamarin.Forms.DataGrid
{
	[Xaml.XamlCompilation(Xaml.XamlCompilationOptions.Compile)]
	public partial class DataGrid : Grid
	{
		public event EventHandler Refreshing;
		public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

		#region Bindable properties
		public static readonly BindableProperty HeaderBackgroundProperty =
			BindableProperty.Create(nameof(HeaderBackground), typeof(Color), typeof(DataGrid), Color.Aqua);

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
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(DataGrid), null);

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
			BindableProperty.Create(nameof(HeaderBordersVisible), typeof(bool), typeof(DataGrid), true);

		public static readonly BindableProperty SortedColumnIndexProperty =
			BindableProperty.Create(nameof(SortedColumnIndex), typeof(int), typeof(DataGrid), -1, BindingMode.TwoWay,
				propertyChanged: (b, o, n) =>
				{
					if (o != n && (int)n >= 0)
						(b as DataGrid).SortItems((int)n);
				});
		#endregion

		#region properties
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
			Children.Add(_headerView);
		}

		#region header creation methods

		private View GetHeaderViewForColumn(DataGridColumn column)
		{
			Label text = new Label
			{
				Text = column.Title,
				VerticalOptions = column.VerticalHeaderAlignment,
				HorizontalOptions = column.HorizontalHeaderAlignment,
				HorizontalTextAlignment = TextAlignment.Center,
				TextColor = HeaderTextColor,
				FontAttributes = FontAttributes.Bold,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = HeaderFontSize,
			};

			Grid grid = new Grid
			{
				BackgroundColor = HeaderBackground,
				ColumnSpacing = 0,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
			grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });

			if (IsSortable)
			{
				var orderingIcon = new Image
				{
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.Center,
					WidthRequest = 8,
					HeightRequest = 6,
					Margin = new Thickness(0, 0, 4, 0),
				};

				column.Params = orderingIcon;
				grid.Children.Add(orderingIcon);
				Grid.SetColumn(orderingIcon, 1);

				TapGestureRecognizer tgr = new TapGestureRecognizer();
				tgr.Tapped += (s, e) => SortItems(Columns.IndexOf(column));
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

				_sortingOrders.Add(Columns.IndexOf(col), SortingOrder.NotDetermined);
			}

			return header;
		}

		#endregion

		#region Sorting methods
		private void SortItems(int propertyIndex)
		{
			if (ItemsSource == null || ItemsSource.Cast<object>().Count() < 1 || !Columns[propertyIndex].SortingEnabled)
				return;

			List<object> item = new List<object>();
			foreach (var itm in ItemsSource)
				item.Add(itm);

			List<object> sortedItems = null;

			if (!IsSortable)
				throw new InvalidOperationException("This DataGrid is not sortable");
			if (Columns[propertyIndex].PropertyName == null)
				throw new InvalidOperationException("Please set the PropertyName property of Column");

			Image sortingImage = Columns[propertyIndex].Params as Image;

			if (_sortingOrders[propertyIndex] != SortingOrder.Descendant)
			{
				sortedItems = item.OrderByDescending((x) => x.GetType().GetRuntimeProperty(Columns[propertyIndex].PropertyName).GetValue(x)).ToList();
				_sortingOrders[propertyIndex] = SortingOrder.Descendant;
				sortingImage.Source = ImageSource.FromResource("Xamarin.Forms.DataGrid.down.png");
			}
			else
			{
				sortedItems = item.OrderBy((x) => x.GetType().GetRuntimeProperty(Columns[propertyIndex].PropertyName).GetValue(x)).ToList();
				_sortingOrders[propertyIndex] = SortingOrder.Ascendant;
				sortingImage.Source = ImageSource.FromResource("Xamarin.Forms.DataGrid.up.png");
			}

			foreach (var column in Columns)
			{
				if ((column.Params as Image).Source != null && Columns[propertyIndex] != column)
					(column.Params as Image).Source = null;
			}

			_listView.ItemsSource = sortedItems;

			SortedColumnIndex = propertyIndex;
		}
		#endregion


	}
}
