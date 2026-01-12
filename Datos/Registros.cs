using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class Registros
    {
        private BD_Conexion Conexion = new BD_Conexion();

        SqlDataReader leer = null;
        DataTable tabla = new DataTable();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        SqlParameter parametro = new SqlParameter();
        SqlCommand comando = new SqlCommand();


        public bool GuardaRegistro(int opcion, string usuario, string modulo, string formulario, int accion, string pc)
        {
            int filasIngresadas = 0;
            comando.Parameters.Clear();

            try
            {
                comando.Connection = Conexion.AbrirConexion(3);
                comando.Parameters.AddWithValue("@opcion", opcion);
                comando.Parameters.AddWithValue("@usuario", usuario);
                comando.Parameters.AddWithValue("@modulo", modulo);
                comando.Parameters.AddWithValue("@formulario", formulario);
                comando.Parameters.AddWithValue("@accion", accion);
                comando.Parameters.AddWithValue("@pc", pc);
                comando.CommandText = "[Registros].[SP_RegistroAcciones]";
                comando.CommandType = CommandType.StoredProcedure;
                filasIngresadas = comando.ExecuteNonQuery();

                return true;
            }
            catch (SqlException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Conexion.CerrarConexion();

            }
        }
    }
}
