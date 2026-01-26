using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class frmCotizacionExistente : Form
    {
        clsLogica logica = new clsLogica();
        public frmCotizacionExistente()
        {
            InitializeComponent();
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            Form MensajeAdvertencia = new Form();
            using (frmBuscarClientes Mensaje = new frmBuscarClientes())
            {
                MensajeAdvertencia.StartPosition = FormStartPosition.CenterScreen;
                MensajeAdvertencia.FormBorderStyle = FormBorderStyle.None;
                MensajeAdvertencia.Opacity = .70d;
                MensajeAdvertencia.BackColor = Color.Black;
                MensajeAdvertencia.WindowState = FormWindowState.Maximized;
                MensajeAdvertencia.Location = this.Location;
                MensajeAdvertencia.ShowInTaskbar = false;
                Mensaje.Owner = MensajeAdvertencia;
                MensajeAdvertencia.Show();

                if (Mensaje.ShowDialog() == DialogResult.OK)
                {
                    if (Mensaje.ClienteId != 0)
                    {
                        txtClienteNombre.Text = Mensaje.ClienteNombre;
                        txtClienteID.Text = Mensaje.ClienteId.ToString();
                    }
                }

                MensajeAdvertencia.Dispose();
            }
        }

        private void txtClienteID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (String.IsNullOrWhiteSpace(txtClienteID.Text))
                    return;

                e.SuppressKeyPress = true;
                cargarClientes();
            }
        }
        private void cargarClientes()
        {
            string _Opcion = "BuscarPorCodigoLigero";

            ClienteENAC getClientes = new ClienteENAC
            {
                Opcion = _Opcion,
                Cliente = txtClienteID.Text,
                Nombre = txtClienteID.Text,
            };
            DataTable dtGetCliente = logica.SP_ClientesENAC(getClientes);
            if (dtGetCliente.Rows.Count > 0)
            {
                txtClienteID.Text = dtGetCliente.Rows[0]["ClienteID"].ToString();
                txtClienteNombre.Text = dtGetCliente.Rows[0]["NombreCompleto"].ToString();
            }
            else
            {
                Toast.Mostrar("No se encontro ese codigo de cliente.", TipoAlerta.Warning);
                txtClienteNombre.Text = String.Empty;
                txtClienteID.Text = String.Empty;
                txtClienteID.Focus();
            }
        }
        private void txtClienteID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void frmCotizacionExistente_Load(object sender, EventArgs e)
        {

        }
    }
}
