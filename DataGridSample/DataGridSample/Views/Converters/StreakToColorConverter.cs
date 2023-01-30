using System;
using System.Globalization;
using DataGridSample.Models;
using Xamarin.Forms;

namespace DataGridSample.Views.Converters
{
  public class StreakToColorConverter : IValueConverter
  {
    public static string[] WinStreakColors = new[]
      { "#CEF6CE", "#A9F5A9", "#81F781", "#58FA58", "#2EFE2E", "#00FF00", "#01DF01" };

    public static string[] LooseStreakColors = new[]
      { "#F5A9A9", "#F78181", "#FA5858", "#FE2E2E", "#FF0000", "#DF0101", "8A0808" };

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value == null)
      {
        return Color.Transparent;
      }

      else
      {
        if (value is Streak s)
        {
          if (s.Result == Result.Win)
            return Color.FromHex(WinStreakColors.Length > s.NumStreak
              ? WinStreakColors[s.NumStreak]
              : WinStreakColors[WinStreakColors.Length - 1]);

          return Color.FromHex(LooseStreakColors.Length > s.NumStreak
            ? LooseStreakColors[s.NumStreak]
            : LooseStreakColors[LooseStreakColors.Length - 1]);
        }

        return Color.Orange;
      }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}