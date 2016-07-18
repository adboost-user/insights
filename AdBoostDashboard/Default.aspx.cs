using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Api.Ads.Common.Lib;
using Google.Api.Ads.Common.Util.Reports;
using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201602;
using Google.Api.Ads.Dfp.v201602;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Xml;
using System.Text;


namespace AdBoostDashboard
{
    public partial class Default : System.Web.UI.Page
    {
        DfpUser user;
        DfpApi vGapi;
        ReportDfp vReport;
        
        long vOrderId;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            vReport = new ReportDfp();
            vGapi = new DfpApi();
            //vReport.Report();
            String vCompany = Convert.ToString(Session["companyId"]);
            LitTitle.Text = "Información General de Campañas";
            //TotalOrdenes("59769708");
            if (vCompany !="")
            {
                TotalOrdenes(vCompany, out vOrderId);
            }
            if (!IsPostBack)
            {
                getChartData(vOrderId, "7");
            }
           
            //ReadXml();
            /*if (vOrderId > 0)
            {
                getChartData(vOrderId);
            }
            else
            {
                TopInfo.Visible = false;
                GraficoInfo.Visible = false;
            }*/
        }
        protected void itemSelected(object sender, EventArgs e)
        {
            /*Response.Write("Getting clicked; " + sender.GetType().ToString());            
            Response.Write("<script>alert('Hello')</script>");*/
            string opcion = GraficoLista.SelectedIndex.ToString();
            int vopcionInt = Convert.ToInt32(opcion);
            if (vopcionInt > 0)
            {
                getChartData(vOrderId, opcion);
            }
            else {
                LitTitulo.Text = "Seleccione una opción";
                lblData2.Text = "";
                lblData3.Text = "";
                LitImpGrafico.Text = "";
                LitClicsGrafico.Text = "";
                LitCTRgrafico.Text = "";
            }
            
        }
        public void getChartData(long vOrderId,string opc)        
        {
            string vXmlData = "";
            StringBuilder sb = new StringBuilder();
            StringBuilder sbData2 = new StringBuilder(); 
            try 
	        {
                System.DateTime vFecha;
                string vImpredia = "";
                string vClicsdia = "";
                System.DateTime st = new System.DateTime();
                var vFechaConvert = GetUnixEpoch(st);
                DataTable dataTable = new DataTable();
                Decimal vDecimal = 0;
                Decimal vDecTot = 0;
                Decimal vDecClic = 0;
                string vTotImpresiones = "";
                string vTotClics = "";
                string vCTR = "";
                LitTitulo.Text = "";
                LitGrafInfo.Text = "";
                dataTable = vReport.ReportWithFilter(vOrderId,opc);
                if (dataTable.Rows.Count > 0)
                {
                    sb.Append("[");
                    sbData2.Append("[");
                    int i = 0;
                    int tam = dataTable.Rows.Count-1;
                    
                    foreach (DataRow dr in dataTable.Rows)
                    {

                        vFecha = Convert.ToDateTime(dataTable.Rows[i][2]);
                        st = new System.DateTime(vFecha.Year, vFecha.Month, vFecha.Day, 0, 0, 0, DateTimeKind.Utc);
                        vFechaConvert = GetUnixEpoch(st);
                        vImpredia = Convert.ToString(dataTable.Rows[i][3]);
                        vClicsdia = Convert.ToString(dataTable.Rows[i][4]);
                        vDecTot += Convert.ToDecimal(dataTable.Rows[i][3]);
                        vDecClic += Convert.ToDecimal(dataTable.Rows[i][4]);
                        //vDecimal += Convert.ToDecimal(dataTable.Rows[i][5]);      
                        //if (i < 20)
                        if (i < tam)
                        {
                            sb.Append("[" + Convert.ToString(vFechaConvert) + ", " + vImpredia + "],");
                            sbData2.Append("[" + Convert.ToString(vFechaConvert) + ", " + vClicsdia + "],");
                        }
                        else
                        {
                            sb.Append("[" + Convert.ToString(vFechaConvert) + ", " + vImpredia + "]");
                            sbData2.Append("[" + Convert.ToString(vFechaConvert) + ", " + vClicsdia + "]");

                        }
                        i++;

                    }
                    sb.Append("]");
                    sbData2.Append("]");
                    LitTitulo.Text = " Campaña " + Convert.ToString(dataTable.Rows[0][0]);
                    vDecimal = (Convert.ToDecimal(vDecClic) / Convert.ToDecimal(vDecTot));
                    vTotImpresiones = vDecTot.ToString("0,0");
                    vTotClics = vDecClic.ToString("0,0");
                    
                    vCTR = vDecimal.ToString("P");
                    LitImpGrafico.Text = vTotImpresiones;
                    LitClicsGrafico.Text = vTotClics;
                    LitCTRgrafico.Text = vCTR;
                    LitGrafInfo.Text = "";
                }
                else
                {
                    LitTitulo.Text = "";
                    LitImpGrafico.Text = "";
                    LitClicsGrafico.Text = "";
                    LitCTRgrafico.Text = "";
                    sbData2 = sbData2.Clear();
                    sb = sb.Clear();
                    if (!IsPostBack)
                    {
                        LitGrafInfo.Text = "No se pudo obtener información " +
                        "Intente con una búsqueda de las opciones";
                    }
                    else
                    {
                        LitGrafInfo.Text = "No se encontró información para esta búsqueda " +
                        "Intente con otra diferente";
                    }
                }
                /*sb.Append("["+
                "["+Convert.ToString(fecha1)+", 800], "+ "["+Convert.ToString(fecha2)+", 500], "+
                "["+ Convert.ToString(fecha3)+ ", 600], " + "[" + Convert.ToString(fecha4) + ", 600], " +
                "["+Convert.ToString(fecha5)+", 700]"+
                "]");*/
                //System.DateTime st = new System.DateTime(2012, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                //var unixTime1 = GetUnixEpoch(st);
                //GetTime();
               /* sb.Append("[" +
                    "[1325397600000, 800], [1325484000000, 500], [1325570400000, 600], [1325656800000, 700]," +
                    "[1325743200000, 500], [1325829600000, 456], [1325916000000, 800], [1326002400000, 589]," +
                    "[1326088800000, 467], [1326175200000, 876], [1326261600000, 689], [1326348000000, 700]," +
                    "[1326434400000, 500], [1326520800000, 600], [1326607200000, 700], [1326693600000, 786]," +
                    "[1326780000000, 345], [1326866400000, 888], [1326952800000, 888], [1327039200000, 888]," +
                    "[1327125600000, 987], [1327212000000, 444], [1327298400000, 999], [1327384800000, 567]," +
                    "[1327471200000, 786], [1327557600000, 666], [1327644000000, 888], [1327730400000, 900]," +
                    "[1327816800000, 178], [1327903200000, 555], [1327989600000, 993]" +
                "]");*/
                
                //sb.Append("[[1325397600000, 800], [1325484000000, 500], [1325570400000, 600], [1325656800000, 700], [1325743200000, 500], [1325829600000, 456], [1325916000000, 800], [1326002400000, 589], [1326088800000, 467], [1326175200000, 876], [1326261600000, 689], [1326348000000, 700], [1326434400000, 500], [1326520800000, 600], [1326607200000, 700], [1326693600000, 786], [1326780000000, 345], [1326866400000, 888], [1326952800000, 888], [1327039200000, 888], [1327125600000, 987], [1327212000000, 444], [1327298400000, 999], [1327384800000, 567], [1327471200000, 786], [1327557600000, 666], [1327644000000, 888], [1327730400000, 900], [1327816800000, 178], [1327903200000, 555], [1327989600000, 993]]");
                lblData2.Text = Convert.ToString(sbData2);
                lblData3.Text = Convert.ToString(sb);
	        }
	        catch (Exception ex)
	        {
		
		        throw;
	        }
            //return vXmlData;
        }
        private double GetUnixEpoch(System.DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() -
                new System.DateTime(1970, 1, 1,0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalMilliseconds;
        }


        public void TotalOrdenes(string vCompanyId, out long vOrder)
        {
            string vTotImpresiones="";
            string vTotClics="";
            string vCTR = "";
            
            try
            {
            Decimal vDecimal = 0;
            Decimal vDecTot = 0;
            Decimal vDecClic = 0;
            DataTable vTotalesDT = vGapi.GetOrderTotals(vCompanyId);
            vOrder = 0;
            if (vTotalesDT.Rows.Count != 0)
            {
                int i =0;
                foreach (DataRow dr in vTotalesDT.Rows)
                {
                    vDecTot += Convert.ToDecimal(vTotalesDT.Rows[i][2]);
                    vDecClic += Convert.ToDecimal(vTotalesDT.Rows[i][3]);
                    vDecimal += Convert.ToDecimal(vTotalesDT.Rows[i][4]);
                    //vOpcionOrderID += "<option value='" + vTotalesDT.Rows[i][5].ToString() + "'>" + vTotalesDT.Rows[i][5].ToString() + "</option>";
                    i++;
                }
                vOrder = Convert.ToInt64(vTotalesDT.Rows[0][5]);
                vTotImpresiones = vDecTot.ToString("0,0");
                vTotClics = vDecClic.ToString("0,0");
                vCTR = vDecimal.ToString("P");
                LitImpresiones.Text = vTotImpresiones;
                LitClics.Text = vTotClics;
                LitCTR.Text = vCTR;                
            }
            
       
            //LitOptions.Text = vOpcionOrderID;

            }
            catch (Exception ex)
            {
                LitTitulo.Text = ex.ToString();
                throw new Exception("Error en respuesta " + ex.ToString()) ;
            }

        }
    }
}