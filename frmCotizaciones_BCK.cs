using LogicaVentasAdmin;
using System.Collections.Generic;
using System;
using System.Data;
using System.Windows.Forms;
using static ModuloVentasAdmin.frmCotizacionDinamica_BCK;
using System.Linq;
using System.Drawing;

namespace ModuloVentasAdmin
{
    public partial class frmCotizaciones_BCK : Form
    {
        public int OrigenID, DestinoID, ProductoID = 0;
        public string NombreOrigen, NombreDestino, NombreProducto = "";

        public DataTable dtOrigen = new DataTable();
        public DataTable dtDestino = new DataTable();
        public DataTable dtProducto = new DataTable();

        clsLogica logica = new clsLogica();
        public frmCotizaciones_BCK()
        {
            InitializeComponent();
            cargarOrigen();
            cargarDestino();
            cargarProductos();
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

        private void dgvOrigen_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvOrigen.Columns["SeleccionarOrigen"].Index && e.RowIndex >= 0)
            {
                dgvOrigen.CommitEdit(DataGridViewDataErrorContexts.Commit);
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

        private List<string> ObtenerOrigenesSeleccionados()
        {
            var lista = new List<string>();

            foreach (DataGridViewRow row in dgvOrigen.Rows)
            {
                bool marcado = Convert.ToBoolean(row.Cells["SeleccionarOrigen"].Value);
                if (marcado)
                {
                    string ciudadID = row.Cells["Ciudad"].Value.ToString();
                    lista.Add(ciudadID);
                }
            }

            return lista;
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

        private void dgvOrigen_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Verificamos si el clic fue en la columna "Seleccionar"
            if (dgvOrigen.Columns[e.ColumnIndex].Name == "Seleccionar")
            {
                bool marcarTodo = true;

                // Revisamos si ya hay al menos una fila marcada
                foreach (DataRow row in dtOrigen.Rows)
                {
                    if (row["Seleccionar"] != DBNull.Value && (bool)row["Seleccionar"])
                    {
                        marcarTodo = false; // Si hay alguna marcada, entonces desmarcamos todas
                        break;
                    }
                }

                // Aplicamos el cambio a todas las filas
                foreach (DataRow row in dtOrigen.Rows)
                {
                    row["Seleccionar"] = marcarTodo;
                }

                dgvOrigen.RefreshEdit(); // Refrescamos la vista
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
            ProcesarPorBloques();
            //AbrirCotizacionDinamica();
        }
        private void ProcesarPorBloques()
        {
            var destinosSeleccionados = ObtenerDestinosSeleccionados();
            var origenesSeleccionados = ObtenerOrigenesSeleccionados();
            var productosSeleccionados = ObtenerProductosSeleccionados();

            // Agrupamos destinos por CiudadPrincipalID
            var grupos = destinosSeleccionados
                .GroupBy(d => d.CiudadPrincipalID)
                .ToList();

            foreach (var grupo in grupos)
            {
                string principalID = grupo.Key;

                // Buscar nombre del principal en dtOrigen
                string nombrePrincipal = dtOrigen.Rows
                    .Cast<DataRow>()
                    .FirstOrDefault(r => r["Ciudad"].ToString().Trim() == principalID)?
                    .Field<string>("Nombre") ?? "Ciudad Principal";

                // Armamos bloque: principal + hijos
                var bloqueDestinos = new List<CiudadDestino>();

                // Agregamos principal
                bloqueDestinos.Add(new CiudadDestino
                {
                    CiudadID = principalID,
                    Nombre = nombrePrincipal,
                    CiudadPrincipalID = principalID
                });

                // Agregamos hijos
                bloqueDestinos.AddRange(grupo.Where(d => d.CiudadID != principalID));

                // Abrimos frmCotizacionDinamica para este bloque
                using (var frm = new frmCotizacionDinamica_BCK())
                {
                    frm.GenerarTablaCotizacion(new List<string> { principalID }, bloqueDestinos, productosSeleccionados, dtOrigen);

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // Recibimos registros procesados
                        var registros = frm.ObtenerCotizaciones();

                        // Insertamos en tlpRegistroFinal
                        InsertarEnRegistroFinal(registros, productosSeleccionados, dtDestino);
                    }
                }
            }
        }

        private void frmCotizaciones_Load(object sender, EventArgs e)
        {

        }

        private void InsertarEnRegistroFinal(List<CotizacionRegistro> registros, List<ProductoSeleccionado> productos, DataTable dtDestino)
        {
            // Si es la primera vez, armamos encabezado y estilos de columnas
            if (tlpRegistroFinal.Controls.Count == 0)
            {
                int totalColumnas = 1 + (productos.Count * 3); // Ciudad + (Nombre + PN + PA por producto)
                tlpRegistroFinal.ColumnCount = totalColumnas;
                tlpRegistroFinal.RowCount = 2;

                // Configurar estilos de columnas
                tlpRegistroFinal.ColumnStyles.Clear();
                tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250)); // Ciudad/Aledaño

                for (int i = 0; i < productos.Count; i++)
                {
                    tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); // Nombre producto
                    tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // PN
                    tlpRegistroFinal.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // PA
                }

