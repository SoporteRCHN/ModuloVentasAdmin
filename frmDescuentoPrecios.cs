using iTextSharp.text.pdf;
using iTextSharp.text;
using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using System.Diagnostics;


namespace ModuloVentasAdmin
{
    public partial class frmDescuentoPrecios : Form
    {
        clsLogica logica = new clsLogica();
        DataTable dtProducto = new DataTable();
        DataTable dtOrigenCompleto = new DataTable();
        DataTable dtgetInfoPrincipal = new DataTable();
        DataTable dtgetInfoAledano = new DataTable();
        DataTable dtgetInfoPrincipalDetalle = new DataTable();
        DataTable dtgetInfoAledanoDetalle = new DataTable();
        DataTable dtgetInfoAledanoDetalleOUT = new DataTable();
        DataTable _dtRegistroPrincipal = new DataTable();
        DataTable _dtRegistroAledano =new DataTable();
        public DataTable dtTerminos = new DataTable();

        private List<ProductoSeleccionado> productosSeleccionados = new List<ProductoSeleccionado>();
        private List<DestinoRegistro> destinosSeleccionados = new List<DestinoRegistro>();

        public string _Nombre, _Cliente, _ProductoID = "";
        public int _TipoCosto, _errorPrincipal, _errorAledano, _EncabezadoID, _PrincipalAledano, _CiudadesOUT = 0;
        public decimal _Costo = 0;
        public frmDescuentoPrecios()
        {
            InitializeComponent();
            cargarOrigenes();
            cargarProductos();
            cargarTerminos();
        
            }
        public class ProductoSeleccionado
        {
            public string ProductoID { get; set; }
            public string Nombre { get; set; }
            public bool Seleccionado { get; set; }
        }

