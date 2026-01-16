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
using static ModuloVentasAdmin.frmCotizacionDinamica;
using static ModuloVentasAdmin.frmCotizacionDinamica_BCK;

namespace ModuloVentasAdmin
{
    public partial class frmCotizaciones : Form
    {
        public int OrigenID, DestinoID, ProductoID = 0;
        public string NombreOrigen, NombreDestino, NombreProducto = "";

        public DataTable dtOrigen = new DataTable();
        public DataTable dtDestino = new DataTable();
        public DataTable dtProducto = new DataTable();

        clsLogica logica = new clsLogica();

        public frmCotizaciones()
        {
            InitializeComponent();
            cargarOrigen();
            cargarDestino();
            cargarProductos();
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
            var origenesSeleccionados = ObtenerOrigenesSeleccionados();
            var destinosSeleccionados = ObtenerDestinosSeleccionados();
            var productosSeleccionados = ObtenerProductosSeleccionados();

            using (var frm = new frmCotizacionDinamica(origenesSeleccionados, destinosSeleccionados, productosSeleccionados))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var registros = frm.ObtenerCotizaciones();

                    GenerarTablaRegistroFinal(registros, productosSeleccionados, origenesSeleccionados, destinosSeleccionados);
                }
            }
        }
        private void GenerarTablaRegistroFinal(
       List<CotizacionRegistro> registros,
       List<ProductoSeleccionado> productos,
       List<frmCotizacionDinamica.CiudadOrigen> origenes,
       List<CiudadDestino> destinos)
        {
            tlpRegistroFinal.Controls.Clear();
            tlpRegistroFinal.ColumnStyles.Clear();
            tlpRegistroFinal.RowStyles.Clear();

            tlpRegistroFinal.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single; // 🔥 líneas visibles

            int totalColumnas = 1 + productos.Count + 1; // +1 columna extra en blanco
            tlpRegistroFinal.ColumnCount = totalColumnas;
            tlpRegistroFinal.RowCount = 2;

            // Alturas: 30% encabezado, 70% detalle
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            // Columna 0 fija para ciudades
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));

            // Columnas de productos (200px exactos)
            foreach (var p in productos)
                tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200));

            // Columna extra en blanco → absorbe espacio sobrante
            tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Encabezado fila 0 → nombres de orígenes concatenados
            string origenesConcat = string.Join(", ", origenes.Select(o => o.Nombre));
            tlpRegistroFinal.Controls.Add(new Label
            {
                Text = origenesConcat,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            }, 0, 0);

            for (int i = 0; i < productos.Count; i++)
            {
                tlpRegistroFinal.Controls.Add(new Label
                {
                    Text = productos[i].Nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
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

            // Fila 1 → precios por producto (NumericUpDown igual que tlpCotizacion pero deshabilitado)
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
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Value = reg != null ? reg.PrecioNormal : 0,
                    Enabled = false // solo lectura
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
