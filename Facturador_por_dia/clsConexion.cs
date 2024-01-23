using MySqlConnector;
using System;
using System.Configuration;
using System.Data;

namespace Facturador_por_dia
{
    public class clsConexion
    {
        private string conexion;

        public clsConexion()
        {
            conexion = ConfigurationManager.ConnectionStrings["db_conn"].ConnectionString;
        }

        public int EjecutarConsulta(string consulta, MySqlParameter[] parametros, CommandType tipo)
        {
            var cn = !consulta.StartsWith("SELECT ", StringComparison.CurrentCultureIgnoreCase) ? new MySqlConnection(conexion) : new MySqlConnection(ConfigurationManager.ConnectionStrings["db_conn_read"].ConnectionString);
            var valor = 0;
            cn.Open();
            var cmd = new MySqlCommand(consulta, cn)
            {
                CommandTimeout = 0,
                CommandType = tipo
            };
            cmd.Parameters.AddRange(parametros);
            valor = cmd.ExecuteNonQuery();
            cn.Close();
            cn.Dispose();
            return valor;
        }

        public DataTable TraeDataTable(string consulta, MySqlParameter[] parametros, CommandType tipo)
        {
            var cn = !consulta.StartsWith("SELECT ", StringComparison.CurrentCultureIgnoreCase) ? new MySqlConnection(conexion) : new MySqlConnection(ConfigurationManager.ConnectionStrings["db_conn_read"].ConnectionString);
            var dt = new DataTable();
            cn.Open();
            var cmd = new MySqlCommand(consulta, cn)
            {
                CommandTimeout = 0,
                CommandType = tipo
            };
            cmd.Parameters.AddRange(parametros);
            var rd = cmd.ExecuteReader();
            dt.Load(rd);
            rd.Close();
            cn.Dispose();
            return dt;
        }
    }

}