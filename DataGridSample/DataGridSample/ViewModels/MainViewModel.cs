using DataGridSample.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DataGridSample.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{

		#region fields
		private ObservableCollection<Team> teams;
		private Team selectedItem;
		private bool isRefreshing;
		#endregion

		#region Properties
		public ObservableCollection<Team> Teams
		{
			get { return teams; }
			set
			{
				teams = value;
				OnPropertyChanged(nameof(Teams));
				OnPropertyChanged(nameof(Count));
			}
		}

		public int Count
		{
			get { return Teams.Count; }
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
		public ICommand AddCommand { get; set; }
		public ICommand ReplaceCommand { get; set; }
		public ICommand RemoveCommand { get; set; }
		#endregion

		public MainViewModel()
		{
			Teams = new ObservableCollection<Team>(Utils.DummyDataProvider.GetTeams());
			RefreshCommand = new Command(CmdRefresh);
			AddCommand = new Command(CmdAdd);
			ReplaceCommand = new Command(ReplaceAdd);
			RemoveCommand = new Command(RemoveAdd);
		}

		private async void CmdRefresh()
		{
			IsRefreshing = true;
			// wait 3 secs for demo
			await Task.Delay(3000);
			IsRefreshing = false;
		}

		private void CmdAdd()
		{
			IsRefreshing = true;

			foreach (Team team in Utils.DummyDataProvider.GetTeams())
			{
				Teams.Add(team);
			}

			OnPropertyChanged(nameof(Count));
			IsRefreshing = false;
		}

		private void ReplaceAdd()
		{
			IsRefreshing = true;

			var t = Teams.ToList();
			t.AddRange(Utils.DummyDataProvider.GetTeams());
			Teams = new ObservableCollection<Team>(t);

			OnPropertyChanged(nameof(Count));
			IsRefreshing = false;
		}

		private void RemoveAdd()
		{
			IsRefreshing = true;

			var even = false;
			foreach (Team team in Teams.ToList())
			{
				if (even)
					Teams.Remove(team);
				even = !even;
			}

			OnPropertyChanged(nameof(Count));
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
