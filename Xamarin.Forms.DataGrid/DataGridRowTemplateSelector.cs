using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Xamarin.Forms.DataGrid
{
    internal class DataGridRowTemplateSelector : DataTemplateSelector
    {
        public DataGridRowTemplateSelector()
        {
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var listView = container as ListView;
            var dataGrid = listView.Parent as DataGrid;

            DataTemplate template = new DataTemplate(typeof(DataGridViewCell));
            template.SetValue(DataGridViewCell.DataGridProperty, dataGrid);
            template.SetValue(DataGridViewCell.IndexProperty, dataGrid.ItemsSource.Cast<object>().ToList().IndexOf(item));

            return template;
        }
    }
}
