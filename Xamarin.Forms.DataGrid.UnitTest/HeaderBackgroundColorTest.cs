using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	[TestCategory("HeaderBackgroundColor")]
	public class HeaderBackgroundColorTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Test
		[TestMethod]
		public void HeaderBackgroundColorInit()
		{
			var dg = new DataGrid();
			Assert.IsTrue(dg.HeaderBackground == (Color)DataGrid.HeaderBackgroundProperty.DefaultValue);

			dg.HeaderBackground = Color.Orange;
			Assert.IsTrue(dg.HeaderBackground == Color.Orange);
		}

		[TestMethod]
		public void HeaderBackgroundColorWithBinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Orange
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.HeaderBackgroundProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.HeaderBackground == Color.Orange);

			vm.Item = Color.Red;
			Assert.IsTrue(dg.HeaderBackground == Color.Red);
		}

		[TestMethod]
		public void HeaderBackgroundColorFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBackgroundProperty, Value = Color.Orange });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.HeaderBackground == Color.Orange);
		}

		[TestMethod]
		public void HeaderBackgroundColorFromStyleWithbinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Orange
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBackgroundProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.HeaderBackground == Color.Orange);

			vm.Item = Color.Red;
			Assert.IsTrue(dg.HeaderBackground == Color.Red);
		}
		#endregion
	}
}
