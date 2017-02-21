using System.Linq;

namespace Xamarin.Forms.DataGrid
{
	internal class DataGridRowTemplateSelector : DataTemplateSelector
	{
		private static DataTemplate _dataGridRowTemplate;

		public DataGridRowTemplateSelector()
		{
			_dataGridRowTemplate = new DataTemplate(typeof(DataGridViewCell));
		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			var listView = container as ListView;
			var dataGrid = listView.Parent as DataGrid;

			_dataGridRowTemplate.SetValue(DataGridViewCell.DataGridProperty, dataGrid);
			_dataGridRowTemplate.SetValue(DataGridViewCell.IndexProperty, listView.ItemsSource.Cast<object>().ToList().IndexOf(item));

			return _dataGridRowTemplate;
		}
	}
}