        public class DestinoRegistro
        {
            public string CiudadID { get; set; }
            public string Nombre { get; set; }
            public int Orden { get; set; }
        }
        public class DestinoRegistroDetalle
        {
            public string SucarsalID { get; set; }
            public string CiudadDestino { get; set; }
            public string Producto { get; set; }
            public string Nombre { get; set; }
            public int Orden { get; set; }
            public decimal Costo { get; set; }
        }
        private void cargarTerminos()
        {
            CotizacionTerminoDTO getTerminos = new CotizacionTerminoDTO
            {
                Opcion = "Listar"
            };
            dtTerminos = logica.SP_CotizacionTerminos(getTerminos);
        }
        private void rdbNombre_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbNombre.Checked == true)
            {
                mostrarClientes();
            }
        }
        private void cargarClientes()
        {
            string _Opcion = "BuscarPorCodigoLigero";

            ClienteENAC getClientes = new ClienteENAC
            {
                Opcion = _Opcion,
                Cliente = txtBuscar.Text,
                Nombre = txtBuscar.Text,
            };
            DataTable dtGetCliente = logica.SP_ClientesENAC(getClientes);
            if (dtGetCliente.Rows.Count > 0)
            {
                _Nombre = dtGetCliente.Rows[0]["NombreCompleto"].ToString();
                _Cliente = dtGetCliente.Rows[0]["ClienteID"].ToString();
                _TipoCosto = Convert.ToInt32(dtGetCliente.Rows[0]["TipoCosto"]);

                lblClienteINombre.Text = "TARIFARIO: " + _Cliente + " - " + _Nombre;
                lblTipoCliente.Text = (_TipoCosto == 2) ? "Negociacion Especial" : "Cliente Normal";
                txtValor.Text = "0";
                txtNombreCliente.Text = _Nombre;
       

                Toast.Mostrar("Cliente " + _Nombre + " ha sido seleccionado.", TipoAlerta.Info);
            }
            else
            {
                Toast.Mostrar("No se encontro ese codigo de cliente.", TipoAlerta.Warning);
                lblClienteINombre.Text = "-";
                _Nombre = String.Empty;
                _Cliente = String.Empty;
                lblTipoCliente.Text = "-";
                txtValor.Enabled = false;
                txtValor.Text = "0";
                label4.Visible = false;
                txtNombreCliente.Text = String.Empty;
     
                txtBuscar.Focus();
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
                        _Nombre = Mensaje.ClienteNombre;
                        _Cliente = Mensaje.ClienteId.ToString();
                        _TipoCosto = Mensaje._TipoCosto;

                        lblClienteINombre.Text = "TARIFARIO: " + _Cliente + " - " + _Nombre;
                        lblTipoCliente.Text = (_TipoCosto == 2) ? "Negociacion Especial" : "Cliente Normal";
                        txtValor.Text = "0";
                        txtNombreCliente.Text = _Nombre;

                        Toast.Mostrar("Cliente " + _Nombre + " ha sido seleccionado.", TipoAlerta.Info);
            
                    }
                    else
                    {
                        lblTipoCliente.Text = "-";
                        txtValor.Enabled = false;
                        txtValor.Text = "0";
                        label4.Visible = false;
                        Toast.Mostrar("No se selecciono cliente.", TipoAlerta.Warning);
                        txtBuscar.Text = String.Empty;
                        txtNombreCliente.Text = String.Empty;
                 
                        txtBuscar.Focus();
                    }
                }
                MensajeAdvertencia.Dispose();

            }
            if (_TipoCosto == 2)
            {
                Toast.Mostrar("El cliente ya tiene una negociacion especial activa.", TipoAlerta.Error);
            }

            rdbCodigo.Checked = true;
            txtBuscar.Focus();
        }
        private void cargarOrigenes()
        {
            TarifarioSucursal getOrigenes = new TarifarioSucursal
            {
                Opcion = "LISTADO"
            };
            DataTable dtOrigenes = logica.SP_TarifarioSucursales(getOrigenes);
            if (dtOrigenes.Rows.Count > 0)
            {
                cmbOrigen.DataSource = dtOrigenes;
                cmbOrigen.DisplayMember = "Nombre";
                cmbOrigen.ValueMember = "SucursalID";
                cmbOrigen.SelectedIndex = 0;

                cargarOrigenesEspecifico();
            }
        }
        private void cargarOrigenesEspecifico()
        {
            TarifarioSucursal getOrigenCompleto = new TarifarioSucursal
            {
                Opcion = "RECUPERAR",
                SucursalID = cmbOrigen.SelectedValue.ToString(),
            };
            dtOrigenCompleto = null;
            dtOrigenCompleto = logica.SP_TarifarioSucursales(getOrigenCompleto);

            if (dtOrigenCompleto.Rows.Count > 0) 
            {
                destinosSeleccionados = null;
                destinosSeleccionados = (from DataRow dr in dtOrigenCompleto.Rows
                                         select new DestinoRegistro
                                         {
                                             CiudadID = dr["CiudadDestino"].ToString(),
                                             Nombre = dr["Nombre"].ToString(),
                                             Orden = Convert.ToInt32(dr["Orden"])
                                         }).ToList();
            }
        }


        private void btnCliente_Click(object sender, EventArgs e)
        {
            mostrarClientes();
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
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

        private void txtBuscar_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.IsInputKey = true; // 🔹 obliga a tratar Tab como tecla de entrada
            }
        }
        private void cargarProductos()
        {
            ProductosENAC getProducto = new ProductosENAC { Opcion = "ListadoMedidas" };
            var dt = logica.SP_ProductosENAC(getProducto);

     
            if (dt.Rows.Count > 0)
            {
                if (!dt.Columns.Contains("SeleccionarProducto"))
                {
                    dt.Columns.Add("SeleccionarProducto", typeof(bool));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["SeleccionarProducto"] = false;
                    }
                }

                if (!dt.Columns.Contains("Producto"))
                {
                    dt.Columns.Add("Producto", typeof(string));
                }

              dgvProducto.AutoGenerateColumns = false;
                    dgvProducto.Columns.Clear();

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

                    DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Nombre",
                        HeaderText = "Nombre",
                        Width = 275,
                        ReadOnly = true,
                        Name = "Nombre"
                    };
                    dgvProducto.Columns.Add(colNombre);

                    DataGridViewTextBoxColumn colProducto = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Producto",
                        HeaderText = "Producto",
                        Name = "Producto"
                    };
                    dgvProducto.Columns.Add(colProducto);

                    DataGridViewTextBoxColumn colMedidas = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Descripcion",
                        HeaderText = "Descripcion",
                        Name = "Descripcion"
                    };
                    dgvProducto.Columns.Add(colMedidas);

                dgvProducto.DataSource = dt;

                dtProducto = dt; 
            }
        }

        private void cmbOrigen_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarOrigenesEspecifico();
        }

        private void GenerarTablaTarifario(List<DestinoRegistro> destinos, List<ProductoSeleccionado> productos)
        {
            dgvTarifario.Columns.Clear();
            dgvTarifario.Rows.Clear();

            dgvTarifario.AutoGenerateColumns = false;
            dgvTarifario.AllowUserToAddRows = false;
            dgvTarifario.RowHeadersVisible = false;
            dgvTarifario.SelectionMode = DataGridViewSelectionMode.CellSelect;

            dgvTarifario.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvTarifario.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // 🔹 Columna 0 → Destinos
            var colDestinos = new DataGridViewTextBoxColumn
            {
                HeaderText = cmbOrigen.Text,
                Name = "Destinos",
                Width = 200,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.TopLeft
                }
            };
            dgvTarifario.Columns.Add(colDestinos);

            // 🔹 Columnas de productos + aledaños
            foreach (var p in productos)
            {
                dgvTarifario.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = p.Nombre,
                    Name = $"Prod_{p.ProductoID}",
                    Width = 100
                });

                dgvTarifario.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "ALEDAÑO",
                    Name = $"ALEDAÑO_{p.ProductoID}",
                    Width = 50
                });
            }

            var grupos = destinos.GroupBy(d => d.Orden).OrderBy(g => g.Key);

            foreach (var grupo in grupos)
            {
                int rowIndex = dgvTarifario.Rows.Add();
                dgvTarifario.Rows[rowIndex].Cells["Destinos"].Value =
                    string.Join(Environment.NewLine, grupo.Select(d => d.Nombre));

                int ordenActual = grupo.Key;

                foreach (var p in productos)
                {
                    string colProd = $"Prod_{p.ProductoID}";
                    string colAledano = $"ALEDAÑO_{p.ProductoID}";

                    // 🔹 Precio principal
                    var rowsCostoPrincipal = dtgetInfoPrincipal.AsEnumerable()
                        .Where(r => r["Producto"].ToString() == p.ProductoID
                                 && Convert.ToInt32(r["Orden"]) == ordenActual)
                        .ToList();

                    if (rowsCostoPrincipal.Any())
                    {
                        // Agrupar por costo y contar ocurrencias
                        var costoAgrupado = rowsCostoPrincipal
                            .GroupBy(r => Convert.ToDecimal(r["Costo"]))
                            .Select(g => new { Valor = g.Key, Conteo = g.Count() })
                            .OrderByDescending(g => g.Conteo)   // primero el que más se repite
                            .ThenByDescending(g => g.Valor)     // si hay empate, el más alto
                            .First();

                        var cellProd = dgvTarifario.Rows[rowIndex].Cells[colProd];
                        cellProd.Value = costoAgrupado.Valor;
                        cellProd.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // centrado
                    }


                    // 🔹 Precio aledaño
                    var rowsCostoAledano = dtgetInfoAledano.AsEnumerable()
                        .Where(r => r["Producto"].ToString() == p.ProductoID
                                 && Convert.ToInt32(r["Orden"]) == ordenActual)
                        .ToList();

                    if (rowsCostoAledano.Any())
                    {
                        // Agrupar por costo y contar ocurrencias
                        var costoAgrupadoAledano = rowsCostoAledano
                            .GroupBy(r => Convert.ToDecimal(r["Costo"]))
                            .Select(g => new { Valor = g.Key, Conteo = g.Count() })
                            .OrderByDescending(g => g.Conteo)   // primero el que más se repite
                            .ThenByDescending(g => g.Valor)     // si hay empate, el más alto
                            .First();

                        var cellAledano = dgvTarifario.Rows[rowIndex].Cells[colAledano];
                        cellAledano.Value = costoAgrupadoAledano.Valor;
                        cellAledano.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // centrado
                    }
                }
            }

            // 🔹 Ajustar altura del encabezado dinámicamente según “Aledaños”
            int altoAledaño = dgvTarifario.Font.Height * "ALEDAÑO".Length +40; // cada letra ocupa una línea
            dgvTarifario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvTarifario.ColumnHeadersHeight = altoAledaño; // altura total

            // 🔹 Pintar encabezados personalizados
            dgvTarifario.Paint += (s, e) =>
            {
                // Rectángulo que cubre todas las columnas de productos + aledaños
                int firstProdIndex = 1; // primera columna de producto
                int totalWidth = dgvTarifario.Columns.Cast<DataGridViewColumn>()
                                  .Skip(firstProdIndex)
                                  .Sum(c => c.Width);

                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(
                    dgvTarifario.GetCellDisplayRectangle(firstProdIndex, -1, true).Left,
                    dgvTarifario.GetCellDisplayRectangle(firstProdIndex, -1, true).Top,
                    totalWidth,
                    25 // altura de la fila superior
                );

                using (var brush = new SolidBrush(dgvTarifario.ColumnHeadersDefaultCellStyle.BackColor))
                    e.Graphics.FillRectangle(brush, rect);

                using (var brush = new SolidBrush(dgvTarifario.ColumnHeadersDefaultCellStyle.ForeColor))
                {
                    var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    e.Graphics.DrawString("DIMENSIONES Y PESO DE CAJA", dgvTarifario.Font, brush, rect, format);
                }

                // Línea divisoria
                e.Graphics.DrawLine(Pens.Black, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
            };
            dgvTarifario.CellPainting += (s, e) =>
            {
                if (e.RowIndex == -1 && e.ColumnIndex >= 0)
                {
                    var col = dgvTarifario.Columns[e.ColumnIndex];
                    var rect = e.CellBounds;

                    // Ajustar para que empiece debajo de la primera fila (DIMENSIONES Y PESO DE CAJA)
                    rect.Y += 25;
                    rect.Height = dgvTarifario.ColumnHeadersHeight - 25;

                    e.PaintBackground(rect, true);

                    using (var brush = new SolidBrush(dgvTarifario.ColumnHeadersDefaultCellStyle.ForeColor))
                    {
                        if (col.HeaderText == "ALEDAÑO")
                        {
                            // 🔹 Texto vertical completo
                            float x = rect.Left + (rect.Width / 2);
                            float y = rect.Top + 5;

                            foreach (char c in col.HeaderText.ToUpper())
                            {
                                e.Graphics.DrawString(c.ToString(),
                                                      dgvTarifario.Font,
                                                      brush,
                                                      new PointF(x - (dgvTarifario.Font.Size / 2), y));
                                y += dgvTarifario.Font.Height;
                            }
                        }
                        else if (col.Name.StartsWith("Prod_"))
                        {
                            // 🔹 Dividir espacio en dos: texto arriba + campo de 20px abajo
                            int campoAltura = 20;
                            System.Drawing.Rectangle rectTexto = new System.Drawing.Rectangle(rect.Left, rect.Top, rect.Width, rect.Height - campoAltura);
                            System.Drawing.Rectangle rectCampo = new System.Drawing.Rectangle(rect.Left, rect.Bottom - campoAltura, rect.Width, campoAltura);

                            // Texto del producto centrado en la parte superior
                            var format = new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            };
                            e.Graphics.DrawString(col.HeaderText, dgvTarifario.Font, brush, rectTexto, format);

                            // 🔹 Buscar medida en dtProducto
                            string medida = "";
                            if (dtProducto != null)
                            {
                                var row = dtProducto.AsEnumerable()
                                    .FirstOrDefault(r => r["Nombre"].ToString() == col.HeaderText);
                                if (row != null)
                                    medida = row["Descripcion"].ToString();
                            }

                            // Campo de 20px dentro del encabezado
                            using (var campoBrush = new SolidBrush(Color.White))
                                e.Graphics.FillRectangle(campoBrush, rectCampo);

                            // 🔹 Borde inferior del campo
                            e.Graphics.DrawRectangle(Pens.Black, rectCampo);

                            // 🔹 Pintar la medida (si existe)
                            if (!string.IsNullOrEmpty(medida))
                            {
                                var formatMedida = new StringFormat
                                {
                                    Alignment = StringAlignment.Center,
                                    LineAlignment = StringAlignment.Center
                                };
                                e.Graphics.DrawString(medida, dgvTarifario.Font, Brushes.Black, rectCampo, formatMedida);
                            }
                        }
                        else
                        {
                            // Columna Destinos u otras → texto normal
                            var format = new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            };
                            e.Graphics.DrawString(col.HeaderText, dgvTarifario.Font, brush, rect, format);
                        }
                    }

                    e.Handled = true;
                }
            };
        }
      
        private void dgvProducto_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void cargarPreciosPrincipales()
        {
            string productosCsv = string.Join(",", productosSeleccionados.Select(p => p.ProductoID));
            decimal _isv = (chkISV.Checked) ? Convert.ToDecimal("0.15") : Convert.ToDecimal(0);
            if (_TipoCosto == 1)
            {
                ProductoCiudadENAC getPrecios = new ProductoCiudadENAC
                {
                    Opcion = "ListadoCiudadPrincipal",
                    Productos = productosCsv, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Impuesto = _isv
                };
                dtgetInfoPrincipal = logica.SP_ProductosCiudadesENAC(getPrecios);
            }
            else if (_TipoCosto == 2) 
            {
                ProductoClienteCostos getPreciosClienteCostos = new ProductoClienteCostos
                {
                    Opcion = "ListadoCiudadPrincipal",
                    Productos = productosCsv, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Cliente = _Cliente,
                    Impuesto = _isv
                };
                dtgetInfoPrincipal = logica.SP_ProductosClienteCostos(getPreciosClienteCostos);
            }
        }
        private void cargarPreciosAledanos()
        {
            string productosCsvAledano = string.Join(",", productosSeleccionados.Select(p => p.ProductoID));
            decimal _isv = (chkISV.Checked) ? Convert.ToDecimal("0.15") : Convert.ToDecimal(0);

            if (_TipoCosto == 1)
            {
                ProductoCiudadENAC getPreciosAledanos = new ProductoCiudadENAC
                {
                    Opcion = "ListadoAledanoPrincipal",
                    Productos = productosCsvAledano,
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Impuesto = _isv
                };

                dtgetInfoAledano = logica.SP_ProductosCiudadesENAC(getPreciosAledanos);
            }
            else if (_TipoCosto == 2) 
            {
                ProductoClienteCostos getPreciosClienteCostos = new ProductoClienteCostos
                {
                    Opcion = "ListadoAledanoPrincipal",
                    Productos = productosCsvAledano, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Cliente = _Cliente,
                    Impuesto = _isv
                };
                dtgetInfoAledano = logica.SP_ProductosClienteCostos(getPreciosClienteCostos);
            }
        }

        private void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(_Cliente) || String.IsNullOrWhiteSpace(_Cliente)) 
            {
                Toast.Mostrar("Debe seleccionar un cliente primero.", TipoAlerta.Warning);
                return;
            }

            bool haySeleccionado = false;

            foreach (DataGridViewRow row in dgvProducto.Rows)
            {
                if (!row.IsNewRow)
                {
                    var cellValue = row.Cells["SeleccionarProducto"].Value;
                    if (cellValue != null && Convert.ToBoolean(cellValue))
                    {
                        haySeleccionado = true;
                        break; 
                    }
                }
            }

            if (!haySeleccionado)
            {
                Toast.Mostrar("No tiene productos seleccionados.", TipoAlerta.Warning);
                return;
            }
           

            productosSeleccionados = null;
            dtgetInfoPrincipal = null;
            dtgetInfoAledano = null;
            dtgetInfoPrincipalDetalle = null;
            dtgetInfoAledanoDetalle = null;
            dgvTarifario.Rows.Clear();
            dgvTarifario.DataSource = null;
            dgvTarifarioDetalle.DataSource = null;
            dgvAledanosDetalle.Columns.Clear();
            dgvAledanosDetalle.DataSource = null;

            if (String.IsNullOrWhiteSpace(txtValor.Text) || String.IsNullOrEmpty(txtValor.Text))
            {
                txtValor.Text = "0";
            }

            productosSeleccionados = (from DataGridViewRow row in dgvProducto.Rows
                                      where Convert.ToBoolean(row.Cells["SeleccionarProducto"].Value) == true
                                      select new ProductoSeleccionado
                                      {
                                          ProductoID = row.Cells["Producto"].Value.ToString(),
                                          Nombre = row.Cells["Nombre"].Value.ToString(),
                                          Seleccionado = true
                                      }).ToList();
         

            cargarPreciosPrincipales(); //Recupero los precios de ciudades principales
            cargarPreciosAledanos(); // Recupero los precios de ciudades aledanos
            GenerarTablaTarifario(destinosSeleccionados, productosSeleccionados);

            cargarPreciosPrincipalDetalle(); //Cargo detalle principales
            cargarPreciosAledanosDetalle(); //Cargo detalle Aledanos

            if (tabControl1.SelectedTab == tabPage1)
            {
                label4.Visible = false;
                label10.Visible = false;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                label4.Visible = true;
                label4.Text =  _errorPrincipal.ToString();

                label10.Visible = false;

            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                label4.Visible = true;
                label4.Text =  _errorAledano.ToString();


                label10.Visible = true;
                label10.Text = _CiudadesOUT.ToString();
            }
        }

        private void cargarPreciosPrincipalDetalle()
        {
            string productosCsv = string.Join(",", productosSeleccionados.Select(p => p.ProductoID));
            decimal _isv = (chkISV.Checked) ? Convert.ToDecimal("0.15") : Convert.ToDecimal(0);
            if (_TipoCosto == 1)
            {
                ProductoCiudadENAC getPreciosDetalle = new ProductoCiudadENAC
                {
                    Opcion = "ListadoPrincipalDetalle",
                    Productos = productosCsv, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Impuesto = _isv
                };

                dtgetInfoPrincipalDetalle = logica.SP_ProductosCiudadesENAC(getPreciosDetalle);
            }
            else if (_TipoCosto == 2) 
            {
                ProductoClienteCostos getPreciosClienteCostos = new ProductoClienteCostos
                {
                    Opcion = "ListadoPrincipalDetalle",
                    Productos = productosCsv, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Cliente = _Cliente,
                    Impuesto = _isv
                };
                dtgetInfoPrincipalDetalle = logica.SP_ProductosClienteCostos(getPreciosClienteCostos);
            }

            if (dtgetInfoPrincipalDetalle.Rows.Count > 0)
            {
                dgvTarifarioDetalle.Columns.Clear();
                dgvTarifarioDetalle.Rows.Clear();

                dgvTarifarioDetalle.AutoGenerateColumns = true; // 🔹 porque ya viene pivotado desde SQL
                dgvTarifarioDetalle.DataSource = dtgetInfoPrincipalDetalle;

                dgvTarifarioDetalle.DataBindingComplete += (s, e) => validarPrincipales();

                // Opcional: ajustar estilo
                dgvTarifarioDetalle.AllowUserToAddRows = false;
                dgvTarifarioDetalle.RowHeadersVisible = false;
                dgvTarifarioDetalle.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvTarifarioDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvTarifarioDetalle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTarifarioDetalle.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvTarifarioDetalle.Columns["Orden"].Visible = false;
                dgvTarifarioDetalle.Refresh();
            }
        }

        private void AgregarOUT() 
        {
           ;
        }
        private void cargarPreciosAledanosDetalle()
        {
            string productosCsvAledanosDetalle = string.Join(",", productosSeleccionados.Select(p => p.ProductoID));
            decimal _isv = (chkISV.Checked) ? Convert.ToDecimal("0.15") : Convert.ToDecimal(0);

            if (_TipoCosto == 1)
            {
                ProductoCiudadENAC getPreciosAledanosDetalle = new ProductoCiudadENAC
                {
                    Opcion = "ListadoAledanoDetalle",
                    Productos = productosCsvAledanosDetalle,
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Impuesto = _isv
                };

                dtgetInfoAledanoDetalle = logica.SP_ProductosCiudadesENAC(getPreciosAledanosDetalle);


                ProductoCiudadENAC getPreciosAledanosDetalleOUT = new ProductoCiudadENAC
                {
                    Opcion = "ListadoAledanoDetalleRegistroOUT",
                    Productos = productosCsvAledanosDetalle,
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text)
                };

                dtgetInfoAledanoDetalleOUT = logica.SP_ProductosCiudadesENAC(getPreciosAledanosDetalleOUT);

            }
            else if (_TipoCosto == 2)
            {
                ProductoClienteCostos getPreciosClienteCostos = new ProductoClienteCostos
                {
                    Opcion = "ListadoAledanoDetalle",
                    Productos = productosCsvAledanosDetalle, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Cliente = _Cliente,
                    Impuesto = _isv
                };
                dtgetInfoAledanoDetalle = logica.SP_ProductosClienteCostos(getPreciosClienteCostos);

                ProductoClienteCostos getPreciosClienteCostosOUT = new ProductoClienteCostos
                {
                    Opcion = "ListadoAledanoDetalleRegistroOUT",
                    Productos = productosCsvAledanosDetalle, // aquí ya va "00001,00002,00003"
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Cliente = _Cliente
                };
                dtgetInfoAledanoDetalleOUT = logica.SP_ProductosClienteCostos(getPreciosClienteCostosOUT);
            }

            if (dtgetInfoAledanoDetalle.Rows.Count > 0)
            {
                dgvAledanosDetalle.Columns.Clear();
                dgvAledanosDetalle.Rows.Clear();

                dgvAledanosDetalle.AutoGenerateColumns = true; // 🔹 ahora sí, porque ya viene pivotado
                dgvAledanosDetalle.DataSource = dtgetInfoAledanoDetalle;

                dgvAledanosDetalle.DataBindingComplete += (s, e) => validarAledanos();

                // Opcional: ajustar estilo
                dgvAledanosDetalle.AllowUserToAddRows = false;
                dgvAledanosDetalle.RowHeadersVisible = false;
                dgvAledanosDetalle.SelectionMode = DataGridViewSelectionMode.CellSelect;
                dgvAledanosDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvAledanosDetalle.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvAledanosDetalle.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //dgvAledanosDetalle.Columns["Orden"].Visible = false;
                dgvAledanosDetalle.Refresh();
            }

            if (dtgetInfoAledanoDetalleOUT.Rows.Count > 0)
            {
                foreach (DataRow rowOut in dtgetInfoAledanoDetalleOUT.Rows)
                {
                    DataRow nuevaFila = dtgetInfoAledanoDetalle.NewRow();

                    // 🔹 Usar el Orden que ya viene en OUT
                    nuevaFila["Orden"] = rowOut["Orden"];
                    nuevaFila["DestinoID"] = rowOut["Ciudad"];
                    nuevaFila["CiudadDestino"] = rowOut["Nombre"];

                    foreach (DataColumn col in dtgetInfoAledanoDetalle.Columns)
                    {
                        if (col.ColumnName != "Orden" && col.ColumnName != "DestinoID" && col.ColumnName != "CiudadDestino")
                        {
                            nuevaFila[col.ColumnName] = 0;
                        }
                    }

                    dtgetInfoAledanoDetalle.Rows.Add(nuevaFila);
                    _CiudadesOUT++;
                }

                dtgetInfoAledanoDetalle.DefaultView.Sort = "Orden ASC";
                dgvAledanosDetalle.DataSource = dtgetInfoAledanoDetalle;
                dgvAledanosDetalle.Refresh();
            }


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                label4.Visible = false;
                label10.Visible = false;
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                label4.Visible = true;
                label4.Text = _errorPrincipal.ToString();

                label10.Visible = false;

            }
            else if (tabControl1.SelectedTab == tabPage3)
            {
                label4.Visible = true;
                label4.Text = _errorAledano.ToString();


                label10.Visible = true;
                label10.Text = _CiudadesOUT.ToString();
            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ExportarTarifarioPDF();
        }
        private void ExportarTarifarioPDF()
        {
            string rutaEscritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string rutaArchivo = Path.Combine(rutaEscritorio, "Tarifario.pdf");

            Document doc = new Document(PageSize.LETTER, 40, 40, 20, 20);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(rutaArchivo, FileMode.Create));
            doc.Open();

            var fontCell = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // ─────────────────────────────────────────────────────────
            // 🔹 ENCABEZADO  ─  logos 135×40
            // ─────────────────────────────────────────────────────────
            PdfPTable headerTable = new PdfPTable(3);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 135f, 200f, 135f });

            iTextSharp.text.Image logoIzq = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\RCHondurasColor.png");
            logoIzq.ScaleAbsolute(135, 40);
            headerTable.AddCell(new PdfPCell(logoIzq)
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            headerTable.AddCell(new PdfPCell(new Phrase("COTIZACION",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16)))
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            iTextSharp.text.Image logoDer = iTextSharp.text.Image.GetInstance(@"\\192.168.1.179\Logos\RCPaqueteria.png");
            logoDer.ScaleAbsolute(135, 40);
            headerTable.AddCell(new PdfPCell(logoDer)
            {
                Border = iTextSharp.text.Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
            doc.Add(headerTable);
            doc.Add(new Paragraph("\n"));

            // ─────────────────────────────────────────────────────────
            // 🔹 CLIENTE / FECHA / ATENCIÓN / CÓDIGO
            // ─────────────────────────────────────────────────────────
            PdfPTable clienteFechaTable = new PdfPTable(2);
            clienteFechaTable.WidthPercentage = 100;
            clienteFechaTable.SetWidths(new float[] { 250f, 180f });
            clienteFechaTable.AddCell(new PdfPCell(new Phrase("CLIENTE: " + txtNombreCliente.Text, fontCell))
            { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 2f });
            clienteFechaTable.AddCell(new PdfPCell(new Phrase("FECHA: " + DateTime.Now.ToString("dd/MM/yyyy"), fontCell))
            { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 2f });
            doc.Add(clienteFechaTable);

            PdfPTable atencionTable = new PdfPTable(2);
            atencionTable.WidthPercentage = 100;
            atencionTable.SetWidths(new float[] { 250f, 200f });
            atencionTable.AddCell(new PdfPCell(new Phrase("ATENCIÓN:  " + txtNombreCliente.Text, fontCell))
            { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT, PaddingBottom = 2f });
            atencionTable.AddCell(new PdfPCell(new Phrase("CÓDIGO:  " + _Cliente.ToString(), fontCell))
            { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_RIGHT, PaddingBottom = 2f });
            doc.Add(atencionTable);
            doc.Add(new Paragraph("\n"));

            // ══════════════════════════════════════════════════════════════
            // 🎛️  VARIABLES DE CONTROL  ← ajusta SOLO aquí para mover todo
            // ══════════════════════════════════════════════════════════════

            // Distancia desde el TOP de la página al TOP de la tabla.
            // Baja este número  → tabla sube (y también suben términos/firmas).
            // Sube este número  → tabla baja (y también bajan términos/firmas).
            float tablaOffsetDesdeTop = 140f;

            // Margen extra entre el borde inferior de la última fila y el bloque Términos+Firmas.
            // Ponlo más negativo  → términos suben más.
            // Ponlo más positivo  → términos bajan más.
            float offsetTerminos = 65f;

            // ──────────────────────────────────────────────────────────────

            PdfContentByte cb = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);

            float startX = 40f;
            float pageWidth = doc.PageSize.Width - 80f;
            float startY = doc.PageSize.Height - tablaOffsetDesdeTop;

            // ──────────────────────────────────────────────────────────────
            // 🔹 MEDIDAS DE LA TABLA
            // ──────────────────────────────────────────────────────────────
            float anchoDestinos = 195f;
            float anchoProducto = 80f;
            float anchoAledano = 50f;

            float headerHeightTop = 14f;  // fila "DIMENSIONES Y PESO DE CAJA"
            float headerHeight = 60f;  // fila nombres de productos + medida
            float campoMedida = 14f;  // subcampo de medida (dentro de headerHeight)

            float fontSizeCiudad = 8f;
            float lineHeight = fontSizeCiudad + 3f;
            float rowPaddingV = 3f;

            int productosPorHoja = 4;
            int columnasPorProducto = 2;
            int totalProductos = dgvTarifario.Columns
                                        .Cast<DataGridViewColumn>()
                                        .Count(c => c.Name.StartsWith("Prod_"));
            int totalBloques = (int)Math.Ceiling((double)totalProductos / productosPorHoja);

            float yPosFinalGlobal = startY;

            for (int bloque = 0; bloque < totalBloques; bloque++)
            {
                var columnasBloque = dgvTarifario.Columns
                    .Cast<DataGridViewColumn>()
                    .Where(c => c.Name.StartsWith("Prod_") || c.Name.StartsWith("ALEDAÑO_"))
                    .Skip(bloque * productosPorHoja * columnasPorProducto)
                    .Take(productosPorHoja * columnasPorProducto)
                    .ToList();

                // Escala si no cabe en la página
                float anchoTotal = anchoDestinos
                                 + columnasBloque.Count(c => c.Name.StartsWith("Prod_")) * anchoProducto
                                 + columnasBloque.Count(c => c.Name.StartsWith("ALEDAÑO_")) * anchoAledano;

                float factor = anchoTotal > pageWidth ? pageWidth / anchoTotal : 1f;
                float aD = anchoDestinos * factor;
                float aP = anchoProducto * factor;
                float aA = anchoAledano * factor;

                float firstProdX = startX + aD;
                float totalWidth = columnasBloque.Count(c => c.Name.StartsWith("Prod_")) * aP
                                 + columnasBloque.Count(c => c.Name.StartsWith("ALEDAÑO_")) * aA;

                // ══════════════════════════════════════════════════════════
                // 🔹 HEADER UNIFICADO SIN ESPACIO
                //
                //  startY ┬────────────────────────────────────────────────
                //         │  DEST.  │   DIMENSIONES Y PESO DE CAJA        │ ← headerHeightTop
                //         ├─────────┼──────────────────┬───────────────────┤
                //         │         │  NOMBRE PRODUCTO  │  NOMBRE PRODUCTO  │
                //         │  DEST.  │                  │                   │ ← headerHeight
                //         │         ├──────────────────┼───────────────────┤
                //         │         │  medida          │  medida           │ ← campoMedida
                //  nomBottom ├──────┴──────────────────┴───────────────────┤
                //         │ filas de datos                                  │
                // ══════════════════════════════════════════════════════════

                float dimTop = startY;                   // top de la franja DIMENSIONES
                float dimBottom = dimTop - headerHeightTop; // bottom DIMENSIONES = top nombres
                float nomTop = dimBottom;                // top de la franja nombres
                float nomBottom = nomTop - headerHeight;    // bottom nombres = top datos

                // ── Franja DIMENSIONES Y PESO DE CAJA ──
                DrawRect(cb, firstProdX, dimBottom, totalWidth, headerHeightTop);
                DrawTextCenter(cb, bf, 7f, "DIMENSIONES Y PESO DE CAJA",
                    firstProdX, dimBottom, totalWidth, headerHeightTop);

                // ── Columna Destinos: abarca TODA la altura del header (unificada) ──
                float altaHeader = headerHeightTop + headerHeight; // altura total del header
                DrawRect(cb, startX, nomBottom, aD, altaHeader);
                DrawTextCenter(cb, bfBold, 8f, dgvTarifario.Columns[0].HeaderText,
                    startX, nomBottom, aD, altaHeader);

                // ── Columnas de productos y aledaños ──
                float x = startX + aD;

                foreach (var col in columnasBloque)
                {
                    float cw = col.Name.StartsWith("Prod_") ? aP : aA;

                    // Marco de la franja de nombres (solo la franja nomTop→nomBottom)
                    DrawRect(cb, x, nomBottom, cw, headerHeight);

                    if (col.Name.StartsWith("ALEDAÑO_") || col.HeaderText == "ALEDAÑO")
                    {
                        // Texto "ALEDAÑO" vertical, fuente 6, centrado en la celda
                        float ty = nomTop - 10f;
                        foreach (char ch in "ALEDAÑO")
                        {
                            ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER,
                                new Phrase(ch.ToString(),
                                    new iTextSharp.text.Font(bf, 6f, iTextSharp.text.Font.BOLD)),
                                x + cw / 2f, ty, 0);
                            ty -= 7f;
                        }
                    }
                    else if (col.Name.StartsWith("Prod_"))
                    {
                        // Nombre del producto centrado en la zona superior (encima del subcampo)
                        ColumnText ctNombre = new ColumnText(cb);
                        ctNombre.SetSimpleColumn(
                            new Phrase(col.HeaderText, new iTextSharp.text.Font(bfBold, 8f)),
                            x + 2f,
                            nomBottom + campoMedida,  // encima del subcampo medida
                            x + cw - 2f,
                            nomTop,                   // hasta el tope de la franja nombres
                            10f,
                            Element.ALIGN_CENTER
                        );
                        ctNombre.Go();

                        // Subcampo de medida (borde inferior dentro de headerHeight)
                        DrawRect(cb, x, nomBottom, cw, campoMedida);

                        string medida = "";
                        if (dtProducto != null)
                        {
                            var rowMedida = dtProducto.AsEnumerable()
                                .FirstOrDefault(r => r["Nombre"].ToString() == col.HeaderText);
                            if (rowMedida != null)
                                medida = rowMedida["Descripcion"].ToString();
                        }

                        if (!string.IsNullOrEmpty(medida))
                            DrawTextCenter(cb, bf, 7f, medida, x, nomBottom, cw, campoMedida);
                    }

                    x += cw;
                }

                // ──────────────────────────────────────────────────────────
                // 🔹 FILAS DE DATOS  ─  pegadas justo debajo del header
                // ──────────────────────────────────────────────────────────
                float yPos = nomBottom;  // empieza exactamente donde terminó el header

                foreach (DataGridViewRow row in dgvTarifario.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Calcular altura real de la fila según el texto
                    float maxHeight = lineHeight+15 + rowPaddingV * 2;

                    var cellDestino = row.Cells[0];
                    if (cellDestino.Value != null)
                    {
                        string textoDestino = cellDestino.Value.ToString();
                        int lineas = textoDestino.Split('\n').Length;
                        float hCalc = lineas * lineHeight + rowPaddingV * 2;
                        if (hCalc > maxHeight) maxHeight = hCalc;
                    }

                    float xPos = startX;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        bool esDestino = cell.OwningColumn == dgvTarifario.Columns[0];
                        bool esBloque = columnasBloque.Contains(cell.OwningColumn);
                        if (!esDestino && !esBloque) continue;

                        float cw = esDestino ? aD :
                                   cell.OwningColumn.Name.StartsWith("Prod_") ? aP :
                                   cell.OwningColumn.Name.StartsWith("ALEDAÑO_") ? aA : aD;

                        string texto = cell.Value?.ToString() ?? "";

                        DrawRect(cb, xPos, yPos - maxHeight, cw, maxHeight);

                        ColumnText ct = new ColumnText(cb);
                        ct.SetSimpleColumn(
                            new Phrase(texto, new iTextSharp.text.Font(bf, fontSizeCiudad)),
                            xPos + 2f,
                            yPos - maxHeight + rowPaddingV,
                            xPos + cw - 2f,
                            yPos - rowPaddingV,
                            lineHeight,
                            esDestino ? Element.ALIGN_LEFT : Element.ALIGN_CENTER
                        );
                        ct.Go();

                        xPos += cw;
                    }

                    yPos -= maxHeight;

                    if (yPos < 130f)
                    {
                        doc.NewPage();
                        yPos = doc.PageSize.Height - 60f;
                        startY = yPos;
                    }
                }

                yPosFinalGlobal = yPos;

                if (bloque < totalBloques - 1)
                {
                    doc.NewPage();
                    startY = doc.PageSize.Height - 60f;
                    yPosFinalGlobal = startY;
                }
            }

            // ══════════════════════════════════════════════════════════════
            // ✅ SINCRONIZAR cursor doc.Add() con el yPos final de la tabla
            //    + aplicar offsetTerminos
            // ══════════════════════════════════════════════════════════════
            float yDespuesTabla = yPosFinalGlobal + offsetTerminos;

            if (yDespuesTabla < 130f)
            {
                doc.NewPage();
            }
            else
            {
                float yInicioDoc = doc.PageSize.Height - doc.TopMargin;
                float espacioOcupado = yInicioDoc - yDespuesTabla;

                if (espacioOcupado > 0)
                {
                    PdfPTable spacer = new PdfPTable(1);
                    spacer.WidthPercentage = 100;
                    spacer.AddCell(new PdfPCell(new Phrase(" "))
                    {
                        Border = iTextSharp.text.Rectangle.NO_BORDER,
                        FixedHeight = espacioOcupado,
                        MinimumHeight = espacioOcupado
                    });
                    doc.Add(spacer);
                }
            }

            // ─────────────────────────────────────────────────────────
            // 🔹 TÉRMINOS Y CONDICIONES
            // ─────────────────────────────────────────────────────────
            if (dtTerminos != null && dtTerminos.Rows.Count > 0)
            {
                doc.Add(new Paragraph("TERMINOS Y NEGOCIACIONES ESPECIALES",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10)));

                doc.Add(new Paragraph(
                    chkISV.Checked
                        ? "PRECIOS YA INCLUYEN IMPUESTOS SOBRE VENTAS"
                        : "PRECIOS NO INCLUYEN IMPUESTOS SOBRE VENTAS",
                    FontFactory.GetFont(FontFactory.HELVETICA, 8)));

                foreach (DataRow row in dtTerminos.Rows)
                    doc.Add(new Paragraph("- " + row["Descripcion"].ToString(),
                        FontFactory.GetFont(FontFactory.HELVETICA, 8)));
            }

            // ─────────────────────────────────────────────────────────
            // 🔹 FIRMAS
            // ─────────────────────────────────────────────────────────
            doc.Add(new Paragraph("\n\n"));
            PdfPTable firmasTable = new PdfPTable(2);
            firmasTable.WidthPercentage = 100;
            firmasTable.SetWidths(new float[] { 250f, 250f });

            PdfPCell cellIzq = new PdfPCell { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            cellIzq.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2))));
            cellIzq.AddElement(new Paragraph("Autorizado", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)) { Alignment = Element.ALIGN_CENTER });
            cellIzq.AddElement(new Paragraph("Nancy D. Valle", FontFactory.GetFont(FontFactory.HELVETICA, 8)) { Alignment = Element.ALIGN_CENTER });
            cellIzq.AddElement(new Paragraph("GERENTE ADMINISTRATIVO", FontFactory.GetFont(FontFactory.HELVETICA, 8)) { Alignment = Element.ALIGN_CENTER });
            firmasTable.AddCell(cellIzq);

            PdfPCell cellDer = new PdfPCell { Border = iTextSharp.text.Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER };
            cellDer.AddElement(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 60f, BaseColor.BLACK, Element.ALIGN_CENTER, -2))));
            cellDer.AddElement(new Paragraph("Aprobado Por Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9)) { Alignment = Element.ALIGN_CENTER });
            cellDer.AddElement(new Paragraph("Firma y Sello", FontFactory.GetFont(FontFactory.HELVETICA, 8)) { Alignment = Element.ALIGN_CENTER });
            cellDer.AddElement(new Paragraph(txtNombreCliente.Text, FontFactory.GetFont(FontFactory.HELVETICA, 8)) { Alignment = Element.ALIGN_CENTER });
            firmasTable.AddCell(cellDer);

            doc.Add(firmasTable);

            doc.Close();
            writer.Close();
            Process.Start(new ProcessStartInfo(rutaArchivo) { UseShellExecute = true });
        }

        // ─────────────────────────────────────────────────────────────────────────────
        // 🔧 HELPERS
        // ─────────────────────────────────────────────────────────────────────────────

        /// <summary>Dibuja un rectángulo sin relleno.</summary>
        private void DrawRect(PdfContentByte cb, float x, float y, float w, float h)
        {
            cb.Rectangle(x, y, w, h);
            cb.Stroke();
        }

        /// <summary>Escribe texto centrado horizontal y verticalmente dentro de un área.</summary>
        private void DrawTextCenter(PdfContentByte cb, BaseFont font, float fontSize,
            string text, float x, float y, float w, float h)
        {
            ColumnText ct = new ColumnText(cb);
            ct.SetSimpleColumn(
                new Phrase(text, new iTextSharp.text.Font(font, fontSize)),
                x + 2f, y + 2f,
                x + w - 2f, y + h - 2f,
                fontSize + 2f,
                Element.ALIGN_CENTER
            );
            ct.Go();
        }


        // ─────────────────────────────────────────────────────────────────────────
        // 🔹 HELPERS  ─  dibujar rectángulo y texto centrado (simplifican el código)
        // ─────────────────────────────────────────────────────────────────────────


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            DialogResult resultado = ToastDialog.Mostrar("Desea solicitar la aprobacion para otorgar el descuento a este cliente?", TipoAlerta.Info);

            string _DesAprobacion = (_TipoCosto == 2) ? "DescuentoNegociacionEspecial" : "DescuentoPrecios";

            if (resultado == DialogResult.OK)
            {
         
                productosSeleccionados = (from DataGridViewRow row in dgvProducto.Rows
                                          where Convert.ToBoolean(row.Cells["SeleccionarProducto"].Value) == true
                                          select new ProductoSeleccionado
                                          {
                                              ProductoID = row.Cells["Producto"].Value.ToString(),
                                              Nombre = row.Cells["Nombre"].Value.ToString(),
                                              Seleccionado = true
                                          }).ToList();

                string productosCsvAledano = string.Join(",", productosSeleccionados.Select(p => p.ProductoID));

                //Insertar en tabla para Aprobacion
                decimal _Isv = (chkISV.Checked) ? Convert.ToDecimal("0.15") : Convert.ToDecimal("0");
                CotizacionDescuento sendDescuento = new CotizacionDescuento 
                {
                    Opcion = "Agregar",
                    TipoCosto = _TipoCosto,
                    Cliente = _Cliente,
                    CiudadRemitente = cmbOrigen.SelectedValue.ToString(),
                    Productos = productosCsvAledano,
                    Descuento = Convert.ToDecimal(txtValor.Text),
                    Impuesto = _Isv,
                    EstadoAprobacion = 1,
                    UPosteo = DynamicMain.usuarionlogin,
                    FPosteo = DateTime.Now,
                    PC = System.Environment.MachineName,
                    Estado = true
                };
                DataTable dtSendDescuento = logica.SP_CotizacionDescuento(sendDescuento);
                if (dtSendDescuento.Rows.Count > 0 && dtSendDescuento.Rows[0]["Estado"].ToString() == "1") {

                    Toast.Mostrar("Solicitud procesada correctamente.", TipoAlerta.Success);
                    Limpiar();
                }
                else { Toast.Mostrar("Ocurrio un error al realizar la solicitud de aprobacion, Encabezado.", TipoAlerta.Error); }
                               
            }
            else
            {
                Toast.Mostrar("Ha decidido no enviar la solicitud a aprobacion...", TipoAlerta.Warning);
            }
        }

        //private void enviarAledanoDetalle() 
        //{
        //    foreach (DataRow item in _dtRegistroAledano.Rows)
        //    {
        //        CotizacionDescuentoDetalle sendDetalle = new CotizacionDescuentoDetalle
        //        {
        //            Opcion = "Agregar",
        //            DescuentoID = _EncabezadoID,
        //            PrincipalAledano = item["PrincipalAledano"].ToString(),
        //            ProductoID = item["Producto"].ToString(),
        //            CiudadRemitente = item["CiudadRemitente"].ToString(),
        //            Costo = Convert.ToDecimal(item["CiudadRemitente"].ToString()),
        //            EstadoAprobacion = 1,//Ingresado
        //            UPosteo = DynamicMain.usuarionlogin,
        //            FPosteo = DateTime.Now,
        //            PC = System.Environment.MachineName,
        //            Estado = true
        //        };
        //        DataTable dtSendDetalle = logica.SP_CotizacionDescuentoDetalle(sendDetalle);
        //        if (dtSendDetalle.Rows.Count > 0 && dtSendDetalle.Rows[0]["Estado"].ToString() == "0")
        //        {
        //            Toast.Mostrar("Ocurrio un error al realizar la solicitud de aprobacion, Detalle.", TipoAlerta.Error);
        //            return;
        //        }
        //    }
        //}

        private int enviarEncabezado() 
        {
            decimal _Isv = (chkISV.Checked) ? Convert.ToDecimal("0.15") : Convert.ToDecimal("0");
            CotizacionDescuentoEncabezado sendEncabezado = new CotizacionDescuentoEncabezado
            {
                Opcion  = "Agregar",
                Tipo = _TipoCosto, 
                ClienteID = _Cliente,
                Descuento = Convert.ToDecimal(txtValor.Text),
                Impuesto = _Isv,
                EstadoAprobacion = 1, //Ingresado
                UPosteo =DynamicMain.usuarionlogin,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true
            };

            DataTable dtSendEncabezado = logica.SP_CotizacionDescuentoEncabezado(sendEncabezado);
            if(dtSendEncabezado.Rows.Count>0 && dtSendEncabezado.Rows[0]["Estado"].ToString() == "1") 
            {
                return Convert.ToInt32(dtSendEncabezado.Rows[0]["UltimoDescuentoID"]);
            }
            else
            {
                return 0; 
            }
        }

        private void txtImpuesto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void Limpiar() 
        {
            //Limpiamos todos los datagirdviews
            dgvTarifario.DataSource = null;
            dgvTarifarioDetalle.DataSource = null;
            dgvAledanosDetalle.DataSource = null;

            foreach (DataGridViewRow row in dgvProducto.Rows)
            {
                row.Cells["SeleccionarProducto"].Value = false;
            }

            //Limpar DataTables.
            dtOrigenCompleto = null;
            dtgetInfoPrincipal = null;
            dtgetInfoAledano = null;
            dtgetInfoPrincipalDetalle = null;
            dtgetInfoAledanoDetalle = null;

            //Limpiar resto de campos
            rdbCodigo.Checked = true;
            txtBuscar.Text = string.Empty;
            txtNombreCliente.Text = string.Empty;
            lblTipoCliente.Text = "-";
            cmbOrigen.SelectedIndex = 0;
            txtValor.Text = "0";
            chkISV.Checked = false;
            txtProducto.Text = string.Empty;
            label4.Text = "-";
            label4.Visible = false;
            _EncabezadoID = 0;

        }
        private void validarAledanos()
        {
            _errorAledano = 0;

            try
            {
                // 🔹 SUSPENDER el repintado durante la actualización
                dgvAledanosDetalle.SuspendLayout();

                // 🔹 Paleta de colores claros
                Color[] colores = new Color[]
                {
                    Color.LightBlue,
                    Color.LightGreen,
                    Color.LightYellow,
                    Color.LightPink,
                    Color.Lavender,
                    Color.MistyRose,
                    Color.Honeydew
                };

                flpColores.SuspendLayout();

                // ✅ Dispose correcto
                foreach (Control ctrl in flpColores.Controls)
                {
                    if (ctrl is FlowLayoutPanel contenedor)
                    {
                        foreach (Control hijo in contenedor.Controls)
                        {
                            hijo.Dispose();
                        }
                        contenedor.Controls.Clear();
                    }
                    ctrl.Dispose();
                }

                flpColores.Controls.Clear();

                // 🔹 Obtener todos los órdenes distintos
                var ordenes = dgvAledanosDetalle.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["Orden"].Value != null)
                    .Select(r => Convert.ToInt32(r.Cells["Orden"].Value))
                    .Distinct()
                    .OrderBy(o => o)
                    .ToList();

                // 🔹 PRE-CALCULAR: Crear diccionario de valores esperados por orden y producto
                var valoresEsperadosCache = new Dictionary<string, decimal>();

                foreach (var orden in ordenes)
                {
                    var productosOrden = dtgetInfoAledano.AsEnumerable()
                        .Where(r => Convert.ToInt32(r["Orden"]) == orden)
                        .GroupBy(r => r["ProductoNombre"].ToString());

                    foreach (var grupo in productosOrden)
                    {
                        var costoAgrupado = grupo
                            .GroupBy(r => Convert.ToDecimal(r["Costo"]))
                            .Select(g => new { Valor = g.Key, Conteo = g.Count() })
                            .OrderByDescending(g => g.Conteo)
                            .ThenByDescending(g => g.Valor)
                            .First();

                        string key = $"{orden}_{grupo.Key}";
                        valoresEsperadosCache[key] = costoAgrupado.Valor;
                    }
                }

                // 🔹 Asignar colores por orden y crear referencia en flpColores
                Dictionary<int, Color> coloresPorOrden = new Dictionary<int, Color>();
                int colorIndex = 0;

                foreach (var orden in ordenes)
                {
                    Color colorAsignado = colores[colorIndex % colores.Length];
                    coloresPorOrden[orden] = colorAsignado;

                    Panel panelColor = new Panel
                    {
                        BackColor = colorAsignado,
                        Width = 28,
                        Height = 20,
                        Margin = new Padding(3),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    Label lblOrden = new Label
                    {
                        Text = $"Orden {orden}",
                        AutoSize = true,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Margin = new Padding(3)
                    };

                    FlowLayoutPanel contenedor = new FlowLayoutPanel
                    {
                        FlowDirection = FlowDirection.LeftToRight,
                        AutoSize = true
                    };
                    contenedor.Controls.Add(panelColor);
                    contenedor.Controls.Add(lblOrden);

                    flpColores.Controls.Add(contenedor);

                    colorIndex++;
                }

                flpColores.ResumeLayout();

                // 🔹 Recorrer filas y aplicar validaciones
                foreach (DataGridViewRow item in dgvAledanosDetalle.Rows)
                {
                    if (item.IsNewRow) continue;

                    int ordenGrid = Convert.ToInt32(item.Cells["Orden"].Value);
                    bool filaConCero = false;

                    if (coloresPorOrden.ContainsKey(ordenGrid))
                    {
                        item.Cells["Orden"].Style.BackColor = coloresPorOrden[ordenGrid];
                    }

                    for (int colIndex = 3; colIndex < dgvAledanosDetalle.Columns.Count; colIndex++)
                    {
                        string productoNombre = dgvAledanosDetalle.Columns[colIndex].HeaderText;
                        var cell = item.Cells[colIndex];

                        if (cell.Value != null && cell.Value != DBNull.Value)
                        {
                            decimal valorGrid = Convert.ToDecimal(cell.Value);

                            if (valorGrid == 0)
                            {
                                filaConCero = true;
                            }

                            string key = $"{ordenGrid}_{productoNombre}";

                            if (valoresEsperadosCache.ContainsKey(key))
                            {
                                decimal valorEsperado = valoresEsperadosCache[key];

                                if (valorGrid != valorEsperado && valorGrid != 0)
                                {
                                    cell.Style.BackColor = Color.Tomato;
                                    _errorAledano++;
                                }
                                else if (valorGrid != valorEsperado && valorGrid == 0)
                                {
                                    cell.Style.BackColor = Color.Gold;
                                }
                                else
                                {
                                    cell.Style.BackColor = Color.White;
                                }
                            }
                        }
                    }

                    if (filaConCero)
                    {
                        item.DefaultCellStyle.BackColor = Color.Gold;
                    }
                    else
                    {
                        item.DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
            finally
            {
                dgvAledanosDetalle.ResumeLayout();
                dgvAledanosDetalle.Refresh();
            }
        }


        private void validarPrincipales()
        {
            _errorPrincipal = 0;
            foreach (DataGridViewRow item in dgvTarifarioDetalle.Rows)
            {
                int ordenGrid = Convert.ToInt32(item.Cells["Orden"].Value);

                // 🔹 Recorrer todas las columnas de productos (a partir de la tercera)
                for (int colIndex = 3; colIndex < dgvTarifarioDetalle.Columns.Count; colIndex++)
                {
                    string productoNombre = dgvTarifarioDetalle.Columns[colIndex].HeaderText;
                    var cell = item.Cells[colIndex];

                    if (cell.Value != null && cell.Value != DBNull.Value)
                    {
                        decimal valorGrid = Convert.ToDecimal(cell.Value);

                        // Buscar todos los registros en dtgetInfoPrincipal para este Orden + Producto
                        var rowsMatch = dtgetInfoPrincipal.AsEnumerable()
                            .Where(r => Convert.ToInt32(r["Orden"]) == ordenGrid
                                     && r["ProductoNombre"].ToString() == productoNombre)
                            .ToList();

                        if (rowsMatch.Any())
                        {
                            // 🔹 Agrupar por costo y contar ocurrencias
                            var costoAgrupado = rowsMatch
                                .GroupBy(r => Convert.ToDecimal(r["Costo"]))
                                .Select(g => new { Valor = g.Key, Conteo = g.Count() })
                                .OrderByDescending(g => g.Conteo)   // primero el que más se repite
                                .ThenByDescending(g => g.Valor)     // si hay empate, el más alto
                                .First();

                            decimal valorEsperado = costoAgrupado.Valor;

                            // Comparar valores
                            if (valorGrid != valorEsperado)
                            {
                                cell.Style.BackColor = Color.Tomato;
                                _errorPrincipal++;
                            }
                            else
                            {
                                cell.Style.BackColor = Color.White;
                            }
                        }
                    }
                }
            }
        }


        private void frmDescuentoPrecios_Load(object sender, EventArgs e)
        {

        }
    }
}
