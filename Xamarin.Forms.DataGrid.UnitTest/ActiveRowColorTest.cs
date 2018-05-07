using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class ActiveRowColorTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void ActiveRowColorOnInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.ActiveRowColor == (Color)DataGrid.ActiveRowColorProperty.DefaultValue);
		}

		[TestMethod]
		public void ActiveRowColorBinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Orange
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ActiveRowColorProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.ActiveRowColor == vm.Item);
		}

		[TestMethod]
		public void ActiveRowColorBindingUpdate()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Red
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ActiveRowColorProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.ActiveRowColor == Color.Red);
			vm.Item = Color.Orange;
			Assert.IsTrue(dg.ActiveRowColor == Color.Orange);
		}

		[TestMethod]
		public void ActiveRowColorWithoutSelectionEnabled()
		{
			var dg = new DataGrid {
				SelectionEnabled = false,
			};

			Assert.ThrowsException<InvalidOperationException>(() => dg.ActiveRowColor = Color.Orange);
		}

		[TestMethod]
		public void ActiveRowColorFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.ActiveRowColorProperty, Value = Color.Orange });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.ActiveRowColor == Color.Orange);
		}

		[TestMethod]
		public void ActiveRowColorFromStyleWithBinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Orange
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.ActiveRowColorProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.ActiveRowColor == Color.Orange);

			vm.Item = Color.Red;
			Assert.IsTrue(dg.ActiveRowColor == Color.Red);

		}
		#endregion
	}
}
