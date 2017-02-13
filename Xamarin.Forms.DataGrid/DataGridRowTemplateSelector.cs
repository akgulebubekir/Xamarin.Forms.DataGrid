using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

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
			var items = dataGrid.InternalItems;

			_dataGridRowTemplate.SetValue(DataGridViewCell.DataGridProperty, dataGrid);

			if (items != null)
				_dataGridRowTemplate.SetValue(DataGridViewCell.IndexProperty, items.IndexOf(item));


			return _dataGridRowTemplate;
		}
	}
}
