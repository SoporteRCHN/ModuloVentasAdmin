using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class frmCotizacionDinamica_BCK : Form
    {
        public frmCotizacionDinamica_BCK()
        {
            InitializeComponent();
        }
        public class CiudadDestino
        {
            public string CiudadID { get; set; }
            public string Nombre { get; set; }
            public string CiudadPrincipalID { get; set; }
        }
        public class ProductoSeleccionado
        {
            public string ProductoID { get; set; }
            public string Nombre { get; set; }
        }
        public class CotizacionRegistro
        {
            public string CiudadID { get; set; }
            public string ProductoID { get; set; }
            public decimal PrecioNormal { get; set; }
            public decimal PrecioAledaño { get; set; }
        }
        private void frmCotizacionDinamica_Load(object sender, EventArgs e)
        {
            // Configuración base del TableLayoutPanel
            tlpCotizacion.Dock = DockStyle.Fill;
            tlpCotizacion.AutoScroll = true;
            tlpCotizacion.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        }
        public void GenerarTablaCotizacion(
            List<string> origenes,
            List<CiudadDestino> destinos,
            List<ProductoSeleccionado> productos,
            DataTable dtOrigen)
        {
            tlpCotizacion.Controls.Clear();

            // Principales = orígenes + los que aparecen como CiudadPrincipal en destinos
            var principales = new HashSet<string>(origenes);
            foreach (var d in destinos)
            {
                if (!string.IsNullOrEmpty(d.CiudadPrincipalID))
                    principales.Add(d.CiudadPrincipalID.Trim());
            }

            int totalFilas = principales.Count + destinos.Count(d => principales.Contains(d.CiudadPrincipalID));
            int totalColumnas = 2 + (productos.Count * 3); // Ciudad + Todas + (Nombre + PN + PA por producto)

            tlpCotizacion.ColumnCount = totalColumnas;
            tlpCotizacion.RowCount = totalFilas + 2;

            // Configurar estilos de columnas
            tlpCotizacion.ColumnStyles.Clear();
            tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250)); // Ciudad/Aledaño
            tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40));  // Todas

            for (int i = 0; i < productos.Count; i++)
            {
                tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); // Nombre producto
                tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // PN
                tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));  // PA
            }

            // Encabezado
            tlpCotizacion.Controls.Add(new Label
            {
                Text = "Ciudad / Aledaño",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            }, 0, 0);

            tlpCotizacion.Controls.Add(new Label
            {
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            }, 1, 0);

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
                    MaximumSize = new Size(200, 0) // ahora tiene 200px de ancho disponible
                };
                tlpCotizacion.Controls.Add(lblProducto, 2 + (i * 3), 0);

                tlpCotizacion.SetColumnSpan(lblProducto, 2);


                tlpCotizacion.Controls.Add(new Label
                {
                    Text = "PN",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                }, 2 + (i * 3), 1);

                tlpCotizacion.Controls.Add(new Label
                {
                    Text = "PA",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                }, 3 + (i * 3), 1);
            }

            int filaActual = 2;

            foreach (var principalID in principales)
            {
                // Buscar nombre del principal en dtOrigen
                string nombrePrincipal = dtOrigen.Rows
                    .Cast<DataRow>()
                    .FirstOrDefault(r => r["Ciudad"].ToString().Trim() == principalID)?.Field<string>("Nombre")
                    ?? "Ciudad Principal";

                // Fila gris del principal
                tlpCotizacion.Controls.Add(new Label
                {
                    Text = nombrePrincipal,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BackColor = Color.LightGray
                }, 0, filaActual);

                var chkTodasPrincipal = new CheckBox
                {
                    AutoSize = true,
                    Anchor = AnchorStyles.None,
                    Tag = principalID
                };

                chkTodasPrincipal.CheckedChanged += (s, e) =>
                {
                    bool marcado = ((CheckBox)s).Checked;

                    foreach (var producto in productos)
                    {
                        // PN del principal
                        var txtPNPrincipal = tlpCotizacion.Controls
                            .OfType<TextBox>()
                            .FirstOrDefault(t =>
                            {
                                dynamic info = t.Tag;
                                return info.CiudadID == principalID && info.ProductoID == producto.ProductoID && info.Tipo == "PN";
                            });

                        string valorPN = txtPNPrincipal?.Text;

                        // Hijos de este principal
                        var hijosPrincipal = destinos
                            .Where(d => d.CiudadPrincipalID.Trim() == principalID && d.CiudadID != principalID)
                            .ToList();

                        foreach (var hijo in hijosPrincipal)
                        {
                            var txtPNHijo = tlpCotizacion.Controls
                                .OfType<TextBox>()
                                .FirstOrDefault(t =>
                                {
                                    dynamic info = t.Tag;
                                    return info.CiudadID == hijo.CiudadID && info.ProductoID == producto.ProductoID && info.Tipo == "PN";
                                });

                            if (txtPNHijo != null)
                            {
                                if (marcado)
                                    txtPNHijo.Text = valorPN; // copia PN del principal
                                else
                                    txtPNHijo.Text = ""; // limpia al desmarcar
                            }
                        }
                    }
                };

                tlpCotizacion.Controls.Add(chkTodasPrincipal, 1, filaActual);

                // Campos PN/PA para el principal
                for (int i = 0; i < productos.Count; i++)
                {
                    tlpCotizacion.Controls.Add(new TextBox
                    {
                        Dock = DockStyle.Fill,
                        Tag = new { CiudadID = principalID, ProductoID = productos[i].ProductoID, Tipo = "PN" }
                    }, 2 + (i * 3), filaActual);

                    tlpCotizacion.Controls.Add(new TextBox
                    {
                        Dock = DockStyle.Fill,
                        Tag = new { CiudadID = principalID, ProductoID = productos[i].ProductoID, Tipo = "PA" }
                    }, 3 + (i * 3), filaActual);
                }

                filaActual++;

                // Hijos / aledaños (SIN checkbox "Todas")
                var hijos = destinos.Where(d => d.CiudadPrincipalID.Trim() == principalID && d.CiudadID != principalID).ToList();

                foreach (var hijo in hijos)
                {
                    tlpCotizacion.Controls.Add(new Label
                    {
                        Text = hijo.Nombre,
                        TextAlign = ContentAlignment.MiddleLeft,
                        Dock = DockStyle.Fill
                    }, 0, filaActual);

                    // Columna "Todas" queda vacía en aledaños
                    tlpCotizacion.Controls.Add(new Label
                    {
                        Text = "",
                        Dock = DockStyle.Fill
                    }, 1, filaActual);

                    for (int i = 0; i < productos.Count; i++)
                    {
                        tlpCotizacion.Controls.Add(new TextBox
                        {
                            Dock = DockStyle.Fill,
                            Tag = new { CiudadID = hijo.CiudadID, ProductoID = productos[i].ProductoID, Tipo = "PN" }
                        }, 2 + (i * 3), filaActual);

                        tlpCotizacion.Controls.Add(new TextBox
                        {
                            Dock = DockStyle.Fill,
                            Tag = new { CiudadID = hijo.CiudadID, ProductoID = productos[i].ProductoID, Tipo = "PA" }
                        }, 3 + (i * 3), filaActual);
                    }

                    filaActual++;
                }
            }
        }

        public List<CotizacionRegistro> ObtenerCotizaciones()
        {
            var registros = new List<CotizacionRegistro>();

            foreach (Control ctrl in tlpCotizacion.Controls)
            {
                if (ctrl is TextBox txt && txt.Tag != null)
                {
                    dynamic info = txt.Tag;
                    decimal valor = 0;
                    decimal.TryParse(txt.Text, out valor);

                    // Buscamos si ya existe un registro para esa ciudad/producto
                    var registro = registros.FirstOrDefault(r => r.CiudadID == info.CiudadID && r.ProductoID == info.ProductoID);
                    if (registro == null)
                    {
                        registro = new CotizacionRegistro
                        {
                            CiudadID = info.CiudadID,
                            ProductoID = info.ProductoID
                        };
                        registros.Add(registro);
                    }

                    // Asignamos según el tipo PN o PA
                    if (info.Tipo == "PN")
                        registro.PrecioNormal = valor;
                    else if (info.Tipo == "PA")
                        registro.PrecioAledaño = valor;
                }
            }

            return registros;
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
