using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.Common.Util.Reports;
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201602;
using Google.Api.Ads.Dfp.v201602;
using Newtonsoft.Json;
using System.Xml;
using Google.Api.Ads.Common.Util;
using System.Text;

namespace AdBoostDashboard
{
    public class ReportDfp
    {
        DfpUser user;
        
        public DataTable Report(long vOrderId)
        {
            string vDatos = "";
            DataTable dataTable = new DataTable();
            try
            {
                user = new DfpUser();
                ReportService reportService = (ReportService)user.GetService(DfpService.v201602.ReportService);
                String filePath = "C:\\archivo\\report.xml";
                //long orderId = 301023708;
                //long orderId = 302108868;
                String advertiserId = "62699388";
                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("ORDER_ID = :id")
                    .AddValue("id", vOrderId);
               /* StatementBuilder statementBuilder = new StatementBuilder()
                .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                .OrderBy("id ASC")                
                .AddValue("advertiserId", advertiserId)
                .AddValue("isArchived", false);*/

                // Create report job.
                /*ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_ID, Dimension.ORDER_NAME, Dimension.DAY,Dimension.DATE};
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                DimensionAttribute.ORDER_TRAFFICKER, DimensionAttribute.ORDER_START_DATE_TIME,
                DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR, Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM};*/
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_NAME, Dimension.ORDER_ID, Dimension.DATE };
                //reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                //DimensionAttribute.ORDER_START_DATE_TIME, DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR};

                 // Set a custom date range 
                //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;

                //reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                System.DateTime endDateTime = System.DateTime.Now;
                reportJob.reportQuery.startDate =
                  DateTimeUtilities.FromDateTime(endDateTime.AddDays(-20), "America/New_York").date;
                reportJob.reportQuery.endDate =
                  DateTimeUtilities.FromDateTime(endDateTime, "America/New_York").date;
                //America/Tegucigalpa
                
                reportJob.reportQuery.statement = statementBuilder.ToStatement();
                //List<string> Filas = new List<string>();
                //string columns = reportJob.reportQuery.
                //Run the Report

                reportJob = reportService.runReportJob(reportJob);                
                //XmlReader reader;
                //reportService.ResponseHeader.ReadXml(reader);
                //Obtener la Url y la data
                //reportService.getReportJobStatus(reportJob.id);
                int progress = 0;
                while (reportService.getReportJobStatus(reportJob.id) == ReportJobStatus.IN_PROGRESS)
                {
                    progress = 1;
                }
                string url = reportService.getReportDownloadURL(reportJob.id, ExportFormat.CSV_DUMP);
                byte[] gzipReport = MediaUtilities.GetAssetDataFromUrl(url);
                string reportContents = Encoding.UTF8.GetString(MediaUtilities.DeflateGZipData(gzipReport));
                //List<string> result = reportContents.Split(',').ToList();
                string[] vLineresult = reportContents.Split('\n');
                string result="";
                string[] vSplitResult = {};
                DataTable table = new DataTable();
                table.Columns.Add("order", typeof(string));
                table.Columns.Add("date", typeof(string));
                table.Columns.Add("impre", typeof(string));
                table.Columns.Add("clics", typeof(string));
                table.Columns.Add("ctr", typeof(string));
                table.Columns.Add("Name", typeof(string));
                for (int i = 1; i < vLineresult.Length; i++)
                {
                    result = vLineresult[i];
                    vSplitResult = result.Split(',');
                    if (result != "")
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[0], vSplitResult[1], vSplitResult[2], vSplitResult[3], vSplitResult[4], vSplitResult[5] };
                        table.Rows.Add(dataRow);
                    }
                    /*for (int j = 0; j < vSplitResult.Length; j++)
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[j], vSplitResult[j+1], vSplitResult[j+2], vSplitResult[j+3], vSplitResult[j+4] };                    
                        table.Rows.Add(dataRow); 
                    }*/
                    /*DataRow dataRow = table.NewRow();
                    dataRow.ItemArray = new object[] { vLineresult[i] };
                    
                    table.Rows.Add(dataRow);                    */
                }
                dataTable = table.Clone();
                dataTable = table;
                
