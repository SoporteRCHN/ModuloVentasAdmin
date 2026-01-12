using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Datos
{

    public class BD_Conexion
    {
        public static string servidor = "192.168.1.180";
        public static string servidorENAC = "192.168.1.52";
        public static string servidorINTER = "192.168.1.50";
        private static string cadena = "";
        private static string cadena1 = "Data Source = 190.4.7.154; Initial Catalog=RCRH;User ID=sarh; Password=Mkc7wAq\\k$8%ia4.rTgk";
        private static string cadena2 = "Data Source = 192.168.1.179; Initial Catalog=RCCONFIG;User ID=saconfig; Password=xfZ9£K<GPvm6@";
        private static string cadena3 = "Data Source = 192.168.1.179; Initial Catalog=RCRH;User ID=sa; Password=Adm1n1s7r4d0r";
        private SqlConnection Conexion = new SqlConnection(cadena);

        public SqlConnection AbrirConexion(int valor)
        {
            if (valor == 1)
            {
                Conexion = new SqlConnection("Data Source = " + servidor + "; Initial Catalog=RCRH;User ID=sarh; Password=Qwerty!2024");
            }
            else if (valor == 2)
            {
                Conexion = new SqlConnection("Data Source = " + servidor + "; Initial Catalog=RCCONFIG;User ID=saconf; Password=Qwerty!2024");
            }
            else if (valor == 3)
            {
                Conexion = new SqlConnection("Data Source =  " + servidor + "; Initial Catalog=RCRH;User ID=sa; Password=Adm1n1s7r4d0r");
            }
            else if (valor == 4)
            {
                Conexion = new SqlConnection("Data Source =  " + servidorENAC + "; Initial Catalog=ENAC;User ID=sa; Password=Adm1n1s7r4d0r");
            }
            else if (valor == 5)
            {
                Conexion = new SqlConnection("Data Source =  " + servidorINTER + "; Initial Catalog=ENAC;User ID=sa2; Password=Adm1n1s7r4d0r");
            }
            try
            {
                if (Conexion.State == ConnectionState.Closed)
                    Conexion.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            return Conexion;
        }
        public SqlConnection CerrarConexion()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();
            return Conexion;
        }
    }
}
