using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.Common.Util.Reports;
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201602;
using Google.Api.Ads.Dfp.v201602;
using Newtonsoft.Json;

namespace AdBoostDashboard
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        DfpUser user;
        DfpApi vGapi;
        protected void Page_Load(object sender, EventArgs e)
        {
            user = new DfpUser();
            vGapi = new DfpApi();
            //GetOrders();
            //LbGetOrder();
            //GetLineItems();

            if (Request["Option"] == null)
                return;

            string cmd = Request["Option"].ToString();
            if (cmd == "LbGetOrder")
            {
                string vCompanyId = Request["vCompanyId"].ToString();
                Response.Clear();
                Response.Write(LbGetOrder(vCompanyId));
                Response.End();
            }
            if (cmd == "GetLineItemsFilter")
            {
                string vCompanyId = Request["vCompanyId"].ToString();
                string vFiltro = Request["vFiltro"].ToString();
                Response.Clear();
                Response.Write(GetLineItemsFilter(vCompanyId, vFiltro));
                Response.End();
            }

            if (cmd == "GetLineItems")
            {
                string vOrderID = Request["vOrderID"].ToString();
                Response.Clear();
                Response.Write(GetLineItems(vOrderID));
                Response.End();
            }
            if (cmd == "GetOrdersFiltered")
            {
                string vOrderID = Request["vOrderID"].ToString();
                Response.Clear();
                Response.Write(GetOrdersFiltered(vOrderID));
                Response.End();
            }
            if (cmd == "GetLineItemsbyID")
            {
                string vLineID = Request["vLineID"].ToString();
                Response.Clear();
                Response.Write(GetLineItemsbyID(vLineID));
                Response.End();
            }
            if (cmd == "GetCreativesByLine")
            {
                string vLineID = Request["vLineID"].ToString();
                Response.Clear();
                Response.Write(GetCreativesById(vLineID));
                Response.End();
            }
        }
        //object sender, EventArgs e
        public string GetCreativesById(string vLineID)//obtener Creatives by LineId
        {
            string vJsonresponse = "";
            long LineId = Convert.ToInt64(vLineID);
            try
            {
                //long vOrderID = Convert.ToInt64(orderID);
                vJsonresponse = vGapi.GetLicaCreativeId(LineId);

                
            }
            catch (Exception ex)
            {
                LitOrders.Text = "Error en obtener order \"{0}\"" + ex.Message;
            }
            return vJsonresponse;
        }
        public string GetLineItemsbyID(string vLineID)//obtener ordenes de un filtradas por OrderID
        {
            string vJsonresponse = "";
            string vJsonLineItem = "";
            DataTable vLineData = new DataTable();
            try
            {
                //long vOrderID = Convert.ToInt64(orderID);
                vLineData = vGapi.GetLineItemsbyLineID(vLineID);

                vJsonresponse = JsonConvert.SerializeObject(vLineData);
            }
            catch (Exception ex)
            {
                LitOrders.Text = "Error en obtener order \"{0}\"" + ex.Message;
            }
            return vJsonresponse;
        }
        public string GetOrdersFiltered(string orderID)//obtener ordenes de un filtradas por OrderID
        {
            string vJsonresponse = "";
            string vJsonLineItem = "";
            
            try
            {
                //long vOrderID = Convert.ToInt64(orderID);
                vJsonresponse = vGapi.GetOrderDetailbyOrderId(orderID);
               

            }
            catch (Exception ex)
            {
                LitOrders.Text = "Error en obtener order \"{0}\"" + ex.Message;
            }
            return vJsonresponse;
        }
        public string LbGetOrder(string vCompanyId)//obtener ordenes de un advertiserID
        {
            string vJsonresponse = "";
            string vJsonLineItem = "";
            try
            {
                String vOrderID = "";
                //String vCompanyId = "";
                //vCompanyId = "59769708";
               // DataTable vOrderData = vGapi.GetOrder(vCompanyId);
                //usando JSON.Net Dll
                //LitOrders.Text = JsonConvert.SerializeObject(vOrderData);
                vJsonresponse = vGapi.GetOrder(vCompanyId);
                //vJsonresponse = JsonConvert.SerializeObject(vOrderData);
                //vJsonresponse = vJsonLineItem;
                
            }
            catch (Exception ex)
            {
                LitOrders.Text = "Error en obtener order \"{0}\"" + ex.Message;
            }
            return vJsonresponse;
        }
        public string GetLineItemsFilter(string vCompanyId,string vFiltro)
        {
            string vJsonresponse = "";
            try
            {
                vJsonresponse = vGapi.GetOrderDetailsbyFilter(vCompanyId, vFiltro);
            }
            catch (Exception)
            {
                
                throw;
            }
            return vJsonresponse;
        }
        public string GetLineItems(string vOrderID)
        {
            string vJsonLineResponse = "";
            try
            {
                //String vOrderID = "302108868";
                string vTotImpre = "";
                string vTotClicks = "";
                //vOrderID = txtCompany.Text;
                DataTable vLineData = vGapi.GetLineItemsbyOrder(vOrderID);
               // vJsonLineResponse = vGapi.GetLineItemsbyOrder(vOrderID);
                //DataTable vLineData = new DataTable(); ;
                //usando JSON.Net Dll
                //LitLineItem.Text += JsonConvert.SerializeObject(vLineData) + Environment.NewLine;
                /*DataRow[] result = vLineData.Select("N° = 99999");
                foreach(DataRow row in result)
                {
                    vTotImpre= row[4].ToString();
                    vTotClicks = row[5].ToString();
                }*/
                //vJsonLineResponse = JsonConvert.SerializeObject(vLineData);
            }
            catch (Exception ex)
            {
                
                LitLineItem.Text = "Error en obtener LineItems \"{0}\"" + ex.Message;
            }
            
            return vJsonLineResponse;
        }
        private void ConfigureUserForOAuth()
        {
            DfpAppConfig config = (user.Config as DfpAppConfig);
            if (config.AuthorizationMethod == DfpAuthorizationMethod.OAuth2)
            {
                if (config.OAuth2Mode == OAuth2Flow.APPLICATION &&
                      string.IsNullOrEmpty(config.OAuth2RefreshToken))
                {
                    user.OAuthProvider = (OAuth2ProviderForApplications)Session["OAuthProvider"];
                }
            }
            else
            {
                throw new Exception("Authorization mode is not OAuth.");
            }
        }
        protected void GetOrders()
        {
            ConfigureUserForOAuth();
            try
            {
                // Get the OrderService.
                OrderService orderService = (OrderService)user.GetService(DfpService.v201602.OrderService);

                // Create a statement to get all orders.
                StatementBuilder statementBuilder = new StatementBuilder()
                    .OrderBy("id ASC")
                    .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT);

                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),
            new DataColumn("Order Id", typeof(long)),
            new DataColumn("Nombre", typeof(string)),
            new DataColumn("AdvertiserID", typeof(long))
            });
                // Set default for page.
                OrderPage page = new OrderPage();               
                    do
                    {
                        // Get orders by statement.
                        page = orderService.getOrdersByStatement(statementBuilder.ToStatement());

                        if (page.results != null && page.results.Length > 0)
                        {
                            int i = page.startIndex;
                            foreach (Order order in page.results)
                            {
                                DataRow dataRow = dataTable.NewRow();
                                dataRow.ItemArray = new object[] { i + 1, order.id, order.name, order.advertiserId };
                                dataTable.Rows.Add(dataRow);
                                i++;
                            }
                        }

                        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                    } while (statementBuilder.GetOffset() < page.totalResultSetSize);
                    if (dataTable.Rows.Count > 0)
                    {
                        String vListaOrdenes = "";
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            vListaOrdenes += dataTable.Rows[i][2].ToString();
                        }
                        LitOrders.Text = vListaOrdenes;

                    }
                    else
                    {
                        LitOrders.Text="No se Encontraron ordenes";
                    }



                }
                catch (Exception ex)
                {

                   LitOrders.Text="Error en obtener orders \"{0}\""+ ex.Message;
                }
            }        
    }
}