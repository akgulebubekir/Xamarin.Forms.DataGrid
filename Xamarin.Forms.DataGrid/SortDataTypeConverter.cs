using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.DataGrid
{
	public class SortDataTypeConverter : TypeConverter
	{

		public override bool CanConvertFrom(Type sourceType)
		{
			return base.CanConvertFrom(sourceType);
		}

		public override object ConvertFromInvariantString(string value)
		{
			int index = 0;

			if (int.TryParse(value, out index))
				return (SortData)index;
			else
				return null;
		}
	}
}
