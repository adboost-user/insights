using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdBoostDashboard
{
    public class Logs
    {

        db vBaseDatos;
        public Logs()
        {
            vBaseDatos = new db();
        }

        public void IngresarLog(String vUsuario, String vAcceso)
        {
            try
            {
                String vQuery = "CALL logsIngreso('" + vUsuario + "','" + vAcceso + "');";
                vBaseDatos.Ejecutar(vQuery);
            }
            catch 
            {
                
            }
        }

    }
}