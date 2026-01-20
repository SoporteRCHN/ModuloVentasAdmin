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

namespace ModuloVentasAdmin
{
    public partial class frmCotizaciones : Form
    {
        public int OrigenID, DestinoID, ProductoID, _EncabezadoID, _DetalleID = 0;
        public string NombreOrigen, NombreDestino, NombreProducto = "";
        public bool _TerminosExisten, _estaActualizando = false;

        public DataTable dtOrigen = new DataTable();
        public DataTable dtDestino = new DataTable();
        public DataTable dtProducto = new DataTable();
        public DataTable dtTerminos = new DataTable();
        public DataTable dtTipos = new DataTable();
        public DataTable dtImpuestos = new DataTable();
        public DataTable dtGetCotizaciones = new DataTable();
        public DataTable dtEncabezado = new DataTable();
        public DataTable dtDetalle = new DataTable();

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
            cargarDestino();
            cargarProductos();
            cargarTipos();
            cargarTerminos();
            cargarImpuestos();
            cmbImpuesto.SelectedIndex = 0;
            cargarCotizaciones();
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

                // Aplicamos el texto ingresdo como filtro de busqueda
                dv.RowFilter = string.Format("Convert(Ciudad, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtroDestino.Replace("'", "''"));

                dgvDestino.DataSource = dv;
            }
            string filtroProducto = txtProducto.Text.Trim();

            if (dtProducto != null && dtProducto.Rows.Count > 0)
            {
                DataView dv = new DataView(dtProducto);

                // Aplicamos el texto ingresdo como filtro de busqueda
                dv.RowFilter = string.Format("Convert(Producto, 'System.String') LIKE '%{0}%' OR Nombre LIKE '%{0}%'", filtroProducto.Replace("'", "''"));

                dgvProducto.DataSource = dv;
            }
            var origenesSeleccionados = ObtenerOrigenesSeleccionados();
            var destinosSeleccionados = ObtenerDestinosSeleccionados();
            var productosSeleccionados = ObtenerProductosSeleccionados();

