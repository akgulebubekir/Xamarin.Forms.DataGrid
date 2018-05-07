using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.Common;
using Xamarin.Forms.DataGrid.UnitTest.Models;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class SelectedItemTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void SelectedItemOnInit()
		{
			var dg = new DataGrid();

			Assert.IsNull(dg.SelectedItem);
		}

		[TestMethod]
		public void SelectedItemWithoutItemsSource()
		{
			var teams = Util.GetTeams();

			var dg = new DataGrid();
			dg.SelectedItem = teams.First();

			Assert.IsNull(dg.SelectedItem);
		}

		[TestMethod]
		public void SelectedItemRemoveFromItemsSource()
		{
			var teams = Util.GetTeams();
			var dg = new DataGrid();

			dg.ItemsSource = teams;
			dg.SelectedItem = teams.First();

			dg.ItemsSource = teams.Skip(1);

			Assert.IsTrue(dg.ItemsSource.Cast<Team>().Count() == 14);
			Assert.IsNull(dg.SelectedItem);

		}

		[TestMethod]
		public void SelectedItemBinding()
		{
			var teams = Util.GetTeams();

			var vm = new SingleVM<List<Team>> {
				Item = teams,
			};

			var sVm = new SingleVM<Team> {
				Item = teams.First()
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SelectedItemProperty, new Binding("Item", source: sVm));

			Assert.IsTrue(dg.ItemsSource == teams);
			Assert.IsTrue(dg.SelectedItem == teams.First());
		}

		[TestMethod]
		public void SelectedItemBindingResetNull()
		{
			var teams = Util.GetTeams();
			var vm = new SingleVM<List<Team>> {
				Item = teams,
			};

			var sVm = new SingleVM<Team> {
				Item = teams.First()
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SelectedItemProperty, new Binding("Item", source: sVm));

			Assert.IsTrue(dg.ItemsSource == teams);
			Assert.IsTrue(dg.SelectedItem == teams.First());

			sVm.Item = null;
			Assert.IsNull(dg.SelectedItem);

		}

		[TestMethod]
		public void SelectedItemBindingResetNonexistingItem()
		{
			var teams = Util.GetTeams();
			var vm = new SingleVM<List<Team>> {
				Item = teams,
			};

			var sVm = new SingleVM<Team> {
				Item = teams.First()
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SelectedItemProperty, new Binding("Item", source: sVm));

			Assert.IsTrue(dg.ItemsSource == teams);
			Assert.IsTrue(dg.SelectedItem == teams.First());

			vm.Item = teams.Skip(1).ToList();
			Assert.IsNull(dg.SelectedItem);
		}

		[TestMethod]
		public void SelectedItemTwoWayBinding()
		{
			var teams = Util.GetTeams();
			var vm = new SingleVM<List<Team>> {
				Item = teams,
			};

			var sVm = new SingleVM<Team> {
				Item = null
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SelectedItemProperty, new Binding("Item", mode: BindingMode.TwoWay, source: sVm));

			dg.SelectedItem = teams.First();
			Assert.IsTrue(sVm.Item == teams.First());

			sVm.Item = teams.ElementAt(3);
			Assert.IsTrue(dg.SelectedItem == teams.ElementAt(3));
		}

		[TestMethod]
		public void SelectedItemWithoutSelectionEnabled()
		{
			var teams = Util.GetTeams();

			var dg = new DataGrid {
				SelectionEnabled = false,
			};

			Assert.ThrowsException<InvalidOperationException>(() => dg.SelectedItem = teams.First());
		}

		[TestMethod]
		public void SelectedItemBindingWithoutSelectionEnabled()
		{
			var teams = Util.GetTeams();
			var sVm = new SingleVM<Team> {
				Item = teams.First()
			};

			var dg = new DataGrid {
				SelectionEnabled = false,
			};

			Assert.ThrowsException<InvalidOperationException>(() => dg.SetBinding(DataGrid.SelectedItemProperty, new Binding("Item", source: sVm)));
		}

		[TestMethod]
		public void SelectedItemDisablingSelectionAfterSetting()
		{
			var teams = Util.GetTeams();

			var dg = new DataGrid {
				ItemsSource = teams,
				SelectedItem = teams.First()
			};

			Assert.IsTrue(dg.SelectedItem == teams.First());

			dg.SelectionEnabled = false;
			Assert.IsNull(dg.SelectedItem);


		}
		#endregion
	}
}
