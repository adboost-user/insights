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

namespace AdBoostDashboard
{
    public partial class Dashboard : System.Web.UI.MasterPage
    {
        DfpUser user;
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Convert.ToBoolean(Session["autenticado"]))
                Response.Redirect("Login.aspx");

            int vPrivilegio = Convert.ToInt32(Session["privilegio"]);

            String vNombre = Convert.ToString(Session["Nombre"]);
            LitNombre.Text = vNombre;
            String vCompanyID = Convert.ToString(Session["companyId"]);
            String vCompanyName = Convert.ToString(Session["companyName"]);
            LitCompany.Text = vCompanyName;
            HideDiv.InnerText = vCompanyID;

            //menu();

            user = new DfpUser();
            String vListaCampaigns = "<li><a href=\"Campaigns.aspx?id=123456\">La Tribuna</a></li>" +
                "<li><a href=\"#\">El Heraldo</a></li>";
           
            LbCampañasCargadas.Text = vListaCampaigns;
            //onGetCompaniesButtonClick();
            String vListaOrders = "<li><a href=\"ordenes.aspx?id=1\">Todas las Campañas</a></li>" +
                "<li><a href=\"ordenes.aspx?id=2\">Ejecutando</a></li>" +
                "<li><a href=\"ordenes.aspx?id=3\">Completadas</a></li>" +
                "<li><a href=\"ordenes.aspx?id=4\">Inactivas</a></li>"+
            "<li><a href=\"ordenes.aspx?id=5\">Archivadas</a></li>"+
            "<li><a href=\"ordenes.aspx?id=6\">Pausadas</a></li>";
            LitOrders.Text = vListaOrders;
            Literal7.Text = vListaOrders;
            
            
        }
        private void menu()
        {
            String vOrdenes =   "<li ><a href=\"#\">"+
                                "<i class=\"fa fa-sitemap\"></i>"+
                                "<span class=\"nav-label\">Ordenes</span> <span class=\"fa arrow\"></span></a>"+
                                "<ul class=\"nav nav-second-level\">"+
                                "<asp:Literal ID=\"LbCampañasCargadas\" runat=\"server\"></asp:Literal></ul></li>";
            //LitMenuOrder.Text = vOrdenes;     
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
        protected void onGetCompaniesButtonClick()
        {
            ConfigureUserForOAuth();
            try
            {
                // Get the CompanyService.
                CompanyService companyService =
                    (CompanyService)user.GetService(DfpService.v201602.CompanyService);

                // Set defaults for page and statement.
                CompanyPage page = new CompanyPage();
                StatementBuilder statementBuilder = new StatementBuilder()
                    .OrderBy("id ASC")
                    .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT);

                DataTable dataTable = new DataTable();
                dataTable.Columns.AddRange(new DataColumn[] {
            new DataColumn("N°", typeof(int)),
            new DataColumn("Comp Id", typeof(long)),
            new DataColumn("Nombre", typeof(string)),
            new DataColumn("Tipo", typeof(string)),            
            new DataColumn("Correo Electronico", typeof(string))        
        });
                do
                {
                    // Get companies by statement.
                    page = companyService.getCompaniesByStatement(statementBuilder.ToStatement());

                    if (page.results != null && page.results.Length > 0)
                    {
                        int i = page.startIndex;
                        foreach (Company company in page.results)
                        {
                            DataRow dataRow = dataTable.NewRow();
                            dataRow.ItemArray = new object[] { i + 1, company.id, company.name, company.type, company.email };
                            dataTable.Rows.Add(dataRow);
                            i++;
                        }
                    }
                    statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
                } while (statementBuilder.GetOffset() < page.totalResultSetSize);

                if (dataTable.Rows.Count > 0)
                {
                    String vListaCompanias = "";

                    
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        vListaCompanias += "<li><a href=\"Campaigns.aspx?id=123456\">" + dataTable.Rows[i][2].ToString() + "</a></li>";
                    }
                    LitCompañias.Text = vListaCompanias;
                }
                else
                {
                    Response.Write("No se encontraron compañias");
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("Error en obtener compañias \"{0}\"",
                ex.Message));
            }
        }
    }
}