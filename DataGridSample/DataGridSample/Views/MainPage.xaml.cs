using DataGridSample.Models;
using DataGridSample.ViewModels;
using DataGridSample.Views.Converters;
using Xamarin.Forms;
using Xamarin.Forms.DataGrid;

namespace DataGridSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = (MainViewModel) this.BindingContext;
            var chartTable = vm.ChartTable;

            var dataGridColumns = this.GenerateColumns(chartTable);
            this.ChartTable.Columns = dataGridColumns;
        }

        private ColumnCollection GenerateColumns(ChartTable chartTable)
        {
            var columns = new ColumnCollection();

            for (var c = 0; c < chartTable.Header.Columns.Length; c++)
            {
                var columnIndex = c;
                var dataGridColumn = new DataGridColumn
                {
                    Title = chartTable.Header.Columns[c].Value.ToString(),
                    SortingEnabled = false,
                    PropertyName = $"{nameof(ChartTableRow.Columns)}[{columnIndex}].{nameof(ChartTableColumn.Value)}",
                    Width = new GridLength(1, GridUnitType.Star),
                    CellTemplate = new DataTemplate(() =>
                    {
                        var grid = new Grid();
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                        var label = new Label { FontAttributes = FontAttributes.Bold };
                        label.SetBinding(Label.TextProperty, new Binding(".", BindingMode.OneWay));

                        grid.Children.Add(label);

                        return new ViewCell
                        {
                            View = grid
                        };
                    })
                };
                columns.Add(dataGridColumn);
            }

            return columns;
        }
    }
}