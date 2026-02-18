namespace ModuloVentasAdmin
{
    partial class frmDescuentoPrecios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtValor = new System.Windows.Forms.TextBox();
            this.rdbNombre = new System.Windows.Forms.RadioButton();
            this.rdbCodigo = new System.Windows.Forms.RadioButton();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbOrigen = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dgvProducto = new System.Windows.Forms.DataGridView();
            this.txtProducto = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvTarifario = new System.Windows.Forms.DataGridView();
            this.chkISV = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvTarifarioDetalle = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvAledanosDetalle = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTipoCliente = new System.Windows.Forms.Label();
            this.txtNombreCliente = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.flpColores = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.txtNombreHistorico = new System.Windows.Forms.TextBox();
            this.btnGenerarPDF = new System.Windows.Forms.Button();
            this.dgvHistorico = new System.Windows.Forms.DataGridView();
            this.rdbCodigoH = new System.Windows.Forms.RadioButton();
            this.rdbNombreH = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpHasta = new System.Windows.Forms.DateTimePicker();
            this.dtpDesde = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCodigoCliente = new System.Windows.Forms.TextBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnLimpiarHistorico = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnCliente = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnVistaPrevia = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarifario)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarifarioDetalle)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAledanosDetalle)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(280, 198);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 15);
            this.label12.TabIndex = 107;
            this.label12.Text = "%";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label11.Location = new System.Drawing.Point(39, 200);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 16);
            this.label11.TabIndex = 106;
            this.label11.Text = "Desc.";
            // 
            // txtValor
            // 
            this.txtValor.BackColor = System.Drawing.Color.White;
            this.txtValor.ForeColor = System.Drawing.Color.Black;
            this.txtValor.Location = new System.Drawing.Point(104, 196);
            this.txtValor.Name = "txtValor";
            this.txtValor.Size = new System.Drawing.Size(171, 20);
            this.txtValor.TabIndex = 105;
            this.txtValor.Text = "0";
            this.txtValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtValor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtValor_KeyPress);
            // 
            // rdbNombre
            // 
            this.rdbNombre.AutoSize = true;
            this.rdbNombre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.rdbNombre.Location = new System.Drawing.Point(196, 65);
            this.rdbNombre.Name = "rdbNombre";
            this.rdbNombre.Size = new System.Drawing.Size(74, 20);
            this.rdbNombre.TabIndex = 1;
            this.rdbNombre.Text = "Nombre";
            this.rdbNombre.UseVisualStyleBackColor = true;
            this.rdbNombre.CheckedChanged += new System.EventHandler(this.rdbNombre_CheckedChanged);
            // 
            // rdbCodigo
            // 
            this.rdbCodigo.AutoSize = true;
            this.rdbCodigo.Checked = true;
            this.rdbCodigo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.rdbCodigo.Location = new System.Drawing.Point(121, 65);
            this.rdbCodigo.Name = "rdbCodigo";
            this.rdbCodigo.Size = new System.Drawing.Size(69, 20);
            this.rdbCodigo.TabIndex = 0;
            this.rdbCodigo.TabStop = true;
            this.rdbCodigo.Text = "Codigo";
            this.rdbCodigo.UseVisualStyleBackColor = true;
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(292, 64);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(94, 20);
            this.txtBuscar.TabIndex = 1;
            this.txtBuscar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBuscar_KeyDown);
            this.txtBuscar.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBuscar_KeyPress);
            this.txtBuscar.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtBuscar_PreviewKeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(39, 67);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 26;
            this.label5.Text = "Filtrar Por:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(226, 18);
            this.label1.TabIndex = 139;
            this.label1.Text = "Control Descuento de Precios";
            // 
            // cmbOrigen
            // 
            this.cmbOrigen.BackColor = System.Drawing.Color.White;
            this.cmbOrigen.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrigen.ForeColor = System.Drawing.Color.Black;
            this.cmbOrigen.FormattingEnabled = true;
            this.cmbOrigen.Location = new System.Drawing.Point(104, 163);
            this.cmbOrigen.Name = "cmbOrigen";
            this.cmbOrigen.Size = new System.Drawing.Size(332, 21);
            this.cmbOrigen.TabIndex = 0;
            this.cmbOrigen.SelectedIndexChanged += new System.EventHandler(this.cmbOrigen_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label8.Location = new System.Drawing.Point(39, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 16);
            this.label8.TabIndex = 143;
            this.label8.Text = "Origen:";
            // 
            // dgvProducto
            // 
            this.dgvProducto.AllowUserToAddRows = false;
            this.dgvProducto.AllowUserToDeleteRows = false;
            this.dgvProducto.AllowUserToResizeRows = false;
            this.dgvProducto.BackgroundColor = System.Drawing.Color.White;
            this.dgvProducto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducto.GridColor = System.Drawing.Color.LightGray;
            this.dgvProducto.Location = new System.Drawing.Point(33, 271);
            this.dgvProducto.Name = "dgvProducto";
            this.dgvProducto.RowHeadersVisible = false;
            this.dgvProducto.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvProducto.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dgvProducto.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvProducto.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducto.Size = new System.Drawing.Size(404, 185);
            this.dgvProducto.TabIndex = 144;
            this.dgvProducto.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducto_CellValueChanged);
            // 
            // txtProducto
            // 
            this.txtProducto.Location = new System.Drawing.Point(151, 245);
            this.txtProducto.Name = "txtProducto";
            this.txtProducto.Size = new System.Drawing.Size(285, 20);
            this.txtProducto.TabIndex = 145;
            this.txtProducto.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtProducto_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label2.Location = new System.Drawing.Point(39, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 16);
            this.label2.TabIndex = 146;
            this.label2.Text = "Buscar Producto";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label3.Location = new System.Drawing.Point(39, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 149;
            this.label3.Text = "Cliente:";
            // 
            // dgvTarifario
            // 
            this.dgvTarifario.BackgroundColor = System.Drawing.Color.White;
            this.dgvTarifario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTarifario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTarifario.Location = new System.Drawing.Point(3, 3);
            this.dgvTarifario.Name = "dgvTarifario";
            this.dgvTarifario.RowHeadersVisible = false;
            this.dgvTarifario.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTarifario.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dgvTarifario.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTarifario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTarifario.Size = new System.Drawing.Size(825, 398);
            this.dgvTarifario.TabIndex = 152;
            // 
            // chkISV
            // 
            this.chkISV.AutoSize = true;
            this.chkISV.Location = new System.Drawing.Point(328, 200);
            this.chkISV.Name = "chkISV";
            this.chkISV.Size = new System.Drawing.Size(105, 17);
            this.chkISV.TabIndex = 153;
            this.chkISV.Text = "Tiene Impuesto?";
            this.chkISV.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(456, 26);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(839, 430);
            this.tabControl1.TabIndex = 154;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgvTarifario);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(831, 404);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tarifario";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvTarifarioDetalle);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(831, 404);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Principales Detalle";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvTarifarioDetalle
            // 
            this.dgvTarifarioDetalle.BackgroundColor = System.Drawing.Color.White;
            this.dgvTarifarioDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTarifarioDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTarifarioDetalle.Location = new System.Drawing.Point(3, 3);
            this.dgvTarifarioDetalle.Name = "dgvTarifarioDetalle";
            this.dgvTarifarioDetalle.RowHeadersVisible = false;
            this.dgvTarifarioDetalle.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTarifarioDetalle.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dgvTarifarioDetalle.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvTarifarioDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTarifarioDetalle.Size = new System.Drawing.Size(825, 398);
            this.dgvTarifarioDetalle.TabIndex = 153;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dgvAledanosDetalle);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(831, 404);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Aledaños Detalle";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dgvAledanosDetalle
            // 
            this.dgvAledanosDetalle.BackgroundColor = System.Drawing.Color.White;
            this.dgvAledanosDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAledanosDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAledanosDetalle.Location = new System.Drawing.Point(3, 3);
            this.dgvAledanosDetalle.Name = "dgvAledanosDetalle";
            this.dgvAledanosDetalle.RowHeadersVisible = false;
            this.dgvAledanosDetalle.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvAledanosDetalle.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dgvAledanosDetalle.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvAledanosDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAledanosDetalle.Size = new System.Drawing.Size(825, 398);
            this.dgvAledanosDetalle.TabIndex = 154;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(837, 462);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 15);
            this.label4.TabIndex = 155;
            this.label4.Text = "-";
            this.label4.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label6.Location = new System.Drawing.Point(39, 133);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 16);
            this.label6.TabIndex = 156;
            this.label6.Text = "Tipo De Cliente: ";
            // 
            // lblTipoCliente
            // 
            this.lblTipoCliente.AutoSize = true;
            this.lblTipoCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipoCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(115)))));
            this.lblTipoCliente.Location = new System.Drawing.Point(159, 133);
            this.lblTipoCliente.Name = "lblTipoCliente";
            this.lblTipoCliente.Size = new System.Drawing.Size(12, 15);
            this.lblTipoCliente.TabIndex = 157;
            this.lblTipoCliente.Text = "-";
            // 
            // txtNombreCliente
            // 
            this.txtNombreCliente.BackColor = System.Drawing.Color.White;
            this.txtNombreCliente.Enabled = false;
            this.txtNombreCliente.ForeColor = System.Drawing.Color.Black;
            this.txtNombreCliente.Location = new System.Drawing.Point(96, 100);
            this.txtNombreCliente.Name = "txtNombreCliente";
            this.txtNombreCliente.Size = new System.Drawing.Size(341, 20);
            this.txtNombreCliente.TabIndex = 158;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Tomato;
            this.panel1.Location = new System.Drawing.Point(474, 459);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(28, 20);
            this.panel1.TabIndex = 160;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label7.Location = new System.Drawing.Point(506, 461);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(230, 16);
            this.label7.TabIndex = 161;
            this.label7.Text = "Inconsistencia con precio de Tarifario";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label9.Location = new System.Drawing.Point(506, 486);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(313, 16);
            this.label9.TabIndex = 163;
            this.label9.Text = "Ciudades que aun no tienen referencia en el cliente.";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gold;
            this.panel2.Location = new System.Drawing.Point(474, 484);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(28, 20);
            this.panel2.TabIndex = 162;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(837, 487);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(12, 15);
            this.label10.TabIndex = 164;
            this.label10.Text = "-";
            this.label10.Visible = false;
            // 
            // flpColores
            // 
            this.flpColores.Location = new System.Drawing.Point(468, 508);
            this.flpColores.Name = "flpColores";
            this.flpColores.Size = new System.Drawing.Size(814, 27);
            this.flpColores.TabIndex = 165;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(12, 11);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1311, 569);
            this.tabControl2.TabIndex = 166;
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.White;
            this.tabPage4.Controls.Add(this.pictureBox2);
            this.tabPage4.Controls.Add(this.label16);
            this.tabPage4.Controls.Add(this.btnLimpiarHistorico);
            this.tabPage4.Controls.Add(this.txtNombreHistorico);
            this.tabPage4.Controls.Add(this.button1);
            this.tabPage4.Controls.Add(this.btnGenerarPDF);
            this.tabPage4.Controls.Add(this.dgvHistorico);
            this.tabPage4.Controls.Add(this.rdbCodigoH);
            this.tabPage4.Controls.Add(this.rdbNombreH);
            this.tabPage4.Controls.Add(this.label15);
            this.tabPage4.Controls.Add(this.dtpHasta);
            this.tabPage4.Controls.Add(this.dtpDesde);
            this.tabPage4.Controls.Add(this.label14);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Controls.Add(this.txtCodigoCliente);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1303, 543);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Historico";
            // 
            // txtNombreHistorico
            // 
            this.txtNombreHistorico.Enabled = false;
            this.txtNombreHistorico.Location = new System.Drawing.Point(65, 86);
            this.txtNombreHistorico.Name = "txtNombreHistorico";
            this.txtNombreHistorico.Size = new System.Drawing.Size(428, 20);
            this.txtNombreHistorico.TabIndex = 14;
            // 
            // btnGenerarPDF
            // 
            this.btnGenerarPDF.Location = new System.Drawing.Point(441, 489);
            this.btnGenerarPDF.Name = "btnGenerarPDF";
            this.btnGenerarPDF.Size = new System.Drawing.Size(132, 33);
            this.btnGenerarPDF.TabIndex = 11;
            this.btnGenerarPDF.Text = "Generar PDF";
            this.btnGenerarPDF.UseVisualStyleBackColor = true;
            this.btnGenerarPDF.Click += new System.EventHandler(this.btnGenerarPDF_Click);
            // 
            // dgvHistorico
            // 
            this.dgvHistorico.AllowUserToAddRows = false;
            this.dgvHistorico.AllowUserToDeleteRows = false;
            this.dgvHistorico.AllowUserToResizeRows = false;
            this.dgvHistorico.BackgroundColor = System.Drawing.Color.White;
            this.dgvHistorico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorico.Location = new System.Drawing.Point(34, 159);
            this.dgvHistorico.Name = "dgvHistorico";
            this.dgvHistorico.ReadOnly = true;
            this.dgvHistorico.RowHeadersVisible = false;
            this.dgvHistorico.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvHistorico.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dgvHistorico.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvHistorico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorico.Size = new System.Drawing.Size(539, 324);
            this.dgvHistorico.TabIndex = 10;
            this.dgvHistorico.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHistorico_CellDoubleClick);
            // 
            // rdbCodigoH
            // 
            this.rdbCodigoH.AutoSize = true;
            this.rdbCodigoH.Checked = true;
            this.rdbCodigoH.Location = new System.Drawing.Point(266, 56);
            this.rdbCodigoH.Name = "rdbCodigoH";
            this.rdbCodigoH.Size = new System.Drawing.Size(93, 17);
            this.rdbCodigoH.TabIndex = 9;
            this.rdbCodigoH.TabStop = true;
            this.rdbCodigoH.Text = "Codigo Cliente";
            this.rdbCodigoH.UseVisualStyleBackColor = true;
            // 
            // rdbNombreH
            // 
            this.rdbNombreH.AutoSize = true;
            this.rdbNombreH.Location = new System.Drawing.Point(143, 56);
            this.rdbNombreH.Name = "rdbNombreH";
            this.rdbNombreH.Size = new System.Drawing.Size(97, 17);
            this.rdbNombreH.TabIndex = 8;
            this.rdbNombreH.Text = "Nombre Cliente";
            this.rdbNombreH.UseVisualStyleBackColor = true;
            this.rdbNombreH.CheckedChanged += new System.EventHandler(this.rdbNombreH_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(308, 123);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = " Hasta";
            // 
            // dtpHasta
            // 
            this.dtpHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpHasta.Location = new System.Drawing.Point(352, 119);
            this.dtpHasta.Name = "dtpHasta";
            this.dtpHasta.Size = new System.Drawing.Size(141, 20);
            this.dtpHasta.TabIndex = 4;
            this.dtpHasta.ValueChanged += new System.EventHandler(this.dtpHasta_ValueChanged);
            // 
            // dtpDesde
            // 
            this.dtpDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDesde.Location = new System.Drawing.Point(155, 120);
            this.dtpDesde.Name = "dtpDesde";
            this.dtpDesde.Size = new System.Drawing.Size(141, 20);
            this.dtpDesde.TabIndex = 3;
            this.dtpDesde.ValueChanged += new System.EventHandler(this.dtpDesde_ValueChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(62, 123);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 15);
            this.label14.TabIndex = 2;
            this.label14.Text = "Filtrar Desde";
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(62, 57);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 15);
            this.label13.TabIndex = 1;
            this.label13.Text = "Buscar Por";
            // 
            // txtCodigoCliente
            // 
            this.txtCodigoCliente.Location = new System.Drawing.Point(365, 54);
            this.txtCodigoCliente.Name = "txtCodigoCliente";
            this.txtCodigoCliente.Size = new System.Drawing.Size(128, 20);
            this.txtCodigoCliente.TabIndex = 0;
            this.txtCodigoCliente.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCodigoCliente_KeyDown);
            this.txtCodigoCliente.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCodigoCliente_KeyPress);
            this.txtCodigoCliente.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.txtCodigoCliente_PreviewKeyDown);
            // 
            // tabPage5
            // 
            this.tabPage5.BackColor = System.Drawing.Color.White;
            this.tabPage5.Controls.Add(this.pictureBox1);
            this.tabPage5.Controls.Add(this.flpColores);
            this.tabPage5.Controls.Add(this.label1);
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Controls.Add(this.rdbCodigo);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.rdbNombre);
            this.tabPage5.Controls.Add(this.panel2);
            this.tabPage5.Controls.Add(this.txtBuscar);
            this.tabPage5.Controls.Add(this.label7);
            this.tabPage5.Controls.Add(this.label5);
            this.tabPage5.Controls.Add(this.panel1);
            this.tabPage5.Controls.Add(this.txtNombreCliente);
            this.tabPage5.Controls.Add(this.lblTipoCliente);
            this.tabPage5.Controls.Add(this.label11);
            this.tabPage5.Controls.Add(this.label6);
            this.tabPage5.Controls.Add(this.cmbOrigen);
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Controls.Add(this.txtValor);
            this.tabPage5.Controls.Add(this.tabControl1);
            this.tabPage5.Controls.Add(this.label8);
            this.tabPage5.Controls.Add(this.chkISV);
            this.tabPage5.Controls.Add(this.label12);
            this.tabPage5.Controls.Add(this.label3);
            this.tabPage5.Controls.Add(this.dgvProducto);
            this.tabPage5.Controls.Add(this.label2);
            this.tabPage5.Controls.Add(this.txtProducto);
            this.tabPage5.Controls.Add(this.btnCliente);
            this.tabPage5.Controls.Add(this.button3);
            this.tabPage5.Controls.Add(this.btnGuardar);
            this.tabPage5.Controls.Add(this.btnVistaPrevia);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1303, 543);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "Edicion";
            // 
            // btnLimpiarHistorico
            // 
            this.btnLimpiarHistorico.BackColor = System.Drawing.Color.Tomato;
            this.btnLimpiarHistorico.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiarHistorico.ForeColor = System.Drawing.Color.White;
            this.btnLimpiarHistorico.Image = global::ModuloVentasAdmin.Properties.Resources.white_trash_can_16px;
            this.btnLimpiarHistorico.Location = new System.Drawing.Point(507, 81);
            this.btnLimpiarHistorico.Margin = new System.Windows.Forms.Padding(0);
            this.btnLimpiarHistorico.Name = "btnLimpiarHistorico";
            this.btnLimpiarHistorico.Size = new System.Drawing.Size(49, 29);
            this.btnLimpiarHistorico.TabIndex = 15;
            this.btnLimpiarHistorico.UseVisualStyleBackColor = false;
            this.btnLimpiarHistorico.Click += new System.EventHandler(this.btnLimpiarHistorico_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(102)))), ((int)(((byte)(118)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Image = global::ModuloVentasAdmin.Properties.Resources.search_16px;
            this.button1.Location = new System.Drawing.Point(507, 49);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 29);
            this.button1.TabIndex = 13;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ModuloVentasAdmin.Properties.Resources.blue_Checkmark_22px;
            this.pictureBox1.Location = new System.Drawing.Point(32, 24);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 22);
            this.pictureBox1.TabIndex = 140;
            this.pictureBox1.TabStop = false;
            // 
            // btnCliente
            // 
            this.btnCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(102)))), ((int)(((byte)(118)))));
            this.btnCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCliente.ForeColor = System.Drawing.Color.White;
            this.btnCliente.Image = global::ModuloVentasAdmin.Properties.Resources.search_16px;
            this.btnCliente.Location = new System.Drawing.Point(389, 56);
            this.btnCliente.Margin = new System.Windows.Forms.Padding(0);
            this.btnCliente.Name = "btnCliente";
            this.btnCliente.Size = new System.Drawing.Size(48, 32);
            this.btnCliente.TabIndex = 4;
            this.btnCliente.UseVisualStyleBackColor = false;
            this.btnCliente.Click += new System.EventHandler(this.btnCliente_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(115)))));
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Image = global::ModuloVentasAdmin.Properties.Resources.TikTok_Verified_Account_28px;
            this.button3.Location = new System.Drawing.Point(169, 475);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 39);
            this.button3.TabIndex = 151;
            this.button3.Text = "VER PDF";
            this.button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(114)))), ((int)(((byte)(174)))));
            this.btnGuardar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Image = global::ModuloVentasAdmin.Properties.Resources.TikTok_Verified_Account_28px;
            this.btnGuardar.Location = new System.Drawing.Point(32, 475);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(0);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(130, 39);
            this.btnGuardar.TabIndex = 104;
            this.btnGuardar.Text = "GUARDAR";
            this.btnGuardar.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Visible = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // btnVistaPrevia
            // 
            this.btnVistaPrevia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(66)))), ((int)(((byte)(115)))));
            this.btnVistaPrevia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVistaPrevia.ForeColor = System.Drawing.Color.White;
            this.btnVistaPrevia.Image = global::ModuloVentasAdmin.Properties.Resources.TikTok_Verified_Account_28px;
            this.btnVistaPrevia.Location = new System.Drawing.Point(306, 475);
            this.btnVistaPrevia.Margin = new System.Windows.Forms.Padding(0);
            this.btnVistaPrevia.Name = "btnVistaPrevia";
            this.btnVistaPrevia.Size = new System.Drawing.Size(130, 39);
            this.btnVistaPrevia.TabIndex = 108;
            this.btnVistaPrevia.Text = "VER PREVIA";
            this.btnVistaPrevia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVistaPrevia.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnVistaPrevia.UseVisualStyleBackColor = false;
            this.btnVistaPrevia.Click += new System.EventHandler(this.btnVistaPrevia_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ModuloVentasAdmin.Properties.Resources.blue_Checkmark_22px;
            this.pictureBox2.Location = new System.Drawing.Point(24, 17);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(24, 22);
            this.pictureBox2.TabIndex = 133;
            this.pictureBox2.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(49, 18);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(267, 18);
            this.label16.TabIndex = 132;
            this.label16.Text = "Estado de solicitudes de descuento";
            // 
            // frmDescuentoPrecios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1341, 592);
            this.Controls.Add(this.tabControl2);
            this.Name = "frmDescuentoPrecios";
            this.Text = "frmDescuentoPrecios";
            this.Load += new System.EventHandler(this.frmDescuentoPrecios_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarifario)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTarifarioDetalle)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAledanosDetalle)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtValor;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.RadioButton rdbNombre;
        private System.Windows.Forms.RadioButton rdbCodigo;
        private System.Windows.Forms.Button btnCliente;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVistaPrevia;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbOrigen;
        private System.Windows.Forms.DataGridView dgvProducto;
        private System.Windows.Forms.TextBox txtProducto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dgvTarifario;
        private System.Windows.Forms.CheckBox chkISV;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvTarifarioDetalle;
        private System.Windows.Forms.DataGridView dgvAledanosDetalle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblTipoCliente;
        private System.Windows.Forms.TextBox txtNombreCliente;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.FlowLayoutPanel flpColores;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TextBox txtCodigoCliente;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtpHasta;
        private System.Windows.Forms.DateTimePicker dtpDesde;
        private System.Windows.Forms.RadioButton rdbCodigoH;
        private System.Windows.Forms.RadioButton rdbNombreH;
        private System.Windows.Forms.Button btnGenerarPDF;
        private System.Windows.Forms.DataGridView dgvHistorico;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLimpiarHistorico;
        private System.Windows.Forms.TextBox txtNombreHistorico;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label16;
    }
}