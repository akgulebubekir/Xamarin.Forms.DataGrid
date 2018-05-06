using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class ColumnTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}
		#region Tests
		[TestMethod]
		public void ColumnsWithoutSet()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.Columns.Count == 0);
		}

		[TestMethod]
		public void ColumnsWithoutSetLoad()
		{
			var dg = new DataGrid();
			var view = new ContentView {
				Content = dg
			};

			Assert.IsTrue(dg.Columns.Count == 0);
		}

		[TestMethod]
		public void ColumnsWithBinding()
		{
			var columns = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ColumnsProperty, new Binding(".", source: columns));

			Assert.IsTrue(dg.Columns.Count == columns.Count);
		}

		[TestMethod]
		public void ColumnsWithBindingAndManipulated()
		{
			var vm = new SingleVM<ColumnCollection> {
				Item = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
				new DataGridColumn{Title = "C3", PropertyName="c3"},
				}
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ColumnsProperty, new Binding("Item", source: vm));

			Assert.IsTrue(dg.Columns.Count == 3);

			vm.Item = new ColumnCollection {
				new DataGridColumn{Title = "C1", PropertyName="c1"},
				new DataGridColumn{Title = "C2", PropertyName="c2"},
			};

			Assert.IsTrue(dg.Columns.Count == 2);
		}

		[TestMethod]
		public void ColumnsFromConverter()
		{
			var cList = new List<string> {
				"Name", "Logo", "Win", "Test"
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ColumnsProperty, new Binding(".", source: cList, converter: new ColumnConverter()));

			Assert.IsTrue(dg.Columns.Count == cList.Count);

			for (int i = 0; i < cList.Count; i++)
				Assert.IsTrue(dg.Columns[i].Title == cList[i]);

		}

		[TestMethod]
		public void ColumnsFromConverterWithManipulation()
		{
			var vm = new SingleVM<List<string>> {
				Item = new List<string> {
					"Name", "Logo", "Win", "Test"
				}
			};

			var dg = new DataGrid();
			dg.SetBinding(DataGrid.ColumnsProperty, new Binding("Item", source: vm, converter: new ColumnConverter()));

			Assert.IsTrue(dg.Columns.Count == vm.Item.Count);

			for (int i = 0; i < vm.Item.Count; i++)
				Assert.IsTrue(dg.Columns[i].Title == vm.Item[i]);

			vm.Item = new List<string> { "Col1", "Col2" };

			Assert.IsTrue(dg.Columns.Count == 2);
			Assert.IsTrue(dg.Columns[0].Title == "Col1" && dg.Columns[1].Title == "Col2");
		}
		#endregion

		#region  helper classes
		private class ColumnConverter : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{

				Assert.IsTrue(value != null && value is IEnumerable<string>);

				var cols = new ColumnCollection();
				var list = value as IEnumerable<string>;

				foreach (var i in list)
					cols.Add(new DataGridColumn { Title = i, PropertyName = i });

				return cols;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				throw new NotImplementedException();
			}
		}
		#endregion
	}
}
