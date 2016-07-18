using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace AdBoostDashboard
{
    public class db
    {
        MySqlConnection vConexion;

        public db()
        {
            vConexion = new MySqlConnection(ConfigurationManager.AppSettings["MysqlServer"]);
        }

        public String ObtenerResultado(String vQuery)
        {
            String vResultado = "";
            try
            {
                DataTable vDataTable = new DataTable();
                MySqlDataAdapter vDataAdapter = new MySqlDataAdapter(vQuery, vConexion);
                vDataAdapter.Fill(vDataTable);

                if(vDataTable.Rows.Count != 0)
                    vResultado = vDataTable.Rows[0][0].ToString();
            }
            catch (Exception Ex)
            {
                vResultado = Ex.Message;
            }
            return vResultado;
        }

        public DataTable ObtenerTabla(String vQuery)
        {
            DataTable vResultado = new DataTable(); ;
            try
            {
                
                MySqlDataAdapter vDataAdapter = new MySqlDataAdapter(vQuery, vConexion);
                vDataAdapter.Fill(vResultado);
            }
            catch
            {
                vResultado = null;
            }
            return vResultado;
        }

        public bool Ejecutar(String vQuery)
        {
            bool vEjecutado = false;
            try
            {
                vConexion.Open();
                MySqlCommand vCommand = new MySqlCommand(vQuery, vConexion);
                int vResultado =  vCommand.ExecuteNonQuery();
                vConexion.Close();

                if (Convert.ToBoolean(vResultado))
                    vEjecutado = true;
            }
            catch (Exception ex)
            {
                vEjecutado = false;
            }
            return vEjecutado;
        }
    }
}