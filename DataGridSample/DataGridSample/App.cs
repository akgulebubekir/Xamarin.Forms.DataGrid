using Xamarin.Forms;

namespace DataGridSample
{
	public class App : Application
	{
		public App()
		{
			//Load the assembly
			Xamarin.Forms.DataGrid.DataGridComponent.Init();

			MainPage = new MainPage() { BindingContext = new ViewModels.MainViewModel() };
		}

		#region App Lifecycle
		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
		#endregion
	}
}
