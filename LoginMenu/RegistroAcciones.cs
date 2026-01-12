using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuloLanzador.LoginMenu
{
    public class RegistroAcciones
    {
        registrosLogica registros = new registrosLogica(); // LOGICA DE REGISTRO DE ACCIONES

        public void Registrar(int opcion, string usuario, string modulo, string formulario, int accion, string pc)
        {

            bool result = registros.guardarAccion(opcion, usuario, modulo, formulario, accion, pc);
        }
    }
}
