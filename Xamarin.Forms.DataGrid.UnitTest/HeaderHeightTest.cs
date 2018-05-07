using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class HeaderHeightTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void HeaderHeightInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.HeaderHeight == 40);
		}

		[TestMethod]
		public void HeaderHeightFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderHeightProperty, Value = 44 });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.HeaderHeight == 44);
		}

		[TestMethod]
		public void HeaderHeightFromBinding()
		{
			var vm = new SingleVM<int> {
				Item = 44
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.HeaderHeightProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.HeaderHeight == 44);

			vm.Item = 42;
			Assert.IsTrue(dg.HeaderHeight == 42);
		}

		[TestMethod]
		public void HeaderHeightFromStyleWithBinding()
		{
			var vm = new SingleVM<int> {
				Item = 44
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderHeightProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.HeaderHeight == 44);

			vm.Item = 42;
			Assert.IsTrue(dg.HeaderHeight == 42);
		}
		#endregion
	}
}
