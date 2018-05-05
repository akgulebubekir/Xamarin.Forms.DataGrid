using System.ComponentModel;

namespace Xamarin.Forms.DataGrid.UnitTest.VM
{
	internal class SingleVM<T> : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private T _item;
		public T Item
		{
			get { return _item; }
			set
			{
				_item = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item)));
			}
		}
	}
}
