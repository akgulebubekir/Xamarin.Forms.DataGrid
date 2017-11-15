using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid.Utils;

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

        public static readonly BindableProperty RowContextProperty =
            BindableProperty.Create(nameof(RowContext), typeof(object), typeof(DataGridViewCell), null);
        #endregion

        #region properties
        protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			if (BindingContext != null)
				UpdateBackgroundColor(BindingContext.Equals(_previouslySelectedBindingContext));
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

        public object RowContext
        {
            get { return GetValue(RowContextProperty); }
            set { SetValue(RowContextProperty, value); }
        }
        #endregion

        #region Fields
        static DataGridViewCell _previouslySelectedViewCell;
		static object _previouslySelectedBindingContext;

		Grid _mainLayout;
		Color _bgColor;
		Color _textColor;

		#endregion

		#region UIMethods
		protected override void OnTapped()
		{
			base.OnTapped();
			if (!DataGrid.IsEnabled || !DataGrid.SelectionEnabled) return;

			_previouslySelectedViewCell?.UpdateBackgroundColor();

			_bgColor = DataGrid.ActiveRowColor;
			ChangeColor(_bgColor);

			_previouslySelectedViewCell = this;
			_previouslySelectedBindingContext = BindingContext;
		}

		private void CreateView()
		{
			_bgColor = DataGrid.RowsBackgroundColorPalette.ElementAtOrDefault(Index % DataGrid.RowsBackgroundColorPalette.Count());
			_textColor = DataGrid.RowsTextColorPalette.ElementAtOrDefault(Index % DataGrid.RowsTextColorPalette.Count());

			_mainLayout = new Grid()
			{
				BackgroundColor = DataGrid.BorderColor,
				RowSpacing = 0,
				ColumnSpacing = DataGrid.BorderThickness.HorizontalThickness / 2,
				Padding = new Thickness(DataGrid.BorderThickness.HorizontalThickness / 2,
										DataGrid.BorderThickness.VerticalThickness / 2),
			};

			foreach (var col in DataGrid.Columns)
			{
				_mainLayout.ColumnDefinitions.Add(new ColumnDefinition() { Width = col.Width });
				View cell;

				if (col.CellTemplate != null)
                {
                    cell = new ContentView() { Content = col.CellTemplate.CreateContent() as View };
                    if (col.PropertyName != null)
                    {
                        cell.SetBinding(BindableObject.BindingContextProperty, 
                            new Binding(col.PropertyName, source: RowContext));
                    }
                }
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
					text.SetBinding(Label.FontSizeProperty, new Binding(DataGrid.FontSizeProperty.PropertyName, BindingMode.Default, source: DataGrid));
					text.SetBinding(Label.FontFamilyProperty, new Binding(DataGrid.FontFamilyProperty.PropertyName, BindingMode.Default, source: DataGrid));

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

		private void UpdateBackgroundColor(bool isSelected = false)
		{
			int index = Index;
			var items = DataGrid?.InternalItems;
			
			if (items != null)
				index = items.IndexOf(BindingContext);

			_bgColor = isSelected ?
				DataGrid.ActiveRowColor :
				DataGrid.RowsBackgroundColorPalette.ElementAtOrDefault(index % DataGrid.RowsBackgroundColorPalette.Count);
			_textColor = DataGrid.RowsTextColorPalette.ElementAtOrDefault(index % DataGrid.RowsTextColorPalette.Count);

			ChangeColor(_bgColor);
		}

		private void ChangeColor(Color color)
		{
			foreach (var v in _mainLayout.Children)
			{
				v.BackgroundColor = color;
				var contentView = v as ContentView;
				if (contentView?.Content is Label)
					((Label)contentView.Content).TextColor = _textColor;
			}
		}
		#endregion
	}
}
