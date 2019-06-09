using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace Etisalat.CIT.OPS.Robotics.Reporting
{
    public class DataTableToReport : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<System.Data.DataTable> DataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [EditorBrowsable]
        public InArgument<string> TemplateFilePath { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> OutputDirectory { get; set; }

        [Category("Output")]
        [RequiredArgument]
        public OutArgument<string> HtmlReport { get; set; }

        object missing = Type.Missing;

        protected override void Execute(CodeActivityContext context)
        {
            var dataTable = this.DataTable.Get(context);
            var templateFilePath = this.TemplateFilePath.Get(context);
            var outputDirectory = this.OutputDirectory.Get(context);

            if (File.Exists(templateFilePath))
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                var tempDirectory = Path.GetTempPath();
                var timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                tempDirectory = Path.Combine(tempDirectory, timeStamp);
                Directory.CreateDirectory(tempDirectory);

                var destFilePath = Path.Combine(tempDirectory, timeStamp + Path.GetExtension(templateFilePath));
                File.Copy(templateFilePath, destFilePath);

                var htmlFilePath = Path.Combine(tempDirectory, timeStamp + ".html");
                ConvertToExcelToHtml(dataTable, destFilePath, htmlFilePath);

                string html = File.ReadAllText(htmlFilePath);
                this.HtmlReport.Set(context, html);

                CopyFilesRecursively(new DirectoryInfo(tempDirectory), new DirectoryInfo(outputDirectory));

                try
                {
                    Directory.Delete(tempDirectory);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Unable to Delete Temp Folder - " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Template File Does not exist!");
            }
        }

        private void ConvertToExcelToHtml(DataTable dataTable, string excelfilePath, string htmlFilePath)
        {
            ExcelFile excelFile = new ExcelFile(excelfilePath);
            excelFile.RowNumber = 2;

            try
            {
                excelFile.OpenExcel();

                for (int r = 0; r < dataTable.Rows.Count; r++)
                {
                    excelFile.AddDataToExcel(dataTable.Rows[r]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Illegal permission - " + ex.Message);
            }
            finally
            {
                excelFile.SaveAsHtml(htmlFilePath);
                excelFile.CloseExcel();
            }
        }

        private void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
    }
}
