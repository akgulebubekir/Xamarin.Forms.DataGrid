using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class SelectionEnabledTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void SelectionEnabledInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.SelectionEnabled);
		}

		[TestMethod]
		public void SelectionEnabledFormStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.SelectionEnabledProperty, Value = false });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsFalse(dg.SelectionEnabled);
		}

		[TestMethod]
		public void SelectionEnabledFromBinding()
		{
			var vm = new SingleVM<bool> {
				Item = false
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.SelectionEnabledProperty, new Binding("Item", source: vm));

			Assert.IsFalse(dg.SelectionEnabled);

			vm.Item = true;
			Assert.IsTrue(dg.SelectionEnabled);
		}

		[TestMethod]
		public void SelectionEnabledFromStyleWithBinding()
		{
			var vm = new SingleVM<bool> {
				Item = false
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.SelectionEnabledProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsFalse(dg.SelectionEnabled);

			vm.Item = true;
			Assert.IsTrue(dg.SelectionEnabled);
		}
		#endregion
	}
}
