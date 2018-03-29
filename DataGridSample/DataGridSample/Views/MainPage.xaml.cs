using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DataGridSample.ViewModels;

namespace DataGridSample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void DataGrid_ItemDoubleTapped(object sender, ItemTappedEventArgs e)
		{
		    (this.BindingContext as MainViewModel).GridDoubleTapped(e);
		}
	}
}
