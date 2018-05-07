using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class BorderThicknessTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void BorderThicknessInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.BorderThickness == new Thickness(1));
		}

		[TestMethod]
		public void BorderThicknessFormStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.BorderThicknessProperty, Value = new Thickness(3) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.BorderThickness == 3);
		}

		[TestMethod]
		public void BorderThicknessFromBinding()
		{
			var vm = new SingleVM<Thickness> {
				Item = new Thickness(3)
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.BorderThicknessProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.BorderThickness == 3);

			vm.Item = 5;
			Assert.IsTrue(dg.BorderThickness == 5);
		}

		[TestMethod]
		public void BorderThicknessFromStyleWithBinding()
		{
			var vm = new SingleVM<Thickness> {
				Item = 3
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.BorderThicknessProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.BorderThickness == 3);

			vm.Item = 5;
			Assert.IsTrue(dg.BorderThickness == 5);
		}
		#endregion
	}
}
