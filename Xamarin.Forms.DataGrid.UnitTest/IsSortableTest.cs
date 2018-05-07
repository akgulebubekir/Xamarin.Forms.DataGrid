using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class IsSortableTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void IsSortableInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.IsSortable);

			dg.IsSortable = false;
			Assert.IsFalse(dg.IsSortable);

		}

		[TestMethod]
		public void IsSortableFormStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.IsSortableProperty, Value = false });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsFalse(dg.IsSortable);
		}

		[TestMethod]
		public void IsSortableFromBinding()
		{
			var vm = new SingleVM<bool> {
				Item = false
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.IsSortableProperty, new Binding("Item", source: vm));

			Assert.IsFalse(dg.IsSortable);

			vm.Item = true;
			Assert.IsTrue(dg.IsSortable);
		}

		[TestMethod]
		public void IsSortableFromStyleWithBinding()
		{
			var vm = new SingleVM<bool> {
				Item = false
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.IsSortableProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsFalse(dg.IsSortable);

			vm.Item = true;
			Assert.IsTrue(dg.IsSortable);
		}
		#endregion
	}
}
