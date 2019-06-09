using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data;

namespace Etisalat.CIT.OPS.Robotics.Reporting
{
    public class ExcelFile
    {
        private string excelFilePath = string.Empty;
        private int rowNumber = 1;

        Excel.Application excelApplication;
        Excel.Workbook excelWorkbook;
        Excel.Worksheet excelWorkSheet;

        public ExcelFile(string filePath)
        {
            ExcelFilePath = filePath;
        }

        public string ExcelFilePath
        {
            get { return excelFilePath; }
            private set { excelFilePath = value; }
        }

        public int RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        public void OpenExcel()
        {
            excelApplication = null;

            excelApplication = new Excel.Application();
            excelApplication.Visible = false;
            excelApplication.DisplayAlerts = false;

            excelWorkbook = (Excel.Workbook)(excelApplication.Workbooks._Open(excelFilePath, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
               System.Reflection.Missing.Value, System.Reflection.Missing.Value));
            
            excelWorkSheet = (Excel.Worksheet)excelWorkbook.Worksheets[1];
            excelWorkSheet.Name = "Report";
        }

        public void AddDataToExcel(DataRow dataRow)
        {
            int columnNumber = 1;
            foreach (object item in dataRow.ItemArray)
            {
                excelWorkSheet.Cells[rowNumber, columnNumber] = item;
                columnNumber++;
            }
            rowNumber++;
        }

        public void SaveAsHtml(string htmlFilePath)
        {
            try
            {
                if (excelWorkbook != null)
                {
                    excelWorkbook.SaveAs(htmlFilePath, Excel.XlFileFormat.xlHtml);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("ERROR - " + ex.Message);
            }
        }

        public void CloseExcel()
        {
            try
            {
                if(excelWorkbook != null)
                {
                    excelWorkbook.SaveAs(excelFilePath, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                   System.Reflection.Missing.Value, System.Reflection.Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange,
                                                   System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                                   System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                    excelWorkbook.Close(true, excelFilePath, System.Reflection.Missing.Value);
                }
            }
            finally
            {
                if (excelApplication != null)
                {
                    excelApplication.Quit();
                }
            }
        }
    }
}
