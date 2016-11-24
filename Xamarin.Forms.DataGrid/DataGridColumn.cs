using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.DataGrid
{
	public class DataGridColumn : BindableObject, IDefinition
	{
		#region bindable properties
		public static readonly BindableProperty WidthProperty =
			BindableProperty.Create(nameof(Width), typeof(GridLength), typeof(DataGridColumn), new GridLength(1, GridUnitType.Star),
				propertyChanged: (b, o, n) => { if (o != n) (b as DataGridColumn).OnSizeChanged(); });

		public static readonly BindableProperty TitleProperty =
			BindableProperty.Create(nameof(Title), typeof(string), typeof(DataGridColumn), string.Empty);

		public static readonly BindableProperty PopertyNameProperty =
			BindableProperty.Create(nameof(PropertyName), typeof(string), typeof(DataGridColumn), null);

		public static readonly BindableProperty StringFormatProperty =
			BindableProperty.Create(nameof(StringFormat), typeof(string), typeof(DataGrid), null);

		public static readonly BindableProperty CellTemplateProperty =
			BindableProperty.Create(nameof(CellTemplate), typeof(DataTemplate), typeof(DataGridColumn), null);

		public static readonly BindableProperty HorizontalContentAlignmentProperty =
			BindableProperty.Create(nameof(HorizontalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);

		public static readonly BindableProperty VerticalContentAlignmentProperty =
			BindableProperty.Create(nameof(VerticalContentAlignment), typeof(LayoutOptions), typeof(DataGridColumn), LayoutOptions.Center);

		public static readonly BindableProperty SortingEnabledProperty =
			BindableProperty.Create(nameof(SortingEnabled), typeof(bool), typeof(DataGridColumn), true);

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

		public string PropertyName
		{
			get { return (string)GetValue(PopertyNameProperty); }
			set { SetValue(PopertyNameProperty, value); }
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

		internal object Params { get; set; }

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

		#endregion

		public DataGridColumn()
		{
		}

		public event EventHandler SizeChanged;

		void OnSizeChanged()
		{
			EventHandler eh = SizeChanged;
			if (eh != null)
				eh(this, EventArgs.Empty);
		}
	}

}
