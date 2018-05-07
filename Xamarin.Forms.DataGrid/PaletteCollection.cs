using System.Collections.Generic;
using System.Linq;

namespace Xamarin.Forms.DataGrid
{
	public sealed class PaletteCollection : List<Color>, IColorProvider
	{
		public Color GetColor(int rowIndex, object item)
		{
			if (Count > 0)
				return this.ElementAt(rowIndex % Count);
			else
				return default(Color);
		}
	}
}
