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

        public static ChartTable GetChartTable()
        {
            var chartTable = new ChartTable();
            chartTable.Header = new ChartTableRow
            {
                Columns = new []{ new ChartTableColumn{Value="Header_Col0"}, new ChartTableColumn { Value = "Header_Col1" }}
            };
            chartTable.Rows = new []
            {
                new ChartTableRow{ Columns = new []{ new ChartTableColumn{Value= "Row0_Col0" }, new ChartTableColumn { Value = "Row0_Col1" } }},
                new ChartTableRow{ Columns = new []{ new ChartTableColumn{Value= "Row1_Col0" }, new ChartTableColumn { Value = "Row1_Col1" } }},
                new ChartTableRow{ Columns = new []{ new ChartTableColumn{Value= "Row2_Col0" }, new ChartTableColumn { Value = "Row2_Col1" } }},
            };

            return chartTable;
        }
    }
}
