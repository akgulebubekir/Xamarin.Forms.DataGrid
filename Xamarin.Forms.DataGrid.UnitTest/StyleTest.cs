using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamarin.Forms.DataGrid.UnitTest.VM;

namespace Xamarin.Forms.DataGrid.UnitTest
{
	[TestClass]
	public class StyleTest
	{
		[ClassInitialize]
		public static void Init(TestContext context)
		{
			MockPlatform.MockForms.Init();
		}

		[TestMethod]
		public void StyleWithoutAny()
		{
			var dg = new DataGrid();

			Assert.IsTrue(dg.ActiveRowColor == (Color)DataGrid.ActiveRowColorProperty.DefaultValue);
			Assert.IsTrue(dg.BorderColor == (Color)DataGrid.BorderColorProperty.DefaultValue);
			Assert.IsTrue(dg.HeaderBackground == (Color)DataGrid.HeaderBackgroundProperty.DefaultValue);
			Assert.IsTrue(dg.FontSize == (double)DataGrid.FontSizeProperty.DefaultValue);
			Assert.IsTrue(dg.BorderThickness == (Thickness)DataGrid.BorderThicknessProperty.DefaultValue);
			Assert.IsTrue(dg.HeaderBordersVisible == (bool)DataGrid.HeaderBordersVisibleProperty.DefaultValue);
			Assert.IsTrue(dg.HeaderHeight == (int)DataGrid.HeaderHeightProperty.DefaultValue);
			Assert.IsTrue(dg.IsSortable == (bool)DataGrid.IsSortableProperty.DefaultValue);
			Assert.IsTrue(dg.SortedColumnIndex == (SortData)DataGrid.SortedColumnIndexProperty.DefaultValue);

		}

		[TestMethod]
		public void StyleWithHeaderColors()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBackgroundProperty, Value = Color.Orange });

			var dg = new DataGrid() { Style = style };

			Assert.IsTrue(dg.HeaderBackground == Color.Orange);
		}

		[TestMethod]
		public void StyleWithHeader()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBackgroundProperty, Value = Color.Orange });
			style.Setters.Add(new Setter { Property = DataGrid.HeaderBordersVisibleProperty, Value = false });
			style.Setters.Add(new Setter { Property = DataGrid.HeaderHeightProperty, Value = 24 });

			var dg = new DataGrid { Style = style };

			Assert.IsTrue(dg.HeaderBackground == Color.Orange);
			Assert.IsTrue(dg.HeaderBordersVisible == false);
			Assert.IsTrue(dg.HeaderHeight == 24);
		}

		[TestMethod]
		public void StyleWithSortingIcons()
		{
			var style = new Style(typeof(DataGrid));
			style.Setters.Add(new Setter { Property = DataGrid.AscendingIconProperty, Value = "test_asc.png" });
			style.Setters.Add(new Setter { Property = DataGrid.DescendingIconProperty, Value = "test_desc.png" });

			var dg = new DataGrid { Style = style };

			Assert.IsTrue(dg.AscendingIcon is FileImageSource);
			Assert.IsTrue(dg.DescendingIcon is FileImageSource);

			Assert.IsTrue((dg.AscendingIcon as FileImageSource).File == "test_asc.png");
			Assert.IsTrue((dg.DescendingIcon as FileImageSource).File == "test_desc.png");
		}

		[TestMethod]
		public void StyleWithSortingIconStyle()
		{
			var ascStyle = new Style(typeof(Image));
			ascStyle.Setters.Add(new Setter { Property = Image.SourceProperty, Value = "test_asc.png" });

			var descStyle = new Style(typeof(Image));
			descStyle.Setters.Add(new Setter { Property = Image.SourceProperty, Value = "test_desc.png" });

			var dg = new DataGrid {
				AscendingIconStyle = ascStyle,
				DescendingIconStyle = descStyle
			};

			Assert.IsTrue(dg.AscendingIcon is FileImageSource);
			Assert.IsTrue(dg.DescendingIcon is FileImageSource);

			Assert.IsTrue((dg.AscendingIcon as FileImageSource).File == "test_asc.png");
			Assert.IsTrue((dg.DescendingIcon as FileImageSource).File == "test_desc.png");
		}

		[TestMethod]
		public void StyleWithSortingIconStyleInStyle()
		{
			var ascStyle = new Style(typeof(Image));
			ascStyle.Setters.Add(new Setter { Property = Image.SourceProperty, Value = "test_asc.png" });

			var descStyle = new Style(typeof(Image));
			descStyle.Setters.Add(new Setter { Property = Image.SourceProperty, Value = "test_desc.png" });

			var dgStyle = new Style(typeof(DataGrid));
			dgStyle.Setters.Add(new Setter { Property = DataGrid.AscendingIconStyleProperty, Value = ascStyle });
			dgStyle.Setters.Add(new Setter { Property = DataGrid.DescendingIconStyleProperty, Value = descStyle });

			var dg = new DataGrid {
				Style = dgStyle
			};

			Assert.IsTrue(dg.AscendingIcon is FileImageSource);
			Assert.IsTrue(dg.DescendingIcon is FileImageSource);

			Assert.IsTrue((dg.AscendingIcon as FileImageSource).File == "test_asc.png");
			Assert.IsTrue((dg.DescendingIcon as FileImageSource).File == "test_desc.png");
		}

		[TestMethod]
		public void StyleWithHeight()
		{
			var dgStyle = new Style(typeof(DataGrid));
			dgStyle.Setters.Add(new Setter { Property = DataGrid.RowHeightProperty, Value = 22 });

			var dg = new DataGrid {
				Style = dgStyle
			};

			Assert.IsTrue(dg.RowHeight == 22);
		}

		[TestMethod]
		public void StyleRowHeightWithBinding()
		{
			var vm = new SingleVM<int> {
				Item = 44
			};

			var intbinding = new Binding("Item", source: vm);

			var dgStyle = new Style(typeof(DataGrid));
			dgStyle.Setters.Add(new Setter { Property = DataGrid.RowHeightProperty, Value = intbinding });

			var dg = new DataGrid {
				Style = dgStyle
			};

			Assert.IsTrue(dg.RowHeight == 44);

			vm.Item = 40;
			Assert.IsTrue(dg.RowHeight == 40);
		}

		[TestMethod]
		public void StyleHeaderHeightWithBinding()
		{
			var vm = new SingleVM<int> {
				Item = 44
			};

			var intbinding = new Binding("Item", source: vm);

			var dgStyle = new Style(typeof(DataGrid));
			dgStyle.Setters.Add(new Setter { Property = DataGrid.HeaderHeightProperty, Value = intbinding });

			var dg = new DataGrid {
				Style = dgStyle
			};

			Assert.IsTrue(dg.HeaderHeight == 44);

			vm.Item = 40;
			Assert.IsTrue(dg.HeaderHeight == 40);
		}

		[TestMethod]
		public void StyleBorderColorWithBinding()
		{
			var vm = new SingleVM<Color> {
				Item = Color.Red
			};

			var itemBinding = new Binding("Item", source: vm);

			var dgStyle = new Style(typeof(DataGrid));
			dgStyle.Setters.Add(new Setter { Property = DataGrid.BorderColorProperty, Value = itemBinding });

			var dg = new DataGrid {
				Style = dgStyle
			};

			Assert.IsTrue(dg.BorderColor == Color.Red);

			vm.Item = Color.Orange;
			Assert.IsTrue(dg.BorderColor == Color.Orange);
		}

		public void StyleWithColumns()
		{
			Assert.Fail("Not implemented");
		}
	}
}
