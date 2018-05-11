using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Xamarin.Forms.DataGrid.UnitTest.Models;

namespace Xamarin.Forms.DataGrid.UnitTest.Common
{
	internal static class Util
	{
		internal static List<Team> GetTeams()
		{
			var json = File.ReadAllText("../../../DataGridSample/DataGridSample/teams.json");
			var teams = JsonConvert.DeserializeObject<List<Team>>(json);

			Assert.IsTrue(teams.Count == 15);
			return teams;
		}

		internal static ColumnCollection CreateColumns()
		{
			return new ColumnCollection {
				new DataGridColumn{PropertyName ="Name", Title = "Name"},
				new DataGridColumn{PropertyName ="Logo", Title = "Logo"},
				new DataGridColumn{PropertyName ="Win", Title = "Win"},
				new DataGridColumn{PropertyName ="Percentage", Title = "Percentage"},
				new DataGridColumn{PropertyName ="Div", Title = "Div"},
				new DataGridColumn{PropertyName ="Name", Title = "Name"},
			};
		}
	}
}
