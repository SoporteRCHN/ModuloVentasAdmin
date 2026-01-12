using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datos;
using Microsoft.Win32;

namespace Logica
{
    public class registrosLogica
    {
        private readonly Registros enviar = new Registros();
        public bool guardarAccion(int opcion, string usuario, string modulo, string formulario, int accion, string pc)
        {
            bool valor =
            enviar.GuardaRegistro(opcion, usuario, modulo, formulario, accion, pc);

            return valor;

        }
    }
}
