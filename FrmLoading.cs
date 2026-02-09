using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class FrmLoading : Form
    {
        public FrmLoading()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None; // sin bordes
            this.StartPosition = FormStartPosition.CenterScreen; // centrado
            this.BackColor = Color.White;
            this.Width = 200;
            this.Height = 150;

            // PictureBox para el GIF
            PictureBox pic = new PictureBox();
            pic.Dock = DockStyle.Fill;
            pic.SizeMode = PictureBoxSizeMode.CenterImage;
            pic.Image = Properties.Resources.loading2; // tu GIF en Resources
            this.Controls.Add(pic);

            // Opcional: etiqueta de texto
            Label lbl = new Label();
            lbl.Text = "Procesando...";
            lbl.Dock = DockStyle.Bottom;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.Controls.Add(lbl);
        }

        private void FrmLoading_Load(object sender, EventArgs e)
        {

        }
    }

}
