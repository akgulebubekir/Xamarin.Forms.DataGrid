using System;

namespace Xamarin.Forms.DataGrid
{
	public class DataGridColumn : BindableObject, IDefinition
	{
		#region bindable properties
		public static readonly BindableProperty WidthProperty =
			BindableProperty.Create(nameof(Width), typeof(GridLength), typeof(DataGridColumn), new GridLength(1, GridUnitType.Star),
				propertyChanged: (b, o, n) => { if (o != n) (b as DataGridColumn).OnSizeChanged(); });

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(DataGridColumn), string.Empty,
				propertyChanged: (b, o, n) => (b as DataGridColumn).HeaderLabel.Text = (string)n);

		public static readonly BindableProperty FormattedTitleProperty =
			BindableProperty.Create(nameof(FormattedTitle), typeof(FormattedString), typeof(DataGridColumn),
				propertyChanged: (b, o, n) => (b as DataGridColumn).HeaderLabel.FormattedText = (FormattedString)n);

		public static readonly BindableProperty PropertyNameProperty =
			BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DataGridColumn), null);

		public static readonly BindableProperty StringFormatProperty =
			BindableProperty.Create(nameof(StringFormat), typeof(string), typeof(DataGridColumn), null);

		public static readonly BindableProperty CellTemplateProperty =
			BindableProperty.Create(nameof(CellTemplate), typeof(DataTemplate), typeof(DataGridColumn), null);

		public static readonly BindableProperty HorizontalContentAlignmentProperty =
			BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);

		public static readonly BindableProperty VerticalContentAlignmentProperty =
			BindableProperty.Create(nameof(VerticalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);

		public static readonly BindableProperty SortingEnabledProperty =
			BindableProperty.Create(nameof(SortingEnabled), typeof(bool), typeof(DataGridColumn), true);

		public static readonly BindableProperty HeaderLabelStyleProperty =
			BindableProperty.Create(nameof(HeaderLabelStyle), typeof(Style), typeof(DataGridColumn),
				propertyChanged: (b, o, n) => {
					if ((b as DataGridColumn).HeaderLabel != null && (o != n))
						(b as DataGridColumn).HeaderLabel.Style = n as Style;
				});

		#endregion

		#region properties

		public GridLength Width
		{
			get { return (GridLength)GetValue(WidthProperty); }
			set { SetValue(WidthProperty, value); }
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public FormattedString FormattedTitle
		{
			get { return (string)GetValue(FormattedTitleProperty); }
			set { SetValue(FormattedTitleProperty, value); }
		}
		public string PropertyName
		{
			get { return (string)GetValue(PropertyNameProperty); }
			set { SetValue(PropertyNameProperty, value); }
		}

		public string StringFormat
		{
			get { return (string)GetValue(StringFormatProperty); }
			set { SetValue(StringFormatProperty, value); }
		}

		public DataTemplate CellTemplate
		{
			get { return (DataTemplate)GetValue(CellTemplateProperty); }
			set { SetValue(CellTemplateProperty, value); }
		}

		internal Image SortingIcon { get; set; }
		internal Label HeaderLabel { get; set; }

		public LayoutOptions HorizontalContentAlignment
		{
			get { return (LayoutOptions)GetValue(HorizontalContentAlignmentProperty); }
			set { SetValue(HorizontalContentAlignmentProperty, value); }
		}

		public LayoutOptions VerticalContentAlignment
		{
			get { return (LayoutOptions)GetValue(VerticalContentAlignmentProperty); }
			set { SetValue(VerticalContentAlignmentProperty, value); }
		}

		public bool SortingEnabled
		{
			get { return (bool)GetValue(SortingEnabledProperty); }
			set { SetValue(SortingEnabledProperty, value); }
		}

		public Style HeaderLabelStyle
		{
			get { return (Style)GetValue(HeaderLabelStyleProperty); }
			set { SetValue(HeaderLabelStyleProperty, value); }
		}

		#endregion

		public event EventHandler SizeChanged;

		public DataGridColumn()
		{
			HeaderLabel = new Label();
			SortingIcon = new Image();
		}

		void OnSizeChanged()
		{
			SizeChanged?.Invoke(this, EventArgs.Empty);
		}
	}

}
