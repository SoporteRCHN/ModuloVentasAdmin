using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public class login
    {

        private loginD loginn = new loginD();
        private readonly EnviarConsultas enviar = new EnviarConsultas();
        public DataTable CargarDatos(int Usuario, int CRUD, int OpcionSP, int Opcion, string valor)
        {
            DataTable tabla = new DataTable();
            switch (CRUD)
            {
                case 1:
                    tabla = enviar.CargarDatos(Usuario, OpcionSP, Opcion, valor); //"Paso todos los llamados por aqui"
                    break;

            }
            return tabla;
        }
        public string Validacion(string user, string contra, string modulo, out string mensajeResultadoDetalle)
        {
            loginn.EnviarUsuarioContrasena(user, contra, modulo, out mensajeResultadoDetalle);
            return mensajeResultadoDetalle;

        }
        public DataTable DatosEncabezado(string usuario, string modulo)
        {
            DataTable tabla = new DataTable();
            tabla = loginn.ConsultaEncabezado(usuario, modulo);
            return tabla;
        }
    }
}