            // Validación: al menos un origen marcado
            if (origenesSeleccionados == null || origenesSeleccionados.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un origen antes de continuar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // no sigue
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

            using (var frm = new frmCotizacionDinamica(origenesSeleccionados, destinosSeleccionados, productosSeleccionados))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var registros = frm.ObtenerCotizaciones();

                    // Pintar la tabla
                    GenerarTablaRegistroFinal(registros, productosSeleccionados, origenesSeleccionados, destinosSeleccionados);

                    // Guardar en variables de clase para usarlas luego
                    registrosGenerados = registros;
                    productosGenerados = productosSeleccionados;
                    origenesGenerados = origenesSeleccionados;
                    destinosGenerados = destinosSeleccionados;
                }
            }
        }

        private void GenerarTablaRegistroFinal(List<CotizacionRegistro> registros, List<ProductoSeleccionado> productos, List<frmCotizacionDinamica.CiudadOrigen> origenes, List<CiudadDestino> destinos)
        {
            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            int totalColumnas = 1 + productos.Count + 1; // +1 columna extra en blanco
            tlpRegistroFinal.ColumnCount = totalColumnas;
            tlpRegistroFinal.RowCount = 2;

            // Alturas: 30% encabezado, 70% detalle
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            // Columna 0 fija para ciudades
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));

            // Columnas de productos (máximo 200px)
            foreach (var p in productos)
                tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));

            // Columna extra en blanco → absorbe espacio sobrante
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Encabezado fila 0 → nombres de orígenes concatenados
            string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
            tlpRegistroFinal.Controls.Add(new Label
            {
                Text = origenesConcat,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = false,
                MaximumSize = new Size(250, 0)
            }, 0, 0);

            for (int i = 0; i < productos.Count; i++)
            {
                tlpRegistroFinal.Controls.Add(new Label
                {
                    Text = productos[i].Nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    MaximumSize = new Size(200, 0)
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

            // Fila 1 → precios por producto (igual que tlpCotizacion pero deshabilitado)
            for (int i = 0; i < productos.Count; i++)
            {
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

                tlpRegistroFinal.Controls.Add(nudPrecio, 1 + i, 1);
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
            // Recorremos todos los destinos y productos
            for (int d = 0; d < destinosGenerados.Count; d++)
            {
                for (int p = 0; p < productosGenerados.Count; p++)
                {
                    // La celda está en columna (1+p), fila (1+d)
                    var control = tlpRegistroFinal.GetControlFromPosition(1 + p, 1 + d);
                    if (control is NumericUpDown nud)
                    {
                        decimal nuevoPrecio = nud.Value;

                        // Buscar el registro correspondiente
                        var reg = registrosGenerados.FirstOrDefault(r =>
                            r.ProductoID == productosGenerados[p].ProductoID &&
                            r.CiudadDestinoID == destinosGenerados[d].CiudadID &&
                            r.CiudadOrigenID == origenesGenerados.First().CiudadID // si manejas varios orígenes, ajusta aquí
                        );

                        if (reg != null)
                        {
                            reg.PrecioNormal = nuevoPrecio;
                        }
                    }
                }
            }
        }


        private void btnGuardarNacional_Click(object sender, EventArgs e)
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
                // 🔹 Actualizar registros con los valores actuales del NumericUpDown
                ActualizarRegistrosDesdeTabla();

                // 🔹 Guardar y generar PDF con los valores actualizados
                guardarEncabezado(registrosGenerados, origenesGenerados, destinosGenerados, productosGenerados);
                GenerarPDFCotizacion(registrosGenerados, productosGenerados, origenesGenerados, destinosGenerados, dtTerminos);

                LimpiarTodo();
                cargarCotizaciones();
                tabControl1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Primero debe generar la cotización antes de guardarla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
                MessageBox.Show("Cotización guardada correctamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Ha ocurrido un error al guardar la cotización.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btnPDF_Click(object sender, EventArgs e)
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
            
                
           
        }
        private void GenerarPDFCotizacion(List<CotizacionRegistro> registros,List<ProductoSeleccionado> productos,List<frmCotizacionDinamica.CiudadOrigen> origenes,List<CiudadDestino> destinos,DataTable dtTerminos)
        {
            Document doc = new Document(PageSize.LETTER, 40, 40, 40, 40);
            string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Cotizacion.pdf");

            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // 🔹 Encabezado con logos y título
                PdfPTable headerTable = new PdfPTable(3);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 120, 150, 100f });

                // Logo izquierda
                iTextSharp.text.Image logoIzq = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\RCHondurasColor.png");
                logoIzq.ScaleAbsolute(130, 150);
                PdfPCell cellLogoIzq = new PdfPCell(logoIzq);
                cellLogoIzq.Border = Rectangle.NO_BORDER;
                cellLogoIzq.HorizontalAlignment = Element.ALIGN_LEFT;
                cellLogoIzq.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(cellLogoIzq);

                // Texto central
                var fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                PdfPCell cellTitulo = new PdfPCell(new Phrase("COTIZACION", fontTitulo));
                cellTitulo.Border = Rectangle.NO_BORDER;
                cellTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(cellTitulo);

                // Logo derecha
                iTextSharp.text.Image logoDer = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\RCPaqueteria.png");
                logoDer.ScaleAbsolute(130, 30);
                PdfPCell cellLogoDer = new PdfPCell(logoDer);
                cellLogoDer.Border = Rectangle.NO_BORDER;
                cellLogoDer.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellLogoDer.VerticalAlignment = Element.ALIGN_MIDDLE;
                headerTable.AddCell(cellLogoDer);

                // 🔹 Opcional: altura fija para toda la fila
                headerTable.DefaultCell.FixedHeight = 50;

                doc.Add(headerTable);


                // 🔹 Cliente y Fecha
                PdfPTable clienteFechaTable = new PdfPTable(2);
                clienteFechaTable.WidthPercentage = 100;
                clienteFechaTable.SetWidths(new float[] { 250f, 180f });

                PdfPCell cellCliente = new PdfPCell(new Phrase("CLIENTE: " + txtClienteNombre.Text, fontCell));
                cellCliente.Border = Rectangle.NO_BORDER;
                cellCliente.HorizontalAlignment = Element.ALIGN_LEFT;
                clienteFechaTable.AddCell(cellCliente);

                PdfPCell cellFecha = new PdfPCell(new Phrase("FECHA: " + DateTime.Now.ToString("dd/MM/yyyy"), fontCell));
                cellFecha.Border = iTextSharp.text.Rectangle.NO_BORDER;
                cellFecha.HorizontalAlignment = Element.ALIGN_RIGHT;
                clienteFechaTable.AddCell(cellFecha);

                doc.Add(clienteFechaTable);

                // 🔹 Atención y Código
                PdfPTable atencionCodigoTable = new PdfPTable(2);
                atencionCodigoTable.WidthPercentage = 100;
                atencionCodigoTable.SetWidths(new float[] { 250f, 250f });

                PdfPCell cellAtencion = new PdfPCell(new Phrase("ATENCIÓN:  " + txtAtencion.Text, fontCell));
                cellAtencion.Border = Rectangle.NO_BORDER;
                cellAtencion.HorizontalAlignment = Element.ALIGN_LEFT;
                atencionCodigoTable.AddCell(cellAtencion);

                PdfPCell cellCodigo = new PdfPCell(new Phrase("CÓDIGO:  " + txtClienteID.Text, fontCell));
                cellCodigo.Border = Rectangle.NO_BORDER;
                cellCodigo.HorizontalAlignment = Element.ALIGN_RIGHT;
                atencionCodigoTable.AddCell(cellCodigo);

                doc.Add(atencionCodigoTable);

                // 🔹 Espacio
                doc.Add(new Paragraph("\n"));

                // 🔹 Cotización (productos/destinos/precios)
                int maxColsPorBloque = 5;
                for (int start = 0; start < productos.Count; start += maxColsPorBloque)
                {
                    int end = Math.Min(start + maxColsPorBloque, productos.Count);

                    PdfPTable table = new PdfPTable(1 + maxColsPorBloque);
                    table.WidthPercentage = 100;

                    float[] widths = new float[1 + maxColsPorBloque];
                    widths[0] = 200f;
                    for (int i = 1; i < widths.Length; i++)
                        widths[i] = 100f;
                    table.SetWidths(widths);

                    // 🔹 Fila 1: encabezado
                    string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
                    PdfPCell cellOrigen = new PdfPCell(new Phrase("SUCURSAL REMITENTE: " + origenesConcat, fontHeader));
                    cellOrigen.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellOrigen.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellOrigen.PaddingTop = 10f;
                    cellOrigen.PaddingBottom = 10f;
                    table.AddCell(cellOrigen);

                    for (int i = 0; i < maxColsPorBloque; i++)
                    {
                        if (start + i < productos.Count)
                        {
                            PdfPCell cellProd = new PdfPCell(new Phrase(productos[start + i].Nombre, fontHeader));
                            cellProd.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellProd.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellProd.PaddingTop = 10f;
                            cellProd.PaddingBottom = 10f;
                            table.AddCell(cellProd);
                        }
                        else
                        {
                            PdfPCell empty = new PdfPCell(new Phrase("", fontHeader));
                            empty.Border = Rectangle.NO_BORDER;
                            empty.PaddingTop = 10f;
                            empty.PaddingBottom = 10f;
                            table.AddCell(empty);
                        }
                    }

                    // 🔹 Fila 2: destinos + precios
                    string destinosConcat = string.Join("\n", destinos.Select(d => d.Nombre));
                    PdfPCell cellDestinos = new PdfPCell(new Phrase(destinosConcat, fontCell));
                    cellDestinos.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellDestinos.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellDestinos.PaddingTop = 5f;
                    cellDestinos.PaddingBottom = 5f;
                    table.AddCell(cellDestinos);

                    for (int i = 0; i < maxColsPorBloque; i++)
                    {
                        if (start + i < productos.Count)
                        {
                            var reg = registros.FirstOrDefault(r => r.ProductoID == productos[start + i].ProductoID);
                            string precio = reg != null ? reg.PrecioNormal.ToString("N2") : "0.00";

                            PdfPCell cellPrecio = new PdfPCell(new Phrase(precio, fontCell));
                            cellPrecio.HorizontalAlignment = Element.ALIGN_CENTER;
                            cellPrecio.VerticalAlignment = Element.ALIGN_MIDDLE;
                            cellPrecio.PaddingTop = 20f;
                            cellPrecio.PaddingBottom = 20f;
                            table.AddCell(cellPrecio);
                        }
                        else
                        {
                            PdfPCell empty = new PdfPCell(new Phrase("", fontCell));
                            empty.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            empty.PaddingTop = 20f;
                            empty.PaddingBottom = 20f;
                            table.AddCell(empty);
                        }
                    }

                    doc.Add(table);
                    doc.Add(new Paragraph(" ", fontCell));
                }

                // 🔹 Después de la tabla, agregamos los términos
                if (dtTerminos != null && dtTerminos.Rows.Count > 0)
                {
                    doc.Add(new Paragraph("\n"));

                    var fontTituloTerminos = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11);
                    Paragraph titulo = new Paragraph("TERMINOS Y NEGOCIACIONES ESPECIALES", fontTituloTerminos);
                    titulo.Alignment = Element.ALIGN_LEFT;
                    doc.Add(titulo);

                    // 🔹 Texto del impuesto seleccionado
                    if (cmbImpuesto.SelectedItem != null)
                    {
                        var fontImpuesto = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                        Paragraph impuestoSel = new Paragraph(cmbImpuesto.Text, fontImpuesto);
                        impuestoSel.Alignment = Element.ALIGN_LEFT;
                        doc.Add(impuestoSel);
                    }

                    var fontTermino = FontFactory.GetFont(FontFactory.HELVETICA, 9);
                    foreach (DataRow row in dtTerminos.Rows)
                    {
                        string descripcion = row["Descripcion"].ToString();
                        Paragraph p = new Paragraph("- " + descripcion, fontTermino);
                        p.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p);
                    }
                }

                // 🔹 Espacio de 2 líneas
                doc.Add(new Paragraph("\n\n\n\n"));

                // 🔹 Tabla de firmas (Autorizado / Aprobado Por)
                PdfPTable firmasTable = new PdfPTable(2);
                firmasTable.WidthPercentage = 100;
                firmasTable.SetWidths(new float[] { 250f, 250f });

                // Celda izquierda: línea + texto "Autorizado"
                PdfPCell cellLineaIzq = new PdfPCell();
                cellLineaIzq.Border = Rectangle.NO_BORDER;
                cellLineaIzq.HorizontalAlignment = Element.ALIGN_CENTER;

                // Línea
                Paragraph lineaIzq = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2)));
                cellLineaIzq.AddElement(lineaIzq);

                // Texto debajo
                Paragraph autorizado = new Paragraph("Autorizado", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10));
                autorizado.Alignment = Element.ALIGN_CENTER;
                cellLineaIzq.AddElement(autorizado);

                // 🔹 Nombre y cargo
                Paragraph nombreAutorizado = new Paragraph("Nancy D. Valle", FontFactory.GetFont(FontFactory.HELVETICA, 9));
                nombreAutorizado.Alignment = Element.ALIGN_CENTER;
                cellLineaIzq.AddElement(nombreAutorizado);

                Paragraph cargoAutorizado = new Paragraph("Gerente Administrativo", FontFactory.GetFont(FontFactory.HELVETICA, 9));
                cargoAutorizado.Alignment = Element.ALIGN_CENTER;
                cellLineaIzq.AddElement(cargoAutorizado);

                firmasTable.AddCell(cellLineaIzq);

                // Celda derecha: línea + texto "Aprobado Por"
                PdfPCell cellLineaDer = new PdfPCell();
                cellLineaDer.Border = Rectangle.NO_BORDER;
                cellLineaDer.HorizontalAlignment = Element.ALIGN_CENTER;

                // Línea
                Paragraph lineaDer = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2)));
                cellLineaDer.AddElement(lineaDer);

                // Texto debajo
                Paragraph aprobado = new Paragraph("Aprobado Por Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10));
                aprobado.Alignment = Element.ALIGN_CENTER;
                cellLineaDer.AddElement(aprobado);

                // 🔹 Firma y sello
                Paragraph firmaSello = new Paragraph("Firma y Sello", FontFactory.GetFont(FontFactory.HELVETICA, 9));
                firmaSello.Alignment = Element.ALIGN_CENTER;
                cellLineaDer.AddElement(firmaSello);

                // 🔹 Nombre del cliente
                Paragraph nombreCliente = new Paragraph(txtAtencion.Text, FontFactory.GetFont(FontFactory.HELVETICA, 9));
                nombreCliente.Alignment = Element.ALIGN_CENTER;
                cellLineaDer.AddElement(nombreCliente);

                firmasTable.AddCell(cellLineaDer);

                doc.Add(firmasTable);


                doc.Close();
            }

            System.Diagnostics.Process.Start(path);
            MessageBox.Show("PDF generado en el escritorio.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                _EncabezadoID = Convert.ToInt32(dgvCotizaciones.CurrentRow.Cells["EncabezadoID"].Value);
                recuperarEncabezado(_EncabezadoID);

                btnGuardarNacional.Enabled = false;
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
                btnGuardarNacional.Visible = false;
                btnLimpiar.Visible = false;

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
            GenerarPDFCotizacion(registrosGenerados, productosGenerados, origenesGenerados, destinosGenerados, dtTerminos);
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
                        var filasCoincidentesDestino = dtDestino.Select($"CiudadPrincipal = '{ciudadDestinoID}'");
                        foreach (var fila in filasCoincidentesDestino)
                            fila["SeleccionarDestino"] = true;
                    }
                }

                // 🔹 Marcamos coincidencias de PRODUCTO
                foreach (DataRow rowDetalle in dtDetalle.Rows)
                {
                    string productoID = rowDetalle["ProductoID"].ToString();
                    var filasCoincidentesProducto = dtProducto.Select($"Producto = '{productoID}'");
                    foreach (var fila in filasCoincidentesProducto)
                        fila["SeleccionarProducto"] = true;
                }

                // 🔹 Refrescamos los DataGridView
                dgvOrigen.Refresh();
                dgvDestino.Refresh();
                dgvProducto.Refresh();

                // 🔹 Construimos listas de seleccionados (blindadas con ?? new List<>)
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


        public List<CotizacionRegistro> ObtenerCotizacionesDesdeDetalle(
    List<ProductoSeleccionado> productos,
    List<CiudadOrigen> origenes,
    List<CiudadDestino> destinos,
    DataTable dtDetalle)
        {
            var registros = new List<CotizacionRegistro>();

            foreach (var producto in productos)
            {
                foreach (var destino in destinos)
                {
                    foreach (var origen in origenes)
                    {
                        // Buscar el precio en dtDetalle para esta combinación
                        var filas = dtDetalle.Select(
                            $"ProductoID = '{producto.ProductoID}' AND CiudadOrigenID = '{origen.CiudadID}' AND CiudadDestinoID = '{destino.CiudadID}'");

                        decimal precio = 0;
                        if (filas.Length > 0)
                            precio = Convert.ToDecimal(filas[0]["Precio"]);

                        registros.Add(new CotizacionRegistro
                        {
                            ProductoID = producto.ProductoID,
                            CiudadOrigenID = origen.CiudadID,
                            CiudadDestinoID = destino.CiudadID,
                            PrecioNormal = precio
                        });
                    }
                }
            }

            return registros;
        }
        private void GenerarTablaRecuperada(
        List<CotizacionRegistro> registros,
        List<ProductoSeleccionado> productos,
        List<CiudadOrigen> origenes,
        List<CiudadDestino> destinos)
        {
            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            int totalColumnas = 1 + productos.Count + 1;
            tlpRegistroFinal.ColumnCount = totalColumnas;
            tlpRegistroFinal.RowCount = 2;

            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
            foreach (var p in productos)
                tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Encabezado
            string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
            tlpRegistroFinal.Controls.Add(new Label
            {
                Text = origenesConcat,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = false,
                MaximumSize = new Size(250, 0)
            }, 0, 0);

            for (int i = 0; i < productos.Count; i++)
            {
                tlpRegistroFinal.Controls.Add(new Label
                {
                    Text = productos[i].Nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    MaximumSize = new Size(200, 0)
                }, 1 + i, 0);
            }

            // Destinos con centrado
            var panelDestinos = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
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

            // 🔹 Centrado vertical y horizontal como en el primer método
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

            // Precios desde registros (mejor que dtDetalle)
            for (int i = 0; i < productos.Count; i++)
            {
                var reg = registros.FirstOrDefault(r => r.ProductoID == productos[i].ProductoID);
                decimal precio = reg != null ? reg.PrecioNormal : 0;

                var nudPrecio = new NumericUpDown
                {
                    Anchor = AnchorStyles.None,
                    Size = new Size(120, 40),
                    DecimalPlaces = 2,
                    Maximum = 1000000,
                    Minimum = 0,
                    Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Regular),
                    Value = precio,
                    Enabled = false
                };

                tlpRegistroFinal.Controls.Add(nudPrecio, 1 + i, 1);
            }
        }


        private void frmCotizacionesV2_Load(object sender, EventArgs e)
        {

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
            dtProducto = logica.SP_ProductosENAC(getProducto);

            if (dtProducto.Rows.Count > 0)
            {
                // Si no existe la columna Seleccionar en el DataTable, la agregamos
                if (!dtProducto.Columns.Contains("SeleccionarProducto"))
                {
                    dtProducto.Columns.Add("SeleccionarProducto", typeof(bool));

                    // Inicializamos todas las filas en false
                    foreach (DataRow row in dtProducto.Rows)
                    {
                        row["SeleccionarProducto"] = false;
                    }
                }

                dgvProducto.AutoGenerateColumns = false;
                dgvProducto.Columns.Clear();

                // Columna CheckBox
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "SeleccionarProducto";
                chkCol.HeaderText = "✔";
                chkCol.Width = 50;
                chkCol.DataPropertyName = "SeleccionarProducto";
                chkCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                chkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvProducto.Columns.Add(chkCol);

                // Columna Nombre
                DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
                colNombre.DataPropertyName = "Nombre";
                colNombre.HeaderText = "Nombre";
                colNombre.Width = 275;
                colNombre.ReadOnly = true;
                colNombre.Name = "Nombre";
                dgvProducto.Columns.Add(colNombre);

                // Columna Producto (oculta)
                DataGridViewTextBoxColumn colProducto = new DataGridViewTextBoxColumn();
                colProducto.DataPropertyName = "Producto";
                colProducto.HeaderText = "Producto";
                // colProducto.Visible = false;
                colProducto.Name = "Producto";
                dgvProducto.Columns.Add(colProducto);

                dgvProducto.DataSource = dtProducto;
            }
        }
    }
}
