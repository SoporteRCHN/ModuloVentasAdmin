namespace ModuloVentasAdmin
{
    partial class frmCotizacionExistente
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rbdNombre = new System.Windows.Forms.RadioButton();
            this.rdbCodigo = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtClienteID = new System.Windows.Forms.TextBox();
            this.btnCliente = new System.Windows.Forms.Button();
            this.txtClienteNombre = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAtencion = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ModuloVentasAdmin.Properties.Resources.blue_Checkmark_22px;
            this.pictureBox1.Location = new System.Drawing.Point(20, 21);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 22);
            this.pictureBox1.TabIndex = 133;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(45, 22);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 18);
            this.label1.TabIndex = 132;
            this.label1.Text = "Cotizaciones Generadas";
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.rbdNombre);
            this.panel1.Controls.Add(this.rdbCodigo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(85, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 33);
            this.panel1.TabIndex = 134;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 135;
            this.label2.Text = "FILTRO POR:";
            // 
            // rbdNombre
            // 
            this.rbdNombre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rbdNombre.AutoSize = true;
            this.rbdNombre.Location = new System.Drawing.Point(146, 9);
            this.rbdNombre.Name = "rbdNombre";
            this.rbdNombre.Size = new System.Drawing.Size(138, 17);
            this.rbdNombre.TabIndex = 136;
            this.rbdNombre.Text = "NOMBRE DE CLIENTE";
            this.rbdNombre.UseVisualStyleBackColor = true;
            // 
            // rdbCodigo
            // 
            this.rdbCodigo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbCodigo.AutoSize = true;
            this.rdbCodigo.Checked = true;
            this.rdbCodigo.Location = new System.Drawing.Point(7, 9);
            this.rdbCodigo.Name = "rdbCodigo";
            this.rdbCodigo.Size = new System.Drawing.Size(133, 17);
            this.rdbCodigo.TabIndex = 135;
            this.rdbCodigo.TabStop = true;
            this.rdbCodigo.Text = "CODIGO DE CLIENTE";
            this.rdbCodigo.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 135;
            this.label3.Text = "CODIGO:";
            // 
            // txtClienteID
            // 
            this.txtClienteID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClienteID.BackColor = System.Drawing.Color.White;
            this.txtClienteID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtClienteID.ForeColor = System.Drawing.Color.Black;
            this.txtClienteID.Location = new System.Drawing.Point(85, 46);
            this.txtClienteID.Name = "txtClienteID";
            this.txtClienteID.Size = new System.Drawing.Size(350, 20);
            this.txtClienteID.TabIndex = 136;
            this.txtClienteID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtClienteID_KeyDown);
            this.txtClienteID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClienteID_KeyPress);
            // 
            // btnCliente
            // 
            this.btnCliente.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(102)))), ((int)(((byte)(118)))));
            this.btnCliente.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCliente.ForeColor = System.Drawing.Color.White;
            this.btnCliente.Image = global::ModuloVentasAdmin.Properties.Resources.search_16px;
            this.btnCliente.Location = new System.Drawing.Point(438, 39);
            this.btnCliente.Margin = new System.Windows.Forms.Padding(0);
            this.btnCliente.Name = "btnCliente";
            this.btnCliente.Size = new System.Drawing.Size(60, 29);
            this.btnCliente.TabIndex = 137;
            this.btnCliente.UseVisualStyleBackColor = false;
            this.btnCliente.Click += new System.EventHandler(this.btnCliente_Click);
            // 
            // txtClienteNombre
            // 
            this.txtClienteNombre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClienteNombre.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtClienteNombre.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtClienteNombre.BackColor = System.Drawing.Color.White;
            this.txtClienteNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtClienteNombre.ForeColor = System.Drawing.Color.Black;
            this.txtClienteNombre.Location = new System.Drawing.Point(576, 46);
            this.txtClienteNombre.Name = "txtClienteNombre";
            this.txtClienteNombre.ReadOnly = true;
            this.txtClienteNombre.Size = new System.Drawing.Size(352, 20);
            this.txtClienteNombre.TabIndex = 139;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(515, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 138;
            this.label7.Text = "CLIENTE:";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 83);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 141;
            this.label9.Text = "ATENCIÓN:";
            // 
            // txtAtencion
            // 
            this.txtAtencion.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtAtencion.Location = new System.Drawing.Point(85, 76);
            this.txtAtencion.Name = "txtAtencion";
            this.txtAtencion.Size = new System.Drawing.Size(350, 20);
            this.txtAtencion.TabIndex = 140;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 5);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 110);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dataGridView1.Size = new System.Drawing.Size(925, 269);
            this.dataGridView1.TabIndex = 142;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.877006F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.28877F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 6.524065F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.128343F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.18182F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtAtencion, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtClienteID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtClienteNombre, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCliente, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 3, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(20, 50);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.23102F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.910892F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.910892F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 71.9472F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(931, 426);
            this.tableLayoutPanel1.TabIndex = 143;
            // 
            // frmCotizacionExistente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1003, 511);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Name = "frmCotizacionExistente";
            this.Text = "COTIZACION EXISTENTE";
            this.Load += new System.EventHandler(this.frmCotizacionExistente_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbdNombre;
        private System.Windows.Forms.RadioButton rdbCodigo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtClienteID;
        private System.Windows.Forms.Button btnCliente;
        private System.Windows.Forms.TextBox txtClienteNombre;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAtencion;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}