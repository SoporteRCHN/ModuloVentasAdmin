using DatosVentasAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicaVentasAdmin
{
    public partial class frmBuscarClientes : Form
    {
        DataTable dtClientes = new DataTable();
        clsLogica logica = new clsLogica();

        public int ClienteId { get; private set; } = 0;
        public string ClienteNombre { get; private set; } = "";


        public frmBuscarClientes()
        {
            InitializeComponent();
            BuscarClientes();
            DgvClientes.CurrentCellDirtyStateChanged += DgvClientes_CurrentCellDirtyStateChanged;
            DgvClientes.CellValueChanged += DgvClientes_CellValueChanged;

        }

        private void frmBuscarClientes_Load(object sender, EventArgs e)
        {
            SetGradientBackground(panel5, Color.FromArgb(0, 91, 166), Color.FromArgb(226, 236, 245), LinearGradientMode.Horizontal);
        }
        private void SetGradientBackground(Panel panel, Color color1, Color color2, LinearGradientMode mode)
        {
            panel.Paint += (sender, e) =>
            {
                Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);
                LinearGradientBrush brush = new LinearGradientBrush(rect, color1, color2, mode);
                e.Graphics.FillRectangle(brush, rect);
            };
        }

        private void BuscarClientes()
        {
            ClienteENAC getClientes = new ClienteENAC
            {
                Opcion = "BUSCAR"
            };
            dtClientes = logica.SP_ClientesENAC(getClientes);

            if (dtClientes.Rows.Count > 0)
            {
                if (!dtClientes.Columns.Contains("Seleccionar"))
                {
                    dtClientes.Columns.Add("Seleccionar", typeof(bool));

                    // Inicializamos todas las filas en false
                    foreach (DataRow row in dtClientes.Rows)
                    {
                        row["Seleccionar"] = false;
                    }
                }

                // Evitamos que el DataGridView genere columnas automáticas
                DgvClientes.AutoGenerateColumns = false;

                // Limpiamos columnas previas para evitar duplicados
                DgvClientes.Columns.Clear();

                // Columna CheckBox personalizada
                DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
                chkCol.Name = "Seleccionar";
                chkCol.HeaderText = "✔";   // Tu header minimalista
                chkCol.Width = 50;
                chkCol.DataPropertyName = "Seleccionar"; // Vinculada al DataTable
                chkCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                chkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                DgvClientes.Columns.Add(chkCol);
               
                DataGridViewTextBoxColumn colClienteID = new DataGridViewTextBoxColumn();
                colClienteID.DataPropertyName = "ClienteID";
                colClienteID.HeaderText = "ClienteID";
                colClienteID.Width = 100;
                colClienteID.Name = "ClienteID";
                DgvClientes.Columns.Add(colClienteID);

                DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn();
                colNombre.DataPropertyName = "NombreCompleto";
                colNombre.HeaderText = "NombreCompleto";
                colNombre.Name = "NombreCompleto";
                colNombre.Width = 300;
                colNombre.ReadOnly = true;
                DgvClientes.Columns.Add(colNombre);

                // Asignamos el DataSource
                DgvClientes.DataSource = dtClientes.DefaultView;
            }
        }



        private void DgvClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DgvClientes.Columns["Seleccionar"].Index && e.RowIndex >= 0)
            {
                DgvClientes.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DgvClientes_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (DgvClientes.IsCurrentCellDirty)
            {
                DgvClientes.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void DgvClientes_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == DgvClientes.Columns["Seleccionar"].Index && e.RowIndex >= 0)
            {
                bool marcado = Convert.ToBoolean(DgvClientes.Rows[e.RowIndex].Cells["Seleccionar"].Value);

                if (marcado)
                {
                    foreach (DataGridViewRow row in DgvClientes.Rows)
                    {
                        if (row.Index != e.RowIndex)
                        {
                            row.Cells["Seleccionar"].Value = false;
                            dtClientes.Rows[row.Index]["Seleccionar"] = false;
                        }
                    }
                }

                dtClientes.Rows[e.RowIndex]["Seleccionar"] = marcado;
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var filaSeleccionada = DgvClientes.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(r => Convert.ToBoolean(r.Cells["Seleccionar"].Value) == true);

            if (filaSeleccionada != null)
            {
                ClienteId = Convert.ToInt32(filaSeleccionada.Cells["ClienteID"].Value);
                ClienteNombre = filaSeleccionada.Cells["NombreCompleto"].Value.ToString();

                this.DialogResult = DialogResult.OK; 
                this.Close();
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente antes de aceptar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void TxtCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo ejecutamos si presiona Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true; // evita que se agregue un salto de línea en el TextBox

                string filtro = TxtCliente.Text.Trim();

                if (string.IsNullOrEmpty(filtro))
                {
                    // 🔹 Si está vacío → traer todos los clientes
                    ClienteENAC getClientes = new ClienteENAC
                    {
                        Opcion = "BUSCAR"
                    };
                    dtClientes = logica.SP_ClientesENAC(getClientes);
                }
                else if (int.TryParse(filtro, out int codigo))
                {
                    // 🔹 Si es numérico → buscar por código
                    ClienteENAC getClientes = new ClienteENAC
                    {
                        Opcion = "BuscarPorCodigo",
                        Cliente = filtro
                    };
                    dtClientes = logica.SP_ClientesENAC(getClientes);
                }
                else
                {
                    // 🔹 Si es texto → buscar por nombre
                    ClienteENAC getClientes = new ClienteENAC
                    {
                        Opcion = "BuscarPorNombre",
                        Nombre = filtro
                    };
                    dtClientes = logica.SP_ClientesENAC(getClientes);
                }

                // 🔹 Configuramos el DataGridView con los resultados
                if (dtClientes != null && dtClientes.Rows.Count > 0)
                {
                    if (!dtClientes.Columns.Contains("Seleccionar"))
                    {
                        dtClientes.Columns.Add("Seleccionar", typeof(bool));
                        foreach (DataRow row in dtClientes.Rows)
                            row["Seleccionar"] = false;
                    }

                    DgvClientes.AutoGenerateColumns = false;
                    DgvClientes.Columns.Clear();

                    // CheckBox
                    DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn
                    {
                        Name = "Seleccionar",
                        HeaderText = "✔",
                        Width = 50,
                        DataPropertyName = "Seleccionar"
                    };
                    chkCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    chkCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    DgvClientes.Columns.Add(chkCol);

                    // ClienteID
                    DataGridViewTextBoxColumn colClienteID = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ClienteID",
                        HeaderText = "ClienteID",
                        Width = 100,
                        Name = "ClienteID"
                    };
                    DgvClientes.Columns.Add(colClienteID);

                    // NombreCompleto
                    DataGridViewTextBoxColumn colNombre = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "NombreCompleto",
                        HeaderText = "NombreCompleto",
                        Name = "NombreCompleto",
                        Width = 300,
                        ReadOnly = true
                    };
                    DgvClientes.Columns.Add(colNombre);

                    // Asignamos el DataSource
                    DgvClientes.DataSource = dtClientes.DefaultView;
                }
                else
                {
                    // 🔹 Si no hay resultados, limpiamos el grid
                    DgvClientes.DataSource = null;
                }
            }
        }

    }
}
