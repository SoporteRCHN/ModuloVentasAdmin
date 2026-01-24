using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class frmMensajePersonalizado : Form
    {
        HttpClient httpClient = new HttpClient();
        public string _RutaArchivo;
        public frmMensajePersonalizado(string RutaArchivo)
        {
            InitializeComponent();
            _RutaArchivo = RutaArchivo;
        }

        private void txtNumero_KeyUp(object sender, KeyEventArgs e)
        {
            string input = txtNumero.Text;

            if (!Regex.IsMatch(input, "^[0-9]+$") && txtNumero.Text != "")
            {
                MessageBox.Show("Por favor, ingrese solo números.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNumero.Text = "";
            }

            e.Handled = true;
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            EnviarDocumento(_RutaArchivo);
        }
        private async void EnviarDocumento(string _RutaArchivo)
        {
            if (txtNumero.Text.Trim().Length != 8)
            {
                Toast.Mostrar("Número debe tener 8 caracteres", TipoAlerta.Warning);
                return;
            }

            try
            {
                string url = "https://app.rapidocargo.online:3000/emitirEventoCotizacionNegociacion/" + _RutaArchivo + "/" + txtNumero.Text;
                string response = await httpClient.GetStringAsync(url);
                Toast.Mostrar("Documento enviado exitosamente.", TipoAlerta.Success);
                this.Close();
            }
            catch (Exception)
            {
                Toast.Mostrar("Error al enviar cotizacion", TipoAlerta.Success);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txtNumero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; 
                btnProcesar.Focus(); 
            }
        }
    }
}
