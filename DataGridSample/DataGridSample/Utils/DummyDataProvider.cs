using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DataGridSample.Models;
using Newtonsoft.Json;

namespace DataGridSample.Utils
{
	internal static class DummyDataProvider
	{
		public static List<Team> GetTeams()
		{
			var assembly = typeof(DummyDataProvider).GetTypeInfo().Assembly;
			string json;

			using (var stream = assembly.GetManifestResourceStream("DataGridSample.teams.json"))
			{
				using (var reader = new StreamReader(stream))
				{
					json = reader.ReadToEnd();
				}
			}

			return JsonConvert.DeserializeObject<List<Team>>(json);
		}
	}
}