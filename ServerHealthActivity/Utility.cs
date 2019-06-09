using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.ServerHealthActivity
{
    public static class Utility
    {
        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(GetStringWithSpace(prop.Name), type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static List<T> DataTableToList<T>(DataTable dataTable)
        {
            List<T> dataList = new List<T>();
            if (dataTable != null)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    T item = GetItem<T>(row);
                    dataList.Add(item);
                }
            }
            return dataList;
        }

        public static string DataTableToHtml(DataTable dataTable)
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

        private static T GetItem<T>(DataRow dataRow)
        {
            Type temp = typeof(T);
            T item = Activator.CreateInstance<T>();

            foreach (DataColumn column in dataRow.Table.Columns)
            {
                var xx = dataRow[column.ColumnName];
                if (dataRow[column.ColumnName] == DBNull.Value) continue;
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(item, dataRow[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return item;
        }

        private static string GetStringWithSpace(string inputText)
        {
            StringBuilder outputText = new StringBuilder();

            foreach (char letter in inputText)
            {
                if (Char.IsUpper(letter) && outputText.Length > 0)
                    outputText.Append(" " + letter);
                else
                    outputText.Append(letter);
            }

            return outputText.ToString();
        }
    }
}
