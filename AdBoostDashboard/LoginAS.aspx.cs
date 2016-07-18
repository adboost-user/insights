using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace AdBoostDashboard
{
    public partial class LoginAS : System.Web.UI.Page
    {
        db vBaseDatos;
        protected void Page_Load(object sender, EventArgs e)
        {
            LbError.Text = "";
            vBaseDatos = new db();
            if (!Page.IsPostBack)
            {
                listarcompanies();
            }
            if (Request.QueryString.Count > 0)
                if (Request.QueryString["Error"] == "1")
                    LbError.Text = "Usuario Inactivo";
                else if (Request.QueryString["Error"] == "2")
                    LbError.Text = "Usuario o password Incorrecto";
                else if (Request.QueryString["Error"] == "3")
                    LbError.Text = "No tiene privilegios para realizar esta acción";


        }
        public void listarcompanies()
        {
            try
            {
                string vLista = "";
                String vQuery = "CALL GetAllCompanies();";
                DataTable vCompanies = vBaseDatos.ObtenerTabla(vQuery);
               
                //CompanyId.DataBind();                
                if (vCompanies.Rows.Count > 0)
                {
                    
                    CompanyId.DataSource = vCompanies;
                    CompanyId.DataTextField = "CompanyName";
                    CompanyId.DataValueField = "CompanyId";
                    CompanyId.DataBind();                }
                else
                {
                    CompanyId.Items.Insert(0, new ListItem("No se obtuvieron compañias", "1"));
                }
                //CompanyId.Items.Insert(0, new ListItem("Seleccione una opción", "0"));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        protected void LbSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "CALL getUsuarios('" + TxUsername.Text + "','" + TxPassword.Text + "');";
                DataTable vDatos = vBaseDatos.ObtenerTabla(vQuery);

                String vFechaExp = "";
                Boolean vEstado = false;
                Boolean vPrivilegio = false;
                string vCompany = CompanyId.Text;
                string vCompanyName = CompanyId.SelectedItem.Text;
                
                if (vCompany != "")
                {
                    if (vDatos.Rows.Count != 0)
                    {
                        Session["UsuID"] = TxUsername.Text;
                        Session["nombre"] = vDatos.Rows[0][0].ToString();
                        Session["apelido"] = vDatos.Rows[0][1].ToString();
                        Session["privilegio"] = vDatos.Rows[0][2].ToString();
                        //Session["company"] = vDatos.Rows[0][5].ToString();
                        Session["companyId"] = vCompany;
                        //Session["companyName"] = vDatos.Rows[0][6].ToString();
                        Session["companyName"] = vCompanyName;
                        vFechaExp = vDatos.Rows[0][3].ToString();
                        if (vDatos.Rows[0][4].ToString() == "1")
                        {
                            vEstado = true;
                            if (vDatos.Rows[0][2].ToString() == "1000")
                            {
                                vPrivilegio = true;
                            }
                            else
                            {
                                Response.Redirect("LoginAS.aspx?Error=3");
                            }
                        }

                    }
                    else
                        Response.Redirect("LoginAS.aspx?Error=2");

                    if (vEstado || Convert.ToDateTime(vFechaExp) < DateTime.Now)
                    {
                        Session["autenticado"] = true;
                        Response.Redirect("Default.aspx");
                    }
                    else
                        Response.Redirect("LoginAS.aspx?Error=1");

                }
                else
                {
                    Response.Redirect("LoginAS.aspx?Error=4");
                }
            }
            catch
            {
                throw;
            }
        }
    }
}