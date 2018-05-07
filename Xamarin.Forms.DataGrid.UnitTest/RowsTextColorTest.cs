using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.Common;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class RowsTextColorTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void RowsTextColorInit()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.RowsTextColorPalette == DataGrid.RowsTextColorPaletteProperty.DefaultValue);
		}

		[TestMethod]
		public void RowsTextColorCustomProvider()
		{
			var teams = Util.GetTeams();
			var palette = new PaletteProvider();

			//it will not hit below lines because of not displayed on UI. Needs to be handled on UI test
			palette.OnColorRequested += (i, o) => {

				Assert.IsTrue(i < teams.Count && teams.ElementAt(i) == o);
				return (i % 2 == 0) ? Color.Black : Color.DarkGray;
			};

			var dg = new DataGrid {
				RowsTextColorPalette = palette,
				ItemsSource = teams,
			};

			Assert.IsTrue(dg.RowsTextColorPalette == palette);
		}

		#endregion
	}
}
