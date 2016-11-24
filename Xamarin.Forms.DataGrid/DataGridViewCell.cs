using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin.Forms.DataGrid
{
	internal sealed class DataGridViewCell : ViewCell
	{

		#region Bindable Properties
		public static readonly BindableProperty DataGridProperty =
			BindableProperty.Create(nameof(DataGrid), typeof(DataGrid), typeof(DataGridViewCell), null,
				propertyChanged: (b, o, n) => (b as DataGridViewCell).CreateView());

		public static readonly BindableProperty IndexProperty =
			BindableProperty.Create(nameof(Index), typeof(int), typeof(DataGridViewCell), 0,
				propertyChanged: (b, o, n) => (b as DataGridViewCell).UpdateBackgroundColor());

		#endregion

		#region properties

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			UpdateBackgroundColor();
		}

		public DataGrid DataGrid
		{
			get { return (DataGrid)GetValue(DataGridProperty); }
			set { SetValue(DataGridProperty, value); }
		}

		public int Index
		{
			get { return (int)GetValue(IndexProperty); }
			set { SetValue(IndexProperty, value); }
		}

		#endregion

		#region Fields

		Grid _mainLayout;
		Color _bgColor;
		Color _textColor;

		#endregion

		public DataGridViewCell()
		{
		}


		#region UIMethods
		private void CreateView()
		{
			_bgColor = DataGrid.RowsBackgroundColorPalette.ElementAtOrDefault(Index % DataGrid.RowsBackgroundColorPalette.Count());
			_textColor = DataGrid.RowsTextColorPalette.ElementAtOrDefault(Index % DataGrid.RowsTextColorPalette.Count());

			_mainLayout = new Grid()
			{
				BackgroundColor = DataGrid.BorderColor,
				RowSpacing = 0,
				ColumnSpacing = DataGrid.BorderThickness.HorizontalThickness,
				Padding = new Thickness(DataGrid.BorderThickness.HorizontalThickness / 2,
										DataGrid.BorderThickness.VerticalThickness / 2),
			};

			foreach (var col in DataGrid.Columns)
			{
				_mainLayout.ColumnDefinitions.Add(new ColumnDefinition() { Width = col.Width });
				View cell;

				if (col.CellTemplate != null)
					cell = new ContentView() { Content = col.CellTemplate.CreateContent() as View };
				else
				{
					var text = new Label
					{
						TextColor = _textColor,
						HorizontalOptions = col.HorizontalContentAlignment,
						VerticalOptions = col.VerticalContentAlignment,
						LineBreakMode = LineBreakMode.WordWrap,
					};
					text.SetBinding(Label.TextProperty, new Binding(col.PropertyName, BindingMode.Default, stringFormat: col.StringFormat));

					cell = new ContentView
					{
						Padding = 0,
						BackgroundColor = _bgColor,
						Content = text,
					};
				}

				_mainLayout.Children.Add(cell);
				Grid.SetColumn(cell, DataGrid.Columns.IndexOf(col));
			}

			View = _mainLayout;
		}

		private void UpdateBackgroundColor()
		{
			int index = Index;
			//TODO Report Xamarin bug because of value not binding on recycling cell
			if (Parent != null && Parent is ListView)
				index = (Parent as ListView).ItemsSource.Cast<object>().ToList().IndexOf(BindingContext);

			_bgColor = DataGrid.RowsBackgroundColorPalette.ElementAtOrDefault(index % DataGrid.RowsBackgroundColorPalette.Count());
			_textColor = DataGrid.RowsTextColorPalette.ElementAtOrDefault(index % DataGrid.RowsTextColorPalette.Count());

			foreach (var v in _mainLayout.Children)
			{
				v.BackgroundColor = _bgColor;
				if (v is ContentView && (v as ContentView).Content is Label)
					((v as ContentView).Content as Label).TextColor = _textColor;
			}
		}
		#endregion
	}
}
