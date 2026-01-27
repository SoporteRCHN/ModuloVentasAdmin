using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static ModuloVentasAdmin.frmCotizacionDinamica;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Rectangle = iTextSharp.text.Rectangle;
using iTextSharp.text.pdf.parser;
using ModuloVentasAdmin;
using System.Runtime.CompilerServices;
using Org.BouncyCastle.Asn1.Cmp;

namespace ModuloVentasAdmin
{
    public partial class frmCotizaciones : Form
    {
        public int OrigenID, DestinoID, ProductoID, _EncabezadoID, _DetalleID = 0;
        public string NombreOrigen, NombreDestino, NombreProducto, _RutaArchivo, _NombreArchivo = "";
        public bool _TerminosExisten, _estaActualizando, datosCargados = false;

        AutoCompleteStringCollection listaNombres = new AutoCompleteStringCollection();

        public DataTable dtOrigen = new DataTable();
        public DataTable dtDestino = new DataTable();
        public DataTable dtProducto = new DataTable();
        public DataTable dtTerminos = new DataTable();
        public DataTable dtTipos = new DataTable();
        public DataTable dtImpuestos = new DataTable();
        public DataTable dtGetCotizaciones = new DataTable();
        public DataTable dtEncabezado = new DataTable();
        public DataTable dtDetalle = new DataTable();
        public DataTable dtGruposProductos = new DataTable();
        public DataTable dtDescuentosLocal = new DataTable();

        // Variables de clase para mantener lo que generaste
        public List<CotizacionRegistro> registrosGenerados;
        public List<ProductoSeleccionado> productosGenerados;
        public List<CiudadOrigen> origenesGenerados;
        public List<CiudadDestino> destinosGenerados;

        clsLogica logica = new clsLogica();

