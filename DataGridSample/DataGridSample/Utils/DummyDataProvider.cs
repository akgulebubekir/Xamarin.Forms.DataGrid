using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DataGridSample.Models;
using Newtonsoft.Json;

namespace DataGridSample.Utils
{
	static class DummyDataProvider
	{
		public static List<Team> GetTeams()
		{
			var assembly = typeof(DummyDataProvider).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("DataGridSample.teams.json");
			string json = string.Empty;

			using (var reader = new StreamReader(stream))
			{
				json = reader.ReadToEnd();
			}

			return JsonConvert.DeserializeObject<List<Team>>(json);
		}
	}
}
