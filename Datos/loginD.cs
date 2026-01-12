using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class loginD
    {
        private BD_Conexion Conexion = new BD_Conexion();
        SqlDataReader leer = null;
        DataTable tabla = new DataTable();
        String ControlResultado = "";
        String CajaResultado = "";
        String SucursalResultado = "";

        SqlDataAdapter adaptador = new SqlDataAdapter();
        SqlParameter parametro = new SqlParameter();
        SqlCommand comando = new SqlCommand();


        public DataTable ConsultaEncabezado(string usuario, string modulo)
        {
            comando.Parameters.Clear();
            tabla.Rows.Clear();
            tabla.Clear();
            tabla = new DataTable();

            comando.Connection = Conexion.AbrirConexion(3);
            comando.Parameters.AddWithValue("@usuario", usuario);
            comando.Parameters.AddWithValue("@modulo", modulo);
            comando.CommandText = "empleados.SP_ObtenerEncabezadoGeneral";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            Conexion.CerrarConexion();

            return tabla;
        }


        public String EnviarUsuarioContrasena(string user, string contra, string modulo, out string mensajeResultado)
        {
            int filasIngresadas = 0;
            comando.Parameters.Clear();

            try
            {
                comando.Connection = Conexion.AbrirConexion(2);
                comando.Parameters.AddWithValue("@usuario", user);
                comando.Parameters.AddWithValue("@contrasena", contra);
                comando.Parameters.AddWithValue("@modulo", modulo);
                comando.Parameters.Add("@mensajeResultado", SqlDbType.VarChar, 50);
                comando.Parameters["@mensajeResultado"].Direction = ParameterDirection.Output;
                comando.CommandText = "RCCONFIG.Empresa.SP_ValidaLogin";
                comando.CommandType = CommandType.StoredProcedure;
                filasIngresadas = comando.ExecuteNonQuery();
                mensajeResultado = comando.Parameters["@mensajeResultado"].Value.ToString();

            }
            catch (SqlException ex)
            {
                mensajeResultado = ex.Number.ToString() + " " + ex.Message.ToString();
            }
            catch (Exception ex)
            {
                mensajeResultado = ex.ToString() + " " + ex.Message.ToString();
            }
            finally
            {
                Conexion.CerrarConexion();

            }
            return mensajeResultado;

        }
    }
}
