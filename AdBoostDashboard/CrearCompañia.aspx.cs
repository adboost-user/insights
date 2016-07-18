using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdBoostDashboard
{
    public partial class CrearCompañia : System.Web.UI.Page
    {
        db vBaseDatos;
        protected void Page_Load(object sender, EventArgs e)
        {
            vBaseDatos = new db();
           
        }
        protected void LbSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string vUsuario = Convert.ToString(Session["UsuId"]);
                string vQuery = "CALL setCompany('" + TxtIdCompany.Text + "','" + TxtNombre.Text + "','" + TxtContacto.Text + "','" +
                    TxtEmail.Text + "','" + TxtTelefono.Text + "','" + vUsuario + "');";

                bool vDatos = vBaseDatos.Ejecutar(vQuery);
                if (vDatos == true)
                {
                }

            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
    }
}