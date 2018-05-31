using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.Common;
using Xamarin.Forms.DataGrid.UnitTest.Models;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class SortedColumnIndexTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void SortedColunmIndexInInit()
		{
			var dg = new DataGrid();
			Assert.IsTrue(dg.SortedColumnIndex == null);
		}

		[TestMethod]
		public void SortedColumnIndexBindingToSortData()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var sDataVm = new SingleVM<SortData> {
				Item = new SortData(2, SortingOrder.Ascendant),
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: sDataVm));

			Assert.IsTrue(dg.SortedColumnIndex.Index == 2 && dg.SortedColumnIndex.Order == SortingOrder.Ascendant);
		}

		[TestMethod]
		public void SortedColumnIndexBindintToInt()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var scVM = new SingleVM<int> {
				Item = 2
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: scVM));

			Assert.IsTrue(dg.SortedColumnIndex.Index == 2 && dg.SortedColumnIndex.Order == SortingOrder.Ascendant);
		}

		[TestMethod]
		public void SortedColumnIndexTwoWayBinding()
		{
			var teams = Util.GetTeams();
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var sVm = new SingleVM<SortData> {
				Item = new SortData(2, SortingOrder.Ascendant)
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", mode: BindingMode.TwoWay, source: sVm));

			Assert.IsTrue(dg.SortedColumnIndex.Index == 2 && dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			dg.SortedColumnIndex = new SortData(1, SortingOrder.Descendant);
			Assert.IsTrue(sVm.Item.Index == 1 && sVm.Item.Order == SortingOrder.Descendant);

			sVm.Item = new SortData(0, SortingOrder.Ascendant);
			Assert.IsTrue(dg.SortedColumnIndex.Index == 0 && dg.SortedColumnIndex.Order == SortingOrder.Ascendant);
		}

		[TestMethod]
		public void SortedColumnIndexTwoWayBindingToInt()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var sVm = new SingleVM<SortData> {
				Item = 2,
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", mode: BindingMode.TwoWay, source: sVm));

			Assert.IsTrue(dg.SortedColumnIndex.Index == 2 && dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			dg.SortedColumnIndex = 1;
			Assert.IsTrue(sVm.Item.Index == 1);

			sVm.Item = 0;
			Assert.IsTrue(dg.SortedColumnIndex.Index == 0);
		}

		[TestMethod]
		public void SortedColumnIndexMoreThanColumnCount()
		{
			var teams = Util.GetTeams();
			var dg = new DataGrid {
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "Name", PropertyName="Name"},
					new DataGridColumn{Title = "Logo",PropertyName = "Logo"}
				}
			};

			Assert.ThrowsException<ArgumentException>(() => dg.SortedColumnIndex = 5);
		}

		[TestMethod]
		public void SortedColumnIndexMoreThanColumnCountWithBinding()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var scVm = new SingleVM<SortData> {
				Item = new SortData(1, SortingOrder.Ascendant),
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "Name", PropertyName="Name"},
					new DataGridColumn{Title = "Logo",PropertyName = "Logo"}
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: scVm));

			Assert.ThrowsException<ArgumentException>(() => scVm.Item = new SortData(5, SortingOrder.Ascendant));
		}

		[TestMethod]
		public void SortedColumnIndexMoreThanColumnCountWithBindingToInt()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var scVm = new SingleVM<int> {
				Item = 1,
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "Name", PropertyName="Name"},
					new DataGridColumn{Title = "Logo",PropertyName = "Logo"}
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: scVm));

			Assert.ThrowsException<ArgumentException>(() => scVm.Item = 4);
		}

		[TestMethod]
		public void SortedColumnIndexWithUnsortableDGWithBinding()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var scVm = new SingleVM<int> {
				Item = 1,
			};


			var dg = new DataGrid {
				IsSortable = false,
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "Name", PropertyName="Name"},
					new DataGridColumn{Title = "Logo",PropertyName = "Logo"}
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));

			Assert.ThrowsException<InvalidOperationException>(() =>
				dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: scVm))
			);
		}

		[TestMethod]
		public void SortedColumnIndexWithUnsortableDG()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var dg = new DataGrid {
				IsSortable = false,
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "Name", PropertyName="Name"},
					new DataGridColumn{Title = "Logo",PropertyName = "Logo"}
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));

			Assert.ThrowsException<InvalidOperationException>(() => dg.SortedColumnIndex = 1);
		}

		[TestMethod]
		public void SortedColumnIndexWithUnsortableDGButSortableColumn()
		{
			var vm = new SingleVM<List<Team>> {
				Item = Util.GetTeams(),
			};

			var dg = new DataGrid {
				IsSortable = false,
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "Name", PropertyName  = "Name", SortingEnabled = true},
					new DataGridColumn{Title = "Logo",PropertyName = "Logo"}
				}
			};

			dg.SetBinding(DataGrid.ItemsSourceProperty, new Binding("Item", source: vm));

			Assert.ThrowsException<InvalidOperationException>(() => dg.SortedColumnIndex = 0);
		}

		[TestMethod]
		public void SortedColumnIndexColumnBinding()
		{
			var columns = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
			};

			var vm = new SingleVM<SortData> {
				Item = 1
			};

			var dg = new DataGrid {
				IsSortable = true
			};

			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: vm));
			dg.SetBinding(DataGrid.ColumnsProperty, new Binding(".", source: columns));

			Assert.IsTrue(dg.SortedColumnIndex == vm.Item);
		}

		[TestMethod]
		public void SortedColumnIndexBindToNull()
		{
			var vm = new SingleVM<SortData> {
				Item = null
			};

			var dg = new DataGrid {
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "C1", PropertyName="c1"},
					new DataGridColumn{Title = "C2", PropertyName="c2"},
					new DataGridColumn{Title = "C3", PropertyName="c3"},
				}
			};

			dg.SetBinding(DataGrid.SortedColumnIndexProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.SortedColumnIndex == null);
		}

		[TestMethod]
		public void SortedColumnIndexTryToSetUnsortableColumn()
		{
			var dg = new DataGrid {
				Columns = new ColumnCollection {
					new DataGridColumn{Title = "C1", PropertyName="c1"},
					new DataGridColumn{Title = "C2", PropertyName="c2",SortingEnabled = false},
					new DataGridColumn{Title = "C3", PropertyName="c3"},
				},
			};

			Assert.ThrowsException<ArgumentException>(() => dg.SortedColumnIndex= 1);
		}
		#endregion
	}
}
