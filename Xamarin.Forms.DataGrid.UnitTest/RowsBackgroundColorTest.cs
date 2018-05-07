using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.Common;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class RowsBackgroundColorTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void RowBackgroundColorInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.RowsBackgroundColorPalette == DataGrid.RowsBackgroundColorPaletteProperty.DefaultValue);
		}

		[TestMethod]
		public void RowsBackgroundColorCustomProvider()
		{
			var teams = Util.GetTeams();
			var palette = new PaletteProvider();

			//it will not hit below lines because of not displayed on UI. Needs to be handled on UI test
			palette.OnColorRequested += (i, o) => {

				Assert.IsTrue(i < teams.Count && teams.ElementAt(i) == o);
				return (i % 2 == 0) ? Color.Orange : Color.DarkOrange;
			};

			var dg = new DataGrid {
				RowsBackgroundColorPalette = palette,
				ItemsSource = teams,
			};

			Assert.IsTrue(dg.RowsBackgroundColorPalette == palette);
		}

		#endregion
	}
}
