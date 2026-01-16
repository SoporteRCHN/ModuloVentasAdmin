using LogicaVentasAdmin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuloVentasAdmin.LoginMenu
{
    public partial class Login : Form
    {
        login entrada = new login();
        DataTable dtSeguimientoUsuario = new DataTable();
        clsLogica logica = new clsLogica();
        public Login()
        {
            InitializeComponent();
            InitializeTextBoxes();
            LoadRememberedCredentials();
        }
        private void InitializeTextBoxes()
        {
            // Set shadow text for username textbox
            txtUser.Text = "Ingrese su usuario";
            txtUser.ForeColor = System.Drawing.Color.Gray;

            // Set shadow text for password textbox
            txtContra.Text = "Ingrese su contraseña";
            txtContra.ForeColor = System.Drawing.Color.Gray;

            // Attach event handlers for textbox focus and blur events
            txtUser.GotFocus += TextBox_GotFocus;
            txtUser.LostFocus += TextBox_LostFocus;

            txtContra.GotFocus += TextBox_GotFocus;
            txtContra.LostFocus += TextBox_LostFocus;

            // Attach event handler for textbox Enter key press
            txtUser.KeyDown += TextBox_KeyDown;
            txtContra.KeyDown += TextBox_KeyDown;
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Validate the textbox when Enter key is pressed
                ValidateTextBox(sender as TextBox);
            }
        }
        private void ValidateTextBox(TextBox textBox)
        {
            if (textBox != null)
            {
                // Validate the textbox content
                if (textBox == txtUser)
                {
                    // Validate username
                    // Example validation:
                    if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == "Ingrese su usuario")
                    {
                        MessageBox.Show("Por favor ingrese un usuario valido");
                        return;
                    }
                }
                else if (textBox == txtContra)
                {
                    // Validate password
                    // Example validation:
                    if (string.IsNullOrWhiteSpace(textBox.Text) || textBox.Text == "Ingrese su contraseña")
                    {

                        MessageBox.Show("Por favor ingrese una contraseña valida");
                        return;
                    }
                }

                // Validate and process login as before
                if (RememberMeCheckbox.Checked)
                {
                    SaveRememberedCredentials();
                }
                else
                {
                    ClearRememberedCredentials();
                }
                // VALIDA EL USUARIO Y CONTRASEÑA INGRESADO
                if (validarUsuario(txtUser.Text, txtContra.Text))
                {
                    DynamicMain mainForm = new DynamicMain(txtUser.Text);
                    mainForm.Show();
                    this.Hide();
                }
            }
        }
        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Restore the shadow text if the textbox is empty when it loses focus
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox == txtUser)
                    {
                        textBox.Text = "Ingrese su usuario";
                    }
                    else if (textBox == txtContra)
                    {
                        textBox.Text = "Ingrese su contraseña";
                    }
                    textBox.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }
        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                // Clear the shadow text when the textbox is clicked
                if (textBox.Text == "Ingrese su usuario" || textBox.Text == "Ingrese su contraseña")
                {
                    textBox.Text = "";
                    textBox.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
        private void pbxMostrar_Click(object sender, EventArgs e)
        {
            if (txtContra.PasswordChar == '*')
            {
                // If PasswordChar is '*', set it to null to reveal the password
                txtContra.PasswordChar = '\0';
                pbxOcultar.Visible = true;
                pbxMostrar.Visible = false;

            }
            else
            {
                // If PasswordChar is not '*', set it back to '*' to hide the password
                txtContra.PasswordChar = '*';
                pbxOcultar.Visible = false;
                pbxMostrar.Visible = true;
            }
        }

        private void pbxOcultar_Click(object sender, EventArgs e)
        {
            if (txtContra.PasswordChar == '*')
            {
                // If PasswordChar is '*', set it to null to reveal the password
                txtContra.PasswordChar = '\0';
                pbxMostrar.Visible = true;
                pbxOcultar.Visible = false;

            }
            else
            {
                // If PasswordChar is not '*', set it back to '*' to hide the password
                txtContra.PasswordChar = '*';
                pbxMostrar.Visible = true;
                pbxOcultar.Visible = false;
            }
        }
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            // Validate and process login as before
            if (RememberMeCheckbox.Checked)
            {
                SaveRememberedCredentials();
            }
            else
            {
                ClearRememberedCredentials();
            }
            // VALIDA EL USUARIO Y CONTRASEÑA INGRESADO
            if (validarUsuario(txtUser.Text, txtContra.Text))
            {
                SeguimientoUsuario("INSERTAR", 39);

                DynamicMain mainForm = new DynamicMain(txtUser.Text);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                SeguimientoUsuario("INSERTAR", 40);
            }
        }
        private void SeguimientoUsuario(string _Operacion, int _AccionID)
        {

            SeguimientoUsuario sendSeguimiento = new SeguimientoUsuario
            {
                Operacion = _Operacion,
                Usuario = txtUser.Text,
                Modulo = Assembly.GetExecutingAssembly().GetName().Name,
                Formulario = this.Name,
                AccionID = _AccionID,
                UPosteo = txtUser.Text,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true

            };
            dtSeguimientoUsuario = logica.SP_SeguimientoUsuario(sendSeguimiento);
        }
        private bool validarUsuario(string usuario, string contrasena)
        {
            // LLAMA LA FUNCION Y DEVUELVE MENSAJE DE RESULTADO
            entrada.Validacion(txtUser.Text, txtContra.Text, typeof(Program).Namespace, out string mensajeResultadoDetalle);
            if (mensajeResultadoDetalle == "EXITO")
            {
                return true;
            }
            else
            {
                SeguimientoUsuario("INSERTAR", 40);
                MessageBox.Show(mensajeResultadoDetalle, "Validacion de usuario", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void SaveRememberedCredentials()
        {
            // Save remembered credentials
            Properties.Settings.Default.RememberUsername = txtUser.Text;
            Properties.Settings.Default.RememberPassword = txtContra.Text;
            Properties.Settings.Default.Save();
        }
        private void ClearRememberedCredentials()
        {
            // Clear remembered credentials
            Properties.Settings.Default.RememberUsername = string.Empty;
            Properties.Settings.Default.RememberPassword = string.Empty;
            Properties.Settings.Default.Save();
        }
        private void LoadRememberedCredentials()
        {
            // Load remembered credentials if available
            if (Properties.Settings.Default.RememberUsername != string.Empty)
            {
                txtUser.Text = Properties.Settings.Default.RememberUsername;
                txtContra.Text = Properties.Settings.Default.RememberPassword;
                RememberMeCheckbox.Checked = true; // Check the checkbox if credentials are remembered
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SeguimientoUsuario("INSERTAR", 41);
            Application.Exit();
        }

        private void pbxSalir_Click(object sender, EventArgs e)
        {
            SeguimientoUsuario("INSERTAR", 41);
            Application.Exit();
        }
    }
}
