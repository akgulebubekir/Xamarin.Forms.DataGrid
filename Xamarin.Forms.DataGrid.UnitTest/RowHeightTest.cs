using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class RowHeightTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void RowHeightInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.RowHeight == 40);
		}

		[TestMethod]
		public void RowHeightFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.RowHeightProperty, Value = 44 });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.RowHeight == 44);
		}

		[TestMethod]
		public void RowHeightFromBinding()
		{
			var vm = new SingleVM<int> {
				Item = 44
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.RowHeightProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.RowHeight == 44);

			vm.Item = 42;
			Assert.IsTrue(dg.RowHeight == 42);
		}

		[TestMethod]
		public void RowHeightFromStyleWithbinding()
		{
			var vm = new SingleVM<int> {
				Item = 44
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.RowHeightProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.RowHeight == 44);

			vm.Item = 42;
			Assert.IsTrue(dg.RowHeight == 42);
		}
		#endregion
	}
}
