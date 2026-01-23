using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class frmTablaDescuentos : Form
    {
        public Dictionary<string, decimal> PreciosSeleccionados { get; private set; }
        public string _vClienteID = String.Empty;
        clsLogica logica = new clsLogica();
        public frmTablaDescuentos(string _Cliente, string _ClienteID)
        {
            InitializeComponent();
            cargarDescuentos();
            _vClienteID = _ClienteID;
            lblCliente.Text = _Cliente.ToUpper();
            cargarHistorial(_ClienteID);
        }

        private void frmTablaDescuentos_Load(object sender, EventArgs e)
        {

        }
        private void cargarDescuentos()
        {
            ProductosPreciosENAC getDescuentos = new ProductosPreciosENAC
            {
                Opcion = "Listado"
            };
            DataTable dtDescuentos = logica.SP_ProductosPreciosENAC(getDescuentos);

            if (dtDescuentos.Rows.Count > 0)
            {
                GenerarTablaDescuentos(dtDescuentos);
            }
        }
        private void GenerarTablaDescuentos(DataTable dtDescuentos)
        {
            // Limpiar configuración previa
            dgvDescuentos.Columns.Clear();
            dgvDescuentos.Rows.Clear();
            dgvDescuentos.AutoGenerateColumns = false;
            dgvDescuentos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDescuentos.MultiSelect = false;
            dgvDescuentos.AllowUserToAddRows = false;
            dgvDescuentos.ReadOnly = true;
            dgvDescuentos.RowHeadersVisible = false;

            // 🔹 Obtener lista única de productos
            var productos = dtDescuentos.AsEnumerable()
                .Select(r => r.Field<string>("Nombre"))
                .Distinct()
                .ToList();

            var rangos = dtDescuentos.AsEnumerable()
                .Select(r => new
                {
                    Promedio = r.Field<string>("Promedio"),
                    PromedioDiario = r.Field<string>("PromedioDiario"),
                    Descripcion = r.Field<string>("Descripcion")
                })
                .Distinct()
                .ToList();

            dgvDescuentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PROMEDIO " + rangos.First().Descripcion.ToUpper(),
                Name = "colPromedio",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvDescuentos.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PROMEDIO DIARIO",
                Name = "colPromedioDiario",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            foreach (var p in productos)
            {
                dgvDescuentos.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = p,
                    Name = "col_" + p,
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });
            }

            foreach (var rango in rangos)
            {
                var fila = new List<string>
        {
            rango.Promedio,
            rango.PromedioDiario
        };

                foreach (var p in productos)
                {
                    var row = dtDescuentos.AsEnumerable()
                        .FirstOrDefault(x =>
                            x.Field<string>("Promedio") == rango.Promedio &&
                            x.Field<string>("PromedioDiario") == rango.PromedioDiario &&
                            x.Field<string>("Nombre") == p);

                    string precio = row != null ? row["Precio"].ToString() : "0";
                    fila.Add(precio);
                }

                dgvDescuentos.Rows.Add(fila.ToArray());
            }

            dgvDescuentos.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvDescuentos.DefaultCellStyle.BackColor = Color.White;
            dgvDescuentos.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
            dgvDescuentos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDescuentos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvDescuentos.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // 🔹 Seleccionar primera fila por defecto
            if (dgvDescuentos.Rows.Count > 0)
            {
                dgvDescuentos.Rows[0].Selected = true;
            }
        }
        private void cargarHistorial(string _ClienteID)
        {
            // Obtener historial desde la lógica
            FacturaDTO getHistorial = new FacturaDTO
            {
                Opcion = "HistorialEntreFechas",
                ClienteRemitente = _ClienteID,
                FechaInicio = dtpDesde.Value.Date,
                FechaFin = dtpHasta.Value.Date
            };
            DataTable dtHistorial = logica.SP_FacturasENAC(getHistorial);

            // Calcular valor total
            decimal _valor = 0;
            foreach (DataRow row in dtHistorial.Rows)
            {
                _valor += Convert.ToDecimal(row["MontoRecibido"]);
            }

            // Cantidad de facturas
            lblCantidad.Text = dtHistorial.Rows.Count.ToString();

            // Valor total
            lblValor.Text = "L." + _valor.ToString("N2");

            // Promedio por factura
            if (dtHistorial.Rows.Count > 0)
            {
                lblValorPromedio.Text = (_valor / Convert.ToDecimal(dtHistorial.Rows.Count)).ToString("N2");
            }
            else
            {
                lblValorPromedio.Text = "0";
            }

            // Promedio por día válido (sin domingos)
            int totalDias = DayDiff(dtpDesde.Value, dtpHasta.Value);
            int domingos = ContarDomingos(dtpDesde.Value, dtpHasta.Value);
            int diasValidos = totalDias - domingos;

            if (dtHistorial.Rows.Count > 0 && diasValidos > 0)
            {
                lblFacturaPromedio.Text = (_valor / dtHistorial.Rows.Count / diasValidos).ToString("N2");
            }
            else
            {
                lblFacturaPromedio.Text = "0";
            }

            lblCantidad.TextAlign = ContentAlignment.MiddleRight;
            lblValor.TextAlign = ContentAlignment.MiddleRight;
            lblValorPromedio.TextAlign = ContentAlignment.MiddleRight;
            lblFacturaPromedio.TextAlign = ContentAlignment.MiddleRight;
        }

        private int DayDiff(DateTime desde, DateTime hasta)
        {
            return (hasta.Date - desde.Date).Days + 1;
        }

        private int ContarDomingos(DateTime desde, DateTime hasta)
        {
            int count = 0;
            for (DateTime d = desde.Date; d <= hasta.Date; d = d.AddDays(1))
            {
                if (d.DayOfWeek == DayOfWeek.Sunday)
                    count++;
            }
            return count;
        }

        

        private void dtpHasta_ValueChanged(object sender, EventArgs e)
        {
            cargarHistorial(_vClienteID);
        }

        private void dtpDesde_ValueChanged(object sender, EventArgs e)
        {
            cargarHistorial(_vClienteID);
        }

   

        private void dgvDescuentos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvDescuentos.Rows[e.RowIndex];
                PreciosSeleccionados = new Dictionary<string, decimal>();

                for (int c = 2; c < dgvDescuentos.Columns.Count; c++)
                {
                    string productoNombre = dgvDescuentos.Columns[c].HeaderText;
                    string valorCelda = fila.Cells[c].Value?.ToString() ?? "0";

                    if (decimal.TryParse(valorCelda, out decimal precio))
                    {
                        PreciosSeleccionados[productoNombre] = precio;
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

    }
}
