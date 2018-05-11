using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.Common;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class SortingIconTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		[TestMethod]
		public void SortingIconOnInit()
		{
			var dg = new DataGrid {
				ItemsSource = Util.GetTeams(),
				Columns = Util.CreateColumns(),
			};

			foreach (var c in dg.Columns)
				Assert.IsTrue(c.SortingIcon.Source == null);
		}

		[TestMethod]
		public void SortingIconSort()
		{
			var dg = new DataGrid {
				ItemsSource = Util.GetTeams(),
				Columns = Util.CreateColumns(),
			};

			dg.SortItems(0);

			Assert.IsTrue(dg.SortedColumnIndex.Index == 0);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			Assert.IsTrue(dg.Columns[0].SortingIcon.Source == dg.AscendingIcon);

			for (int i = 1; i < dg.Columns.Count; i++)
				Assert.IsTrue(dg.Columns[i].SortingIcon.Source == null);

		}

		[TestMethod]
		public void SortingIconSortTwice()
		{
			var dg = new DataGrid {
				ItemsSource = Util.GetTeams(),
				Columns = Util.CreateColumns(),
			};

			dg.SortItems(0);

			Assert.IsTrue(dg.SortedColumnIndex.Index == 0);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Ascendant);
			Assert.IsTrue(dg.Columns[0].SortingIcon.Source == dg.AscendingIcon);

			dg.SortItems(new SortData(0, SortingOrder.Descendant));
			Assert.IsTrue(dg.SortedColumnIndex.Index == 0);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Descendant);
			Assert.IsTrue(dg.Columns[0].SortingIcon.Source == dg.DescendingIcon);

			for (int i = 1; i < dg.Columns.Count; i++)
				Assert.IsTrue(dg.Columns[i].SortingIcon.Source == null);
		}

		[TestMethod]
		public void SortingIconSortDifferentColumns()
		{
			var dg = new DataGrid {
				ItemsSource = Util.GetTeams(),
				Columns = Util.CreateColumns(),
			};

			dg.SortItems(0);

			Assert.IsTrue(dg.SortedColumnIndex.Index == 0);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			Assert.IsTrue(dg.Columns[0].SortingIcon.Source == dg.AscendingIcon);

			dg.SortItems(1);
			Assert.IsTrue(dg.SortedColumnIndex.Index == 1);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			for (int i = 0; i < dg.Columns.Count; i++)
			{
				if (i == 1)
					Assert.IsTrue(dg.Columns[i].SortingIcon.Source == dg.AscendingIcon);
				else
					Assert.IsTrue(dg.Columns[i].SortingIcon.Source == null);
			}
		}

		[TestMethod]
		public void SortingIconSortColumns010()
		{
			var dg = new DataGrid {
				ItemsSource = Util.GetTeams(),
				Columns = Util.CreateColumns(),
			};

			dg.SortItems(0);
			Assert.IsTrue(dg.Columns[0].SortingIcon.Source == dg.AscendingIcon);

			dg.SortItems(1);
			Assert.IsTrue(dg.SortedColumnIndex.Index == 1);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			dg.SortItems(0);
			Assert.IsTrue(dg.Columns[0].SortingIcon.Source == dg.AscendingIcon);
			Assert.IsTrue(dg.SortedColumnIndex.Index == 0);
			Assert.IsTrue(dg.SortedColumnIndex.Order == SortingOrder.Ascendant);

			for (int i = 1; i < dg.Columns.Count; i++)
				Assert.IsTrue(dg.Columns[i].SortingIcon.Source == null);
		}
	}
}
