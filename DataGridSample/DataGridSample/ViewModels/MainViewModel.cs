using DataGridSample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridSample.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private List<Team> teams;
        public List<Team> Teams
        {
            get { return teams; }
            set { teams = value; OnPropertyChanged(nameof(Teams)); }
        }

        private Team selectedItem;
        public Team SelectedTeam
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                System.Diagnostics.Debug.WriteLine("Team Selected : "+ value.Name);
            }
        }

        public MainViewModel()
        {
            Teams = Utils.DummyDataProvider.GetTeams();
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