        public frmCotizaciones()
        {
            InitializeComponent();
            cargarOrigen();
            //cargarDestino();
           // cargarProductos();
            cargarTipos();
            cargarTerminos();
            cargarImpuestos();
            cmbImpuesto.SelectedIndex = 0;
            cargarCotizaciones();
            cargarGruposProductos();
            cargarDescuentos();
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

        private void cargarCotizaciones()
        {
            CotizacionEncabezadoDTO getCotizaciones = new CotizacionEncabezadoDTO
            {
                Opcion = "Listado"
            };

            dtGetCotizaciones = logica.SP_CotizacionEncabezado(getCotizaciones);
            if (dtGetCotizaciones.Rows.Count > 0) 
            {
                dgvCotizaciones.DataSource = dtGetCotizaciones.DefaultView;

                dgvCotizaciones.Columns["EncabezadoID"].Visible = false;
                dgvCotizaciones.Columns["TipoCotizacionID"].Visible = false;
                dgvCotizaciones.Columns["ImpuestoID"].Visible = false;
                dgvCotizaciones.Columns["ClienteID"].Visible = false;
                dgvCotizaciones.Columns["Fecha"].Width = 80;
                dgvCotizaciones.Columns["Descripcion"].Width = 250;
                dgvCotizaciones.Columns["Nombre"].Width = 200;
                dgvCotizaciones.Columns["Atencion"].Width = 250;
                dgvCotizaciones.Columns["Archivo"].Visible = false;
                dgvCotizaciones.Columns["Impuesto"].Width = 250;

                if (dtGetCotizaciones == null || dtGetCotizaciones.Rows.Count == 0)
                    return;

                DateTime fechaSeleccionada = dtpFecha.Value.Date;

                dtGetCotizaciones.DefaultView.RowFilter =
                    $"CONVERT(Fecha, 'System.DateTime') = #{fechaSeleccionada:MM/dd/yyyy}#";

                dgvCotizaciones.DataSource = dtGetCotizaciones.DefaultView;
                lblRegistroCotizacion.Text = "REGISTRO: " + dgvCotizaciones.Rows.Count.ToString();
            }
        }
        private void txtOrigen_KeyUp(object sender, KeyEventArgs e)
        {
            string filtro = txtOrigen.Text.Trim();

            if (dtOrigen != null && dtOrigen.Rows.Count > 0)
            {
                DataView dv = new DataView(dtOrigen);
                dv.RowFilter = string.Format("Convert(Ciudad, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtro.Replace("'", "''"));

                dgvOrigen.DataSource = dv;
            }
        }
        private void dgvOrigen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvOrigen.Columns["SeleccionarOrigen"].Index && e.RowIndex >= 0)
            {
                dgvOrigen.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvDestino_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvDestino.Columns["SeleccionarDestino"].Index && e.RowIndex >= 0)
            {
                dgvDestino.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void dgvProducto_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvProducto.Columns["SeleccionarProducto"].Index && e.RowIndex >= 0)
            {
                dgvProducto.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void txtDestino_KeyUp(object sender, KeyEventArgs e)
        {
            string filtro = txtDestino.Text.Trim();

            if (dtDestino != null && dtDestino.Rows.Count > 0)
            {
                DataView dv = new DataView(dtDestino);

                // Aplicamos el texto ingresdo como filtro de busqueda
                dv.RowFilter = string.Format("Convert(Ciudad, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtro.Replace("'", "''"));

                dgvDestino.DataSource = dv;
            }
        }
        private void txtProducto_KeyUp(object sender, KeyEventArgs e)
        {
            string filtro = txtProducto.Text.Trim();

            if (dtProducto != null && dtProducto.Rows.Count > 0)
            {
                DataView dv = new DataView(dtProducto);

                // Aplicamos el texto ingresdo como filtro de busqueda
                dv.RowFilter = string.Format("Convert(Producto, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtro.Replace("'", "''"));

                dgvProducto.DataSource = dv;
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            txtOrigen.Text = string.Empty;
            txtDestino.Text = string.Empty;
            txtProducto.Text = string.Empty;

            // 🔹 Filtros (igual que antes)
            string filtroOrigen = txtOrigen.Text.Trim();
            if (dtOrigen != null && dtOrigen.Rows.Count > 0)
            {
                DataView dv = new DataView(dtOrigen);
                dv.RowFilter = string.Format("Convert(Ciudad, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtroOrigen.Replace("'", "''"));
                dgvOrigen.DataSource = dv;
            }

            string filtroDestino = txtDestino.Text.Trim();
            if (dtDestino != null && dtDestino.Rows.Count > 0)
            {
                DataView dv = new DataView(dtDestino);
                dv.RowFilter = string.Format("Convert(Ciudad, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtroDestino.Replace("'", "''"));
                dgvDestino.DataSource = dv;
            }

            string filtroProducto = txtProducto.Text.Trim();
            if (dtProducto != null && dtProducto.Rows.Count > 0)
            {
                DataView dv = new DataView(dtProducto);
                dv.RowFilter = string.Format("Convert(Producto, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtroProducto.Replace("'", "''"));
                dgvProducto.DataSource = dv;
            }

            // 🔹 Obtener selecciones
            var origenesSeleccionados = ObtenerOrigenesSeleccionados();
            var destinosSeleccionados = ObtenerDestinosSeleccionados();
            var productosSeleccionados = ObtenerProductosSeleccionados();

            // 🔹 Validaciones
            if (origenesSeleccionados == null || origenesSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un origen antes de continuar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (destinosSeleccionados == null || destinosSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un destino antes de continuar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (productosSeleccionados == null || productosSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un producto antes de continuar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var registros = new List<CotizacionRegistro>();
            foreach (var prod in productosSeleccionados)
            {
                registros.Add(new CotizacionRegistro
                {
                    ProductoID = prod.ProductoID,
                    PrecioNormal = 0 // valor inicial, luego el usuario lo ajusta en los NumericUpDown
                });
            }

            GenerarTablaRegistroFinal(registros, productosSeleccionados, origenesSeleccionados, destinosSeleccionados);

            productosGenerados = productosSeleccionados;
            origenesGenerados = origenesSeleccionados;
            destinosGenerados = destinosSeleccionados;

            registrosGenerados = new List<CotizacionRegistro>();

            foreach (var origen in origenesGenerados)
            {
                foreach (var destino in destinosGenerados)
                {
                    foreach (var producto in productosGenerados)
                    {
                        registrosGenerados.Add(new CotizacionRegistro
                        {
                            CiudadOrigenID = origen.CiudadID,
                            CiudadDestinoID = destino.CiudadID,
                            ProductoID = producto.ProductoID,
                            PrecioNormal = 0 // inicial
                        });
                    }
                }
            }

            bool hayDescuentos = dtDescuentosLocal.AsEnumerable()
                .Any(row => productosGenerados.Any(p => p.ProductoID == row["ProductoID"].ToString()));

            if (hayDescuentos)
            {
                using (var frm = new frmTablaDescuentos(txtClienteNombre.Text, txtClienteID.Text))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        var precios = frm.PreciosSeleccionados;
                        if (precios != null && precios.Count > 0)
                        {
                            AplicarPreciosEnRegistroFinal(precios);
                        }
                    }
                }
            }

            ValidarCamposRegistroFinal();
        }
        private void ValidarCamposRegistroFinal()
        {
            Color colorError = Color.FromArgb(242, 175, 138);

            for (int r = 0; r < tlpRegistroFinal.RowCount; r++)
            {
                for (int c = 0; c < tlpRegistroFinal.ColumnCount; c++)
                {
                    var control = tlpRegistroFinal.GetControlFromPosition(c, r);

                    if (control is NumericUpDown nud)
                    {
                        if (nud.Value == 0)
                        {
                            nud.BackColor = colorError;
                        }
                        else
                        {
                            nud.BackColor = Color.White; // restaurar color normal
                        }
                    }
                }
            }
        }

        private bool productosSinPrecio()
        {
            Color colorError = Color.FromArgb(242, 175, 138);

            for (int r = 0; r < tlpRegistroFinal.RowCount; r++)
            {
                for (int c = 0; c < tlpRegistroFinal.ColumnCount; c++)
                {
                    var control = tlpRegistroFinal.GetControlFromPosition(c, r);

                    if (control is NumericUpDown nud)
                    {
                        // Evaluar si está vacío (texto) o en cero
                        if (string.IsNullOrWhiteSpace(nud.Text) || nud.Value == 0)
                        {
                            nud.BackColor = colorError;
                                                        
                            return true; // salir inmediatamente
                        }
                    }
                }
            }
            return false;
        }


        private void AplicarPreciosEnRegistroFinal(Dictionary<string, (decimal Precio, string Medidas)> precios)
        {
            for (int c = 1; c < tlpRegistroFinal.ColumnCount - 1; c++)
            {
                var header = tlpRegistroFinal.GetControlFromPosition(c, 0) as Label;
                if (header == null) continue;

                string productoNombre = header.Text;
                if (string.IsNullOrEmpty(productoNombre)) continue;

                if (precios.ContainsKey(productoNombre))
                {
                    // 🔹 Actualizar precio
                    var nud = tlpRegistroFinal.GetControlFromPosition(c, 2) as NumericUpDown;
                    if (nud != null)
                    {
                        nud.Value = precios[productoNombre].Precio;
                    }

                    // 🔹 Actualizar medidas (fila 1 debajo del encabezado)
                    var lblMedida = tlpRegistroFinal.GetControlFromPosition(c, 1) as Label;
                    if (lblMedida != null)
                    {
                        lblMedida.Text = precios[productoNombre].Medidas;
                    }
                }
            }
        }
        private void GenerarTablaRegistroFinal( List<CotizacionRegistro> registros, List<ProductoSeleccionado> productos,
            List<frmCotizacionDinamica.CiudadOrigen> origenes, List<CiudadDestino> destinos)
        {
            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            int totalColumnas = 1 + productos.Count + 1;
            tlpRegistroFinal.ColumnCount = totalColumnas;
            tlpRegistroFinal.RowCount = 3;

            // 🔹 Fila 0 y 1 se ajustan automáticamente, fila 2 ocupa el resto
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // encabezado
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // medidas
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // detalle

            // Columna 0 fija para ciudades
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));

            foreach (var p in productos)
                tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));

            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // 🔹 Encabezado de orígenes (fila 0 y 1 con RowSpan)
            string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
            var lblOrigenes = new Label
            {
                Text = origenesConcat,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = true,
                AutoEllipsis = false,
                Padding = new Padding(0, 2, 0, 2)
            };

            tlpRegistroFinal.Controls.Add(lblOrigenes, 0, 0);
            tlpRegistroFinal.SetRowSpan(lblOrigenes, 2);
            // 🔹 Fila 2 columna 0 → destinos
            var panelDestinos = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = destinos.Count,
                AutoScroll = true
            };

            panelDestinos.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            for (int i = 0; i < destinos.Count; i++)
            {
                panelDestinos.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var lbl = new Label
                {
                    Text = destinos[i].Nombre,
                    TextAlign = ContentAlignment.MiddleCenter, 
                    Dock = DockStyle.Fill,                    
                    Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Regular),
                    AutoSize = true,
                    Margin = new Padding(0)
                };

                panelDestinos.Controls.Add(lbl, 0, i);
            }

            tlpRegistroFinal.Controls.Add(panelDestinos, 0, 2);


            // 🔹 Columnas de productos
            for (int i = 0; i < productos.Count; i++)
            {
                string nombre = productos[i].Nombre;

                // Fila 0 → nombre del producto
                var lblNombre = new Label
                {
                    Text = nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = true,
                    AutoEllipsis = false,
                    Padding = new Padding(0, 10, 0, 10)
                };
                tlpRegistroFinal.Controls.Add(lblNombre, 1 + i, 0);

                // Fila 1 → medidas
                var lblMedida = new Label
                {
                    Text = "",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Italic),
                    AutoSize = true,
                    AutoEllipsis = false
                };
                tlpRegistroFinal.Controls.Add(lblMedida, 1 + i, 1);

                // Fila 2 → precio
                var reg = registros.FirstOrDefault(r => r.ProductoID == productos[i].ProductoID);

                var nudPrecio = new NumericUpDown
                {
                    Anchor = AnchorStyles.None,
                    Size = new Size(120, 40),
                    DecimalPlaces = 2,
                    Maximum = 1000000,
                    Minimum = 0,
                    Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular),
                    Value = reg != null ? reg.PrecioNormal : 0,
                    Enabled = true
                };

                nudPrecio.BackColor = nudPrecio.Value == 0
                    ? Color.FromArgb(242, 175, 138)
                    : Color.White;

                nudPrecio.ValueChanged += (s, e) =>
                {
                    var nud = (NumericUpDown)s;
                    nud.BackColor = nud.Value == 0
                        ? Color.FromArgb(242, 175, 138)
                        : Color.White;
                };

                nudPrecio.Leave += (s, e) =>
                {
                    var nud = (NumericUpDown)s;
                    if (string.IsNullOrWhiteSpace(nud.Text) || nud.Value == 0)
                        nud.BackColor = Color.FromArgb(242, 175, 138);
                    else
                        nud.BackColor = Color.White;
                };

                nudPrecio.ResetText();
                tlpRegistroFinal.Controls.Add(nudPrecio, 1 + i, 2);
            }
        }

        private List<ProductoSeleccionado> ObtenerProductosSeleccionados()
        {
            var lista = new List<ProductoSeleccionado>();

            foreach (DataGridViewRow row in dgvProducto.Rows)
            {
                bool marcado = Convert.ToBoolean(row.Cells["SeleccionarProducto"].Value);
                if (marcado)
                {
                    var producto = new ProductoSeleccionado
                    {
                        ProductoID = row.Cells["Producto"].Value.ToString(),
                        Nombre = row.Cells["Nombre"].Value.ToString()
                    };
                    lista.Add(producto);
                }
            }

            return lista;
        }
        private List<CiudadDestino> ObtenerDestinosSeleccionados()
        {
            var lista = new List<CiudadDestino>();

            foreach (DataGridViewRow row in dgvDestino.Rows)
            {
                bool marcado = Convert.ToBoolean(row.Cells["SeleccionarDestino"].Value);
                if (marcado)
                {
                    var destino = new CiudadDestino
                    {
                        CiudadID = row.Cells["Ciudad"].Value.ToString(),
                        Nombre = row.Cells["Nombre"].Value.ToString(),
                        CiudadPrincipalID = row.Cells["CiudadPrincipal"].Value.ToString()
                    };
                    lista.Add(destino);

                }
            }

            return lista;
        }
        private List<frmCotizacionDinamica.CiudadOrigen> ObtenerOrigenesSeleccionados()
        {
            var lista = new List<frmCotizacionDinamica.CiudadOrigen>();

            foreach (DataGridViewRow row in dgvOrigen.Rows)
            {
                bool marcado = Convert.ToBoolean(row.Cells["SeleccionarOrigen"].Value);
                if (marcado)
                {
                    lista.Add(new frmCotizacionDinamica.CiudadOrigen
                    {
                        CiudadID = row.Cells["Ciudad"].Value?.ToString(),
                        Nombre = row.Cells["Nombre"].Value?.ToString()
                    });
                }
            }

            return lista;
        }
        private void ActualizarRegistrosDesdeTabla()
        {
            for (int p = 0; p < productosGenerados.Count; p++)
            {
                var control = tlpRegistroFinal.GetControlFromPosition(1 + p, 2);
                if (control is NumericUpDown nud)
                {
                    decimal nuevoPrecio = nud.Value;

                    foreach (var destino in destinosGenerados)
                    {
                        foreach (var origen in origenesGenerados)
                        {
                            var reg = registrosGenerados.FirstOrDefault(r =>
                                r.ProductoID == productosGenerados[p].ProductoID &&
                                r.CiudadDestinoID == destino.CiudadID &&
                                r.CiudadOrigenID == origen.CiudadID);

                            if (reg != null)
                                reg.PrecioNormal = nuevoPrecio;
                        }
                    }
                }
            }
        }

        private void btnGuardarNacional_Click(object sender, EventArgs e)
        {

            if (btnGuardarNacional.Text == "GUARDAR") 
            {
                if (String.IsNullOrWhiteSpace(txtClienteNombre.Text) && String.IsNullOrWhiteSpace(txtClienteID.Text))
                {
                    MessageBox.Show("Aun no ha seleccionado un cliente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tlpRegistroFinal.RowCount <= 0)
                {
                    MessageBox.Show("Aun no ha agregado los registros de la cotizacion.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (registrosGenerados != null && productosGenerados != null && origenesGenerados != null && destinosGenerados != null)
                {
                    // Actualizar registros con los valores actuales del NumericUpDown
                    ActualizarRegistrosDesdeTabla();

                    bool precioVacio = productosSinPrecio();

                    if (precioVacio)
                    {
                        Toast.Mostrar("Faltan ingresar precios en productos.", TipoAlerta.Warning);
                        return;
                    }

                    // Guardar y generar PDF con los valores actualizados
                   
                    GenerarPDFCotizacion(registrosGenerados, productosGenerados, origenesGenerados, destinosGenerados, dtTerminos);
                    guardarEncabezado(registrosGenerados, origenesGenerados, destinosGenerados, productosGenerados);
                    enviarCotizacion(_RutaArchivo, _NombreArchivo);
                    LimpiarTodo();
                    cargarCotizaciones();
                    tabControl1.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Primero debe generar la cotización antes de guardarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                string nombreArchivo = System.IO.Path.GetFileName(_RutaArchivo);
                enviarCotizacion(nombreArchivo, nombreArchivo);
            }
            
        }
        private void enviarCotizacion(String RutaArchivo, String NombreArchivo)
        {
            Form MensajeAdvertencia = new Form();
            using (frmMensajePersonalizado Mensaje = new frmMensajePersonalizado(RutaArchivo, NombreArchivo))
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
                Mensaje.ShowDialog();
                MensajeAdvertencia.Dispose();
            }
        }

        private void cargarGruposProductos() 
        {
            ProductosGruposENAC getGrupos = new ProductosGruposENAC 
            {
                Opcion = "Listado"
            };
            dtGruposProductos = logica.SP_ProductosGruposENAC(getGrupos);
        }

        private void guardarEncabezado(List<CotizacionRegistro> registros, List<CiudadOrigen> origenes, List<CiudadDestino> destinos, List<ProductoSeleccionado> productos)
        {
            bool todoGuardado = true;

            CotizacionEncabezadoDTO sendEncabezado = new CotizacionEncabezadoDTO
            {
                Opcion = "Agregar",
                EncabezadoID = 0,
                TipoCotizacionID = Convert.ToInt32(cmbTipoCotizacion.SelectedValue),
                ClienteID = Convert.ToInt32(txtClienteID.Text),
                Atencion = txtAtencion.Text,
                ImpuestoID = Convert.ToInt32(cmbImpuesto.SelectedValue),
                EstadoSeguimiento = 1,//Enviado como ingresado
                Archivo = _RutaArchivo,
                UPosteo = DynamicMain.usuarionlogin,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true
            };

            DataTable dtSendEncabezado = logica.SP_CotizacionEncabezado(sendEncabezado);

            if (dtSendEncabezado.Rows.Count > 0 && dtSendEncabezado.Rows[0]["Estado"].ToString() == "1")
            {
                int _CotizacionID = Convert.ToInt32(dtSendEncabezado.Rows[0]["UltimoEncabezadoID"]);

                foreach (var origen in origenes)
                {
                    foreach (var destino in destinos)
                    {
                        foreach (var producto in productos)
                        {
                            var registrosDestinoProducto = registros
                                .Where(r => r.CiudadOrigenID == origen.CiudadID
                                         && r.CiudadDestinoID == destino.CiudadID
                                         && r.ProductoID == producto.ProductoID)
                                .ToList();

                            foreach (var reg in registrosDestinoProducto)
                            {
                                CotizacionDetalleDTO sendDetalle = new CotizacionDetalleDTO
                                {
                                    Opcion = "Agregar",
                                    DetalleID = 0,
                                    EncabezadoID = _CotizacionID,
                                    CiudadOrigenID = origen.CiudadID,
                                    CiudadDestinoID = destino.CiudadID,
                                    ProductoID = producto.ProductoID,
                                    Precio = reg.PrecioNormal,
                                    UPosteo = DynamicMain.usuarionlogin,
                                    FPosteo = DateTime.Now,
                                    PC = System.Environment.MachineName,
                                    Estado = true
                                };

                                DataTable dtDetalle = logica.SP_CotizacionDetalle(sendDetalle);

                                if (dtDetalle.Rows.Count > 0 && dtDetalle.Rows[0]["Estado"].ToString() == "0")
                                {
                                    todoGuardado = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                todoGuardado = false;
            }

            if (todoGuardado)
                Toast.Mostrar("Cotizacion guardada exitosamente.", TipoAlerta.Success);
            else
                Toast.Mostrar("Ha ocurrido un error al guardar la cotizacion", TipoAlerta.Error);
        }
        private void cargarTerminos()
        {
            CotizacionTerminoDTO getTerminos = new CotizacionTerminoDTO
            {
                Opcion = "Listar"
            };
            dtTerminos = logica.SP_CotizacionTerminos(getTerminos);
            if (dtTerminos.Rows.Count > 0)
            {
                _TerminosExisten = true;
            }
        }
        private void cargarTipos() 
        {
            CotizacionTipoDTO getTipos = new CotizacionTipoDTO
            {
                Opcion = "Listado"
            };
            dtTipos = logica.SP_CotizacionTipo(getTipos);
            if (dtTipos.Rows.Count > 0) 
            {
                cmbTipoCotizacion.DataSource = dtTipos;
                cmbTipoCotizacion.DisplayMember = "Descripcion";
                cmbTipoCotizacion.ValueMember = "TipoID";
                cmbTipoCotizacion.SelectedIndex = 0;
            }
        }
        private void cargarImpuestos()
        {
            CotizacionImpuestoDTO getImpuestos = new CotizacionImpuestoDTO
            {
                Opcion = "Listado"
            };
            dtImpuestos = logica.SP_CotizacionImpuesto(getImpuestos);
            if (dtImpuestos.Rows.Count > 0)
            {
                cmbImpuesto.DataSource = dtImpuestos;
                cmbImpuesto.DisplayMember = "Descripcion";
                cmbImpuesto.ValueMember = "ImpuestoID";
                cmbImpuesto.SelectedIndex = 0;
            }
        }

        private void GenerarPDFCotizacion(List<CotizacionRegistro> registros,List<ProductoSeleccionado> productos,
    List<frmCotizacionDinamica.CiudadOrigen> origenes,  List<CiudadDestino> destinos,  DataTable dtTerminos)
        {
            Document doc = new Document(PageSize.LETTER, 40, 40, 40, 40);
            //string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Cotizacion.pdf");
            // 🔹 Carpeta destino en el servidor
            string rutaDestino = @"\\192.168.1.179\CotizacionesEspecificas";
            Directory.CreateDirectory(rutaDestino);

            // 🔹 Nombre único usando GUID
            string nombreArchivo = "Cotizacion-" + Guid.NewGuid().ToString() + ".pdf";
            string path = System.IO.Path.Combine(rutaDestino, nombreArchivo);
            _RutaArchivo = path;
            _NombreArchivo = nombreArchivo;

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // 🔹 Encabezado con logos (misma fila)
                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 150f, 150f });

                // Logo izquierda
                iTextSharp.text.Image logoIzq = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\RCHondurasColor.png");
                logoIzq.ScaleAbsolute(140, 130); // 🔹 tamaño uniforme
                PdfPCell cellLogoIzq = new PdfPCell(logoIzq)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_MIDDLE // 🔹 centrado vertical
                };
                headerTable.AddCell(cellLogoIzq);

                // Logo derecha
                iTextSharp.text.Image logoDer = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\RCPaqueteria.png");
                logoDer.ScaleAbsolute(150, 40); // 🔹 mismo tamaño que el izquierdo
                PdfPCell cellLogoDer = new PdfPCell(logoDer)
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_RIGHT,
                    VerticalAlignment = Element.ALIGN_MIDDLE // 🔹 centrado vertical
                };
                headerTable.AddCell(cellLogoDer);

                doc.Add(headerTable);

                // 🔹 Título "COTIZACION" debajo de los logos
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                Paragraph titulo = new Paragraph("COTIZACION", fontTitulo)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 10f,
                    SpacingAfter = 20f
                };
                doc.Add(titulo);

                // 🔹 Cliente y Fecha
                PdfPTable clienteFechaTable = new PdfPTable(2);
                clienteFechaTable.WidthPercentage = 100;
                clienteFechaTable.SetWidths(new float[] { 250f, 180f });

                clienteFechaTable.AddCell(new PdfPCell(new Phrase("CLIENTE: " + txtClienteNombre.Text, fontCell)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                clienteFechaTable.AddCell(new PdfPCell(new Phrase("FECHA: " + DateTime.Now.ToString("dd/MM/yyyy"), fontCell)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                doc.Add(clienteFechaTable);

                // 🔹 Atención y Código
                PdfPTable atencionCodigoTable = new PdfPTable(2);
                atencionCodigoTable.WidthPercentage = 100;
                atencionCodigoTable.SetWidths(new float[] { 250f, 250f });

                atencionCodigoTable.AddCell(new PdfPCell(new Phrase("ATENCIÓN:  " + txtAtencion.Text, fontCell)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
                atencionCodigoTable.AddCell(new PdfPCell(new Phrase("CÓDIGO:  " + txtClienteID.Text, fontCell)) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT });
                doc.Add(atencionCodigoTable);

                doc.Add(new Paragraph("\n"));

                // 🔹 Agrupar productos por GrupoID
                var productosAgrupados = (from p in productos
                                          join g in dtGruposProductos.AsEnumerable()
                                          on p.ProductoID equals g.Field<string>("ProductoID") into gj
                                          from subg in gj.DefaultIfEmpty()
                                          select new { Producto = p, GrupoID = subg != null ? subg.Field<int>("GrupoID") : (int?)null })
                                          .GroupBy(x => x.GrupoID)
                                          .OrderBy(g => g.Key ?? int.MaxValue)
                                          .ToList();

                int maxColsPorBloque = 5;

                foreach (var grupo in productosAgrupados)
                {
                    var productosDelGrupo = grupo.Select(x => x.Producto).ToList();

                    for (int start = 0; start < productosDelGrupo.Count; start += maxColsPorBloque)
                    {
                        PdfPTable table = new PdfPTable(1 + maxColsPorBloque);
                        table.WidthPercentage = 100;

                        float[] widths = new float[1 + maxColsPorBloque];
                        widths[0] = 200f;
                        for (int i = 1; i < widths.Length; i++) widths[i] = 100f;
                        table.SetWidths(widths);

                        // 🔹 Fila 1: encabezado (origen ocupa fila 1 y 2 con RowSpan)
                        string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
                        PdfPCell cellOrigen = new PdfPCell(new Phrase("SUCURSAL REMITENTE: " + origenesConcat, fontHeader))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            VerticalAlignment = Element.ALIGN_MIDDLE,
                            PaddingTop = 10f,
                            PaddingBottom = 10f,
                            BackgroundColor = new BaseColor(185, 203, 226) // 🔹 color de fondo
                        };
                        cellOrigen.Rowspan = 2; // 🔹 ocupa encabezado y medidas
                        table.AddCell(cellOrigen);

                        for (int i = 0; i < maxColsPorBloque; i++)
                        {
                            if (start + i < productosDelGrupo.Count)
                            {
                                PdfPCell cellProd = new PdfPCell(new Phrase(productosDelGrupo[start + i].Nombre, fontHeader))
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    VerticalAlignment = Element.ALIGN_MIDDLE,
                                    PaddingTop = 10f,
                                    PaddingBottom = 10f,
                                    BackgroundColor = new BaseColor(185, 203, 226) // 🔹 color de fondo
                                };
                                table.AddCell(cellProd);
                            }
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase("", fontHeader)) { Border = Rectangle.NO_BORDER });
                            }
                        }

                        // 🔹 Fila 2: medidas (sin título "MEDIDAS")
                        for (int i = 0; i < maxColsPorBloque; i++)
                        {
                            if (start + i < productosDelGrupo.Count)
                            {
                                // Buscar la medida desde tlpRegistroFinal
                                int columnaGlobal = productos.FindIndex(p => p.ProductoID == productosDelGrupo[start + i].ProductoID);
                                var controlMedida = tlpRegistroFinal.GetControlFromPosition(1 + columnaGlobal, 1);
                                string medidas = "";
                                if (controlMedida is Label lblMedida)
                                    medidas = lblMedida.Text;

                                PdfPCell cellMedida = new PdfPCell(new Phrase(medidas, fontHeader)) // 🔹 fontHeader = negrita
                                {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    VerticalAlignment = Element.ALIGN_MIDDLE,
                                    PaddingTop = 5f,
                                    PaddingBottom = 5f,
                                    BackgroundColor = new BaseColor(250, 255, 73) // 🔹 color amarillo
                                };
                                table.AddCell(cellMedida);
                            }
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase("", fontHeader)) { Border = Rectangle.NO_BORDER });
                            }
                        }


                        // 🔹 Fila 3: destinos + precios
                        string destinosConcat = string.Join("\n", destinos.Select(d => d.Nombre));
                        PdfPCell cellDestinos = new PdfPCell(new Phrase(destinosConcat, fontCell)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, PaddingBottom = 10f };
                        table.AddCell(cellDestinos);

                        for (int i = 0; i < maxColsPorBloque; i++)
                        {
                            if (start + i < productosDelGrupo.Count)
                            {
                                var reg = registros.FirstOrDefault(r => r.ProductoID == productosDelGrupo[start + i].ProductoID);
                                string precio = reg != null ? reg.PrecioNormal.ToString("N2") : "0.00";

                                PdfPCell cellPrecio = new PdfPCell(new Phrase(precio, fontCell)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, PaddingBottom = 10f };
                                table.AddCell(cellPrecio);
                            }
                            else
                            {
                                table.AddCell(new PdfPCell(new Phrase("", fontCell)) { Border = Rectangle.NO_BORDER });
                            }
                        }

                        doc.Add(table);
                        doc.Add(new Paragraph(" ", fontCell));
                    }
                }

                // 🔹 Términos
                if (dtTerminos != null && dtTerminos.Rows.Count > 0)
                {
                    doc.Add(new Paragraph("\n"));
                    var fontTituloTerminos = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                    doc.Add(new Paragraph("TERMINOS Y NEGOCIACIONES ESPECIALES", fontTituloTerminos));

                    if (cmbImpuesto.SelectedItem != null)
                        doc.Add(new Paragraph(cmbImpuesto.Text, FontFactory.GetFont(FontFactory.HELVETICA, 9)));

                    foreach (DataRow row in dtTerminos.Rows)
                        doc.Add(new Paragraph("- " + row["Descripcion"].ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 9)));
                }

                // 🔹 Firmas
                doc.Add(new Paragraph("\n\n\n"));
                PdfPTable firmasTable = new PdfPTable(2);
                firmasTable.WidthPercentage = 100;
                firmasTable.SetWidths(new float[] { 250f, 250f });

                PdfPCell cellLineaIzq = new PdfPCell { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
                cellLineaIzq.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2))));
                cellLineaIzq.AddElement(new Paragraph("Autorizado", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)) { Alignment = Element.ALIGN_CENTER });
                cellLineaIzq.AddElement(new Paragraph("Nancy D. Valle", FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER });
                cellLineaIzq.AddElement(new Paragraph("GERENTE ADMINISTRATIVO", FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER });
                firmasTable.AddCell(cellLineaIzq);

                // 🔹 Celda derecha: línea + texto "Aprobado Por"
                PdfPCell cellLineaDer = new PdfPCell { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
                cellLineaDer.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2))));
                cellLineaDer.AddElement(new Paragraph("Aprobado Por Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)) { Alignment = Element.ALIGN_CENTER });
                cellLineaDer.AddElement(new Paragraph("Firma y Sello", FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER });
                cellLineaDer.AddElement(new Paragraph(txtClienteNombre.Text, FontFactory.GetFont(FontFactory.HELVETICA, 9)) { Alignment = Element.ALIGN_CENTER });
                firmasTable.AddCell(cellLineaDer);

                doc.Add(firmasTable);

                // 🔹 Cerrar documento
                doc.Close();
            }

           // System.Diagnostics.Process.Start(path);
            //enviarCotizacion(nombreArchivo);
           // Toast.Mostrar("PDF generado con exito.", TipoAlerta.Success);
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
            txtAtencion.Focus();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            LimpiarTodo();
        }
        private void LimpiarTodo() 
        {
            OrigenID = 0;
            DestinoID = 0;
            ProductoID = 0;

            btnGuardarNacional.Enabled = true;
            txtClienteNombre.Enabled = true;
            txtClienteID.Enabled = true;
            txtAtencion.Enabled = true;
            cmbImpuesto.Enabled = true;
            cmbTipoCotizacion.Enabled = true;
            dgvOrigen.Enabled = true;
            dgvDestino.Enabled = true;
            dgvProducto.Enabled = true;
            txtOrigen.Enabled = true;
            txtDestino.Enabled = true;
            txtProducto.Enabled = true;
            btnAgregar.Enabled = true;
            btnLimpiar.Enabled = true;
            btnCliente.Enabled = true;
            btnCancelar.Enabled = true;
            btnPDF.Visible = false;
            btnGuardarNacional.Visible = true;
            btnLimpiar.Visible = true;
            btnGuardarNacional.Text = "GUARDAR";

            NombreOrigen = string.Empty;
            NombreDestino = string.Empty;
            NombreProducto = string.Empty;

            txtClienteNombre.Text = string.Empty;
            txtClienteID.Text = string.Empty;
            txtAtencion.Text = string.Empty;

            cmbTipoCotizacion.SelectedIndex = 0;
            cmbImpuesto.SelectedIndex = 0;

            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            foreach (DataGridViewRow row in dgvOrigen.Rows)
            {
                row.Cells["SeleccionarOrigen"].Value = false;
            }

            foreach (DataRow dr in dtOrigen.Rows)
            {
                dr["SeleccionarOrigen"] = false;
            }

            foreach (DataGridViewRow row in dgvDestino.Rows)
            {
                row.Cells["SeleccionarDestino"].Value = false;
            }

            foreach (DataRow dr in dtDestino.Rows)
            {
                dr["SeleccionarDestino"] = false;
            }

            foreach (DataGridViewRow row in dgvProducto.Rows)
            {
                row.Cells["SeleccionarProducto"].Value = false;
            }

            foreach (DataRow dr in dtProducto.Rows)
            {
                dr["SeleccionarProducto"] = false;
            }

        }

        private void txtBusqueda_KeyUp(object sender, KeyEventArgs e)
        {
            string filtro = txtBusqueda.Text.Trim();

            if (dtGetCotizaciones != null && dtGetCotizaciones.Rows.Count > 0)
            {
                DataView dv = new DataView(dtGetCotizaciones);
                dv.RowFilter = string.Format("Atencion LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtro.Replace("'", "''"));

                dgvCotizaciones.DataSource = dv;

                lblRegistroCotizacion.Text = "REGISTRO: " + dgvCotizaciones.Rows.Count.ToString();
            }
        }
        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void dgvCotizaciones_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCotizaciones.Rows.Count > 0) 
            {
                if (!datosCargados) { Toast.Mostrar("Registros aún procesando, espere un momento...", TipoAlerta.Warning); return; }

                _EncabezadoID = Convert.ToInt32(dgvCotizaciones.CurrentRow.Cells["EncabezadoID"].Value);
                recuperarEncabezado(_EncabezadoID);

                txtClienteNombre.Enabled = false;
                txtClienteID.Enabled = false;
                txtAtencion.Enabled = false;
                cmbImpuesto.Enabled = false;
                cmbTipoCotizacion.Enabled = false;
                dgvOrigen.Enabled = false;
                dgvDestino.Enabled = false;
                dgvProducto.Enabled = false;
                txtOrigen.Enabled = false;
                txtDestino.Enabled = false;
                txtProducto.Enabled = false;
                btnAgregar.Enabled = false;
                btnLimpiar.Enabled = false;
                btnCliente.Enabled = false;
                btnCancelar.Enabled = true;
                btnPDF.Visible = true;
                //btnGuardarNacional.Visible = false;
                //btnGuardarNacional.Enabled = false;
                btnLimpiar.Visible = false;
                btnGuardarNacional.Text = "Enviar";
                tabControl1.SelectedIndex = 1; 
            }
        }
        private void recuperarEncabezado(int _EncabezadoID) 
        {
            CotizacionEncabezadoDTO getCotizacion = new CotizacionEncabezadoDTO 
            {
                Opcion = "Recuperar",
                EncabezadoID = _EncabezadoID,
            };
            dtEncabezado = logica.SP_CotizacionEncabezado(getCotizacion);
            if (dtEncabezado.Rows.Count > 0) 
            {
                txtClienteNombre.Text = dtEncabezado.Rows[0]["Nombre"].ToString();
                txtClienteID.Text = dtEncabezado.Rows[0]["ClienteID"].ToString();
                txtAtencion.Text = dtEncabezado.Rows[0]["Atencion"].ToString();
                cmbTipoCotizacion.SelectedValue = Convert.ToInt32(dtEncabezado.Rows[0]["TipoCotizacionID"].ToString());
                cmbImpuesto.SelectedValue = Convert.ToInt32(dtEncabezado.Rows[0]["ImpuestoID"].ToString());
                _RutaArchivo = dtEncabezado.Rows[0]["Archivo"].ToString();
                recuperarDetalle(_EncabezadoID);
            }
        }
        private void cmbTipoCotizacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtDestino == null || dtDestino.Rows.Count == 0)
                return;

            if (cmbTipoCotizacion.SelectedIndex == 1)
            {
                foreach (DataRow row in dtDestino.Rows)
                {
                    if (row["Ciudad"].ToString() == "0")
                        row["SeleccionarDestino"] = true;
                    else
                        row["SeleccionarDestino"] = false; 
                }

                dgvDestino.Refresh();
                dgvDestino.Enabled = false;
            }
            else if (cmbTipoCotizacion.SelectedIndex == 0)
            {
                foreach (DataRow row in dtDestino.Rows)
                {
                    row["SeleccionarDestino"] = false;
                }

                dgvDestino.Refresh();
                dgvDestino.Enabled = true;
            }
        }

        private void btnNuevaCotizacion_Click(object sender, EventArgs e)
        {
            LimpiarTodo();
            tabControl1.SelectedIndex = 1;
            btnPDF.Visible = false;
            btnGuardarNacional.Visible = true;
            btnLimpiar.Visible = true;
        }
        private void dtpFecha_ValueChanged(object sender, EventArgs e)
        {
            if (dtGetCotizaciones == null || dtGetCotizaciones.Rows.Count == 0)
                return;

            DateTime fechaSeleccionada = dtpFecha.Value.Date;

            dtGetCotizaciones.DefaultView.RowFilter =
                $"CONVERT(Fecha, 'System.DateTime') = #{fechaSeleccionada:MM/dd/yyyy}#";

            dgvCotizaciones.DataSource = dtGetCotizaciones.DefaultView;

            lblRegistroCotizacion.Text = "REGISTRO: " + dgvCotizaciones.Rows.Count.ToString();
        }

        private void btnPDF_Click_1(object sender, EventArgs e)
        {
            //ActualizarRegistrosDesdeTabla();

           // GenerarPDFCotizacion(registrosGenerados, productosGenerados, origenesGenerados, destinosGenerados, dtTerminos);

            System.Diagnostics.Process.Start(_RutaArchivo);
        }

        private void rbdNombre_CheckedChanged(object sender, EventArgs e)
        {
            if (rbdNombre.Checked) 
            {
                txtClienteID.ReadOnly = true;
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
        }
        private void txtClienteNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void rdbCodigo_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbCodigo.Checked) 
            {
                txtClienteID.ReadOnly = false;
            }
        }

        private void txtClienteID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                if (String.IsNullOrWhiteSpace(txtClienteID.Text))
                    return;

                e.SuppressKeyPress = true;
                cargarClientes();

                txtAtencion.Focus();
            }
        }
        private async void frmCotizacionDinamica_Load(object sender, EventArgs e)
        {
            var tareaDestino = Task.Run(() => cargarDestino());
            var tareaProductos = Task.Run(() => cargarProductos());

            await Task.WhenAll(tareaDestino, tareaProductos);

            datosCargados = true; // 🔹 marcar que ya terminó
            Toast.Mostrar("Datos cargados correctamente", TipoAlerta.Success);
        }


        private void cargarDescuentos() 
        {
            ProductosPreciosENAC getDescuentos = new ProductosPreciosENAC
            {
                Opcion = "ListadoDistinct"
            };
            dtDescuentosLocal = logica.SP_ProductosPreciosENAC(getDescuentos);
        }

        private void txtAtencion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            if (!(char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' '))
            {
                e.Handled = true; 
            }
        }

        private void dgvOrigen_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvOrigen.IsCurrentCellDirty)
            {
                dgvOrigen.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvOrigen_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvOrigen.Columns[e.ColumnIndex].Name == "SeleccionarOrigen")
            {
                ActualizarSeleccionadosOrigen();
            }
        }

        private void ActualizarSeleccionadosOrigen()
        {
            int cantidadSeleccionados = dtOrigen.AsEnumerable()
                .Count(r => r.Field<bool>("SeleccionarOrigen"));

            lblSeleccionadoOrigen.Text = $"REGISTROS ORIGEN SELECCIONADOS: {cantidadSeleccionados}";
        }

        private void ActualizarSeleccionadosDestino()
        {
            int cantidadSeleccionados = dtDestino.AsEnumerable()
                .Count(r => r.Field<bool>("SeleccionarDestino"));

            lblSeleccionadoDestino.Text = $"REGISTROS DESTINOS SELECCIONADOS: {cantidadSeleccionados}";
        }

        private void dgvDestino_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvDestino.IsCurrentCellDirty)
            {
                dgvDestino.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvDestino_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvDestino.Columns[e.ColumnIndex].Name == "SeleccionarDestino")
            {
                ActualizarSeleccionadosDestino();
            }
        }

        private void ActualizarSeleccionadosProducto()
        {
            int cantidadSeleccionados = dtProducto.AsEnumerable()
                .Count(r => r.Field<bool>("SeleccionarProducto"));

            lblSeleccionadoProducto.Text = $"REGISTROS PRODUCTOS SELECCIONADOS: {cantidadSeleccionados}";
        }

        private void dgvProducto_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvProducto.IsCurrentCellDirty)
            {
                dgvProducto.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtClienteID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true; // 🔹 obliga a tratar Tab como tecla de entrada
            }
        }

        private void dgvProducto_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvProducto.Columns[e.ColumnIndex].Name == "SeleccionarProducto")
            {
                ActualizarSeleccionadosProducto();
            }
        }

        private void txtClienteID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void recuperarDetalle(int _DetalleID)
        {
            CotizacionDetalleDTO getDetalle = new CotizacionDetalleDTO
            {
                Opcion = "Recuperar",
                DetalleID = _DetalleID,
            };
            dtDetalle = logica.SP_CotizacionDetalle(getDetalle);

            if (dtDetalle.Rows.Count > 0)
            {
                // 🔹 Primero desmarcamos todo
                foreach (DataRow rowOrigen in dtOrigen.Rows)
                    rowOrigen["SeleccionarOrigen"] = false;

                foreach (DataRow rowDestino in dtDestino.Rows)
                    rowDestino["SeleccionarDestino"] = false;

                foreach (DataRow rowProducto in dtProducto.Rows)
                    rowProducto["SeleccionarProducto"] = false;

                // 🔹 Marcamos coincidencias de ORIGEN
                foreach (DataRow rowDetalle in dtDetalle.Rows)
                {
                    string ciudadOrigenID = rowDetalle["CiudadOrigenID"].ToString();
                    var filasCoincidentesOrigen = dtOrigen.Select($"Ciudad = '{ciudadOrigenID}'");
                    foreach (var fila in filasCoincidentesOrigen)
                        fila["SeleccionarOrigen"] = true;
                }

                // 🔹 Marcamos coincidencias de DESTINO
                foreach (DataRow rowDetalle in dtDetalle.Rows)
                {
                    string ciudadDestinoID = rowDetalle["CiudadDestinoID"].ToString();

                    if (ciudadDestinoID == "0")
                    {
                        var filasCoincidentesDestino = dtDestino.Select("Ciudad = 0");
                        foreach (var fila in filasCoincidentesDestino)
                            fila["SeleccionarDestino"] = true;
                    }
                    else
                    {
                        var filasCoincidentesDestino = dtDestino.Select($"Ciudad = '{ciudadDestinoID}'");
                        foreach (var fila in filasCoincidentesDestino)
                            fila["SeleccionarDestino"] = true;
                    }
                }

                // 🔹 Marcamos coincidencias de PRODUCTO (blindaje: validar columna)
                if (dtProducto.Columns.Contains("Producto"))
                {
                    foreach (DataRow rowDetalle in dtDetalle.Rows)
                    {
                        string productoID = rowDetalle["ProductoID"].ToString();
                        var filasCoincidentesProducto = dtProducto.Select($"Producto = '{productoID}'");
                        foreach (var fila in filasCoincidentesProducto)
                            fila["SeleccionarProducto"] = true;
                    }
                }

                // 🔹 Refrescamos los DataGridView
                dgvOrigen.Refresh();
                dgvDestino.Refresh();
                dgvProducto.Refresh();

                // 🔹 Construimos listas de seleccionados
                var origenesSeleccionados = dtOrigen?.AsEnumerable()
                    .Where(r => r.Field<bool>("SeleccionarOrigen"))
                    .Select(r => new CiudadOrigen
                    {
                        CiudadID = r["Ciudad"].ToString(),
                        Nombre = r["Nombre"].ToString()
                    }).ToList() ?? new List<CiudadOrigen>();

                var destinosSeleccionados = dtDestino?.AsEnumerable()
                    .Where(r => r.Field<bool>("SeleccionarDestino"))
                    .Select(r => new CiudadDestino
                    {
                        CiudadID = r["Ciudad"].ToString(),
                        Nombre = r["Nombre"].ToString(),
                        CiudadPrincipalID = r["CiudadPrincipal"].ToString()
                    }).ToList() ?? new List<CiudadDestino>();

                var productosSeleccionados = dtProducto?.AsEnumerable()
                    .Where(r => r.Field<bool>("SeleccionarProducto"))
                    .Select(r => new ProductoSeleccionado
                    {
                        ProductoID = r["Producto"].ToString(),
                        Nombre = r["Nombre"].ToString()
                    }).ToList() ?? new List<ProductoSeleccionado>();

                // 🔹 Construir la lista de registros desde dtDetalle
                var registros = ObtenerCotizacionesDesdeDetalle(productosSeleccionados, origenesSeleccionados, destinosSeleccionados, dtDetalle);

                registrosGenerados = registros;
                productosGenerados = productosSeleccionados;
                origenesGenerados = origenesSeleccionados;
                destinosGenerados = destinosSeleccionados;

                // 🔹 Generamos la tabla final con precios desde dtDetalle
                GenerarTablaRecuperada(registros, productosSeleccionados, origenesSeleccionados, destinosSeleccionados);
            }
        }

        private List<CotizacionRegistro> ObtenerCotizacionesDesdeDetalle(
            List<ProductoSeleccionado> productos,
            List<CiudadOrigen> origenes,
            List<CiudadDestino> destinos,
            DataTable dtDetalle)
        {
            var registros = new List<CotizacionRegistro>();

            foreach (DataRow row in dtDetalle.Rows)
            {
                registros.Add(new CotizacionRegistro
                {
                    ProductoID = row["ProductoID"].ToString(),
                    CiudadOrigenID = row["CiudadOrigenID"].ToString(),
                    CiudadDestinoID = row["CiudadDestinoID"].ToString(),
                    PrecioNormal = row["Precio"] != DBNull.Value ? Convert.ToDecimal(row["Precio"]) : 0
                });
            }

            return registros;
        }

        private void GenerarTablaRecuperada(List<CotizacionRegistro> registros,List<ProductoSeleccionado> productos,
       List<CiudadOrigen> origenes,List<CiudadDestino> destinos)
        {
            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // 🔹 Columnas: 1 para destinos + productos + extra
            int totalColumnas = 1 + productos.Count + 1;
            tlpRegistroFinal.ColumnCount = totalColumnas;
            tlpRegistroFinal.RowCount = 2; // solo encabezado + detalle

            // 🔹 Configuración de filas
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Absolute, 30)); // encabezado
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // detalle

            // 🔹 Configuración de columnas
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250)); // columna destinos
            foreach (var p in productos)
                tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // columna extra

            // 🔹 Encabezado fila 0
            string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
            tlpRegistroFinal.Controls.Add(new Label
            {
                Text = origenesConcat,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold)
            }, 0, 0);

            for (int i = 0; i < productos.Count; i++)
            {
                tlpRegistroFinal.Controls.Add(new Label
                {
                    Text = productos[i].Nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold)
                }, 1 + i, 0);
            }

            // Fila 1 → destinos con scroll y centrado igual que tlpCotizacion
            var panelDestinos = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            var contentDestinos = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            foreach (var destino in destinos)
            {
                var lbl = new Label
                {
                    Text = destino.Nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Width = 250,
                    Height = 26,
                    Margin = new Padding(0)
                };
                contentDestinos.Controls.Add(lbl);
            }

            panelDestinos.Controls.Add(contentDestinos);

            panelDestinos.Resize += (s, e) =>
            {
                var contentSize = contentDestinos.PreferredSize;
                panelDestinos.AutoScrollMinSize = contentSize;

                var client = panelDestinos.ClientSize;
                int x = Math.Max(0, (client.Width - contentSize.Width) / 2);
                int y = Math.Max(0, (client.Height - contentSize.Height) / 2);

                if (contentSize.Height > client.Height)
                    y = 0;

                contentDestinos.Location = new Point(x, y);
            };

            tlpRegistroFinal.Controls.Add(panelDestinos, 0, 1);


            // 🔹 Fila 1 → precios por producto (un solo NumericUpDown por producto)
            for (int i = 0; i < productos.Count; i++)
            {
                var reg = registros.FirstOrDefault(r => r.ProductoID == productos[i].ProductoID);

                var nudPrecio = new NumericUpDown
                {
                    Anchor = AnchorStyles.None,
                    DecimalPlaces = 2,
                    Maximum = 1000000,
                    Minimum = 0,
                    Font = new System.Drawing.Font("Segoe UI", 12),
                    Value = reg != null ? reg.PrecioNormal : 0,
                    Enabled = false
                    
                };

                tlpRegistroFinal.Controls.Add(nudPrecio, 1 + i, 1);
            }

            // 🔹 Guardar referencias
            registrosGenerados = registros;
            productosGenerados = productos;
            origenesGenerados = origenes;
            destinosGenerados = destinos;
        }

        private void cargarOrigen()
        {
            CiudadesENAC getOrigen = new CiudadesENAC
            {
                Opcion = "CiudadesPrincipales"
            };
            dtOrigen = logica.SP_CiudadesENAC(getOrigen);

            if (dtOrigen.Rows.Count > 0)
            {
                if (!dtOrigen.Columns.Contains("SeleccionarOrigen"))
                {
                    dtOrigen.Columns.Add("SeleccionarOrigen", typeof(bool));

                    // Inicializamos todas las filas en false
                    foreach (DataRow row in dtOrigen.Rows)
                    {
                        row["SeleccionarOrigen"] = false;
                    }
                }

                // Evitamos que el DataGridView genere columnas automáticas
                dgvOrigen.AutoGenerateColumns = false;

                // Limpiamos columnas previas para evitar duplicados
                dgvOrigen.Columns.Clear();

                // Columna CheckBox personalizada
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "SeleccionarOrigen";
                chkCol.HeaderText = "✔";   // Tu header minimalista
                chkCol.Width = 50;
                chkCol.DataPropertyName = "SeleccionarOrigen"; // Vinculada al DataTable
                chkCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                chkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvOrigen.Columns.Add(chkCol);

                // Columna Nombre
                DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
                colNombre.DataPropertyName = "Nombre";
                colNombre.HeaderText = "Nombre";
                colNombre.Name = "Nombre";
                colNombre.Width = 275;
                colNombre.ReadOnly = true;
                dgvOrigen.Columns.Add(colNombre);

                // Ocultamos Ciudad pero sigue en el DataTable
                DataGridViewTextBoxColumn colCiudad = new DataGridViewTextBoxColumn();
                colCiudad.DataPropertyName = "Ciudad";
                colCiudad.HeaderText = "Ciudad";
                colCiudad.Name = "Ciudad";
                // colCiudad.Visible = false;
                dgvOrigen.Columns.Add(colCiudad);

                // Asignamos el DataSource
                dgvOrigen.DataSource = dtOrigen;
            }
        }
        private void cargarDestino()
        {
            CiudadesENAC getDestino = new CiudadesENAC
            {
                Opcion = "ListadoTodas"
            };
            dtDestino = logica.SP_CiudadesENAC(getDestino);

            if (dtDestino.Rows.Count > 0)
            {
                // Insertamos el registro especial al inicio
                DataRow rowNacional = dtDestino.NewRow();
                rowNacional["Ciudad"] = 0;
                rowNacional["Nombre"] = "A NIVEL NACIONAL";
                rowNacional["CiudadPrincipal"] = 0;
                dtDestino.Rows.InsertAt(rowNacional, 0);

                // Si no existe la columna Seleccionar en el DataTable, la agregamos
                if (!dtDestino.Columns.Contains("SeleccionarDestino"))
                {
                    dtDestino.Columns.Add("SeleccionarDestino", typeof(bool));
                    // Inicializamos todas las filas en false
                    foreach (DataRow row in dtDestino.Rows)
                    {
                        row["SeleccionarDestino"] = false;
                    }
                }

                dgvDestino.AutoGenerateColumns = false;
                dgvDestino.Columns.Clear();

                // Columna CheckBox
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "SeleccionarDestino";
                chkCol.HeaderText = "✔";
                chkCol.Width = 50;
                chkCol.DataPropertyName = "SeleccionarDestino";
                chkCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                chkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvDestino.Columns.Add(chkCol);

                // Columna Nombre
                DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
                colNombre.DataPropertyName = "Nombre";
                colNombre.HeaderText = "Nombre";
                colNombre.Width = 275;
                colNombre.ReadOnly = true;
                colNombre.Name = "Nombre";
                dgvDestino.Columns.Add(colNombre);

                // Columna Ciudad (oculta)
                DataGridViewTextBoxColumn colCiudad = new DataGridViewTextBoxColumn();
                colCiudad.DataPropertyName = "Ciudad";
                colCiudad.HeaderText = "Ciudad";
                colCiudad.Visible = false;
                colCiudad.Name = "Ciudad";
                dgvDestino.Columns.Add(colCiudad);

                // Columna Ciudad (oculta)
                DataGridViewTextBoxColumn colPrincipal = new DataGridViewTextBoxColumn();
                colPrincipal.DataPropertyName = "CiudadPrincipal";
                colPrincipal.HeaderText = "Principal";
                // colPrincipal.Visible = false;
                colPrincipal.Name = "CiudadPrincipal";
                dgvDestino.Columns.Add(colPrincipal);

                dgvDestino.DataSource = dtDestino;
            }
        }
        private void cargarProductos()
        {
            ProductosENAC getProducto = new ProductosENAC
            {
                Opcion = "Listado"
            };
            var dt = logica.SP_ProductosENAC(getProducto);

            if (dt.Rows.Count > 0)
            {
                // 🔹 Blindaje: asegurar columna SeleccionarProducto
                if (!dt.Columns.Contains("SeleccionarProducto"))
                {
                    dt.Columns.Add("SeleccionarProducto", typeof(bool));
                    foreach (DataRow row in dt.Rows)
                        row["SeleccionarProducto"] = false;
                }

                // 🔹 Blindaje: asegurar columna Producto
                if (!dt.Columns.Contains("Producto"))
                {
                    dt.Columns.Add("Producto", typeof(string));
                }

                // 🔹 Actualizar el DataGridView en el hilo de la UI
                
                this.Invoke((MethodInvoker)delegate
                {
                    dgvProducto.AutoGenerateColumns = false;
                    dgvProducto.Columns.Clear();

                    // Columna CheckBox
                    DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn
                    {
                        Name = "SeleccionarProducto",
                        HeaderText = "✔",
                        Width = 50,
                        DataPropertyName = "SeleccionarProducto"
                    };
                    chkCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    chkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvProducto.Columns.Add(chkCol);

                    // Columna GrupoID
                    DataGridViewTextBoxColumn colGrupoID = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "GrupoID",
                        HeaderText = "GrupoID",
                        Width = 275,
                        ReadOnly = true,
                        Name = "GrupoID",
                        Visible = false
                    };
                    dgvProducto.Columns.Add(colGrupoID);

                    // Columna Nombre
                    DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Nombre",
                        HeaderText = "Nombre",
                        Width = 275,
                        ReadOnly = true,
                        Name = "Nombre"
                    };
                    dgvProducto.Columns.Add(colNombre);

                    // Columna Producto
                    DataGridViewTextBoxColumn colProducto = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Producto",
                        HeaderText = "Producto",
                        Name = "Producto"
                    };
                    dgvProducto.Columns.Add(colProducto);

                    dgvProducto.DataSource = dt;
                });

                dtProducto = dt; // 🔹 Guardar referencia global
            }
        }


    }
}
