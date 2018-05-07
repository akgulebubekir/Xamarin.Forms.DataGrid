using System;
using System.Globalization;
using System.Linq;

using Xamarin.Forms;

namespace DataGridSample.Views.Converters
{
	public class StreakToColorConverter : IValueConverter
	{
		public static string[] WinStreakColors = new string[] { "#CEF6CE", "#A9F5A9", "#81F781", "#58FA58", "#2EFE2E", "#00FF00", "#01DF01" };
		public static string[] LooseStreakColors = new string[] { "#F5A9A9", "#F78181", "#FA5858", "#FE2E2E", "#FF0000", "#DF0101", "8A0808" };

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return Color.Transparent;

			string val = value.ToString();

			var tokens = val.Split(' ');
			if (tokens.Length != 2)
				throw new ArgumentException("incorrect streak format");

			int numStreak;
			if (!int.TryParse(tokens[1], out numStreak))
				throw new ArgumentException("incorrect streak format");

			if (tokens.First() == "W")
				return Color.FromHex((WinStreakColors.Length > numStreak) ? WinStreakColors[numStreak] : WinStreakColors[WinStreakColors.Length - 1]);
			else
				return Color.FromHex((LooseStreakColors.Length > numStreak) ? LooseStreakColors[numStreak] : LooseStreakColors[LooseStreakColors.Length - 1]);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