                // Encabezado
                tlpRegistroFinal.Controls.Add(new Label
                {
                    Text = "Ciudad / Aledaño",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold)
                }, 0, 0);

                for (int i = 0; i < productos.Count; i++)
                {
                    var p = productos[i];

                    var lblProducto = new Label
                    {
                        Text = p.Nombre,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        AutoSize = false,
                        MaximumSize = new Size(200, 0) // wrap en 200px
                    };
                    tlpRegistroFinal.Controls.Add(lblProducto, 1 + (i * 3), 0);
                    tlpRegistroFinal.SetColumnSpan(lblProducto, 2);

                    tlpRegistroFinal.Controls.Add(new Label
                    {
                        Text = "PN",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    }, 1 + (i * 3), 1);

                    tlpRegistroFinal.Controls.Add(new Label
                    {
                        Text = "PA",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    }, 2 + (i * 3), 1);
                }
            }

            // Agrupamos registros por ciudad
            var ciudades = registros.Select(r => r.CiudadID).Distinct();

            foreach (var ciudadID in ciudades)
            {
                int fila = tlpRegistroFinal.RowCount++;
                tlpRegistroFinal.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                // Buscar nombre de la ciudad en dtDestino
                string nombreCiudad = dtDestino.Rows
                    .Cast<DataRow>()
                    .FirstOrDefault(r => r["Ciudad"].ToString().Trim() == ciudadID)?
                    .Field<string>("Nombre") ?? ciudadID;

                // Columna ciudad con nombre
                tlpRegistroFinal.Controls.Add(new Label
                {
                    Text = nombreCiudad,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9, FontStyle.Regular)
                }, 0, fila);

                // PN/PA por producto
                for (int i = 0; i < productos.Count; i++)
                {
                    var producto = productos[i];
                    var reg = registros.FirstOrDefault(r => r.CiudadID == ciudadID && r.ProductoID == producto.ProductoID);

                    // PN
                    tlpRegistroFinal.Controls.Add(new Label
                    {
                        Text = reg?.PrecioNormal.ToString() ?? "",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    }, 1 + (i * 3), fila);

                    // PA
                    tlpRegistroFinal.Controls.Add(new Label
                    {
                        Text = reg?.PrecioAledaño.ToString() ?? "",
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill
                    }, 2 + (i * 3), fila);
                }
            }
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
        // En frmCotizaciones
        private void AbrirCotizacionDinamica()
        {
            var origenes = ObtenerOrigenesSeleccionados();              // IDs de ciudades principales (ej. "00001")
            var destinos = ObtenerDestinosSeleccionados();              // hijos (y principales si los seleccionaste)
            var productos = ObtenerProductosSeleccionados();

            if (origenes.Count == 0 || destinos.Count == 0 || productos.Count == 0)
            {
                MessageBox.Show("Debe seleccionar al menos un origen, un destino y un producto.",
                                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var frm = new frmCotizacionDinamica_BCK();
            frm.GenerarTablaCotizacion(origenes, destinos, productos, dtOrigen);
            frm.ShowDialog();
        }


    }
}
