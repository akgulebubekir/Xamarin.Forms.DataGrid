using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
