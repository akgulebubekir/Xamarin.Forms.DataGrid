namespace Xamarin.Forms.DataGrid
{
	public class SortDataTypeConverter : TypeConverter
	{
		public override object ConvertFromInvariantString(string value)
		{
			if (int.TryParse(value, out var index))
				return (SortData)index;
			return null;
		}
	}
}
