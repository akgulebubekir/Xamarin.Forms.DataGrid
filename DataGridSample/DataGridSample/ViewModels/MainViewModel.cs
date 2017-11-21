using DataGridSample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataGridSample.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{

		#region fields
		private List<Team> teams;
		private Team selectedItem;
		private bool isRefreshing;
	    private ChartTable chartTable;
	    private ChartTableRow selectedRow;

	    #endregion

        #region Properties

	    public ChartTable ChartTable
	    {
	        get { return chartTable; }
	        set { chartTable = value; OnPropertyChanged(nameof(ChartTable)); }
	    }

        public List<Team> Teams
		{
			get { return teams; }
			set { teams = value;
                OnPropertyChanged(nameof(Teams)); }
		}

		public ChartTableRow SelectedRow
        {
			get { return selectedRow; }
			set
			{
			    selectedRow = value;
			    OnPropertyChanged(nameof(SelectedRow));
            }
		}

		public Team SelectedTeam
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				System.Diagnostics.Debug.WriteLine("Team Selected : " + value.Name);
			}
		}

		public bool IsRefreshing
		{
			get { return isRefreshing; }
			set { isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
		}

		public ICommand RefreshCommand { get; set; }
		#endregion

		public MainViewModel()
		{
			Teams = Utils.DummyDataProvider.GetTeams();
		    ChartTable = Utils.DummyDataProvider.GetChartTable();
            RefreshCommand = new Command(CmdRefresh);
		}

		private async void CmdRefresh()
		{
			IsRefreshing = true;
			// wait 3 secs for demo
			await Task.Delay(3000);
			IsRefreshing = false;
		}

		#region INotifyPropertyChanged implementation
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}

		#endregion
	}
}
