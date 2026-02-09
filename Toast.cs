using System;
using System.Drawing;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public enum TipoAlerta
    {
        Info,
        Success,
        Warning,
        Error
    }

    public class Toast : Form
    {
        // 🔹 Configuración global de fade
        public static int FadeInterval { get; set; } = 30;   // milisegundos entre ticks
        public static double FadeStep { get; set; } = 0.2;   // incremento de opacidad

        private static int offsetY = 0;
        private Timer timerFadeIn;
        private Timer timerDuracion;
        private Label lblMensaje;
        private Panel panelIcono;
        private PictureBox icono;

        public Toast(string mensaje, TipoAlerta tipo, int duracionMs = 3000)
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Width = 300;
            Height = 60;
            TopMost = true;
            ShowInTaskbar = false;
            Opacity = 0;

            // Posición inferior derecha
            int x = Screen.PrimaryScreen.WorkingArea.Width - Width - 10;
            int y = Screen.PrimaryScreen.WorkingArea.Height - Height - 10 - offsetY;
            Location = new Point(x, y);
            offsetY += Height + 10;

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
                Location = new Point(14, 14),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = GetIcono(tipo)
            };
            panelIcono.Controls.Add(icono);

            // Mensaje
            lblMensaje = new Label
            {
                Text = mensaje,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = GetColorSuave(tipo),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 10, 0)
            };

            Controls.Add(lblMensaje);
            Controls.Add(panelIcono);

            // Fade in
            timerFadeIn = new Timer { Interval = FadeInterval };
            timerFadeIn.Tick += (s, e) =>
            {
                if (Opacity < 1)
                    Opacity += FadeStep;
                else
                    timerFadeIn.Stop();
            };

            // Duración
            timerDuracion = new Timer { Interval = duracionMs };
            timerDuracion.Tick += (s, e) =>
            {
                timerDuracion.Stop();
                FadeOut();
            };

            Load += (s, e) =>
            {
                timerFadeIn.Start();
                timerDuracion.Start();
            };
        }

        private void FadeOut()
        {
            Timer fade = new Timer { Interval = FadeInterval };
            fade.Tick += (s, e) =>
            {
                if (Opacity > 0)
                    Opacity -= FadeStep;
                else
                {
                    fade.Stop();
                    offsetY -= Height + 10;
                    Close();
                }
            };
            fade.Start();
        }

        private Color GetColorFuerte(TipoAlerta tipo)
        {
            Color color;
            switch (tipo)
            {
                case TipoAlerta.Info:
                    color = Color.FromArgb(0, 120, 215);
                    break;
                case TipoAlerta.Success:
                    color = Color.FromArgb(0, 153, 0);
                    break;
                case TipoAlerta.Warning:
                    color = Color.FromArgb(255, 140, 0);
                    break;
                case TipoAlerta.Error:
                    color = Color.FromArgb(192, 0, 0);
                    break;
                default:
                    color = Color.Gray;
                    break;
            }
            return color;
        }

        private Color GetColorSuave(TipoAlerta tipo)
        {
            Color color;
            switch (tipo)
            {
                case TipoAlerta.Info:
                    color = Color.FromArgb(204, 228, 247);
                    break;
                case TipoAlerta.Success:
                    color = Color.FromArgb(204, 255, 204);
                    break;
                case TipoAlerta.Warning:
                    color = Color.FromArgb(255, 229, 204);
                    break;
                case TipoAlerta.Error:
                    color = Color.FromArgb(255, 204, 204);
                    break;
                default:
                    color = Color.LightGray;
                    break;
            }
            return color;
        }

        private Image GetIcono(TipoAlerta tipo)
        {
            switch (tipo)
            {
                case TipoAlerta.Info:
                    return SystemIcons.Information.ToBitmap();
                case TipoAlerta.Success:
                    return SystemIcons.Shield.ToBitmap();
                case TipoAlerta.Warning:
                    return SystemIcons.Warning.ToBitmap();
                case TipoAlerta.Error:
                    return SystemIcons.Error.ToBitmap();
                default:
                    return SystemIcons.Application.ToBitmap();
            }
        }

        public static void Mostrar(string mensaje, TipoAlerta tipo, int duracionMs = 3000)
        {
            Toast toast = new Toast(mensaje, tipo, duracionMs);
            toast.Show();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Toast
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Toast";
            this.Load += new System.EventHandler(this.Toast_Load);
            this.ResumeLayout(false);

        }

        private void Toast_Load(object sender, EventArgs e)
        {

        }
    }

}
