using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class frmAumentoPrecios : Form
    {
        public DataTable dtClientes = new DataTable();
        DataTable dtGetOmitidos = new DataTable();
        public bool _EstadoInserta, _MostrarMensaje = false;
        DataTable dtEnviar = new DataTable();
        clsLogica logica = new clsLogica();

        public frmAumentoPrecios()
        {
            InitializeComponent();
            
            getClientesTotales();

            dgvClientes.Columns["Cliente"].Width = 120;
            dgvClientes.Columns["Nombre"].Width = 430;
            dgvClientes.Columns["Quitar"].Width = 50;

            dgvClientes.Columns["Cliente"].ReadOnly = true;
            dgvClientes.Columns["Nombre"].ReadOnly = true;

            dgvClientes.Columns["Quitar"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvClientes.Columns["Quitar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private void getClientesTotales() 
        {
            ClienteENAC getTotales = new ClienteENAC
            {
                Opcion = "getClientesTotales"
            };
            DataTable dtTotales = logica.SP_ClientesENAC(getTotales);
            if (dtTotales.Rows.Count > 0) 
            {
                lblTotal.Text = dtTotales.Rows[0]["Cliente"].ToString();
                lblCambio.Text = dtTotales.Rows[0]["Cliente"].ToString();
            }
        }

        private void cargarClientes()
        {
            ClienteENAC getClientes = new ClienteENAC
            {
                Opcion = "getClientesCostosEspecifico",
                Cliente = txtBuscar.Text,
            };
            dtClientes = logica.SP_ClientesENAC(getClientes);

            if (dtClientes.Rows.Count > 0)
            {

                foreach (DataRow row in dtClientes.Rows)
                {
                    bool existe = dgvClientes.Rows.Cast<DataGridViewRow>().Any(r => r.Cells["Cliente"].Value?.ToString() == row["Cliente"].ToString());

                    if (!existe)
                    {
                        if (rdbMasivo.Checked)
                        {
                            dgvClientes.Rows.Add(row["Cliente"], row["Nombre"]);
                        }
                        else
                        {
                            dgvClientes.Rows.Add(false, row["Cliente"], row["Nombre"], "", null);
                        }
                        txtBuscar.Text = String.Empty;
                    }
                    else
                    {
                        Toast.Mostrar("Cliente ya esta agregado", TipoAlerta.Warning);
                        txtBuscar.Text = String.Empty;
                    }
                }

                lblOmite.Text = dgvClientes.Rows.Count.ToString();
                lblCambio.Text = (Convert.ToDecimal(lblTotal.Text) - Convert.ToDecimal(lblOmite.Text)).ToString();
            }
            else
            {
                Toast.Mostrar("Cliente no tiene negociacion especial", TipoAlerta.Warning);
                txtBuscar.Text = String.Empty;
            }
        }

        private void dgvClientes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvClientes.IsCurrentCellDirty)
            {
                dgvClientes.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvClientes.Columns["Quitar"].Index && e.RowIndex >= 0)
            {
                dgvClientes.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (String.IsNullOrWhiteSpace(txtBuscar.Text))
                    return;

                e.SuppressKeyPress = true;
                cargarClientes();
                txtBuscar.Focus();
            }
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            mostrarClientes();
        }

        private void rdbNombre_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNombre.Checked == true)
            {
                mostrarClientes();
            }
        }
        private void mostrarClientes()
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
                        string _Nombre = Mensaje.ClienteNombre;
                        string _Cliente = Mensaje.ClienteId.ToString();

                        bool existe = dgvClientes.Rows.Cast<DataGridViewRow>().Any(r => r.Cells["Cliente"].Value?.ToString() == _Cliente);

                        if (!existe)
                        {
                            if (rdbMasivo.Checked) 
                            {
                                dgvClientes.Rows.Add(_Cliente, _Nombre);
                            }
                            else
                            {
                                dgvClientes.Rows.Add(false,_Cliente, _Nombre, "",null);
                            }
                            lblOmite.Text = dgvClientes.Rows.Count.ToString();
                            lblCambio.Text = (Convert.ToDecimal(lblTotal.Text) - Convert.ToDecimal(lblOmite.Text)).ToString();
                        }
                        else
                        {
                            Toast.Mostrar("Cliente ya ha sido agregado.", TipoAlerta.Warning);
                        }
                    }
                    else
                    {
                        Toast.Mostrar("No se selecciono cliente.", TipoAlerta.Warning);
                    }
                }
                MensajeAdvertencia.Dispose();
            }

            rdbCodigo.Checked = true;
            txtBuscar.Focus();
        }
        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Validamos que sea una fila válida
            if (e.RowIndex >= 0 && dgvClientes.Columns[e.ColumnIndex].Name == "Quitar")
            {
                dgvClientes.Rows.RemoveAt(e.RowIndex);
                lblOmite.Text = dgvClientes.Rows.Count.ToString();
                lblCambio.Text = (Convert.ToDecimal(lblTotal.Text)- Convert.ToDecimal(lblOmite.Text)).ToString();
            }

            // Validar que sea el header de la columna Seleccionar
            if (e.RowIndex == -1 && dgvClientes.Columns[e.ColumnIndex].Name == "Seleccionar")
            {
                // Determinar si actualmente hay filas marcadas
                bool todosMarcados = dgvClientes.Rows.Cast<DataGridViewRow>()
                    .All(r => Convert.ToBoolean(r.Cells["Seleccionar"].Value) == true);

                // Si todos están marcados, desmarcamos; si no, marcamos todos
                foreach (DataGridViewRow row in dgvClientes.Rows)
                {
                    row.Cells["Seleccionar"].Value = !todosMarcados;
                }
            }
        }
        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                return;
            }

            e.Handled = true;
        }

        private void txtValor_Enter(object sender, EventArgs e)
        {
            if (txtValor.Text == "0")
            {
                txtValor.Text = string.Empty;
            }
        }

        private async void btnAplicar_Click(object sender, EventArgs e)
        {
            if (!rdbMasivo.Checked && dgvClientes.Rows.Count == 0)
            {
                Toast.Mostrar("No hay clientes cargados para aplicar aumento específico.", TipoAlerta.Warning);
                return;
            }

            if (!rdbMasivo.Checked)
            {
                bool vacio = false;
                foreach (DataGridViewRow row in dgvClientes.Rows)
                {
                    var valor = row.Cells["ValorExtra"].Value;

                    if (valor == null || string.IsNullOrWhiteSpace(valor.ToString()) || valor.ToString() == "0" || valor.ToString() == "0.00")
                    {
                        row.Cells["ValorExtra"].Style.BackColor = Color.FromArgb(242, 175, 138);
                        vacio = true;
                    }
                    else
                    {
                        row.Cells["ValorExtra"].Style.BackColor = Color.White;
                    }
                }

                if (vacio)
                {
                    Toast.Mostrar("Existen valores vacíos que deben ser completados.", TipoAlerta.Error);
                    dgvClientes.ClearSelection();
                    return;
                }
            }

            string tipo = rdbMasivo.Checked ? "MASIVO" : "ESPECIFICO";
            string opcion = rdbMasivo.Checked ? "ActualizarTodos" : "ActualizarEspecifico";

            DialogResult result = MessageBox.Show($"¿Desea aplicar el aumento {tipo} de precios?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
               bool respuesta = enviarPeticionAumento(opcion);
                if (respuesta)
                {
                    Toast.Mostrar("Solicitud de aprobación enviada exitosamente.", TipoAlerta.Success);
                    Limpiar();
                }
                else
                {
                    Toast.Mostrar("Ocurrió un error al momento de enviar la solicitud.", TipoAlerta.Error);
                }

            }
            else
            {
                Toast.Mostrar("Se canceló el aumento de precios.", TipoAlerta.Info);
            }
        }

        private void Limpiar() 
        {
            txtValor.Text = string.Empty;
            txtBuscar.Text = string.Empty;
            dtEnviar = null;
            dtClientes = null;
            dtEnviar = null;
            _EstadoInserta = false;
            dgvClientes.DataSource = null;
            dgvClientes.Rows.Clear();
            lblCambio.Text = "0";
            lblOmite.Text = "0";
            lblTotal.Text = "0";
        }

        private bool enviarPeticionAumento(string _TipoAumento)
        {
            
            int _AumentoID = (rdbMasivo.Checked) ? 1 : 2;
            string _Comentario = (rdbMasivo.Checked) ? "Ingresando solicitud Masiva" : "Ingresando solicitud Especifica";
            AumentoPreciosEncabezado getClientes = new AumentoPreciosEncabezado
            {
                Opcion = "Agregar",
                TipoAumento = _AumentoID,
                ValorAumento = Convert.ToDecimal(txtValor.Text),
                EstadoAprobacion = 1, //Solicitado
                Comentario = _Comentario,
                UPosteo = DynamicMain.usuarionlogin,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true
            };

            int _EncabezadoID = 0;
            
            DataTable dtEnviar = logica.SP_AumentoPreciosEncabezado(getClientes);

            if (dtEnviar.Rows.Count > 0 && dtEnviar.Rows[0]["Estado"].ToString() == "1") 
            {
                 _EncabezadoID = Convert.ToInt32(dtEnviar.Rows[0]["UltimoID"]);
            }

            AumentoPreciosDetalle getDetalle = new AumentoPreciosDetalle
            {
                Opcion = "Agregar",
                AumentoID = _EncabezadoID,
                ClienteID = "",//ClienteID
                ValorAumento = Convert.ToDecimal(txtValor.Text),
                Comentario = _Comentario,
                UPosteo = DynamicMain.usuarionlogin,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true
            };

            if (_TipoAumento == "ActualizarTodos")
            {
                List<string> clientesSeleccionados = new List<string>();

                if (dgvClientes.Rows.Count > 0) 
                {
                    foreach (DataGridViewRow row in dgvClientes.Rows)
                    {
                        clientesSeleccionados.Add(row.Cells["Cliente"].Value.ToString());
                    }
                    getDetalle.ClienteID = string.Join(",", clientesSeleccionados);
                    getDetalle.Comentario = "Aumento Masivo, Omitir estos clientes";
                    DataTable dtDetalle = logica.SP_AumentoPreciosDetalle(getDetalle);
                    _EstadoInserta = (dtDetalle.Rows.Count > 0 && dtDetalle.Rows[0]["Estado"].ToString() == "1") ? true : false;
                }
                else
                {
                    foreach (DataGridViewRow row in dgvClientes.Rows)
                    {
                        clientesSeleccionados.Add(row.Cells["Cliente"].Value.ToString());
                    }
                    getDetalle.ClienteID = "0"; //Cliente no definido
                    getDetalle.Comentario = "Aumento Masivo, No se definieron clientes para omitir";
                    DataTable dtDetalle = logica.SP_AumentoPreciosDetalle(getDetalle);
                    _EstadoInserta = (dtDetalle.Rows.Count > 0 && dtDetalle.Rows[0]["Estado"].ToString() == "1") ? true : false;
                }
                
            }
            else if (_TipoAumento == "ActualizarEspecifico")
            {
               
                foreach (DataGridViewRow row in dgvClientes.Rows)
                {
                    getDetalle.ClienteID = row.Cells["Cliente"].Value.ToString();
                    getDetalle.ValorAumento = Convert.ToDecimal(row.Cells["ValorExtra"].Value);
                    getDetalle.Comentario = "Aumento Especifico, Solo aumentar estos clientes";
                    DataTable dtDetalle = logica.SP_AumentoPreciosDetalle(getDetalle);
                    _EstadoInserta = (dtDetalle.Rows.Count > 0 && dtDetalle.Rows[0]["Estado"].ToString() == "1") ? true : false;
                }
            }

            return _EstadoInserta;
        }
        private void revisarOmitidos() 
        {
            AumentoPreciosEncabezado getOmitidos = new AumentoPreciosEncabezado
            {
                Opcion = "ClientesOmitidos",
            };

            dtGetOmitidos = logica.SP_AumentoPreciosEncabezado(getOmitidos);
            if (dtGetOmitidos.Rows.Count >0) 
            {
                DialogResult resultado = ToastDialog.Mostrar(dtGetOmitidos.Rows.Count.ToString() + " clientes fueron omitidos en la ultima actualizacion, deseas cargarlos?", TipoAlerta.Error);

                if (resultado == DialogResult.OK)
                {
                    dgvClientes.Rows.Clear();
                    foreach (DataRow row in dtGetOmitidos.Rows)
                    {
                        dgvClientes.Rows.Add(false, row["Codigo"], row["Nombre"], "", null);
                    }
                }
                else
                {
                    _MostrarMensaje = true;
                    return;
                }
            }
        }
        private void rdbEspecifico_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbEspecifico.Checked)
            {
                lblTotal.Visible = false;
                label3.Visible = false;

                lblCambio.Visible = false;
                label9.Visible = false;

                label4.Text = "Clientes Para Cambio:";


                // Evitar duplicar columnas si ya existen
                if (!dgvClientes.Columns.Contains("Seleccionar"))
                {
                    // Columna de selección (checkbox)
                    DataGridViewCheckBoxColumn colSeleccionar = new DataGridViewCheckBoxColumn();
                    colSeleccionar.Name = "Seleccionar";
                    colSeleccionar.HeaderText = "✔"; // ícono check en el header
                    colSeleccionar.Width = 50;
                    colSeleccionar.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    colSeleccionar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgvClientes.Columns.Insert(0, colSeleccionar); // al inicio
                }

                if (!dgvClientes.Columns.Contains("ValorExtra"))
                {
                    // Columna penúltima tipo TextBox
                    DataGridViewTextBoxColumn colValorExtra = new DataGridViewTextBoxColumn();
                    colValorExtra.Name = "ValorExtra";
                    colValorExtra.HeaderText = "Valor Extra";
                    colValorExtra.Width = 100;
                    colValorExtra.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    // Insertar en penúltima posición
                    dgvClientes.Columns.Insert(dgvClientes.Columns.Count - 1, colValorExtra);

                    dgvClientes.Columns["Cliente"].Width = 120;
                    dgvClientes.Columns["Nombre"].Width = 280;
                    dgvClientes.Columns["Quitar"].Width = 50;
                }
                if (_MostrarMensaje == false) { revisarOmitidos(); }
            }
            else
            {
                // Si desmarcas el radio, puedes quitar las columnas
                if (dgvClientes.Columns.Contains("Seleccionar"))
                    dgvClientes.Columns.Remove("Seleccionar");

                if (dgvClientes.Columns.Contains("ValorExtra"))
                    dgvClientes.Columns.Remove("ValorExtra");

                dgvClientes.Columns["Cliente"].Width = 120;
                dgvClientes.Columns["Nombre"].Width = 430;
                dgvClientes.Columns["Quitar"].Width = 50;
            }
        }

        private void txtValor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Evitar beep del Enter
                e.SuppressKeyPress = true;

                string valor = txtValor.Text.Trim();

                if (!string.IsNullOrEmpty(valor))
                {
                    foreach (DataGridViewRow row in dgvClientes.Rows)
                    {
                        // Validar que la columna Seleccionar esté marcada
                        bool seleccionado = false;

                        if (row.Cells["Seleccionar"] is DataGridViewCheckBoxCell chkCell &&
                            chkCell.Value != null &&
                            chkCell.Value.ToString() == "True")
                        {
                            seleccionado = true;
                        }

                        if (seleccionado)
                        {
                            row.Cells["ValorExtra"].Value = valor;
                        }
                    }

                    // Limpiar el TextBox después de aplicar
                    txtValor.Clear();

                    foreach (DataGridViewRow row in dgvClientes.Rows)
                    {
                        row.Cells["Seleccionar"].Value = false;
                    }
                }
            }
        }

        private void txtValor_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtValor.Text) || String.IsNullOrWhiteSpace(txtValor.Text))
            {
                txtValor.Text = "0";
            }
        }

        private void frmAumentoPrecios_Load(object sender, EventArgs e)
        {

        }

        private void rdbMasivo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMasivo.Checked) 
            {
                lblTotal.Visible = true;
                label3.Visible = true;

                lblCambio.Visible = true;
                label9.Visible = true;

                label4.Text = "Clientes Omitidos:";
            }
        }
    }
}
