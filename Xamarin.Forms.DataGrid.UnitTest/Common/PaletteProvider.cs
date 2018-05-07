namespace Xamarin.Forms.DataGrid.UnitTest.Common
{
	delegate Color GetColorDelegate(int index, object item);
	public class PaletteProvider : IColorProvider
	{
		internal event GetColorDelegate OnColorRequested;

		public Color GetColor(int rowIndex, object item)
		{
			return OnColorRequested(rowIndex, item);
		}
	}
}
