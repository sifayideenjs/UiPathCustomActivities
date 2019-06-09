using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;

namespace Etisalat.CIT.OPS.Robotics.Reporting
{
    public class DataTableToHtmlTable : CodeActivity
    {
        [Category("Input"), RequiredArgument]
        public InArgument<System.Data.DataTable> DataTable { get; set; }

        [Category("Output")]
        public OutArgument<string> HtmlTable { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            try
            {
                System.Data.DataTable dataTable = this.DataTable.Get(context);
                string htmlTable = ExportDatatableToHtml(dataTable);
                this.HtmlTable.Set(context, htmlTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string ExportDatatableToHtml(System.Data.DataTable dataTable)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<style type='text/css'>.TFtable{width:100%;border-collapse:collapse;}.TFtable td{padding:7px;border:#4e95f4 1px solid;}.TFtable tr{background: #b8d1f3;}.TFtable tr:nth - child(odd){background:#b8d1f3;}.TFtable tr:nth - child(even){background: #dae5f4;}</style>");
            strHTMLBuilder.Append("<table border='1px' cellpadding='1' cellspacing='1' class='TFtable' style='font-family:Arial; font-size:small;'>");
            strHTMLBuilder.Append("<tr valign='top'>");

            foreach (DataColumn dataColumn in dataTable.Columns)
            {
                strHTMLBuilder.Append("<td><b>");
                strHTMLBuilder.Append(dataColumn.ColumnName);
                strHTMLBuilder.Append("</b></td>");
            }

            strHTMLBuilder.Append("</tr>");

            int currRow = 1;
            foreach (DataRow dataRow in dataTable.Rows)
            {

                strHTMLBuilder.Append("<tr style='" + (currRow % 2 == 0 ? "background-color:white" : "background-color:lightgreen") + "' valign='top'>");
                foreach (DataColumn dataColumn in dataTable.Columns)
                {
                    strHTMLBuilder.Append("<td>");
                    strHTMLBuilder.Append(dataRow[dataColumn.ColumnName].ToString());
                    strHTMLBuilder.Append("</td>");

                }
                strHTMLBuilder.Append("</tr>");
                currRow++;
            }

            strHTMLBuilder.Append("</table>");
            string Htmltext = strHTMLBuilder.ToString();

            return Htmltext;
        }
    }
}
