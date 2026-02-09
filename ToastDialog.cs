using System;
using System.Drawing;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class ToastDialog : Form
    {
        private Label lblMensaje;
        private Panel panelIcono;
        private PictureBox icono;
        private Button btnAceptar;
        private Button btnCancelar;
        private Panel panelBotones;

        public ToastDialog(string mensaje, TipoAlerta tipo)
        {
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            Width = 420;
            Height = 200;
            TopMost = true;
            ShowInTaskbar = false;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.White;

            // Panel izquierdo para icono
            panelIcono = new Panel
            {
                Width = 60,
                Dock = DockStyle.Left,
                BackColor = GetColorFuerte(tipo)
            };

            icono = new PictureBox
            {
                Size = new Size(32, 32),
                Location = new Point(14, 40),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = GetIcono(tipo)
            };
            panelIcono.Controls.Add(icono);

            // Mensaje
            lblMensaje = new Label
            {
                Text = mensaje,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10)
            };

            // Panel inferior para botones
            panelBotones = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.White
            };

            btnAceptar = CrearBoton("Aceptar", DialogResult.OK, new Point(100, 15));
            btnCancelar = CrearBoton("Cancelar", DialogResult.Cancel, new Point(220, 15));

            panelBotones.Controls.Add(btnAceptar);
            panelBotones.Controls.Add(btnCancelar);

            Controls.Add(lblMensaje);
            Controls.Add(panelIcono);
            Controls.Add(panelBotones);

            AcceptButton = btnAceptar;
            CancelButton = btnCancelar;
        }

        private Button CrearBoton(string texto, DialogResult resultado, Point posicion)
        {
            Button boton = new Button
            {
                Text = texto,
                DialogResult = resultado,
                Width = 100,
                Height = 35,
                Location = posicion,
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Regular)
            };
            return boton;
        }

        private Color GetColorFuerte(TipoAlerta tipo)
        {
            switch (tipo)
            {
                case TipoAlerta.Info: return Color.FromArgb(0, 120, 215);
                case TipoAlerta.Success: return Color.FromArgb(0, 153, 0);
                case TipoAlerta.Warning: return Color.FromArgb(255, 140, 0);
                case TipoAlerta.Error: return Color.FromArgb(192, 0, 0);
                default: return Color.Gray;
            }
        }

        private Image GetIcono(TipoAlerta tipo)
        {
            switch (tipo)
            {
                case TipoAlerta.Info: return SystemIcons.Information.ToBitmap();
                case TipoAlerta.Success: return SystemIcons.Shield.ToBitmap();
                case TipoAlerta.Warning: return SystemIcons.Warning.ToBitmap();
                case TipoAlerta.Error: return SystemIcons.Error.ToBitmap();
                default: return SystemIcons.Application.ToBitmap();
            }
        }

        public static DialogResult Mostrar(string mensaje, TipoAlerta tipo)
        {
            using (ToastDialog dialog = new ToastDialog(mensaje, tipo))
            {
                return dialog.ShowDialog();
            }
        }
    }
}
