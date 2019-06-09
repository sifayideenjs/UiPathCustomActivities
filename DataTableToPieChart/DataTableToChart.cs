using Microsoft.Office.Interop.Excel;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Etisalat.CIT.OPS.Robotics.Reporting
{
    public class DataTableToChart : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<System.Data.DataTable> DataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ChartName { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> MailBody { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public CHARTTYPE ChartType { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public bool AttachDataTable { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> MailSubject { get; set; }
        
        [Category("Output")]
        public OutArgument<MailMessage> MailMessage { get; set; }

        [Category("Output")]
        public OutArgument<string> TempFolder { get; set; }

        object missing = Type.Missing;

        protected override void Execute(CodeActivityContext context)
        {
            var dataTable = this.DataTable.Get(context);
            var chartName = this.ChartName.Get(context);
            var mailSubject = this.MailSubject.Get(context);
            var canAttachExcel = this.AttachDataTable;
            var mailBody = this.MailBody.Get(context);

            var tempDirectory = Path.GetTempPath();
            var tempFolderName = DateTime.Now.ToString("yyyyMMddHHmmssffff");
            var outputDirectory = Path.Combine(tempDirectory, tempFolderName);
            Directory.CreateDirectory(outputDirectory);

            Application excelApp = null;
            Workbook xlWorkbook = null;

            try
            {
                excelApp = new Application();
                xlWorkbook = excelApp.Workbooks.Add(1);

                Worksheet xlSheet = (Worksheet)xlWorkbook.Sheets.Add();
                xlSheet.Name = "DataSource";

                for (int r = 0; r < dataTable.Rows.Count; r++)
                {
                    for (int c = 0; c < dataTable.Columns.Count; c++)
                    {
                        if (r == 0)
                        {
                            xlSheet.Cells[1, c + 1] = dataTable.Columns[c].ColumnName.ToString();
                        }
                        xlSheet.Cells[r + 2, c + 1] = dataTable.Rows[r][c].ToString();
                    }
                }

                List<string> chartFilePaths = ExportToChart(dataTable, chartName, outputDirectory, xlSheet, xlWorkbook);

                string chartSourceDirectoryName = outputDirectory + @"\chart\sourcedata";
                string chartSourceFileName = "DataSource.xlsx";
                string excelFilePath = chartSourceDirectoryName + @"\" + chartSourceFileName;
                if (canAttachExcel)
                {
                    if (!Directory.Exists(chartSourceDirectoryName)) Directory.CreateDirectory(chartSourceDirectoryName);
                    Console.WriteLine(excelFilePath);

                    xlWorkbook.Saved = true;
                    xlWorkbook.SaveCopyAs(excelFilePath);
                }

                xlWorkbook.Close(true);
                excelApp.Quit();

                MailMessage mail = new MailMessage();
                mail.IsBodyHtml = true;
                mail.Subject = mailSubject;

                List<string> contentIds = CreateLinkedResourse(chartFilePaths, mail);
                
                string body = TextToHtml(mailBody);

                string charts = string.Empty;
                foreach(string contentId in contentIds)
                {
                    charts +=   "<span style='font-family:\"Neo Tech Alt\",sans-serif'>" +
                                "&nbsp;" +
                                "<img src=\"cid:" + contentId + "\">" +
                                "</span>";
                }

                mail.Body = "<htm><body>" +
                            "<p>" + body + "</p>" +
                            "<p>" + charts + "</p>" +
                            "<p><span style='font-family:\"Neo Tech Alt\",sans-serif'>Best Regards,</span></p>" +
                            "<p><span style = 'font-family:\"Neo Tech Alt\",sans-serif' > CIT Robotics Services</span></p>" +
                            "</body></html>";
                
                if (canAttachExcel)
                {
                    Attachment dtAttachment = new Attachment(excelFilePath);
                    mail.Attachments.Add(dtAttachment);
                }

                this.MailMessage.Set(context, mail);

                //try
                //{
                //    File.Delete(chartFilePath);
                //    File.Delete(excelFilePath);
                //    Directory.Delete(outputDirectory, true);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Clear Temp Files - " + ex.Message);
                //}

                this.TempFolder.Set(context, outputDirectory);
            }
            catch (Exception ex)
            {
                xlWorkbook.Saved = true;
                xlWorkbook.Close();
                excelApp.Quit();
                Console.WriteLine("Illegal permission - " + ex.Message);
            }
        }

        private List<string> CreateLinkedResourse(List<string> chartFilePaths, MailMessage mail)
        {
            List<string> contentIds = new List<string>();
            foreach (string chartFilePath in chartFilePaths)
            {
                string contentId = Path.GetFileName(chartFilePath);
                string htmlBody = "<html><body><h1>Picture</h1><br><img src=\"cid:{0}\"></body></html>";
                htmlBody = string.Format(htmlBody, contentId);
                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

                LinkedResource inline = new LinkedResource(chartFilePath, MediaTypeNames.Image.Jpeg);
                inline.ContentId = contentId;
                avHtml.LinkedResources.Add(inline);
                mail.AlternateViews.Add(avHtml);

                Attachment inlineChart = new Attachment(chartFilePath);
                mail.Attachments.Add(inlineChart);

                contentIds.Add(contentId);
            }
            return contentIds;
        }

        private List<string> ExportToChart(System.Data.DataTable dataTable, string chartName, string outputDirectory, Worksheet xlSheet, Workbook xlWorkbook)
        {
            List<string> chartFilePaths = new List<string>();
            switch(ChartType)
            {
                case CHARTTYPE.PIE:
                    {
                        Worksheet xlChartSheet = (Worksheet)xlWorkbook.Sheets.Add();
                        xlChartSheet.Name = "PieChart";
                        string chartFilePath = CreateChart(XlChartType.xlPie, dataTable, chartName, outputDirectory, xlSheet, xlChartSheet);
                        chartFilePaths.Add(chartFilePath);
                        break;
                    }
                case CHARTTYPE.BAR:
                    {
                        Worksheet xlChartSheet = (Worksheet)xlWorkbook.Sheets.Add();
                        xlChartSheet.Name = "BarChart";
                        string chartFilePath = CreateChart(XlChartType.xlColumnClustered, dataTable, chartName, outputDirectory, xlSheet, xlChartSheet);
                        chartFilePaths.Add(chartFilePath);
                        break;
                    }
                case CHARTTYPE.BOTH:
                    {
                        Worksheet xlChartSheet1 = (Worksheet)xlWorkbook.Sheets.Add();
                        xlChartSheet1.Name = "PieChart";
                        string chartFilePath1 = CreateChart(XlChartType.xlPie, dataTable, chartName, outputDirectory, xlSheet, xlChartSheet1);
                        chartFilePaths.Add(chartFilePath1);

                        Worksheet xlChartSheet2 = (Worksheet)xlWorkbook.Sheets.Add();
                        xlChartSheet2.Name = "BarChart";
                        string chartFilePath2 = CreateChart(XlChartType.xlColumnClustered, dataTable, chartName, outputDirectory, xlSheet, xlChartSheet2);
                        chartFilePaths.Add(chartFilePath2);
                        break;
                    }
            }

            return chartFilePaths;
        }

        private string CreateChart(XlChartType chartType, System.Data.DataTable dataTable, string chartName, string outputDirectory, Worksheet xlSheet, Worksheet xlChartSheet)
        {
            string chartFilePath = string.Empty;
            int rowIndex = dataTable.Rows.Count + 1;
            int columnIndex = dataTable.Columns.Count;

            ChartObjects xlCharts = (ChartObjects)xlChartSheet.ChartObjects(missing);
            ChartObject chartObj = xlCharts.Add(0, 0, 348, 268);
            Chart chart = chartObj.Chart;


            Range chartRange = xlSheet.Range[xlSheet.Cells[1, 1], xlSheet.Cells[rowIndex, columnIndex]];
            chart.SetSourceData(chartRange, missing);
            chart.ChartType = chartType;

            chart.ChartStyle = 261;
            chart.HasTitle = true;
            chart.ChartTitle.Text = chartName;

            //chart.ApplyDataLabels(XlDataLabelsType.xlDataLabelsShowPercent, XlDataLabelsType.xlDataLabelsShowNone, true, false, false, true, false, true);
            //chart.HasLegend = false;

            string chartDirectoryName = outputDirectory + @"\chart";
            string chartFileName = "DataChart" + chartType.ToString() + ".Jpeg";

            if (!Directory.Exists(chartDirectoryName)) Directory.CreateDirectory(chartDirectoryName);
            chartFilePath = chartDirectoryName + @"\" + chartFileName;
            Console.WriteLine(chartFilePath);

            chart.Export(chartFilePath, "JPEG", false);

            //chart.Delete();
            //chart = null;

            return chartFilePath;
        }

        private string TextToHtml(string text)
        {
            string result = string.Empty;
            //string[] splits = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            //string[] splits = text.Split(Environment.NewLine.ToCharArray());
            foreach (var line in text.SplitToLines())
            {
                result += "<span style='font-family:\"Neo Tech\",sans-serif'>" + line + " </span><br>";
            }

            return result;
        }

        //private void ConvertToExcel(DataSet dataSet)
        //{
        //    Application excelApp = null;
        //    Workbook xlWorkbook = null;

        //    try
        //    {
        //        excelApp = new Application();
        //        xlWorkbook = excelApp.Workbooks.Add(dataSet.Tables.Count);
        //        int sheetcount = 1;

        //        foreach (System.Data.DataTable dt in dataSet.Tables)
        //        {
        //            Worksheet xlSheet = (Worksheet)xlWorkbook.Sheets.Add();
        //            xlSheet.Name = "SheetData" + sheetcount.ToString();

        //            for (int j = 0; j < dt.Rows.Count; j++)
        //            {
        //                for (int i = 0; i < dt.Columns.Count; i++)
        //                {
        //                    if (j == 0)
        //                    {
        //                        xlSheet.Cells[1, i + 1] = dt.Columns[i].ColumnName.ToString();
        //                    }
        //                    xlSheet.Cells[j + 2, i + 1] = dt.Rows[j][i].ToString();
        //                }
        //            }

        //            sheetcount++;
        //        }

        //        xlWorkbook.Saved = true;
        //        xlWorkbook.SaveCopyAs("C:\\" + Guid.NewGuid().ToString() + ".xlsx");
        //        xlWorkbook.Close();
        //        excelApp.Quit();
        //    }
        //    catch (Exception ex)
        //    {
        //        xlWorkbook.Saved = true;
        //        xlWorkbook.Close();
        //        excelApp.Quit();
        //        Console.WriteLine("Illegal permission - " + ex.Message);
        //    }
        //}

        public enum CHARTTYPE
        {
            PIE = 0,
            BAR,
            BOTH
        }
    }
}
