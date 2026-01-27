using LogicaVentasAdmin;

using ModuloVentasAdmin.LoginMenu;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModuloVentasAdmin
{
    public partial class DynamicMain : Form
    {
        DataTable tablaEncabezado = new DataTable();
        DataTable dtMenuOpciones = new DataTable();
        DataTable dtSeguimientoUsuario = new DataTable();
        DataTable dtLlamadas = new DataTable();
        DataTable dtAperturaCaja = new DataTable();
        DataTable dtEstadoENAC = new DataTable();
        DataTable dtMenuOpcionesFinal = new DataTable();

        private static Form activeForm;
        public static RegistroAcciones registro = new RegistroAcciones(); //REGISTRO DE ACCIONES DE USUARIO
        login loginn = new login();
        

        private DateTime inicioLlamada;
        public static bool _EstadoEnac = false;
        public static string usuarionombre;
        public static string usuarioNombreCompleto;
        public static string usuarioSucursal;
        public static string usuarioPerfilID;
        public static string rutaEmitirEvento;
        public static string usuarionlogin;
        public static string usuarionEmpresaNombre;
        public static int usuarioIDNumber;
        public static int usuarioEmpleadoID;
        public static int ModuloID;
        public static int usuarioDepartamentoID;
        public static int usuarioSucursalID;
        DataTable dtTasaCambio = new DataTable();
        public static int usuarioCiudadID;
        public static int cajaActivaSucursal;
        public static int usuarioEmpresaID;
        public static int usuarioSucursalCaja;
        public static int usuarioAutorizaCierreCaja;
        public static int usuarioNivelAccesoSolicitud;
        public static int Confidencial;
        public static bool permisoEditar = false; // variable para poder editar registros / Guardar - Editar - Borrar
        public static bool existeAvisos = false; //Variable para controlar el mostrar o no los avisos.
        public static int cajaID; //Ayuda en el manejo del ControlID cuando se apertura caja
        public static decimal tasa;
        public static DynamicMain Instance { get; private set; }
        public int PlayLlamada = 0;
        public string llamadaID;
        bool panelLeftExpand = true;
        clsLogica logica = new clsLogica();

        public DynamicMain(string usuario)
        {
            InitializeComponent();
            Instance = this;
            EstadoENAC();
            CargarEncabezado(usuario);
            usuarionombre = usuario;
            usuarionlogin = usuario;
            ModuloID = 29;
           
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            string versionStr = $"{versionInfo.ProductMajorPart}.{versionInfo.ProductMinorPart}.{versionInfo.ProductBuildPart}.{versionInfo.ProductPrivatePart}";


            toolStripLabel2.Text = versionStr;
           
            rutaEmitirEvento = "https://app.rapidocargo.online:3000";
            toolStripLabel4.Text = DatosVentasAdmin.BD_Conexion.servidor.ToString();
            toolStripLabel6.Text = usuarionlogin;

            BuscarMenuFinal();
            //CargarMenuDinamico();

            tasa = ActualizarTasaCambio();
            lblTasa.Text = tasa.ToString();
            lblFechaActual.Text = DateTime.Today.ToString("dd/MM/yyyy");

            int ancho = Screen.PrimaryScreen.WorkingArea.Width;
            int alto = Screen.PrimaryScreen.WorkingArea.Height;

            this.MaximumSize = new System.Drawing.Size(ancho, alto);
            this.WindowState = FormWindowState.Maximized; // Iniciar maximizado

            flowLayoutPanelMenu.PerformLayout();
            flowLayoutPanelMenu.Refresh();
            flowLayoutPanelMenu.Invalidate();
        }
        private decimal ActualizarTasaCambio()
        {
            FactorDolar50DTO setTasa = new FactorDolar50DTO
            {
                Opcion = "Recuperar"
            };
            dtTasaCambio = logica.SP_FactorDolar50(setTasa);
            if (dtTasaCambio.Rows.Count > 0)
            {

                return Convert.ToDecimal(dtTasaCambio.Rows[0]["FactorDolar"].ToString());
            }
            else
            {
                MessageBox.Show("Ocurrio un problema al actualizar la tasa de cambio en la base de datos", "Aviso Urgente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }
        public void SeguimientoUsuario(string _Operacion, int _AccionID)
        {
            SeguimientoUsuario sendSeguimiento = new SeguimientoUsuario
            {
                Operacion = _Operacion,
                Usuario = usuarionlogin,
                Modulo = Assembly.GetExecutingAssembly().GetName().Name,
                Formulario = this.Name,
                AccionID = _AccionID,
                UPosteo = usuarionlogin,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true

            };
            dtSeguimientoUsuario = logica.SP_SeguimientoUsuario(sendSeguimiento);
        }
        private void CargarEncabezado(string usuario)
        {
            tablaEncabezado = loginn.DatosEncabezado(usuario, typeof(Program).Namespace);
            foreach (DataRow row in tablaEncabezado.Rows)
            {
                usuarioNombreCompleto = row["NombreCompleto"].ToString();
                lblUbicacion.Text = "HOME / BIENVENIDO: " + row["NombreCompleto"].ToString();
                //lblUsuarioPerfil.Text = row["Puesto"].ToString();
                //lblEmpresa.Text = row["Sucursal"].ToString();
                toolStripLabel7.Text = Environment.MachineName.ToString();
                usuarioIDNumber = Convert.ToInt32(row["UsuarioID"].ToString());
                permisoEditar = Convert.ToBoolean(row["PermiteEditar"]);
                Confidencial = Convert.ToInt32(row["Confidencial"]);
                usuarioEmpleadoID = Convert.ToInt32(row["EmpleadoID"]);
                usuarioDepartamentoID = Convert.ToInt32(row["DepartamentoID"]);
                usuarioSucursalID = Convert.ToInt32(row["SucursalID"]);
                usuarioCiudadID = Convert.ToInt32(row["CiudadID"]);
                usuarioSucursal = row["Sucursal"].ToString();
                usuarioPerfilID = row["PerfilID"].ToString();
                usuarioEmpresaID = Convert.ToInt32(row["EmpresaID"]);
                usuarionEmpresaNombre = row["Empresa"].ToString();
                usuarioSucursalCaja = Convert.ToInt32(row["CajaActiva"]);
                usuarioAutorizaCierreCaja = Convert.ToInt32(row["AutorizaCierreCaja"]);
                toolStripLabel12.Text = row["Sucursal"].ToString();
            }
        }
        private void BuscarMenuFinal()
        {
            // 1. Perfil
            PerfilPermisoDTO perfil = new PerfilPermisoDTO
            {
                Opcion = "ListadoPorPerfil",
                PerfilID = Convert.ToInt32(usuarioPerfilID),
                PerfilPermisoID = ModuloID
            };
            DataTable dtPerfil = logica.SP_PerfilPermisos(perfil);

            // 2. Extras
            PerfilPermisosExtraDTO extra = new PerfilPermisosExtraDTO
            {
                Opcion = "ListadoPorUsuario",
                UsuarioID = usuarioIDNumber,
                PermisoExtraID = ModuloID
            };
            DataTable dtExtra = logica.SP_PerfilPermisosExtra(extra);

            // 3. Merge
            dtMenuOpcionesFinal = CombinarMenus(dtPerfil, dtExtra);

            if (dtMenuOpcionesFinal.Rows.Count > 0)
            {
                CargarMenuDinamico(dtMenuOpcionesFinal);
            }
        }
        private DataTable CombinarMenus(DataTable dtPerfil, DataTable dtExtra)
        {
            // Clonar estructura del perfil
            DataTable dtFinal = dtPerfil.Clone();

            // Copiar todos los registros del perfil
            foreach (DataRow row in dtPerfil.Rows)
            {
                dtFinal.ImportRow(row);
            }

            // Aplicar overlay de extras
            foreach (DataRow extra in dtExtra.Rows)
            {
                int menuID = Convert.ToInt32(extra["MenuID"]);
                bool estadoExtra = Convert.ToBoolean(extra["Estado"]);

                // Buscar si ya existe en el perfil
                DataRow[] existentes = dtFinal.Select("MenuID = " + menuID);

                if (existentes.Length > 0)
                {
                    if (!estadoExtra)
                    {
                        // Quitar del menú final si el extra lo desactiva
                        foreach (DataRow r in existentes)
                            dtFinal.Rows.Remove(r);
                    }
                    else
                    {
                        // Sobreescribir propiedades si está activo
                        DataRow r = existentes[0];
                        r["Estado"] = extra["Estado"];
                        r["Tag"] = extra["Tag"];
                    }
                }
                else
                {
                    if (estadoExtra)
                    {
                        // Crear un nuevo registro en dtFinal con los valores de extra
                        DataRow nuevo = dtFinal.NewRow();
                        foreach (DataColumn col in dtFinal.Columns)
                        {
                            if (dtExtra.Columns.Contains(col.ColumnName))
                                nuevo[col.ColumnName] = extra[col.ColumnName];
                        }
                        dtFinal.Rows.Add(nuevo);
                    }
                    // Si no existe y está desactivado (Estado = 0), simplemente no se agrega
                }
            }

            // Ordenar por PadreID y luego MenuID
            DataView dv = dtFinal.DefaultView;
            dv.Sort = "PadreID ASC, MenuID ASC";
            return dv.ToTable();
        }
        private Dictionary<string, Image> iconDictionary = new Dictionary<string, Image>
        {
             { "frmFacturasGeneral", global::ModuloVentasAdmin.Properties.Resources.bill_26px },
        };

        private void CargarMenuDinamico(DataTable dtMenuOpciones)
        {
            ToolTip toolTip1 = new ToolTip();
            flowLayoutPanelMenu.Controls.Clear();

            // Buscar nodos raíz (PadreID = 0)
            var padres = dtMenuOpciones.AsEnumerable()
                .Where(r => Convert.ToInt32(r["PadreID"]) == 0)
                .OrderBy(r => Convert.ToInt32(r["MenuID"]))
                .ToList();

            foreach (var padre in padres)
            {
                FlowLayoutPanel panelPadre = CrearNodoRecursivo(padre, dtMenuOpciones, toolTip1, 0);
                flowLayoutPanelMenu.Controls.Add(panelPadre);
            }
        }
        private FlowLayoutPanel CrearNodoRecursivo(DataRow row, DataTable dtMenuOpciones, ToolTip toolTip1, int nivel)
        {
            int menuID = Convert.ToInt32(row["MenuID"]);
            int padreID = Convert.ToInt32(row["PadreID"]);

            Button btnMenu = new Button
            {
                Name = "btn" + menuID,
                Text = row["Tag"].ToString(),
                Width = 230,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                ForeColor = Color.White,
                Font = new Font("Century Gothic", 9, FontStyle.Bold),
                Padding = new Padding(10, 0, 0, 0),
                Margin = nivel == 0 ? new Padding(0) : new Padding(15, 0, 0, 0) // indentación por nivel
            };

            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseDownBackColor = Color.FromArgb(47, 51, 54);
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(141, 153, 163);
            btnMenu.MouseClick += new MouseEventHandler(BotonMenu_MouseClick);

            // Iconos según nivel y si tiene hijos
            string elemento = row["Descripcion"].ToString();
            if (!string.IsNullOrEmpty(elemento))
            {
                if (nivel == 0)
                {
                    btnMenu.Image = global::ModuloVentasAdmin.Properties.Resources.white_sort_right_16px;
                    btnMenu.Tag = "collapsed";
                }
                else
                {
                    var hijos = dtMenuOpciones.AsEnumerable().Where(r => Convert.ToInt32(r["PadreID"]) == menuID).ToList();
                    btnMenu.Image = hijos.Count > 0
                        ? global::ModuloVentasAdmin.Properties.Resources.white_sort_down_16px
                        : global::ModuloVentasAdmin.Properties.Resources.white_sort_right_16px;
                }

                btnMenu.ImageAlign = ContentAlignment.MiddleLeft;
                btnMenu.TextAlign = ContentAlignment.MiddleRight;
                btnMenu.TextImageRelation = TextImageRelation.ImageBeforeText;
            }

            toolTip1.SetToolTip(btnMenu, row["Tag"].ToString());

            // Crear panel contenedor para este botón y sus hijos
            FlowLayoutPanel panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };
            panel.Controls.Add(btnMenu);

            // Buscar hijos y agregarlos recursivamente
            var hijosRows = dtMenuOpciones.AsEnumerable()
                .Where(r => Convert.ToInt32(r["PadreID"]) == menuID)
                .OrderBy(r => Convert.ToInt32(r["MenuID"]))
                .ToList();

            foreach (var hijo in hijosRows)
            {
                FlowLayoutPanel subPanel = CrearNodoRecursivo(hijo, dtMenuOpciones, toolTip1, nivel + 1);
                subPanel.Visible = false; // ocultos por defecto
                panel.Controls.Add(subPanel);
            }

            return panel;
        }
        private void BotonMenu_MouseClick(object sender, MouseEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton == null) return;

            int menuID = Convert.ToInt32(clickedButton.Name.Replace("btn", ""));
            bool isParent = dtMenuOpcionesFinal.AsEnumerable()
                .Any(row => Convert.ToInt32(row["MenuID"]) == menuID && Convert.ToInt32(row["PadreID"]) == 0);
            bool hasChild = dtMenuOpcionesFinal.AsEnumerable()
                .Any(row => Convert.ToInt32(row["PadreID"]) == menuID);

            if (isParent && !hasChild)
            {
                // Padre sin hijos → abrir formulario
                switch (clickedButton.Text.Trim())
                {
                    //case "NUEVA COTIZACION":
                    //    LanzarForm(new frmCotizaciones(), "HOME / ADMINISTRAR COTIZACIONES");
                    //    break;
                }
                return;
            }

            if (isParent || hasChild)
            {
                bool shouldExpand = clickedButton.Tag == null || clickedButton.Tag.ToString() == "collapsed";
                FlowLayoutPanel parentPanel = clickedButton.Parent as FlowLayoutPanel;

                // Mostrar/ocultar solo el subPanel de hijos directos
                foreach (Control control in parentPanel.Controls)
                {
                    if (control is FlowLayoutPanel subPanel)
                    {
                        subPanel.Visible = shouldExpand;
                        foreach (Control subControl in subPanel.Controls)
                        {
                            if (subControl is Button subButton)
                                subButton.Visible = shouldExpand;
                        }
                    }
                }

                clickedButton.Tag = shouldExpand ? "expanded" : "collapsed";

                // Cambiar ícono del botón padre según estado
                clickedButton.Image = shouldExpand
                    ? global::ModuloVentasAdmin.Properties.Resources.white_sort_down_16px
                    : global::ModuloVentasAdmin.Properties.Resources.white_sort_right_16px;
                return;
            }

            // Hijo sin hijos → abrir formulario
            switch (clickedButton.Text.Trim())
            {
                case "NUEVA COTIZACION":

                    LanzarForm(new frmCotizaciones(), "HOME / NUEVA COTIZACION");
                    break;
                case "COTIZACION EXISTENTE":
                    LanzarForm(new frmCotizacionExistente(), "HOME / COTIZACION EXISTENTE");
                    break;
            }
        }
        private void ActiveForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ActualizarUbicacion("HOME / BIENVENIDO: " + usuarioNombreCompleto);
        }
        private void ActualizarUbicacion(string ubicacionLabel)
        {
            // Suspender el diseño del formulario temporalmente
            this.SuspendLayout();
            lblUbicacion.Text = ubicacionLabel;
            lblUbicacion.Refresh();
            // Reanudar el diseño del formulario
            this.ResumeLayout();
        }
        private void EnableDoubleBuffering(Form form)
        {
            form.GetType().InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, form, new object[] { true });
        }

        public void LanzarForm(Form childForm, string ubicacionLabel)
        {
            // 🔹 Cerrar y limpiar el formulario activo
            if (activeForm != null)
            {
                activeForm.FormClosed -= ActiveForm_FormClosed;
                activeForm.Close();
                activeForm.Dispose();
                activeForm = null;
            }

            // 🔹 Limpiar el panel antes de agregar el nuevo form
            pWorkspace.Controls.Clear();

            // 🔹 Configurar el nuevo formulario
            activeForm = childForm;
            activeForm.FormClosed += ActiveForm_FormClosed;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            EnableDoubleBuffering(childForm);

            pWorkspace.SuspendLayout();
            pWorkspace.Controls.Add(childForm);
            pWorkspace.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            pWorkspace.ResumeLayout();

            // 🔹 Actualizar ubicación
            ActualizarUbicacion(ubicacionLabel);
        }

        public void SeguimientoUsuario(int _AccionID)
        {
            SeguimientoUsuario sendSeguimiento = new SeguimientoUsuario
            {
                Operacion = "INSERTAR",
                Usuario = DynamicMain.usuarionlogin,
                Modulo = Assembly.GetExecutingAssembly().GetName().Name,
                Formulario = this.Name,
                AccionID = _AccionID,
                UPosteo = DynamicMain.usuarionlogin,
                FPosteo = DateTime.Now,
                PC = System.Environment.MachineName,
                Estado = true
            };
            dtSeguimientoUsuario = logica.SP_SeguimientoUsuario(sendSeguimiento);
        }
        private void ToggleSubMenuVisibility(FlowLayoutPanel parentPanel, Button clickedButton, bool shouldExpand, int nivel)
        {
            foreach (Control subControl in parentPanel.Controls)
            {
                if (subControl is FlowLayoutPanel subPanel)
                {
                    foreach (Control subSubControl in subPanel.Controls)
                    {
                        if (subSubControl is Button subButton)
                        {
                            int subMenuID = Convert.ToInt32(subButton.Name.Replace("btn", ""));
                            bool isSubMenu = dtMenuOpciones.AsEnumerable().Any(row => Convert.ToInt32(row["PadreID"]) == subMenuID);
                            if (nivel < 2 && isSubMenu)
                            {
                                FlowLayoutPanel subSubPanel = subButton.Parent as FlowLayoutPanel;
                                ToggleSubMenuVisibility(subSubPanel, subButton, shouldExpand, nivel + 1);
                            }
                            subButton.Visible = shouldExpand && nivel < 2;
                        }
                    }
                    subPanel.Visible = shouldExpand;
                }
                else if (subControl != clickedButton)
                {
                    subControl.Visible = shouldExpand && nivel < 2;
                }
            }
        }
        private void PanelLeftTimer_Tick(object sender, EventArgs e)
        {
            if (panelLeftExpand)
            {
                pLeftMenu.Width -= 10;
                if (pLeftMenu.Width <= pLeftMenu.MinimumSize.Width)
                {
                    panelLeftExpand = false;
                    PanelLeftTimer.Stop();
                }
            }
            else
            {
                pLeftMenu.Width += 10;
                if (pLeftMenu.Width >= pLeftMenu.MaximumSize.Width)
                {
                    panelLeftExpand = true;
                    PanelLeftTimer.Stop();
                }
            }
        }

        private void pbxMenu_Click(object sender, EventArgs e)
        {
            if (!PanelLeftTimer.Enabled)
            {
                if (pLeftMenu.Width == pLeftMenu.MaximumSize.Width)
                {
                    panelLeftExpand = true;
                }
                else if (pLeftMenu.Width == pLeftMenu.MinimumSize.Width)
                {
                    panelLeftExpand = false;
                }

                PanelLeftTimer.Start();
            }
        }
        private void EstadoENAC()
        {
            dtEstadoENAC.Clear();
            EstadoENAC getEstado = new EstadoENAC()
            {
                Opcion = "Listado",
                ID = 1
            };
            dtEstadoENAC = logica.SP_EstadoENAC(getEstado);
            if (dtEstadoENAC.Rows.Count > 0)
            {
                _EstadoEnac = Convert.ToBoolean(dtEstadoENAC.Rows[0]["Estado"]);

                toolStripLabel10.Text = (_EstadoEnac) ? "ACTIVO" : "INACTIVO";
            }
        }
        private void CargarTema()
        {
            pLeftMenuLogo.BackColor = Color.FromArgb(236, 240, 241);
            pLeftMenu.BackColor = Color.FromArgb(84, 102, 118);
            pHeaderMain.BackColor = Color.FromArgb(84, 102, 118);
            pFooter.BackColor = Color.FromArgb(84, 102, 118);
            pHeaderOptions.BackColor = Color.FromArgb(240, 240, 240);
        }

        private void DynamicMain_Load(object sender, EventArgs e)
        {
            CargarTema();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logout();
        }
        private void Logout()
        {
            DialogResult result = MessageBox.Show("¿Desea cerrar sesion? Recuerda guardar todo tu progreso.", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Hide(); // Ocultar el formulario actual
                var loginForm = new Login();
                loginForm.Show();
            }
        }
        private void DynamicMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing) // Si el usuario cierra con la "X"
            {
                DialogResult result = MessageBox.Show("¿Está seguro de que desea salir del sistema?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true; // Cancela el cierre si el usuario elige "No"
                }
                else
                {
                    Application.Exit(); // Cierra toda la aplicación
                }
            }
        }
    }
}
