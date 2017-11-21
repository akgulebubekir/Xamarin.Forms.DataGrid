using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DataGridSample.Models
{
    [DebuggerDisplay("ChartTable: Rows={this.Rows.Length}, Columns={this.Header.Columns.Length}")]
    public class ChartTable
    {
        public ChartTable()
        {
            this.Header = null;
        }

        public ChartTableRow Header { get; set; }

        public ChartTableRow[] Rows { get; set; }
    }

    [DebuggerDisplay("ChartTableRow: Columns={this.Columns.Length}")]
    public class ChartTableRow
    {
        public ChartTableRow()
        {
            this.Columns = new ChartTableColumn[] { };
        }

        public ChartTableColumn[] Columns { get; set; }
    }

    [DebuggerDisplay("ChartTableColumn: Value={this.Value}")]
    public class ChartTableColumn
    {
        public object Value { get; set; }
    }
}