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
using System.Text;


namespace AdBoostDashboard
{
    public partial class Ordenes : System.Web.UI.Page
    {
        DfpUser user;
        DfpApi vGapi;
        ReportDfp vReport;
        long vOrderId;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            if (!Convert.ToBoolean(Session["autenticado"]))
                Response.Redirect("Login.aspx");
            vReport = new ReportDfp();
            vGapi = new DfpApi();
            String vCompany = Convert.ToString(Session["companyId"]);
            String vCompanyName = Convert.ToString(Session["companyName"]);
            String vIdValue = Request.QueryString["id"];
            //HideDivCompany.InnerText = vCompany;
            HideDivFiltro.InnerText = vIdValue;
            switch (vIdValue)
            {
                case "1":
                    LitTitle.Text = "Todas las Campañas " + vCompanyName;
                    OrdenTotales(vCompany, out vOrderId);
                    if (vOrderId > 0)
                    {
                        if (!IsPostBack)
                        {
                            getChartData(vOrderId, "7");
                        }
                    }
                    break;
                case "2":
                    LitTitle.Text = "Campañas Ejecutándose " + vCompanyName;
                    TotalOrdenes(vCompany, vIdValue,out vOrderId);
                    if (vOrderId > 0)
                    {
                        if (!IsPostBack)
                        {
                            getChartData(vOrderId, "7");
                        }
                    }
                    break;
                case "3":
                    LitTitle.Text = "Campañas Completadas " + vCompanyName;
                    TotalOrdenes(vCompany, vIdValue, out vOrderId);
                    if (vOrderId > 0)
                    {
                        if (!IsPostBack)
                        {
                            getChartData(vOrderId, "7");
                        }
                    }
                    break;
                case "4":
                    LitTitle.Text = "Campañas Inactivas " + vCompanyName;
                    TotalOrdenes(vCompany, vIdValue, out vOrderId);
                    if (vOrderId > 0)
                    {
                        if (!IsPostBack)
                        {
                            getChartData(vOrderId, "7");
                        }
                    }
                    break;
                case "5":
                    LitTitle.Text = "Campañas Archivadas " + vCompanyName;
                    TotalOrdenes(vCompany, vIdValue, out vOrderId);
                    if (vOrderId > 0)
                    {
                        if (!IsPostBack)
                        {
                            getChartData(vOrderId, "7");
                        }
                    }
                    break;
                case "6":
                    LitTitle.Text = "Campañas Pausadas " + vCompanyName;
                    TotalOrdenes(vCompany, vIdValue, out vOrderId);
                    if (vOrderId > 0)
                    {
                        if (!IsPostBack)
                        {
                            getChartData(vOrderId, "7");
                        }
                    }
                    break;
                default:
                    {
                        Response.Redirect("Default.aspx");
                        break;
                    }
            }
            }
            catch (Exception)
            {

                throw;
            }
         
            
        }
        protected void itemSelected(object sender, EventArgs e)
        {
            /*Response.Write("Getting clicked; " + sender.GetType().ToString());            
            Response.Write("<script>alert('Hello')</script>");*/
            String vIdValue = Request.QueryString["id"];
            //long vOrderID = Convert.ToInt64(vIdValue);
            string opcion = GraficoLista.SelectedIndex.ToString();
            int vopcionInt = Convert.ToInt32(opcion);
            if (vopcionInt > 0)
            {
                getChartData(vOrderId, opcion);
            }
            else
            {
                LitTitulo.Text = "Seleccione una opción";
                lblData2.Text = "";
                lblData3.Text = "";
                LitImpGrafico.Text = "";
                LitClicsGrafico.Text = "";
                LitCTRgrafico.Text = "";
            }

        }
        public void getChartData(long vOrderId, string opc)
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
                dataTable = vReport.ReportWithFilter(vOrderId, opc);
                if (dataTable.Rows.Count > 0)
                {
                    sb.Append("[");
                    sbData2.Append("[");
                    int i = 0;
                    int tam = dataTable.Rows.Count - 1;

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
                new System.DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalMilliseconds;
        }

        protected void TotalOrdenes(string vCompanyId, string vFiltro ,out long vOrder)
        {
            string vTotImpresiones = "";
            string vTotClics = "";
            string vCTR = "";
            try
            {
                Decimal vDecimal = 0;
                Decimal vDecTot = 0;
                Decimal vDecClic = 0;
                vOrder = 0;
                DataTable vTotalesDT = vGapi.GetOrderTotalStatus(vCompanyId, vFiltro);
                if (vTotalesDT.Rows.Count != 0)
                {
                    int i = 0;
                    foreach (DataRow dr in vTotalesDT.Rows)
                    {
                        vDecTot += Convert.ToDecimal(vTotalesDT.Rows[i][2]);
                        vDecClic += Convert.ToDecimal(vTotalesDT.Rows[i][3]);
                        vDecimal += Convert.ToDecimal(vTotalesDT.Rows[i][4]);
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
                else
                {
                    TopInfo.Visible = false;
                    GraficoInfo.Visible = false;
                    OrderDetalleInfo.Visible = false;
                    LitTitle.Text += " <<>>No existe Información para esta categoría";
                }

                

            }
            catch (Exception ex)
            {
                LitTitle.Text = "Error en obtener información " + ex.Message;
                throw;
            }
        }

        protected void OrdenTotales(string vCompanyId, out long vOrder)
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
            else
            {
                TopInfo.Visible = false;
                GraficoInfo.Visible = false;
                OrderDetalleInfo.Visible = false;
                LitTitle.Text += " <<>>No existe Información para esta categoría";
            }
           
           
		
	    }
        catch (Exception ex)
        {
            LitTitle.Text = "Error en obtener información " + ex.Message;
            throw;
        }
	}
    }
}