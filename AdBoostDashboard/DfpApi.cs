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

namespace AdBoostDashboard
{
    public class DfpApi
    {
       // private const string DateFormat = "yyyy-MM-dd";
        DfpUser user;
        string vTotImpresiones = "";
        string vTotClics = "";
        string vCTR = "";
        public DataTable GetOrderTotalsByOrderID(string vAdvIDLong,long OrderID)//obtiene totales ordenes de un advertiser
        {
            string vJsonLineItem = "";
            DataTable vLineItemsDT = new DataTable(); ;
            user = new DfpUser();
            DataTable vOrderData = vOrderData = new DataTable(); ;
            // Get the OrderService.
            OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);

            // Set the name of the advertiser (company) to get orders for.
            //String advertiserId =  Convert.ToString(vAdvIDLong);
            String advertiserId = vAdvIDLong;

            // Create a statement to only select orders for a given advertiser.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("id= :id")                
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("id", OrderID);
                //.AddValue("isArchived", false);


            // Set default for page.
            OrderPage page = new OrderPage();

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),                
            new DataColumn("OrderId", typeof(long))                
            /*new DataColumn("Nombre", typeof(string)),        
            new DataColumn("Id Compañia", typeof(string)),
            new DataColumn("Archivado", typeof(long))*/
            });
            try
            {
                do
                {
                    // Get orders by statement.
                    page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;

                        foreach (Order order in page.results)
                        {
                            //vOrderData += Convert.ToString(order.id) + " | " + order.name + " | " + order.advertiserId +  " | "+ Convert.ToString(order.isArchived)+  Environment.NewLine;

                            DataRow dataRow = dataTable.NewRow();
                            //dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId, order.isArchived };
                            dataRow.ItemArray = new object[] { i + 1, order.id };
                            dataTable.Rows.Add(dataRow);
                            i++;

                        }


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                //Console.WriteLine("Number of results found: " + page.totalResultSetSize);
                //DataTable dtAcu = new DataTable();  
                vOrderData = new DataTable();
                vOrderData.Columns.AddRange(new DataColumn[] {
                new DataColumn("N°", typeof(string)),                
                new DataColumn("LineId", typeof(long)),       
                new DataColumn("Impresiones", typeof(long)),
                new DataColumn("Clics", typeof(long)),
                new DataColumn("CTR", typeof(decimal)),
                new DataColumn("OrderId", typeof(long))                
                });
                vLineItemsDT = vOrderData.Clone();
                if (dataTable.Rows.Count > 0)
                {
                    string vOrderID = "";
                    DataRow[] result = dataTable.Select("N° >= 1");
                    if (dataTable.Rows.Count > 0)
                    {

                        foreach (DataRow row in result)
                        {
                            vOrderID = row[1].ToString();
                            //vLineItemsDT.NewRow();
                            vOrderData = GetLineTotals(vOrderID);//Obtiene totales de impresion y clicks
                            foreach (DataRow dr in vOrderData.Rows)
                            {
                                vLineItemsDT.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    else
                    {
                        //LitOrders.Text = "No se encontro Orden N° " + vCompanyId;
                        vJsonLineItem = "";
                    }
                }
                else
                {
                    vOrderData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                String Excep = "No se encontro order \"{0}\"" + ex.Message;
                DataRow dataRow = vOrderData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "", null, null };
                vLineItemsDT.Rows.Add(dataRow);
            }
            return vLineItemsDT;
        }
        public DataTable GetOrderTotals(string vAdvIDLong)//obtiene totales ordenes de un advertiser
        {
            string vJsonLineItem = "";
            DataTable vLineItemsDT = new DataTable(); ;
            user = new DfpUser();
            DataTable vOrderData = vOrderData = new DataTable(); ;
            // Get the OrderService.
            OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);

            // Set the name of the advertiser (company) to get orders for.
            //String advertiserId =  Convert.ToString(vAdvIDLong);
            String advertiserId = vAdvIDLong;

            // Create a statement to only select orders for a given advertiser.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                //.Where("isArchived = :archivo")
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("advertiserId", advertiserId)
                .AddValue("isArchived", false);


            // Set default for page.
            OrderPage page = new OrderPage();

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),                
            new DataColumn("OrderId", typeof(long))                
            /*new DataColumn("Nombre", typeof(string)),        
            new DataColumn("Id Compañia", typeof(string)),
            new DataColumn("Archivado", typeof(long))*/
            });
            try
            {
                do
                {
                    // Get orders by statement.
                    page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;

                        foreach (Order order in page.results)
                        {
                            //vOrderData += Convert.ToString(order.id) + " | " + order.name + " | " + order.advertiserId +  " | "+ Convert.ToString(order.isArchived)+  Environment.NewLine;

                            DataRow dataRow = dataTable.NewRow();
                            //dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId, order.isArchived };
                            dataRow.ItemArray = new object[] { i + 1, order.id};
                            dataTable.Rows.Add(dataRow);
                            i++;

                        }


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                //Console.WriteLine("Number of results found: " + page.totalResultSetSize);
                //DataTable dtAcu = new DataTable();  
                vOrderData = new DataTable();
                vOrderData.Columns.AddRange(new DataColumn[] {
                new DataColumn("N°", typeof(string)),                
                new DataColumn("LineId", typeof(long)),       
                new DataColumn("Impresiones", typeof(long)),
                new DataColumn("Clics", typeof(long)),
                new DataColumn("CTR", typeof(decimal)),
                new DataColumn("OrderId", typeof(long))                
                });
                vLineItemsDT = vOrderData.Clone();
                if (dataTable.Rows.Count > 0)
                {
                    string vOrderID = "";
                    DataRow[] result = dataTable.Select("N° >= 1");
                    if (dataTable.Rows.Count > 0)
                    {
              
                        foreach (DataRow row in result)
                        {
                            vOrderID = row[1].ToString();
                            //vLineItemsDT.NewRow();
                            vOrderData = GetLineTotals(vOrderID);//Obtiene totales de impresion y clicks
                            foreach (DataRow dr in vOrderData.Rows)
                            {
                                vLineItemsDT.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    else
                    {
                        //LitOrders.Text = "No se encontro Orden N° " + vCompanyId;
                        vJsonLineItem = "";
                    }
                }
                else
                {
                    vOrderData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                String Excep = "No se encontro order \"{0}\"" + ex.Message;
                DataRow dataRow = vOrderData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "", null,null };
                vLineItemsDT.Rows.Add(dataRow);
            }
            return vLineItemsDT;
        }
        public DataTable GetLineTotals(String vOrderId)
        {
            DataTable vLineData = new DataTable(); ;
            string vJsonResponse = "";
            try
            {
                user = new DfpUser();
                // Get the LineItemService.
                LineItemService lineItemService =
                    (LineItemService)user.GetService(DfpService.v201602.LineItemService);

                // Set the ID of the order to get line items from.
                //long orderId = long.Parse(("INSERT_ORDER_ID_HERE"));
                long orderId = long.Parse((vOrderId));

                // Create a statement to only select line items that need creatives from a
                // given order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                    .Where("orderId = :orderId")
                    .OrderBy("id ASC")
                    .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                    .AddValue("orderId", orderId)
                    .AddValue("isMissingCreatives", true);

                //ComputedStatus.COMPLETED  para los status delivered inactive y otros

                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {
                new DataColumn("N°", typeof(string)),                
                new DataColumn("LineId", typeof(long)),       
                new DataColumn("Impresiones", typeof(long)),
                new DataColumn("Clics", typeof(long)),
                new DataColumn("CTR", typeof(decimal)),
                new DataColumn("OrderId", typeof(long))                
                });

                // Set default for page.
                LineItemPage page = new LineItemPage();

                do
                {
                    // Get line items by statement.
                    page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                    long vImpresiones = 0;
                    long vclicks = 0;
                    Google.Api.Ads.Dfp.v201602.DateTime vFechaIni = new Google.Api.Ads.Dfp.v201602.DateTime();
                    Google.Api.Ads.Dfp.v201602.DateTime vFechaFin = new Google.Api.Ads.Dfp.v201602.DateTime();
                    Decimal vCTR = 0;
                    string vOrderName = "";
                    if (page.results != null && page.results.Length > 0)
                    {
                        DataRow dataRow;
                        int i = page.startIndex;
                        foreach (LineItem lineItem in page.results)
                        {

                            //dataRow = dataTable.NewRow();
                            //dataRow.ItemArray = new object[] { i + 1, lineItem.id,lineItem.stats.impressionsDelivered, lineItem.stats.clicksDelivered, lineItem.orderId };
                            //dataTable.Rows.Add(dataRow);
                            i++;
                            vImpresiones += lineItem.stats.impressionsDelivered;
                            vclicks += lineItem.stats.clicksDelivered;
                            vOrderName = lineItem.orderName;
                        }


                        vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                        dataTable.Rows.Add(vOrderName, null, vImpresiones, vclicks, vCTR, orderId);


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                if (dataTable.Rows.Count > 0)
                {

                    //for (int i = 0; i < dataTable.Rows.Count; i++)
                    //{
                    //    vLineData += dataTable.Rows[i][2].ToString();
                    //}
                    vLineData = dataTable;
                    //usando JSON.Net Dll
                    vJsonResponse = JsonConvert.SerializeObject(vLineData);

                }
                else
                {
                    vLineData = new DataTable(); ;
                }

            }
            catch (Exception ex)
            {
                String Excep = "Error en buscar LineItem \"{0}\"" + ex.Message;
                DataRow dataRow = vLineData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "" };
                vLineData.Rows.Add(dataRow);

            }

            return vLineData;
        }
        public DataTable GetLineTotFiltByLineId(String vLineItemId)//obtiene total por linea, filtrado por Linea
        {
            DataTable vLineData = new DataTable(); ;
            string vJsonResponse = "";
            try
            {
                user = new DfpUser();
                // Get the LineItemService.
                LineItemService lineItemService =
                    (LineItemService)user.GetService(DfpService.v201602.LineItemService);

                // Set the ID of the order to get line items from.
                //long orderId = long.Parse(("INSERT_ORDER_ID_HERE"));
                long orderId = long.Parse((vLineItemId));

                // Create a statement to only select line items that need creatives from a
                // given order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                    .Where("id = :id")//hace la busqueda por id(LineItemId)
                    .OrderBy("id ASC")
                    .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                    .AddValue("id", vLineItemId);
                    

                //ComputedStatus.COMPLETED  para los status delivered inactive y otros

                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {
                new DataColumn("N°", typeof(string)),                
                new DataColumn("LineId", typeof(long)),       
                new DataColumn("Impresiones", typeof(long)),
                new DataColumn("Clics", typeof(long)),
                new DataColumn("CTR", typeof(decimal)),
                new DataColumn("OrderId", typeof(long)),
                new DataColumn("Estado", typeof(string))
                });

                // Set default for page.
                LineItemPage page = new LineItemPage();

                do
                {
                    // Get line items by statement.
                    page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                    long vImpresiones = 0;
                    long vclicks = 0;
                    Google.Api.Ads.Dfp.v201602.DateTime vFechaIni = new Google.Api.Ads.Dfp.v201602.DateTime();
                    Google.Api.Ads.Dfp.v201602.DateTime vFechaFin = new Google.Api.Ads.Dfp.v201602.DateTime();
                    Decimal vCTR = 0;
                    string vOrderName = "";
                    string estado="";
                    if (page.results != null && page.results.Length > 0)
                    {
                        DataRow dataRow;
                        int i = page.startIndex;
                        foreach (LineItem lineItem in page.results)
                        {
                            
                            //dataRow = dataTable.NewRow();
                            //dataRow.ItemArray = new object[] { i + 1, lineItem.id,lineItem.stats.impressionsDelivered, lineItem.stats.clicksDelivered, lineItem.orderId };
                            //dataTable.Rows.Add(dataRow);
                            i++;
                            vImpresiones += lineItem.stats.impressionsDelivered;
                            vclicks += lineItem.stats.clicksDelivered;
                            vOrderName = lineItem.name;
                             estado = lineItem.status.ToString();
                        }


                        vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                        dataTable.Rows.Add(vOrderName, null, vImpresiones, vclicks, vCTR, orderId,estado);


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                if (dataTable.Rows.Count > 0)
                {

                    //for (int i = 0; i < dataTable.Rows.Count; i++)
                    //{
                    //    vLineData += dataTable.Rows[i][2].ToString();
                    //}
                    vLineData = dataTable;
                    //usando JSON.Net Dll
                    vJsonResponse = JsonConvert.SerializeObject(vLineData);

                }
                else
                {
                    vLineData = new DataTable(); ;
                }

            }
            catch (Exception ex)
            {
                String Excep = "Error en buscar LineItem \"{0}\"" + ex.Message;
                DataRow dataRow = vLineData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "","" };
                vLineData.Rows.Add(dataRow);

            }

            return vLineData;
        }

        public DataTable GetOrderTotalStatus(string vAdvIDLong, string vFiltro)//obtiene totales ordenes de un advertiser por status
        {
            string vJsonLineItem = "";
            DataTable vLineItemsDT = new DataTable(); ;
            user = new DfpUser();
            DataTable vOrderData = vOrderData = new DataTable(); ;
            // Get the OrderService.
            OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);

            // Set the name of the advertiser (company) to get orders for.
            //String advertiserId =  Convert.ToString(vAdvIDLong);
            String advertiserId = vAdvIDLong;

            
            // Create a statement to only select orders for a given advertiser.

            StatementBuilder statementBuilder = new StatementBuilder()
            .Where("advertiserId = :advertiserId")
                //.Where("isArchived = :archivo")
            .OrderBy("id ASC")
            .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
            .AddValue("advertiserId", advertiserId);
               // .AddValue("isArchived", true);
            

            // Set default for page.
            OrderPage page = new OrderPage();

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),                
            new DataColumn("OrderId", typeof(long))                
            /*new DataColumn("Nombre", typeof(string)),        
            new DataColumn("Id Compañia", typeof(string)),
            new DataColumn("Archivado", typeof(long))*/
            });
            try
            {
                do
                {
                    // Get orders by statement.
                    page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;

                        foreach (Order order in page.results)
                        {
                            //vOrderData += Convert.ToString(order.id) + " | " + order.name + " | " + order.advertiserId +  " | "+ Convert.ToString(order.isArchived)+  Environment.NewLine;

                            DataRow dataRow = dataTable.NewRow();
                            //dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId, order.isArchived };
                            dataRow.ItemArray = new object[] { i + 1, order.id };
                            dataTable.Rows.Add(dataRow);
                            i++;

                        }


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                //Console.WriteLine("Number of results found: " + page.totalResultSetSize);
                //DataTable dtAcu = new DataTable();  
                vOrderData = new DataTable();
                vOrderData.Columns.AddRange(new DataColumn[] {
                new DataColumn("N°", typeof(int)),                
                new DataColumn("LineId", typeof(long)),       
                new DataColumn("Impresiones", typeof(long)),
                new DataColumn("Clics", typeof(long)),
                new DataColumn("CTR", typeof(decimal)),
                new DataColumn("OrderId", typeof(long))                
                });
                vLineItemsDT = vOrderData.Clone();
                if (dataTable.Rows.Count > 0)
                {
                    string vOrderID = "";
                    DataRow[] result = dataTable.Select("N° >= 1");
                    if (dataTable.Rows.Count > 0)
                    {

                        foreach (DataRow row in result)
                        {
                            vOrderID = row[1].ToString();
                            //vLineItemsDT.NewRow();
                            vOrderData = GetLineTotalsFilter(vOrderID,vFiltro);//Obtiene totales de impresion y clicks
                            foreach (DataRow dr in vOrderData.Rows)
                            {
                                vLineItemsDT.Rows.Add(dr.ItemArray);
                            }
                        }
                    }
                    else
                    {
                        //LitOrders.Text = "No se encontro Orden N° " + vCompanyId;
                        vJsonLineItem = "";
                    }
                }
                else
                {
                    vOrderData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                /*String Excep = "No se encontro order \"{0}\"" + ex.Message;
                DataRow dataRow = vOrderData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "", null, null };
                vLineItemsDT.Rows.Add(dataRow);*/
                vLineItemsDT = new DataTable(); ;
            }
            return vLineItemsDT;
        }        
        public DataTable GetLineTotalsFilter(String vOrderId,String vFiltro)
        {
            DataTable vLineData = new DataTable(); ;
            string vJsonResponse = "";
            try
            {
                user = new DfpUser();
                // Get the LineItemService.
                LineItemService lineItemService =
                    (LineItemService)user.GetService(DfpService.v201602.LineItemService);

                // Set the ID of the order to get line items from.
                //long orderId = long.Parse(("INSERT_ORDER_ID_HERE"));
                long orderId = long.Parse((vOrderId));

                // Create a statement to only select line items that need creatives from a
                // given order.
                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {
                    new DataColumn("N°", typeof(int)),                
                    new DataColumn("LineId", typeof(long)),       
                    new DataColumn("Impresiones", typeof(long)),
                    new DataColumn("Clics", typeof(long)),
                    new DataColumn("CTR", typeof(decimal)),
                    new DataColumn("OrderId", typeof(long))                
                    });
                // Set default for page.
                LineItemPage page = new LineItemPage();
                long vImpresiones = 0;
                long vclicks = 0;
                Decimal vCTR = 0;
                if (vFiltro == "5")
                {                
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)                        
                        .AddValue("isArchived", true);//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                                           
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                i++;
                                vImpresiones += lineItem.stats.impressionsDelivered;
                                vclicks += lineItem.stats.clicksDelivered;
                            }
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                  } //*** FIN IF = 5
                if (vFiltro == "2")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.DELIVERING.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                i++;
                                vImpresiones += lineItem.stats.impressionsDelivered;
                                vclicks += lineItem.stats.clicksDelivered;
                            }
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 2
                if (vFiltro == "3")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.COMPLETED.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                i++;
                                vImpresiones += lineItem.stats.impressionsDelivered;
                                vclicks += lineItem.stats.clicksDelivered;
                            }
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 3
                if (vFiltro == "4")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.INACTIVE.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                i++;
                                vImpresiones += lineItem.stats.impressionsDelivered;
                                vclicks += lineItem.stats.clicksDelivered;
                            }
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 4
                if (vFiltro == "6")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.PAUSED.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                i++;
                                vImpresiones += lineItem.stats.impressionsDelivered;
                                vclicks += lineItem.stats.clicksDelivered;
                            }
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 6
                if (dataTable.Rows.Count > 0)
                {
                    vLineData = dataTable;
                    vJsonResponse = JsonConvert.SerializeObject(vLineData);
                }
                else
                {
                    vLineData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                String Excep = "Error en buscar LineItem \"{0}\"" + ex.Message;
                DataRow dataRow = vLineData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "" };
                vLineData.Rows.Add(dataRow);

            }

            return vLineData;
        }
        
        public string GetOrderDetailsbyFilter(string vAdvIDLong,string vFiltro)//obtiene ordenes de un advertiser por filtro de status
        {
            string vJsonLineItem = "";
            DataTable vLineItemsDT = new DataTable(); ;
            user = new DfpUser();
            DataTable vOrderData = vOrderData = new DataTable(); ;
            // Get the OrderService.
            OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);

            // Set the name of the advertiser (company) to get orders for.
            //String advertiserId =  Convert.ToString(vAdvIDLong);
            String advertiserId = vAdvIDLong;

            // Create a statement to only select orders for a given advertiser.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("advertiserId = :advertiserId")
                //.Where("advertiserId = :advertiserId and isArchived= :isArchived")
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("advertiserId", advertiserId);
                //.AddValue("isArchived", false);

            // Set default for page.
            OrderPage page = new OrderPage();

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {
        new DataColumn("N°", typeof(int)),                
        new DataColumn("OrderId", typeof(long)),                
        new DataColumn("Nombre", typeof(string)),        
        new DataColumn("Id Compañia", typeof(string)),
        new DataColumn("Archivado", typeof(long))
        });
            try
            {
                do
                {
                    // Get orders by statement.
                    page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;

                        foreach (Order order in page.results)
                        {
                            //vOrderData += Convert.ToString(order.id) + " | " + order.name + " | " + order.advertiserId +  " | "+ Convert.ToString(order.isArchived)+  Environment.NewLine;

                            DataRow dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId, order.isArchived };
                            dataTable.Rows.Add(dataRow);
                            i++;

                        }


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                //Console.WriteLine("Number of results found: " + page.totalResultSetSize);

                if (dataTable.Rows.Count > 0)
                {
                    string vOrderID = "";
                    DataRow[] result = dataTable.Select("N° >= 1");
                    if (dataTable.Rows.Count > 0)
                    {
                        
                        vLineItemsDT.Columns.AddRange(new DataColumn[] {
                        new DataColumn("CampañaId", typeof(long)),      
                        new DataColumn("Nombre Campaña", typeof(string)),
                        new DataColumn("AnuncioId", typeof(long)),                
                        new DataColumn("Nombre Anuncio", typeof(string)),
                        //new DataColumn("Fecha Comienzo", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),
                        //new DataColumn("Fecha Final", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),                
                        new DataColumn("Impresiones", typeof(string)),
                        new DataColumn("Clics", typeof(string)),                
                        new DataColumn("CTR", typeof(string)),
                        new DataColumn("Creatividad", typeof(string))});
                        DataRow RowAdd;
                        DataTable dtAcu = new DataTable();
                        dtAcu = vLineItemsDT.Clone();
                        foreach (DataRow row in result)
                        {
                            vOrderID = row[1].ToString();
                            //vLineItemsDT.NewRow();
                            vOrderData = GetLineDetailsFilter(vOrderID,vFiltro);
                            //vLineItemsDT = (DataTable)JsonConvert.DeserializeObject(vJsonLineItem, (typeof(DataTable)));   //Convierte Json string to a Datatable 
                            //RowAdd = vLineItemsDT.NewRow();
                            //RowAdd=(DataRow[])JsonConvert.DeserializeObject(vJsonLineItem,(typeof(DataRow[]))) ;
                            //object[] response = (object[])JsonConvert.DeserializeObject(vJsonLineItem,(typeof(object[])));
                            //vLineItemsDT.Rows.Add(response);
                            foreach (DataRow dr in vOrderData.Rows)
                            {
                                dtAcu.Rows.Add(dr.ItemArray);
                            }
                        }
                        vJsonLineItem = JsonConvert.SerializeObject(dtAcu);

                    }
                    else
                    {
                        //LitOrders.Text = "No se encontro Orden N° " + vCompanyId;
                        vJsonLineItem = "";
                    }
                }
                else
                {
                    vOrderData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                /*String Excep = "No se encontro order \"{0}\"" + ex.Message;
                DataRow dataRow = vOrderData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "", null };
                vOrderData.Rows.Add(dataRow);*/
                vOrderData = new DataTable(); ;
            }
            return vJsonLineItem;
        }
        public DataTable GetLineDetailsFilter(String vOrderId, String vFiltro)//obtiene el detalle de linea por filtros
        {
            DataTable vLineData = new DataTable(); ;
            string vJsonResponse = "";
            try
            {
                user = new DfpUser();
                // Get the LineItemService.
                LineItemService lineItemService =
                (LineItemService)user.GetService(DfpService.v201602.LineItemService);

                // Set the ID of the order to get line items from.
                //long orderId = long.Parse(("INSERT_ORDER_ID_HERE"));
                long orderId = long.Parse((vOrderId));

                // Create a statement to only select line items that need creatives from a
                // given order.
                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {          
                new DataColumn("CampañaId", typeof(long)),      
                new DataColumn("Nombre Campaña", typeof(string)),
                new DataColumn("AnuncioId", typeof(long)),                
                new DataColumn("Nombre Anuncio", typeof(string)),
                //new DataColumn("Fecha Comienzo", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),
                //new DataColumn("Fecha Final", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),                
                new DataColumn("Impresiones", typeof(string)),
                new DataColumn("Clics", typeof(string)),                
                new DataColumn("CTR", typeof(string)),
                new DataColumn("Creatividad", typeof(string))});
                // Set default for page.
                LineItemPage page = new LineItemPage();
                long vImpresiones = 0;
                long vclicks = 0;
                string vCTRstring="";
                Decimal vCTR = 0;
                if (vFiltro == "5")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", true);//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                    
                    
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                vImpresiones = lineItem.stats.impressionsDelivered;
                                vclicks = lineItem.stats.clicksDelivered;
                                vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                                vCTRstring=vCTR.ToString("P");
                                dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring };
                                dataTable.Rows.Add(dataRow);
                                i++;
                                //vImpresiones += lineItem.stats.impressionsDelivered;
                                //vclicks += lineItem.stats.clicksDelivered;
                            }
                            
                           // vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                           // dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 5
                if (vFiltro == "2")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.DELIVERING.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                vImpresiones = lineItem.stats.impressionsDelivered;
                                vclicks = lineItem.stats.clicksDelivered;
                                vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                                vCTRstring = vCTR.ToString("P");
                                dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring };
                                dataTable.Rows.Add(dataRow);
                                i++;
                                //vImpresiones += lineItem.stats.impressionsDelivered;
                                //vclicks += lineItem.stats.clicksDelivered;
                            }
                           // vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            //dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 2
                if (vFiltro == "3")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.COMPLETED.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                vImpresiones = lineItem.stats.impressionsDelivered;
                                vclicks = lineItem.stats.clicksDelivered;
                                vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                                vCTRstring = vCTR.ToString("P");
                                dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring };
                                dataTable.Rows.Add(dataRow);
                                i++;
                                //vImpresiones += lineItem.stats.impressionsDelivered;
                                //vclicks += lineItem.stats.clicksDelivered;
                            }
                            //vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            //dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 3
                if (vFiltro == "4")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.INACTIVE.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                vImpresiones = lineItem.stats.impressionsDelivered;
                                vclicks = lineItem.stats.clicksDelivered;
                                vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                                vCTRstring = vCTR.ToString("P");
                                dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring };
                                dataTable.Rows.Add(dataRow);
                                i++;
                                //vImpresiones += lineItem.stats.impressionsDelivered;
                                //vclicks += lineItem.stats.clicksDelivered;
                            }
                            //vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            //dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 4
                if (vFiltro == "6")
                {
                    StatementBuilder statementBuilder = new StatementBuilder()
                        //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                        .Where("orderId = :orderId and status = :status and isArchived= :isArchived")
                        .OrderBy("id ASC")
                        .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                        .AddValue("orderId", orderId)
                        .AddValue("isArchived", false)
                        .AddValue("status", ComputedStatus.PAUSED.ToString());//ComputedStatus.COMPLETED  para los status delivered inactive y otros                                       
                    do
                    {
                        // Get line items by statement.
                        page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());
                        if (page.results != null && page.results.Length > 0)
                        {
                            DataRow dataRow;
                            int i = page.startIndex;
                            foreach (LineItem lineItem in page.results)
                            {
                                vImpresiones = lineItem.stats.impressionsDelivered;
                                vclicks = lineItem.stats.clicksDelivered;
                                vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                                vCTRstring = vCTR.ToString("P");
                                dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring };
                                dataTable.Rows.Add(dataRow);
                                i++;
                                //vImpresiones += lineItem.stats.impressionsDelivered;
                                //vclicks += lineItem.stats.clicksDelivered;
                            }
                            //vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            //dataTable.Rows.Add(1, null, vImpresiones, vclicks, vCTR, orderId);
                        }
                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                } //*** FIN IF = 6
                if (dataTable.Rows.Count > 0)
                {
                    vLineData = dataTable;
                    vJsonResponse = JsonConvert.SerializeObject(vLineData);                     
                }
                else
                {
                    vLineData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                String Excep = "Error en buscar LineItem \"{0}\"" + ex.Message;
                DataRow dataRow = vLineData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "" };
                vLineData.Rows.Add(dataRow);

            }

            return vLineData;
        }
        public string GetOrderDetailbyOrderId(string OrderIDString)//obtiene ordenes de un advertiser
        {
            string vJsonLineItem = "";
            DataTable vLineItemsDT = new DataTable(); ;
            user = new DfpUser();
            DataTable vOrderData = vOrderData = new DataTable(); ;
            // Get the OrderService.
            OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);
            long OrderID = Convert.ToInt64(OrderIDString);
            

            // Create a statement to only select orders for a given advertiser.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("id = :id")
                //.Where("isArchived = :archivo")
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("id", OrderID);
                //.AddValue("isArchived", false);


            // Set default for page.
            OrderPage page = new OrderPage();

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),                
            new DataColumn("OrderId", typeof(long)),                
            new DataColumn("Nombre", typeof(string)),        
            new DataColumn("Id Compañia", typeof(string)),
            new DataColumn("Archivado", typeof(long))
            });
            try
            {
                do
                {
                    // Get orders by statement.
                    page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;

                        foreach (Order order in page.results)
                        {
                            //vOrderData += Convert.ToString(order.id) + " | " + order.name + " | " + order.advertiserId +  " | "+ Convert.ToString(order.isArchived)+  Environment.NewLine;

                            DataRow dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId, order.isArchived };
                            dataTable.Rows.Add(dataRow);
                            i++;

                        }


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                //Console.WriteLine("Number of results found: " + page.totalResultSetSize);

                if (dataTable.Rows.Count > 0)
                {
                    string vOrderID = "";
                    DataRow[] result = dataTable.Select("N° >= 1");
                    if (dataTable.Rows.Count > 0)
                    {

                        vLineItemsDT.Columns.AddRange(new DataColumn[] {
                new DataColumn("CampañaId", typeof(long)),      
                new DataColumn("Nombre Campaña", typeof(string)),
                new DataColumn("AnuncioId", typeof(long)),                
                new DataColumn("Nombre Anuncio", typeof(string)),
                //new DataColumn("Fecha Comienzo", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),
                //new DataColumn("Fecha Final", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),                
                new DataColumn("Impresiones", typeof(string)),
                new DataColumn("Clics", typeof(string)),                
                new DataColumn("CTR", typeof(string)),
                new DataColumn("Creatividad", typeof(string))});
                        DataRow RowAdd;
                        DataTable dtAcu = new DataTable();
                        dtAcu = vLineItemsDT.Clone();
                        foreach (DataRow row in result)
                        {
                            vOrderID = row[1].ToString();
                            //vLineItemsDT.NewRow();
                            vOrderData = GetLineItemsbyOrder(vOrderID);
                            //vLineItemsDT = (DataTable)JsonConvert.DeserializeObject(vJsonLineItem, (typeof(DataTable)));   //Convierte Json string to a Datatable 
                            //RowAdd = vLineItemsDT.NewRow();
                            //RowAdd=(DataRow[])JsonConvert.DeserializeObject(vJsonLineItem,(typeof(DataRow[]))) ;
                            //object[] response = (object[])JsonConvert.DeserializeObject(vJsonLineItem,(typeof(object[])));
                            //vLineItemsDT.Rows.Add(response);
                            foreach (DataRow dr in vOrderData.Rows)
                            {
                                dtAcu.Rows.Add(dr.ItemArray);
                            }
                        }
                        vJsonLineItem = JsonConvert.SerializeObject(dtAcu);

                    }
                    else
                    {
                        //LitOrders.Text = "No se encontro Orden N° " + vCompanyId;
                        vJsonLineItem = "";
                    }
                }
                else
                {
                    vOrderData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                String Excep = "No se encontro order \"{0}\"" + ex.Message;
                DataRow dataRow = vOrderData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "", null };
                vOrderData.Rows.Add(dataRow);
            }
            return vJsonLineItem;
        }
        public string GetOrder(string vAdvIDLong)//obtiene ordenes de un advertiser
        {
            string vJsonLineItem = "";
            DataTable vLineItemsDT = new DataTable(); ;
            user = new DfpUser();
            DataTable vOrderData = vOrderData = new DataTable(); ;
            // Get the OrderService.
            OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);

            // Set the name of the advertiser (company) to get orders for.
            //String advertiserId =  Convert.ToString(vAdvIDLong);
            String advertiserId = vAdvIDLong;

            // Create a statement to only select orders for a given advertiser.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("advertiserId = :advertiserId and isArchived= :isArchived")
                //.Where("isArchived = :archivo")
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("advertiserId", advertiserId)
                .AddValue("isArchived", false);


            // Set default for page.
            OrderPage page = new OrderPage();

            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),                
            new DataColumn("OrderId", typeof(long)),                
            new DataColumn("Nombre", typeof(string)),        
            new DataColumn("Id Compañia", typeof(string)),
            new DataColumn("Archivado", typeof(long))
            });
            try
            {
                do
                {
                    // Get orders by statement.
                    page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;

                        foreach (Order order in page.results)
                        {
                            //vOrderData += Convert.ToString(order.id) + " | " + order.name + " | " + order.advertiserId +  " | "+ Convert.ToString(order.isArchived)+  Environment.NewLine;

                            DataRow dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId, order.isArchived };
                            dataTable.Rows.Add(dataRow);
                            i++;

                        }


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                //Console.WriteLine("Number of results found: " + page.totalResultSetSize);

                if (dataTable.Rows.Count > 0)
                {
                    string vOrderID = "";
                    DataRow[] result = dataTable.Select("N° >= 1");
                    if (dataTable.Rows.Count > 0)
                    {

                        vLineItemsDT.Columns.AddRange(new DataColumn[] {
                new DataColumn("CampañaId", typeof(long)),      
                new DataColumn("Nombre Campaña", typeof(string)),
                new DataColumn("AnuncioId", typeof(long)),                
                new DataColumn("Nombre Anuncio", typeof(string)),
                //new DataColumn("Fecha Comienzo", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),
                //new DataColumn("Fecha Final", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),                
                new DataColumn("Impresiones", typeof(string)),
                new DataColumn("Clics", typeof(string)),                
                new DataColumn("CTR", typeof(string)),
                new DataColumn("Creatividad", typeof(string))});
                        DataRow RowAdd;
                        DataTable dtAcu = new DataTable();
                        dtAcu = vLineItemsDT.Clone();
                        foreach (DataRow row in result)
                        {
                            vOrderID = row[1].ToString();
                            //vLineItemsDT.NewRow();
                            vOrderData = GetLineItemsbyOrder(vOrderID);
                            //vLineItemsDT = (DataTable)JsonConvert.DeserializeObject(vJsonLineItem, (typeof(DataTable)));   //Convierte Json string to a Datatable 
                            //RowAdd = vLineItemsDT.NewRow();
                            //RowAdd=(DataRow[])JsonConvert.DeserializeObject(vJsonLineItem,(typeof(DataRow[]))) ;
                            //object[] response = (object[])JsonConvert.DeserializeObject(vJsonLineItem,(typeof(object[])));
                            //vLineItemsDT.Rows.Add(response);
                            foreach (DataRow dr in vOrderData.Rows)
                            {
                                dtAcu.Rows.Add(dr.ItemArray);
                            }
                        }
                        vJsonLineItem = JsonConvert.SerializeObject(dtAcu);

                    }
                    else
                    {
                        //LitOrders.Text = "No se encontro Orden N° " + vCompanyId;
                        vJsonLineItem = "";
                    }
                }
                else
                {
                    vOrderData = new DataTable(); ;
                }
            }
            catch (Exception ex)
            {
                String Excep = "No se encontro order \"{0}\"" + ex.Message;
                DataRow dataRow = vOrderData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "", null };
                vOrderData.Rows.Add(dataRow);
            }
            return vJsonLineItem;
        }
        public DataTable GetLineItemsbyOrder (String vOrderId)
        {
            DataTable vLineData = new DataTable(); ;
            string vJsonResponse = "";
            try
            {
                user = new DfpUser();
                // Get the LineItemService.
                LineItemService lineItemService =
                    (LineItemService)user.GetService(DfpService.v201602.LineItemService);

                // Set the ID of the order to get line items from.
                //long orderId = long.Parse(("INSERT_ORDER_ID_HERE"));
                long orderId = long.Parse((vOrderId));

                // Create a statement to only select line items that need creatives from a
                // given order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                    .Where("orderId = :orderId")
                    .OrderBy("id ASC")
                    .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                    .AddValue("orderId", orderId)
                    .AddValue("isMissingCreatives", true);

                //ComputedStatus.COMPLETED  para los status delivered inactive y otros
                
                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {          
                new DataColumn("CampañaId", typeof(long)),      
                new DataColumn("Nombre Campaña", typeof(string)),
                new DataColumn("AnuncioId", typeof(long)),                
                new DataColumn("Nombre Anuncio", typeof(string)),
                //new DataColumn("Fecha Comienzo", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),
                //new DataColumn("Fecha Final", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),                
                new DataColumn("Impresiones", typeof(string)),
                new DataColumn("Clics", typeof(string)),                
               new DataColumn("CTR", typeof(string)),
                new DataColumn("Creatividad", typeof(string))});
             
                long vImpresiones = 0;
                long vclicks = 0;
                string vCTRstring="";
                Decimal vCTR = 0;

                // Set default for page.
                LineItemPage page = new LineItemPage();

                do
                {
                    // Get line items by statement.
                    page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());                
                    
                    //Decimal vCTR = 0;
                    if (page.results != null && page.results.Length > 0)
                    {
                        DataRow dataRow;
                        int i = page.startIndex;
                        foreach (LineItem lineItem in page.results)
                        {

                            vImpresiones = lineItem.stats.impressionsDelivered;
                            vclicks = lineItem.stats.clicksDelivered;
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            vCTRstring = vCTR.ToString("P");
                            string vLicaUrl = "";
                            //vLicaUrl = GetLicaUrl(lineItem.id);
                            /*vTotImpresiones = vDecTot.ToString("0,0");
                            vTotClics = vDecClic.ToString("0,0");*/
                            dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring, vLicaUrl };                         
                           
                            dataTable.Rows.Add(dataRow);
                            i++;
                            vImpresiones = 0;
                            vclicks = 0;
                            vCTRstring = "";
                           
                        }

                        
                       // vCTR = (Convert.ToDecimal(vclicks)/Convert.ToDecimal(vImpresiones));                        
                       //dataTable.Rows.Add(99999, null, "totales", vCTR.ToString("P"), vImpresiones, vclicks,orderId);

                        
                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                if (dataTable.Rows.Count > 0)
                {
                   
                    //for (int i = 0; i < dataTable.Rows.Count; i++)
                    //{
                    //    vLineData += dataTable.Rows[i][2].ToString();
                    //}
                    vLineData = dataTable;
                    //usando JSON.Net Dll
                    vJsonResponse = JsonConvert.SerializeObject(vLineData);

                }
                else
                {
                    vLineData = new DataTable(); ;
                }

            }
            catch (Exception ex)
            {
                String Excep="Error en buscar LineItem \"{0}\"" + ex.Message;
                DataRow dataRow = vLineData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "" };
                vLineData.Rows.Add(dataRow);
                
            }

            return vLineData;
        }
        public String GetLicaUrl(long LineId)
        {
            string Url = "";
            user = new DfpUser();
            LineItemCreativeAssociationService licaService = (LineItemCreativeAssociationService)
                user.GetService(DfpService.v201602.LineItemCreativeAssociationService);
           

            // Set default for page.
            LineItemCreativeAssociationPage page = new LineItemCreativeAssociationPage();
            try
            {
                StatementBuilder statementBuilder = new StatementBuilder()
               .Where("lineItemId = :lineItemId")
                .OrderBy("lineItemId ASC, creativeId ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
               .AddValue("lineItemId", LineId);

            do {
                page = licaService.getLineItemCreativeAssociationsByStatement(
              statementBuilder.ToStatement());
                long CreativeId;
                if (page.results != null && page.results.Length > 0)
                {
                    int i = page.startIndex;
                    foreach (LineItemCreativeAssociation lica in page.results)
                    {
                        CreativeId = lica.creativeId;
                        Url = GetCreativeUrl(CreativeId);
                        i++;
                    }
                }
            statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
            } while (statementBuilder.GetOffset() < page.totalResultSetSize);
            }
            catch (Exception ex)
            {
                
                throw;
            }

            return Url;
        }
        public String GetCreativeUrl(long CreativeId)
        { 
            string CreativeUrl="";
            user = new DfpUser();
            CreativeService creativeService =
          (CreativeService)user.GetService(DfpService.v201602.CreativeService);

            // Create a statement to only select image creatives.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("id = :id")
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("id", CreativeId);

            // Set default for page.
            CreativePage page = new CreativePage();
            try
            {
             do {
                page = creativeService.getCreativesByStatement(statementBuilder.ToStatement());
                if (page.results != null && page.results.Length > 0)
                {
                    int i = page.startIndex;
                    foreach (Creative creative in page.results)
                    {
                        /*Console.WriteLine("{0}) Creative with ID ='{1}', name ='{2}' and type ='{3}' " +
                            "was found.", i, creative.id, creative.name, creative.GetType().Name, creative.previewUrl);*/
                        CreativeUrl = creative.previewUrl;
                        i++;
                    }
                }
            statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
             } while (statementBuilder.GetOffset() < page.totalResultSetSize);
            }
            catch (Exception ex)
            {

                return CreativeUrl=ex.ToString();
            }
            return CreativeUrl;
        }
        //OBTENER CREATIVE ID PARA BUSCAR EL DETALLE DE CREATIVE
        public String GetLicaCreativeId(long LineId)
        {
            string Url = "";
            string vJsonResponse = "";
            user = new DfpUser();
            DataTable dataTable = new DataTable();
            DataTable Table = new DataTable();
            Table.Columns.AddRange(new DataColumn[] {          
            new DataColumn("Nombre", typeof(string)),      
            new DataColumn("Estado", typeof(string)),                           
            new DataColumn("Impresiones", typeof(string)), 
            new DataColumn("Clics", typeof(string)), 
            new DataColumn("CTR", typeof(string)), 
            new DataColumn("Ver", typeof(string))});
            
            LineItemCreativeAssociationService licaService = (LineItemCreativeAssociationService)
                user.GetService(DfpService.v201602.LineItemCreativeAssociationService);


            // Set default for page.
            LineItemCreativeAssociationPage page = new LineItemCreativeAssociationPage();
            try
            {
                StatementBuilder statementBuilder = new StatementBuilder()
               .Where("lineItemId = :lineItemId")
                .OrderBy("lineItemId ASC, creativeId ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
               .AddValue("lineItemId", LineId);

                do
                {
                    page = licaService.getLineItemCreativeAssociationsByStatement(
                  statementBuilder.ToStatement());
                    long CreativeId;
                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;
                        DataRow dataRow;
                        foreach (LineItemCreativeAssociation lica in page.results)
                        {
                            CreativeId = lica.creativeId;
                            decimal impre = Convert.ToDecimal(lica.stats.stats.impressionsDelivered);
                            decimal clics = Convert.ToDecimal(lica.stats.stats.clicksDelivered);
                            decimal vDecimal = (Convert.ToDecimal(clics) / Convert.ToDecimal(impre));
                            string vCTR = vDecimal.ToString("P");
                            string vTotImpresiones = impre.ToString("0,0");
                            string vTotClics = clics.ToString("0,0");
                            string CreatStatus=lica.status.ToString();
                            dataTable = GetCreativeDetail(CreativeId);
                            //Table = dataTable.Clone();
                            foreach (DataRow dr in dataTable.Rows)
                            {
                                string num =dataTable.Rows[0][0].ToString();
                                long CreatId = Convert.ToInt64(dataTable.Rows[0][1]);
                                string NombreCrea = dataTable.Rows[0][2].ToString();
                                string UrlCreat = dataTable.Rows[0][3].ToString();

                                dataRow = Table.NewRow();
                                dataRow.ItemArray = new object[] { NombreCrea, CreatStatus, vTotImpresiones, vTotClics, vCTR,UrlCreat};
                                Table.Rows.Add(dataRow);
                            }
                           
                                /*foreach (DataRow dr in dataTable.Rows)
                                {
                                    Table.Rows.Add(dr.ItemArray);
                                }*/
                            i++;
                        }
                    }
                    vJsonResponse = JsonConvert.SerializeObject(Table);
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
            }
            catch (Exception ex)
            {
                vJsonResponse=ex.ToString();
                return vJsonResponse;
            }

            return vJsonResponse;
        }
        //Obtiene detalle de Creative
        public DataTable GetCreativeDetail(long CreativeId)
        {
            string CreativeUrl = "";
            user = new DfpUser();
            CreativeService creativeService =
          (CreativeService)user.GetService(DfpService.v201602.CreativeService);

            // Create a statement to only select image creatives.
            StatementBuilder statementBuilder = new StatementBuilder()
                .Where("id = :id")
                .OrderBy("id ASC")
                .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                .AddValue("id", CreativeId);

            // Set default for page.
            CreativePage page = new CreativePage();
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[] {          
            new DataColumn("N°", typeof(int)),      
            new DataColumn("Creatividad Id", typeof(long)),                           
            new DataColumn("Nombre Creatividad", typeof(string)),                                             
            new DataColumn("Url", typeof(string))});
            try
            {
                do
                {
                    page = creativeService.getCreativesByStatement(statementBuilder.ToStatement()); 
                    if (page.results != null && page.results.Length > 0)
                    {
                        DataRow dataRow;
                        int i = 1;
                        foreach (Creative creative in page.results)
                        {
                            /*Console.WriteLine("{0}) Creative with ID ='{1}', name ='{2}' and type ='{3}' " +
                                "was found.", i, creative.id, creative.name, creative.GetType().Name, creative.previewUrl);*/
                            //CreativeUrl = creative.previewUrl;
                            dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { i, creative.id, creative.name, creative.previewUrl };
                            dataTable.Rows.Add(dataRow);
                            i++;                            
                        }
                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
            }
            catch (Exception ex)
            {

                return dataTable = new DataTable();
            }
            return dataTable;
        }


        public DataTable GetLineItemsbyLineID(String vLineID)
        {
            DataTable vLineData = new DataTable(); ;
            string vJsonResponse = "";
            try
            {
                user = new DfpUser();
                // Get the LineItemService.
                LineItemService lineItemService =
                    (LineItemService)user.GetService(DfpService.v201602.LineItemService);
                long LineID = long.Parse((vLineID));

                // Create a statement to only select line items that need creatives from a
                // given order.
                StatementBuilder statementBuilder = new StatementBuilder()
                    //.Where("orderId = :orderId AND isMissingCreatives = :isMissingCreatives")
                    .Where("id = :id")
                    .OrderBy("id ASC")
                    .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
                    .AddValue("id", LineID);
                    

                //ComputedStatus.COMPLETED  para los status delivered inactive y otros

                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {          
               new DataColumn("CampañaId", typeof(long)),      
                new DataColumn("Nombre Campaña", typeof(string)),
                new DataColumn("AnuncioId", typeof(long)),                
                new DataColumn("Nombre Anuncio", typeof(string)),
                //new DataColumn("Fecha Comienzo", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),
                //new DataColumn("Fecha Final", typeof(Google.Api.Ads.Dfp.v201602.DateTime)),                
                new DataColumn("Impresiones", typeof(string)),
                new DataColumn("Clics", typeof(string)),                
               new DataColumn("CTR", typeof(string)),
                new DataColumn("Creatividad", typeof(string))});

                long vImpresiones = 0;
                long vclicks = 0;
                string vCTRstring = "";
                Decimal vCTR = 0;

                // Set default for page.
                LineItemPage page = new LineItemPage();

                do
                {
                    // Get line items by statement.
                    page = lineItemService.getLineItemsByStatement(statementBuilder.ToStatement());

                    //Decimal vCTR = 0;
                    if (page.results != null && page.results.Length > 0)
                    {
                        DataRow dataRow;
                        int i = page.startIndex;
                        foreach (LineItem lineItem in page.results)
                        {

                            vImpresiones = lineItem.stats.impressionsDelivered;
                            vclicks = lineItem.stats.clicksDelivered;
                            vCTR = (Convert.ToDecimal(vclicks) / Convert.ToDecimal(vImpresiones));
                            vCTRstring = vCTR.ToString("P");
                            dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { lineItem.orderId, lineItem.orderName, lineItem.id, lineItem.name, lineItem.stats.impressionsDelivered.ToString("0,0"), lineItem.stats.clicksDelivered.ToString("0,0"), vCTRstring,"" };
                            dataTable.Rows.Add(dataRow);
                            i++;
                            vImpresiones = 0;
                            vclicks = 0;
                            vCTRstring = "";

                        }


                        // vCTR = (Convert.ToDecimal(vclicks)/Convert.ToDecimal(vImpresiones));                        
                        //dataTable.Rows.Add(99999, null, "totales", vCTR.ToString("P"), vImpresiones, vclicks,orderId);


                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                if (dataTable.Rows.Count > 0)
                {

                    //for (int i = 0; i < dataTable.Rows.Count; i++)
                    //{
                    //    vLineData += dataTable.Rows[i][2].ToString();
                    //}
                    vLineData = dataTable;
                    //usando JSON.Net Dll
                    vJsonResponse = JsonConvert.SerializeObject(vLineData);

                }
                else
                {
                    vLineData = new DataTable(); ;
                }

            }
            catch (Exception ex)
            {
                String Excep = "Error en buscar LineItem \"{0}\"" + ex.Message;
                DataRow dataRow = vLineData.NewRow();
                dataRow.ItemArray = new object[] { 0, 0, Excep, "" };
                vLineData.Rows.Add(dataRow);

            }

            return vLineData;
        }

       
    }
}