                /*ReportUtilities reportUtilities = new ReportUtilities(reportService, reportJob.id);                
                // Set download options.
                ReportDownloadOptions options = new ReportDownloadOptions();
                options.exportFormat = ExportFormat.XML;
                options.useGzipCompression = false;
                reportUtilities.reportDownloadOptions = options;*/
                
                 //Download the report.
                /*using (ReportResponse reportResponse = reportUtilities.GetResponse())
                {                   
                    reportResponse.Save(filePath);
                }*/
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return dataTable;
        }
        public DataTable ReportbyLine(long vLineID,string filter)
        {
            string vDatos = "";
            DataTable dataTable = new DataTable();
            try
            {
                user = new DfpUser();
                ReportService reportService = (ReportService)user.GetService(DfpService.v201602.ReportService);
                String filePath = "C:\\archivo\\report.xml";
                //long orderId = 301023708;
                //long orderId = 302108868;
                String advertiserId = "62699388";
                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("LINE_ITEM_ID = :id")
                    .AddValue("id", vLineID);
                /* StatementBuilder statementBuilder = new StatementBuilder()
                 .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                 .OrderBy("id ASC")                
                 .AddValue("advertiserId", advertiserId)
                 .AddValue("isArchived", false);*/

                // Create report job.
                /*ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_ID, Dimension.ORDER_NAME, Dimension.DAY,Dimension.DATE};
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                DimensionAttribute.ORDER_TRAFFICKER, DimensionAttribute.ORDER_START_DATE_TIME,
                DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR, Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM};*/
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.LINE_ITEM_NAME, Dimension.LINE_ITEM_ID, Dimension.DATE };
                //reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                //DimensionAttribute.ORDER_START_DATE_TIME, DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR};

                // Set a custom date range 
                //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                System.DateTime endDateTime = System.DateTime.Now;
                //reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                switch (filter)
                {
                    case "1":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;
                        break;
                    case "2":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        break;
                    case "3":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                        reportJob.reportQuery.dateRangeType = DateRangeType.TODAY;
                        /*endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;*/
                        break;
                    case "4":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-6), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        //America/Tegucigalpa
                        break;
                    case "5":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        System.DateTime endDateTime2 = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime2.AddDays(-13), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime2, "America/Tegucigalpa").date;
                        break;
                    case "6":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        System.DateTime endDateTime3 = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime3.AddDays(-29), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime3, "America/Tegucigalpa").date;
                        break;
                    case "7":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;//DateRangeType.LAST_MONTH;
                        break;
                    case "8":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        int dias = endDateTime.Day - 1;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-dias), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        break;
                    default:
                        {
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                              DateTimeUtilities.FromDateTime(endDateTime.AddDays(-20), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                              DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                            //America/Tegucigalpa
                            break;
                        }
                }

                reportJob.reportQuery.statement = statementBuilder.ToStatement();
                //List<string> Filas = new List<string>();
                //string columns = reportJob.reportQuery.
                //Run the Report

                reportJob = reportService.runReportJob(reportJob);
                //XmlReader reader;
                //reportService.ResponseHeader.ReadXml(reader);
                //Obtener la Url y la data
                //reportService.getReportJobStatus(reportJob.id);
                int progress = 0;
                while (reportService.getReportJobStatus(reportJob.id) == ReportJobStatus.IN_PROGRESS)
                {
                    progress = 1;
                }
                string url = reportService.getReportDownloadURL(reportJob.id, ExportFormat.CSV_DUMP);
                byte[] gzipReport = MediaUtilities.GetAssetDataFromUrl(url);
                string reportContents = Encoding.UTF8.GetString(MediaUtilities.DeflateGZipData(gzipReport));
                //List<string> result = reportContents.Split(',').ToList();
                string[] vLineresult = reportContents.Split('\n');
                string result = "";
                string[] vSplitResult = { };
                DataTable table = new DataTable();
                table.Columns.Add("order", typeof(string));
                table.Columns.Add("date", typeof(string));
                table.Columns.Add("impre", typeof(string));
                table.Columns.Add("clics", typeof(string));
                table.Columns.Add("ctr", typeof(string));
                table.Columns.Add("Name", typeof(string));
                for (int i = 1; i < vLineresult.Length; i++)
                {
                    result = vLineresult[i];
                    vSplitResult = result.Split(',');
                    if (result != "")
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[0], vSplitResult[1], vSplitResult[2], vSplitResult[3], vSplitResult[4], vSplitResult[5] };
                        table.Rows.Add(dataRow);
                    }
                    /*for (int j = 0; j < vSplitResult.Length; j++)
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[j], vSplitResult[j+1], vSplitResult[j+2], vSplitResult[j+3], vSplitResult[j+4] };                    
                        table.Rows.Add(dataRow); 
                    }*/
                    /*DataRow dataRow = table.NewRow();
                    dataRow.ItemArray = new object[] { vLineresult[i] };
                    
                    table.Rows.Add(dataRow);                    */
                }
                dataTable = table.Clone();
                dataTable = table;

                /*ReportUtilities reportUtilities = new ReportUtilities(reportService, reportJob.id);                
                // Set download options.
                ReportDownloadOptions options = new ReportDownloadOptions();
                options.exportFormat = ExportFormat.XML;
                options.useGzipCompression = false;
                reportUtilities.reportDownloadOptions = options;*/

                //Download the report.
                /*using (ReportResponse reportResponse = reportUtilities.GetResponse())
                {                   
                    reportResponse.Save(filePath);
                }*/
            }
            catch (Exception ex)
            {

                throw;
            }

            return dataTable;
        }
        public DataTable ReportWithFilter(long vOrderId,string filter)
        {
            string vDatos = "";
            DataTable dataTable = new DataTable();
            try
            {
                user = new DfpUser();
                ReportService reportService = (ReportService)user.GetService(DfpService.v201602.ReportService);
                String filePath = "C:\\archivo\\report.xml";
                //long orderId = 301023708;
                //long orderId = 302108868;
                String advertiserId = "62699388";
                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("ORDER_ID = :id")
                    .AddValue("id", vOrderId);
                /* StatementBuilder statementBuilder = new StatementBuilder()
                 .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                 .OrderBy("id ASC")                
                 .AddValue("advertiserId", advertiserId)
                 .AddValue("isArchived", false);*/

                // Create report job.
                /*ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_ID, Dimension.ORDER_NAME, Dimension.DAY,Dimension.DATE};
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                DimensionAttribute.ORDER_TRAFFICKER, DimensionAttribute.ORDER_START_DATE_TIME,
                DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR, Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM};*/
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_NAME, Dimension.ORDER_ID, Dimension.DATE };
                //reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                //DimensionAttribute.ORDER_START_DATE_TIME, DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR};

                // Set a custom date range 
                //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                System.DateTime endDateTime = System.DateTime.Now;
                //reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                switch (filter)
                {
                    case "1":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;
                        break;
                    case "2":
                            //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                            DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                            DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        break;
                    case "3":
                            //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                            reportJob.reportQuery.dateRangeType = DateRangeType.TODAY;
                            /*endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                            DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                            DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;*/
                        break;
                    case "4":
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                            DateTimeUtilities.FromDateTime(endDateTime.AddDays(-6), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                            DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                //America/Tegucigalpa
                        break;
                    case "5":
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            System.DateTime endDateTime2 = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                            DateTimeUtilities.FromDateTime(endDateTime2.AddDays(-13), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                            DateTimeUtilities.FromDateTime(endDateTime2, "America/Tegucigalpa").date;
                        break;
                    case "6":
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            System.DateTime endDateTime3 = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                            DateTimeUtilities.FromDateTime(endDateTime3.AddDays(-29), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                            DateTimeUtilities.FromDateTime(endDateTime3, "America/Tegucigalpa").date;
                        break;
                    case "7":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;//DateRangeType.LAST_MONTH;
                        break;
                    case "8":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            int dias = endDateTime.Day -1;
                            reportJob.reportQuery.startDate =
                            DateTimeUtilities.FromDateTime(endDateTime.AddDays(-dias), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                            DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        break;
                    default:
                        {
                             reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                              DateTimeUtilities.FromDateTime(endDateTime.AddDays(-20), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                              DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                //America/Tegucigalpa
                            break;
                        }
                }
               

                reportJob.reportQuery.statement = statementBuilder.ToStatement();
                //List<string> Filas = new List<string>();
                //string columns = reportJob.reportQuery.
                //Run the Report

                reportJob = reportService.runReportJob(reportJob);
                //XmlReader reader;
                //reportService.ResponseHeader.ReadXml(reader);
                //Obtener la Url y la data
                //reportService.getReportJobStatus(reportJob.id);
                int progress = 0;
                while (reportService.getReportJobStatus(reportJob.id) == ReportJobStatus.IN_PROGRESS)
                {
                    progress = 1;
                }
                string url = reportService.getReportDownloadURL(reportJob.id, ExportFormat.CSV_DUMP);
                byte[] gzipReport = MediaUtilities.GetAssetDataFromUrl(url);
                string reportContents = Encoding.UTF8.GetString(MediaUtilities.DeflateGZipData(gzipReport));
                //List<string> result = reportContents.Split(',').ToList();
                dataTable = ConvertOrderToDataTable(reportContents);
                //dataTable = table.Clone();
                //dataTable = table;

                /*ReportUtilities reportUtilities = new ReportUtilities(reportService, reportJob.id);                
                // Set download options.
                ReportDownloadOptions options = new ReportDownloadOptions();
                options.exportFormat = ExportFormat.XML;
                options.useGzipCompression = false;
                reportUtilities.reportDownloadOptions = options;*/

                //Download the report.
                /*using (ReportResponse reportResponse = reportUtilities.GetResponse())
                {                   
                    reportResponse.Save(filePath);
                }*/
            }
            catch (Exception ex)
            {

                throw;
            }

            return dataTable;
        }
        public DataTable ConvertOrderToDataTable(string reportContents)
        { 
                
                string[] vLineresult = reportContents.Split('\n');
                string result = "";
                string[] vSplitResult = { };
                DataTable table = new DataTable();
                table.Columns.Add("order", typeof(string));
                table.Columns.Add("date", typeof(string));
                table.Columns.Add("impre", typeof(string));
                table.Columns.Add("clics", typeof(string));
                table.Columns.Add("ctr", typeof(string));
                table.Columns.Add("Name", typeof(string));
                for (int i = 1; i < vLineresult.Length; i++)
                {
                    result = vLineresult[i];
                    vSplitResult = result.Split(',');
                    if (result != "")
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[0], vSplitResult[1], vSplitResult[2], vSplitResult[3], vSplitResult[4], vSplitResult[5] };
                        table.Rows.Add(dataRow);
                    }
                    /*for (int j = 0; j < vSplitResult.Length; j++)
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[j], vSplitResult[j+1], vSplitResult[j+2], vSplitResult[j+3], vSplitResult[j+4] };                    
                        table.Rows.Add(dataRow); 
                    }*/
                    /*DataRow dataRow = table.NewRow();
                    dataRow.ItemArray = new object[] { vLineresult[i] };
                    
                    table.Rows.Add(dataRow);                    */
                }
                return table;
        }

        public string CreateReportbyLine(long vLineID, string filter)
        {
            string vDatos = "";
            string vRespuesta = "";
            DataTable dataTable = new DataTable();
            try
            {
                user = new DfpUser();
                ReportService reportService = (ReportService)user.GetService(DfpService.v201602.ReportService);
                String filePath = "C:\\archivo\\report.csv";
                //long orderId = 301023708;
                //long orderId = 302108868;
                String advertiserId = "62699388";
                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("LINE_ITEM_ID = :id")
                    .AddValue("id", vLineID);
                /* StatementBuilder statementBuilder = new StatementBuilder()
                 .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                 .OrderBy("id ASC")                
                 .AddValue("advertiserId", advertiserId)
                 .AddValue("isArchived", false);*/

                // Create report job.
                /*ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_ID, Dimension.ORDER_NAME, Dimension.DAY,Dimension.DATE};
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                DimensionAttribute.ORDER_TRAFFICKER, DimensionAttribute.ORDER_START_DATE_TIME,
                DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR, Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM};*/
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();                
                    //reportJob.reportQuery.dimensions = new Dimension[] { Dimension.LINE_ITEM_NAME, Dimension.LINE_ITEM_ID, Dimension.DATE };
                
                //reportJob.reportQuery.customFieldIds = 
                reportJob.reportQuery.dimensions = new Dimension[] {Dimension.LINE_ITEM_NAME, Dimension.DATE };
                //reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {DimensionAttribute.LINE_ITEM_LIFETIME_IMPRESSIONS,DimensionAttribute.LINE_ITEM_LIFETIME_CLICKS,DimensionAttribute.LINE_ITEM_LABELS};
                //reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                //DimensionAttribute.ORDER_START_DATE_TIME, DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR};

                // Set a custom date range 
                //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                System.DateTime endDateTime = System.DateTime.Now;
                //reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                switch (filter)
                {
                    case "1":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;
                        break;
                    case "2":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        break;
                    case "3":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                        reportJob.reportQuery.dateRangeType = DateRangeType.TODAY;
                        /*endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;*/
                        break;
                    case "4":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-6), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        //America/Tegucigalpa
                        break;
                    case "5":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        System.DateTime endDateTime2 = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime2.AddDays(-13), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime2, "America/Tegucigalpa").date;
                        break;
                    case "6":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        System.DateTime endDateTime3 = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime3.AddDays(-29), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime3, "America/Tegucigalpa").date;
                        break;
                    case "7":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;//DateRangeType.LAST_MONTH;
                        break;
                    case "8":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        int dias = endDateTime.Day - 1;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-dias), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        break;
                    default:
                        {
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                              DateTimeUtilities.FromDateTime(endDateTime.AddDays(-20), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                              DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                            //America/Tegucigalpa
                            break;
                        }
                }

                reportJob.reportQuery.statement = statementBuilder.ToStatement();
                //List<string> Filas = new List<string>();
                //string columns = reportJob.reportQuery.
                //Run the Report

                reportJob = reportService.runReportJob(reportJob);
                //XmlReader reader;
                //reportService.ResponseHeader.ReadXml(reader);
                //Obtener la Url y la data
                //reportService.getReportJobStatus(reportJob.id);
                int progress = 0;
                while (reportService.getReportJobStatus(reportJob.id) == ReportJobStatus.IN_PROGRESS)
                {
                    progress = 1;
                }
                
               // ReportUtilities reportUtilities = new ReportUtilities(reportService, reportJob.id);
                // Set download options.
                ReportDownloadOptions options = new ReportDownloadOptions();
                options.exportFormat = ExportFormat.CSV_DUMP;
                options.useGzipCompression = false;
               // reportUtilities.reportDownloadOptions = options;
                string url = reportService.getReportDownloadUrlWithOptions(reportJob.id, options);
                //byte[] gzipReport = MediaUtilities.GetAssetDataFromUrl(url);
                //string reportContents = Encoding.UTF8.GetString(MediaUtilities.DeflateGZipData(gzipReport));
                vRespuesta = url;
                //Download the report.
                /*using (ReportResponse reportResponse = reportUtilities.GetResponse())
                {                   
                    reportResponse.save(filePath);
                    //vRespuesta=reportResponse.OnDownloadSuccess.ToString();
                }*/

                /*ReportUtilities reportUtilities = new ReportUtilities(reportService, reportJob.id);                
                // Set download options.
                ReportDownloadOptions options = new ReportDownloadOptions();
                options.exportFormat = ExportFormat.XML;
                options.useGzipCompression = false;
                reportUtilities.reportDownloadOptions = options;*/

                //Download the report.
                /*using (ReportResponse reportResponse = reportUtilities.GetResponse())
                {                   
                    reportResponse.Save(filePath);
                }*/
            }
            catch (Exception ex)
            {

                vRespuesta = ex.ToString();
            }

            return vRespuesta;
        }

        public DataTable CreatePdfReport(long vLineID, string filter)
        {
            string vDatos = "";
            DataTable dataTable = new DataTable();
            try
            {
                user = new DfpUser();
                ReportService reportService = (ReportService)user.GetService(DfpService.v201602.ReportService);
                String filePath = "C:\\archivo\\report.xml";
                //long orderId = 301023708;
                //long orderId = 302108868;
                String advertiserId = "62699388";
                // Create statement object to filter for an order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .Where("LINE_ITEM_ID = :id")
                    .AddValue("id", vLineID);
                /* StatementBuilder statementBuilder = new StatementBuilder()
                 .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                 .OrderBy("id ASC")                
                 .AddValue("advertiserId", advertiserId)
                 .AddValue("isArchived", false);*/

                // Create report job.
                /*ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.ORDER_ID, Dimension.ORDER_NAME, Dimension.DAY,Dimension.DATE};
                reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                DimensionAttribute.ORDER_TRAFFICKER, DimensionAttribute.ORDER_START_DATE_TIME,
                DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR, Column.AD_SERVER_CPM_AND_CPC_REVENUE,
                Column.AD_SERVER_WITHOUT_CPD_AVERAGE_ECPM};*/
                ReportJob reportJob = new ReportJob();
                reportJob.reportQuery = new ReportQuery();
                reportJob.reportQuery.dimensions = new Dimension[] { Dimension.LINE_ITEM_NAME, Dimension.LINE_ITEM_ID, Dimension.DATE };
                //reportJob.reportQuery.dimensionAttributes = new DimensionAttribute[] {
                //DimensionAttribute.ORDER_START_DATE_TIME, DimensionAttribute.ORDER_END_DATE_TIME};
                reportJob.reportQuery.columns = new Column[] {Column.AD_SERVER_IMPRESSIONS,
                Column.AD_SERVER_CLICKS, Column.AD_SERVER_CTR};

                // Set a custom date range 
                //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                System.DateTime endDateTime = System.DateTime.Now;
                //reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                switch (filter)
                {
                    case "1":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;
                        break;
                    case "2":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        break;
                    case "3":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.YESTERDAY;
                        reportJob.reportQuery.dateRangeType = DateRangeType.TODAY;
                        /*endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-1), "America/Tegucigalpa").date;*/
                        break;
                    case "4":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-6), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        //America/Tegucigalpa
                        break;
                    case "5":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        System.DateTime endDateTime2 = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime2.AddDays(-13), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime2, "America/Tegucigalpa").date;
                        break;
                    case "6":
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        System.DateTime endDateTime3 = System.DateTime.Now;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime3.AddDays(-29), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime3, "America/Tegucigalpa").date;
                        break;
                    case "7":
                        reportJob.reportQuery.dateRangeType = DateRangeType.REACH_LIFETIME;//DateRangeType.LAST_MONTH;
                        break;
                    case "8":
                        //reportJob.reportQuery.dateRangeType = DateRangeType.CURRENT_AND_NEXT_MONTH;
                        reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                        endDateTime = System.DateTime.Now;
                        int dias = endDateTime.Day - 1;
                        reportJob.reportQuery.startDate =
                        DateTimeUtilities.FromDateTime(endDateTime.AddDays(-dias), "America/Tegucigalpa").date;
                        reportJob.reportQuery.endDate =
                        DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                        break;
                    default:
                        {
                            reportJob.reportQuery.dateRangeType = DateRangeType.CUSTOM_DATE;
                            endDateTime = System.DateTime.Now;
                            reportJob.reportQuery.startDate =
                              DateTimeUtilities.FromDateTime(endDateTime.AddDays(-20), "America/Tegucigalpa").date;
                            reportJob.reportQuery.endDate =
                              DateTimeUtilities.FromDateTime(endDateTime, "America/Tegucigalpa").date;
                            //America/Tegucigalpa
                            break;
                        }
                }

                reportJob.reportQuery.statement = statementBuilder.ToStatement();
                //List<string> Filas = new List<string>();
                //string columns = reportJob.reportQuery.
                //Run the Report

                reportJob = reportService.runReportJob(reportJob);
                //XmlReader reader;
                //reportService.ResponseHeader.ReadXml(reader);
                //Obtener la Url y la data
                //reportService.getReportJobStatus(reportJob.id);
                int progress = 0;
                while (reportService.getReportJobStatus(reportJob.id) == ReportJobStatus.IN_PROGRESS)
                {
                    progress = 1;
                }
                string url = reportService.getReportDownloadURL(reportJob.id, ExportFormat.CSV_DUMP);
                byte[] gzipReport = MediaUtilities.GetAssetDataFromUrl(url);
                string reportContents = Encoding.UTF8.GetString(MediaUtilities.DeflateGZipData(gzipReport));
                //List<string> result = reportContents.Split(',').ToList();
                string[] vLineresult = reportContents.Split('\n');
                string result = "";
                string[] vSplitResult = { };
                DataTable table = new DataTable();
                table.Columns.Add("Linea", typeof(string));
                table.Columns.Add("LineaId", typeof(string));
                table.Columns.Add("Fecha", typeof(string));
                table.Columns.Add("Impresiones", typeof(string));
                table.Columns.Add("Clics", typeof(string));
                table.Columns.Add("CTR", typeof(string));
                for (int i = 1; i < vLineresult.Length; i++)
                {
                    result = vLineresult[i];
                    vSplitResult = result.Split(',');
                    if (result != "")
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[0], vSplitResult[1], vSplitResult[2], vSplitResult[3], vSplitResult[4], vSplitResult[5] };
                        table.Rows.Add(dataRow);
                    }
                    /*for (int j = 0; j < vSplitResult.Length; j++)
                    {
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = new object[] { vSplitResult[j], vSplitResult[j+1], vSplitResult[j+2], vSplitResult[j+3], vSplitResult[j+4] };                    
                        table.Rows.Add(dataRow); 
                    }*/
                    /*DataRow dataRow = table.NewRow();
                    dataRow.ItemArray = new object[] { vLineresult[i] };
                    
                    table.Rows.Add(dataRow);                    */
                }
                dataTable = table.Clone();
                dataTable = table;

                /*ReportUtilities reportUtilities = new ReportUtilities(reportService, reportJob.id);                
                // Set download options.
                ReportDownloadOptions options = new ReportDownloadOptions();
                options.exportFormat = ExportFormat.XML;
                options.useGzipCompression = false;
                reportUtilities.reportDownloadOptions = options;*/

                //Download the report.
                /*using (ReportResponse reportResponse = reportUtilities.GetResponse())
                {                   
                    reportResponse.Save(filePath);
                }*/
            }
            catch (Exception ex)
            {

                dataTable = new DataTable();
            }

            return dataTable;
        }

    }
}