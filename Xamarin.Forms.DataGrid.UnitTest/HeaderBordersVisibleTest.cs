using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class HeaderBordersVisibleTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void HeaderBordersVisibleInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.HeaderBordersVisible);
		}

		[TestMethod]
		public void HeaderBordersVisibleFormStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBordersVisibleProperty, Value = false });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsFalse(dg.HeaderBordersVisible);
		}

		[TestMethod]
		public void HeaderBordersVisibleFromBinding()
		{
			var vm = new SingleVM<bool> {
				Item = false
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.HeaderBordersVisibleProperty, new Binding("Item", source: vm));

			Assert.IsFalse(dg.HeaderBordersVisible);

			vm.Item = true;
			Assert.IsTrue(dg.HeaderBordersVisible);
		}

		[TestMethod]
		public void HeaderBordersVisibleFromStyleWithBinding()
		{
			var vm = new SingleVM<bool> {
				Item = false
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBordersVisibleProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsFalse(dg.HeaderBordersVisible);

			vm.Item = true;
			Assert.IsTrue(dg.HeaderBordersVisible);
		}
		#endregion
	}
}
