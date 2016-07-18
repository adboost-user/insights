using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AdBoostDashboard
{
    public partial class CrearUSuario : System.Web.UI.Page
    {
        db vBaseDatos;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string vLista = "";
                string vListUsers = "";
                string vEstado="";
                vBaseDatos = new db();
                DataTable vCompanies=GetCompanyList();
                if (vCompanies.Rows.Count > 0)
                {
                    vLista = "<option selected>Seleccione una opción</option>";
                    for (int i = 0; i < vCompanies.Rows.Count; i++)
                    {
                        //vListaCompanias += "<li><a href=\"Campaigns.aspx?id=123456\">" + dataTable.Rows[i][2].ToString() + "</a></li>";
                        vLista += "<option value='" + vCompanies.Rows[i][0].ToString() + "'>" + vCompanies.Rows[i][1].ToString() + "</option>";
                        //<option value="1">Adboost</option>
                    }
                    LitOptions.Text = vLista;
                }
                DataTable vUsuarios = GetUsuarios();
                if (vUsuarios.Rows.Count > 0)
                {
                    vListUsers = "<thead>" +
                        "<tr role='" + "row" + "'>" +
                        "<th class='" + "sorting_asc" + "'>Nombre</th>" +
                        "<th class='" + "sorting_asc" + "'>Privilegio</th>" +
                        "<th class='" + "sorting_asc" + "'>Estado</th>" +
                        "<th class='" + "sorting_asc" + "'>Compañia</th>" +
                        "</tr>" +
                        "</thead>";
                    LitTblH.Text = vListUsers;
                    vListUsers = "";
                    vListUsers = "<tbody>";
                  //  vListUsers += "<tr>";
                    for (int j = 0; j < vUsuarios.Rows.Count; j++)
                    {
                        vListUsers += "<tr>";
                        for (int i = 0; i < vUsuarios.Columns.Count; i++)
                        {
                            vEstado = vUsuarios.Rows[j][2].ToString();
                            if (i == 2)
                            {
                                if (vEstado == "1")
                                { vEstado = "Activo"; }
                                else { vEstado = "Inactivo"; }
                                vListUsers += "<td>" +vEstado + "</td>";
                            }
                            else
                            {
                                vListUsers += "<td>" + vUsuarios.Rows[j][i].ToString() + "</td>";
                            }
                        }
                        vListUsers += "</tr>";
                    }
                    //vListUsers += "</tr>";
                   // vListUsers += "<tr>";
                    /*for (int i = 0; i < vUsuarios.Columns.Count; i++)
                    {
                        vListUsers += "<td>" + vUsuarios.Rows[1][i].ToString() + "</td>";
                    }
                    vListUsers += "</tr>";
                    vListUsers += "<tr>";
                    for (int i = 0; i < vUsuarios.Columns.Count; i++)
                    {
                        vEstado=vUsuarios.Rows[2][i].ToString();
                        if (vEstado == "1")
                        {
                            vEstado = "Activo";
                        }
                        else
                        {
                            vEstado = "Inactivo";
                        }
                        vListUsers += "<td>" + vUsuarios.Rows[2][i].ToString() + "</td>";
                    }
                    vListUsers += "</tr>";
                    vListUsers += "<tr>";
                    for (int i = 0; i < vUsuarios.Columns.Count; i++)
                    {
                        vListUsers += "<td>" + vUsuarios.Rows[3][i].ToString() + "</td>";
                    }
                    vListUsers += "</tr>";*/

                    vListUsers += "</tbody>";

                    LitTblB.Text = vListUsers;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public DataTable GetUsuarios()
        {
            try
            {
                String vQuery = "CALL GetAllUsers();";
                DataTable vDatos = vBaseDatos.ObtenerTabla(vQuery);
                return vDatos;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable GetCompanyList()
        {
            try
            {
                String vQuery = "CALL GetAllCompanies();";
                DataTable vDatos = vBaseDatos.ObtenerTabla(vQuery);
                return vDatos;
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
                string vUsuario = TxtUser.Text;
                string vPassword = TxtPassword.Text;
                string vNombre = TxtNombre.Text;
                string vPri=Request.Form["privilegio"];
                string vCompany = Request.Form["Company"];
                //string vQuery = "CALL SetUsuario('" + TxUsername.Text + "','" + TxPassword.Text + "');";
                string vQuery = "CALL SetUsuario('" + vUsuario + "','" + vPassword + "','" + vNombre +"','"+
                               "','" + vPri + "','" + vCompany + "');";

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