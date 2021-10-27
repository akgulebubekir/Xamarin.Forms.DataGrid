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
			var collectionView = (CollectionView) container;
			var dataGrid = (DataGrid) collectionView.Parent.Parent;
			var items = dataGrid.InternalItems;

			_dataGridRowTemplate.SetValue(DataGridViewCell.DataGridProperty, dataGrid);
			_dataGridRowTemplate.SetValue(DataGridViewCell.RowContextProperty, item);
			_dataGridRowTemplate.SetValue(VisualElement.HeightRequestProperty, dataGrid.RowHeight);

			if (items != null)
				_dataGridRowTemplate.SetValue(DataGridViewCell.IndexProperty, items.IndexOf(item));

			return _dataGridRowTemplate;
		}
	}
}