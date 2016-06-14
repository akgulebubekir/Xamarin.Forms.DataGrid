using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Newtonsoft.Json;

using DataGridSample.Models;

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
