using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class FontSizeTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Test
		[TestMethod]
		public void FontSizeInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.FontSize == 13.0);
		}

		[TestMethod]
		public void FontSizeFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.FontSizeProperty, Value = 14 });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.FontSize == 14);
		}

		[TestMethod]
		public void FontSizeFromBinding()
		{
			var vm = new SingleVM<double> {
				Item = 14.0
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.FontSizeProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.FontSize == 14);

			vm.Item = 15;
			Assert.IsTrue(dg.FontSize == 15);
		}

		[TestMethod]
		public void FontSizeFromBindingWithStyle()
		{
			var vm = new SingleVM<double> {
				Item = 14.0
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.FontSizeProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.FontSize == 14);

			vm.Item = 15;
			Assert.IsTrue(dg.FontSize == 15);
		}
		#endregion
	}
}
