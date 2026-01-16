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

            int totalColumnas = 1 + _productos.Count;
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
            // Fila 1 → destinos con centrado horizontal, centrado vertical cuando hay pocos,
            // y scroll vertical cuando hay muchos
            var panelDestinos = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            // Contenedor interno (puede ser FlowLayoutPanel o TableLayoutPanel)
            // Aquí uso FlowLayoutPanel para apilar limpio
            var contentDestinos = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            // Agregar labels de ciudades
            foreach (var destino in _destinos)
            {
                var lbl = new Label
                {
                    Text = destino.Nombre,
                    TextAlign = ContentAlignment.MiddleCenter, // centrado horizontal
                    AutoSize = false,
                    Width = 250,   // igual al ancho de la columna
                    Height = 26,   // altura por fila
                    Margin = new Padding(0)
                };
                contentDestinos.Controls.Add(lbl);
            }

            // Agregar el contenedor al panel
            panelDestinos.Controls.Add(contentDestinos);

            // Handler para centrar vertical/horizontal cuando hay espacio
            panelDestinos.Resize += (s, e) =>
            {
                // Tamaño del contenido
                var contentSize = contentDestinos.PreferredSize;

                // Actualiza el mínimo para que el scroll funcione cuando exceda
                panelDestinos.AutoScrollMinSize = contentSize;

                // Área visible del panel (sin scrollbars)
                var client = panelDestinos.ClientSize;

                // Calcular posición centrada
                int x = Math.Max(0, (client.Width - contentSize.Width) / 2);
                int y = Math.Max(0, (client.Height - contentSize.Height) / 2);

                // Si el contenido es más grande que el panel, no centramos vertical para permitir scroll desde arriba
                if (contentSize.Height > client.Height)
                    y = 0;

                // Asignar ubicación
                contentDestinos.Location = new Point(x, y);
            };

            // Insertar en la celda del TLP
            tlpCotizacion.Controls.Add(panelDestinos, 0, 1);

            // Precios por producto
            for (int i = 0; i < _productos.Count; i++)
            {
                var nudPrecio = new NumericUpDown
                {
                    Anchor = AnchorStyles.None,          // 🔥 centra en la celda (horizontal y vertical)
                    Size = new Size(120, 40),            // 🔥 tamaño fijo
                    DecimalPlaces = 2,                   // 🔥 permite decimales
                    Maximum = 1000000,                   // 🔥 límite superior
                    Minimum = 0,                         // 🔥 límite inferior
                    Font = new Font("Segoe UI", 12, FontStyle.Regular),
                    Tag = new { ProductoID = _productos[i].ProductoID }
                };

                // Evento para limpiar al entrar
                nudPrecio.Enter += (s, e) =>
                {
                    nudPrecio.ResetText(); 
                };

                tlpCotizacion.Controls.Add(nudPrecio, 1 + i, 1);
            }

        }
        private void btnProcesar_Click(object sender, EventArgs e)
        {
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
                    registros.Add(new CotizacionRegistro
                    {
                        ProductoID = info.ProductoID,
                        PrecioNormal = nud.Value
                    });
                }
            }

            return registros;
        }


        private void frmCotizacionDinamicaV2_Load(object sender, EventArgs e)
        {

        }
      
    }
}

