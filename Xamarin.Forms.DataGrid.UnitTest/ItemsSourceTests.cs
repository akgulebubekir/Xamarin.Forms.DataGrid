using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.Common;
using Xamarin.Forms.DataGrid.UnitTest.Models;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class ItemsSourceTests
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Test Methods
		[TestMethod]
		public void ItemsSourceWithoutSet()
		{
			var dg = new DataGrid();
			Assert.IsTrue(dg.ItemsSource == null);
		}

		[TestMethod]
		public void ItemsSourceSet()
		{
			var teams = Util.GetTeams();
			var dg = new DataGrid() { ItemsSource = teams };
			Assert.IsTrue(dg.ItemsSource == teams);
		}

		[TestMethod]
		public void ItemsSourceBinding()
		{
			var teams = Util.GetTeams();

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding(".", source: teams));

			Assert.IsTrue(dg.ItemsSource == teams);
		}

		[TestMethod]
		public void ItemsSourceRemoveItem()
		{
			var teams = Util.GetTeams();
			var teamCollection = new ObservableCollection<Team>(teams);

			var dg = new DataGrid();
			dg.ItemsSource = teamCollection;

			var t1 = teams.First();
			teamCollection.Remove(t1);

			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Count() == 14);
			Assert.IsFalse(dg.ItemsSource.Cast<Team>().Contains(t1));
		}

		[TestMethod]
		public void ItemsSourceBindingRemoveItem()
		{
			var teams = Util.GetTeams();
			var teamCollection = new ObservableCollection<Team>(teams);

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding(".", source: teamCollection));

			var t1 = teams.First();
			teamCollection.Remove(t1);

			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Count() == 14);
			Assert.IsFalse(dg.ItemsSource.Cast<Team>().Contains(t1));
		}

		[TestMethod]
		public void ItemsSourceAddItem()
		{
			var teams = Util.GetTeams();
			var teamCollection = new ObservableCollection<Team>(teams);

			var dg = new DataGrid();
			dg.ItemsSource = teamCollection;

			var newteam = new Team { Name = "test team" };
			teamCollection.Add(newteam);

			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Count() == 16);
			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Contains(newteam));
		}

		[TestMethod]
		public void ItemsSourceBindingAddItem()
		{
			var teams = Util.GetTeams();
			var teamCollection = new ObservableCollection<Team>(teams);

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding(".", source: teamCollection));

			var newteam = new Team { Name = "test team" };
			teamCollection.Add(newteam);

			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Count() == 16);
			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Contains(newteam));
		}

		[TestMethod]
		public void ItemsSourceBindingSort()
		{
			var vm = new SingleVM<List<Team>>() {
				Item = Util.GetTeams()
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));

			vm.Item = vm.Item.OrderBy(x => x.Name).ToList();
			var dgSource = dg.ItemsSource.Cast<Team>();

			for (int i = 0; i < 15; i++)
				Assert.IsTrue(vm.Item.ElementAt(i) == dgSource.ElementAt(i));
		}

		[TestMethod]
		public void ItemsSourceBindingSortAndAddNew()
		{
			var vm = new SingleVM<ObservableCollection<Team>> {
				Item = new ObservableCollection<Team>(Util.GetTeams())
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));

			vm.Item = new ObservableCollection<Team>(vm.Item.OrderBy(x => x.Name));
			vm.Item.Add(new Team { Name = "test team" });

			var dgSource = dg.ItemsSource.Cast<Team>();

			Assert.IsTrue(dgSource.Count() == 16);
			Assert.IsTrue(dgSource.ElementAt(15).Name == "test team");

			for (int i = 0; i < 15; i++)
				Assert.IsTrue(vm.Item.ElementAt(i) == dgSource.ElementAt(i));
		}
		#endregion
	}
}
