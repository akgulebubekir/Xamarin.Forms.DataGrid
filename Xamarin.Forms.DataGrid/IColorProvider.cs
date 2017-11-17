using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xamarin.Forms.DataGrid
{
	public interface IColorProvider
	{
		Color GetColor(int rowIndex, object item);
	}
}
