namespace Xamarin.Forms.DataGrid
{
	public interface IColorProvider
	{
		Color GetColor(int rowIndex, object item);
	}
}
