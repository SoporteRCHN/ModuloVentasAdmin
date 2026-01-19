using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ModuloVentasAdmin.frmCotizacionDinamica_BCK;

namespace ModuloVentasAdmin
{
    public partial class frmCotizacionDinamica : Form
    {
        public class CiudadOrigen
        {
            public string CiudadID { get; set; }
            public string Nombre { get; set; }
        }

        private List<CiudadOrigen> _origenes; // ahora con nombres
        private List<CiudadDestino> _destinos;
        private List<ProductoSeleccionado> _productos;

        public frmCotizacionDinamica(List<CiudadOrigen> origenes, List<CiudadDestino> destinos, List<ProductoSeleccionado> productos)
        {
            InitializeComponent();
            _origenes = origenes;
            _destinos = destinos;
            _productos = productos;

            GenerarTablaCotizacion();
        }
        private void GenerarTablaCotizacion()
        {
            tlpCotizacion.Controls.Clear();
            tlpCotizacion.ColumnStyles.Clear();
            tlpCotizacion.RowStyles.Clear();

            int totalColumnas = 1 + _productos.Count + 1; // +1 columna extra en blanco
            tlpCotizacion.ColumnCount = totalColumnas;
            tlpCotizacion.RowCount = 2;

            // Alturas: 30% encabezado, 70% detalle
            tlpCotizacion.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            tlpCotizacion.RowStyles.Add(new RowStyle(SizeType.Percent, 70));

            // Columna 0 fija para ciudades
            tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));

            // Columnas de productos (máximo 200px)
            foreach (var p in _productos)
                tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));

            // Columna extra en blanco → absorbe espacio sobrante
            tlpCotizacion.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Encabezado fila 0 → nombres de orígenes concatenados
            string origenesConcat = string.Join(", ", _origenes.Select(o => o.Nombre));
            tlpCotizacion.Controls.Add(new Label
            {
                Text = origenesConcat,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                AutoSize = false,
                MaximumSize = new Size(250, 0)
            }, 0, 0);

            for (int i = 0; i < _productos.Count; i++)
            {
                tlpCotizacion.Controls.Add(new Label
                {
                    Text = _productos[i].Nombre,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    AutoSize = false,
                    MaximumSize = new Size(200, 0) // wrap en 200px
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

            foreach (var destino in _destinos)
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

            tlpCotizacion.Controls.Add(panelDestinos, 0, 1);

            // Fila 1 → precios por producto
            for (int i = 0; i < _productos.Count; i++)
            {
                var nudPrecio = new NumericUpDown
                {
                    Anchor = AnchorStyles.None,
                    Size = new Size(120, 40),
                    DecimalPlaces = 2,
                    Maximum = 1000000,
                    Minimum = 0,
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Tag = new { ProductoID = _productos[i].ProductoID }
                };

                nudPrecio.Enter += (s, e) =>
                {
                    nudPrecio.ResetText();
                };

                tlpCotizacion.Controls.Add(nudPrecio, 1 + i, 1);
            }
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            bool hayInvalido = false;

            foreach (Control ctrl in tlpCotizacion.Controls)
            {
                if (ctrl is NumericUpDown nud)
                {
                    if (nud.Value <= 0) // 🔥 validación: no puede ser 0 ni vacío
                    {
                        hayInvalido = true;
                        break;
                    }
                }
            }

            if (hayInvalido)
            {
                MessageBox.Show("Debe ingresar un precio válido en todos los productos y que sea mayor a cero.","Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        public List<CotizacionRegistro> ObtenerCotizaciones()
        {
            var registros = new List<CotizacionRegistro>();

            foreach (Control ctrl in tlpCotizacion.Controls)
            {
                if (ctrl is NumericUpDown nud && nud.Tag != null)
                {
                    dynamic info = nud.Tag;
                    decimal precioIngresado = nud.Value;

                    foreach (var destino in _destinos)
                    {
                        foreach (var origen in _origenes)
                        {
                            registros.Add(new CotizacionRegistro
                            {
                                ProductoID = info.ProductoID,
                                CiudadOrigenID = origen.CiudadID,
                                CiudadDestinoID = destino.CiudadID,
                                PrecioNormal = precioIngresado
                            });
                        }
                    }
                }
            }

            return registros;
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
            public string CiudadOrigenID { get; set; }
            public string CiudadDestinoID { get; set; }
            public string ProductoID { get; set; }
            public decimal PrecioNormal { get; set; }
        }

        private void frmCotizacionDinamicaV2_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

