using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class NoDataViewTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		#region Tests
		[TestMethod]
		public void NoDataViewInit()
		{
			var dg = new DataGrid();

			var label = new Label {
				Text = "noData"
			};

			Assert.IsTrue(dg.NoDataView == null);

			dg.NoDataView = label;
			Assert.IsTrue(dg.NoDataView == label);
		}
		#endregion

	}
}
