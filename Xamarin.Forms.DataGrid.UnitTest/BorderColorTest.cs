using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class BorderColorTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Test
		[TestMethod]
		public void BorderColorInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.BorderColor == (Color)DataGrid.BorderColorProperty.DefaultValue);

			dg.BorderColor = Color.Gray;

			Assert.IsTrue(dg.BorderColor == Color.Gray);
		}

		[TestMethod]
		public void BorderColorWithBinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Gray
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.BorderColorProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.BorderColor == Color.Gray);

			vm.Item = Color.Black;
			Assert.IsTrue(dg.BorderColor == Color.Black);
		}

		[TestMethod]
		public void BorderColorFromStyle()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.BorderColorProperty, Value = Color.Gray });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.BorderColor == Color.Gray);
		}

		[TestMethod]
		public void BorderColorFromStyleWithBinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Gray
			};

			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.BorderColorProperty, Value = new Binding("Item", source: vm) });

			var dg = new DataGrid {
				Style = style
			};

			Assert.IsTrue(dg.BorderColor == Color.Gray);

			vm.Item = Color.Black;
			Assert.IsTrue(dg.BorderColor == Color.Black);
		}
		#endregion
	}
}
