using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace AdBoostDashboard
{
    public partial class login : System.Web.UI.Page
    {
        db vBaseDatos;

        protected void Page_Load(object sender, EventArgs e)
        {
            LbError.Text = "";
            vBaseDatos = new db();
            if(Request.QueryString.Count > 0)
                if (Request.QueryString["Error"] == "1")
                    LbError.Text = "Usuario Inactivo";
                else if (Request.QueryString["Error"] == "2")
                    LbError.Text = "Usuario o password Incorrecto";
                
        }

        protected void LbSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String vQuery = "CALL getUsuarios('" + TxUsername.Text + "','" + TxPassword.Text + "');";
                DataTable vDatos = vBaseDatos.ObtenerTabla(vQuery);

                String vFechaExp = "";
                Boolean vEstado = false;
                if (vDatos.Rows.Count != 0)
                {
                    Session["UsuID"] = TxUsername.Text;
                    Session["nombre"] = vDatos.Rows[0][0].ToString();
                    Session["apelido"] = vDatos.Rows[0][1].ToString();
                    Session["privilegio"] = vDatos.Rows[0][2].ToString();
                    Session["companyId"] = vDatos.Rows[0][5].ToString();
                    Session["companyName"] = vDatos.Rows[0][6].ToString();
                    vFechaExp = vDatos.Rows[0][3].ToString();
                    if (vDatos.Rows[0][4].ToString() == "1")
                        vEstado = true;
                }
                else
                    Response.Redirect("Login.aspx?Error=2");

                if(vEstado || Convert.ToDateTime(vFechaExp) < DateTime.Now)
                {
                    Session["autenticado"] = true;
                    Response.Redirect("Default.aspx");
                }
                else
                    Response.Redirect("Login.aspx?Error=1");

            }
            catch
            {
                throw;
            }
        }
    }
}