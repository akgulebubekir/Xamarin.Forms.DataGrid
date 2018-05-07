using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class FontFamilyTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void FontFamilyInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.FontFamily == Font.Default.FontFamily);
		}

		[TestMethod]
		public void FontFamilyFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.FontFamilyProperty, Value = "Arial" });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.FontFamily == "Arial");
		}

		[TestMethod]
		public void FontFamilyFromBinding()
		{
			var vm = new SingleVM<string> {
				Item = "Arial",
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.FontFamilyProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.FontFamily == "Arial");

			vm.Item = "Tahoma";
			Assert.IsTrue(dg.FontFamily == "Tahoma");
		}

		[TestMethod]
		public void FontFamilyFromBindingWithStyle()
		{
			var vm = new SingleVM<string> {
				Item = "Arial",
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.FontFamilyProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.FontFamily == "Arial");

			vm.Item = "Tahoma";
			Assert.IsTrue(dg.FontFamily == "Tahoma");
		}
		#endregion
	}
}
