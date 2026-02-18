using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DatosVentasAdmin
{
    public class EnviarConsultas
    {
        private BD_Conexion Conexion = new BD_Conexion();
        private BD_Conexion ConexionAlterna = new BD_Conexion();

        SqlDataReader leer = null;
        SqlCommand comando = new SqlCommand();
        SqlDataAdapter adaptador = new SqlDataAdapter();
        SqlParameter parametro = new SqlParameter();

        public DataTable SP_Impresoras(dynamic a)
        {

            comando.Parameters.Clear();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            tabla = new DataTable();
            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ImpresoraID", a.ImpresoraID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Tipo", a.Tipo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_Impresoras";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable CargarDatos(int Usuario, int OpcionSP, int OpcionCampo, string valor)
        {
            DataTable tabla = new DataTable();
            comando.Parameters.Clear();


            // Diccionario para mapear opciones a nombres de procedimientos almacenados
            Dictionary<int, Tuple<string, int>> procedimientos = new Dictionary<int, Tuple<string, int>>
                {
                  { 1,Tuple.Create("RCRH.Empleados.SP_ObtenerEmpleadoGeneral",3)},
                  { 2, Tuple.Create("RCCONFIG.Empresa.SP_ObtenerPaises",2)},
                  { 3, Tuple.Create("RCRH.Empleados.SP_ObtenerEmpleadosIngresosEgresos",1)},
                  { 4, Tuple.Create("RCRH.Empleados.SP_ObtenerEstadoCivil",3)},
                  { 5,Tuple.Create ("RCRH.Empleados.SP_ObtenerTipoSangre",1)},
                  { 6,Tuple.Create( "RCCONFIG.Empresa.SP_ObtenerCiudades",2)},
                  { 7, Tuple.Create("RCCONFIG.Empresa.SP_ObtenerSucursales",2)},
                  { 8, Tuple.Create("RCCONFIG.Empresa.SP_ObtenerDepartamentosEmpresa",2)},
                  { 9, Tuple.Create("RCCONFIG.Empresa.SP_ObtenerEmpleadosPuestos",2)},
                  { 10,Tuple.Create( "RCRH.Empleados.SP_ObtenerTurnoPlanillas",1)},
                  { 11, Tuple.Create("RCRH.Empleados.SP_ObtenerPlanillasContratos",1)},
                  { 12, Tuple.Create("RCCONFIG.Empresa.SP_ObtenerMonedaPago",2)},
                  { 13,Tuple.Create( "RCRH.Empleados.SP_ObtenerBancosPago",1)},
                  { 14, Tuple.Create("RCRH.Empleados.SP_ObtenerEmpleadosMovimientos",1)},
                  { 15,Tuple.Create( "RCRH.Empleados.SP_ObtenerEmpleadoDatosLaborales",3)},
                  { 16,Tuple.Create( "RCCONFIG.Empresa.SP_ObtenerEmpresas",2)},
                  { 17, Tuple.Create("RCRH.Empleados.SP_ObtenerCobrosControl",3)},
                  { 18, Tuple.Create("RCRH.Empleados.SP_ObtenerEmpleadoPlanillas",1)},
                  { 19, Tuple.Create("RCRH.Empleados.SP_RPT_PLANILLAHORIZONTAL",3)},
                  { 20, Tuple.Create("RCRH.Empleados.SP_VacacionesObtener",3)},
                  { 21, Tuple.Create("RCRH.Empleados.SP_ObtenerListaDocumentos",3)},
                  { 22, Tuple.Create("RCRH.Empleados.SP_DocumentosEmpleados_GetDel",3)}
                };

            if (!procedimientos.TryGetValue(OpcionSP, out var storedProcedure))
            {
                Console.WriteLine("Opción inválida");
            }

            using (var conexion = Conexion.AbrirConexion(storedProcedure.Item2))
            using (var comando = new SqlCommand(storedProcedure.Item1, conexion))
            {
                comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@Opcion", OpcionCampo);
                comando.Parameters.AddWithValue("@valor", valor);

                using (var lector = comando.ExecuteReader())
                {
                    tabla.Load(lector);
                }
            }
            return tabla;
        }
        public DataTable SP_MenuDinamico_GET(dynamic a)
        {
            leer = null;
            comando.Parameters.Clear();
            DataTable tabla = new DataTable();
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }
            tabla = new DataTable();

            comando.Connection = Conexion.AbrirConexion(3);
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Valor", a.Valor ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Valor2", a.Valor2 ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Valor3", a.Valor3 ?? (object)DBNull.Value);

            comando.CommandText = "RCConfig.Empresa.SP_MenuDinamico_GET";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ClientesINTER(dynamic cliente)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(5);

            comando.Parameters.AddWithValue("@Opcion", cliente.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", cliente.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", cliente.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@GrupoCliente", cliente.GrupoCliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoPago", cliente.TipoPago ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CreditoDia", cliente.CreditoDia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CreditoLimite", cliente.CreditoLimite ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoCosto", cliente.TipoCosto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", cliente.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaIngreso", cliente.FechaIngreso ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SolicitaGuia", cliente.SolicitaGuia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PagoDestinatario", cliente.PagoDestinatario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Bloqueado", cliente.Bloqueado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SucursalPago", cliente.SucursalPago ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Lunes", cliente.Lunes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Martes", cliente.Martes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Miercoles", cliente.Miercoles ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Jueves", cliente.Jueves ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Viernes", cliente.Viernes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Sabados", cliente.Sabados ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Domingos", cliente.Domingos ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ConfirmarRecoleccion", cliente.ConfirmarRecoleccion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", cliente.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", cliente.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", cliente.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", cliente.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@RtnCedula", cliente.RtnCedula ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Inter", cliente.Inter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Act", cliente.Act ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Pago", cliente.Pago ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PrimerNombre", cliente.PrimerNombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SegundoNombre", cliente.SegundoNombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PrimerApellido", cliente.PrimerApellido ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SegundoApellido", cliente.SegundoApellido ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EnviosRecientes", cliente.EnviosRecientes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Aereo", cliente.Aereo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Password", cliente.Password ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_Clientes";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }

        public DataTable SP_ClientesENAC(dynamic cliente)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", cliente.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", cliente.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", cliente.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@GrupoCliente", cliente.GrupoCliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoPago", cliente.TipoPago ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CreditoDia", cliente.CreditoDia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CreditoLimite", cliente.CreditoLimite ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoCosto", cliente.TipoCosto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", cliente.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaIngreso", cliente.FechaIngreso ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SolicitaGuia", cliente.SolicitaGuia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PagoDestinatario", cliente.PagoDestinatario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Bloqueado", cliente.Bloqueado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SucursalPago", cliente.SucursalPago ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Lunes", cliente.Lunes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Martes", cliente.Martes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Miercoles", cliente.Miercoles ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Jueves", cliente.Jueves ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Viernes", cliente.Viernes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Sabados", cliente.Sabados ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Domingos", cliente.Domingos ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ConfirmarRecoleccion", cliente.ConfirmarRecoleccion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", cliente.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", cliente.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", cliente.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", cliente.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@RtnCedula", cliente.RtnCedula ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Inter", cliente.Inter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Act", cliente.Act ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Semanales", cliente.Semanales ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Mensuales", cliente.Mensuales ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Quincenales", cliente.Quincenales ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CampoGrande", cliente.CampoGrande ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_Clientes";
            comando.CommandType = CommandType.StoredProcedure;
            comando.CommandTimeout = 300; 

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }


        public DataTable SP_Clientes(dynamic cliente)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", cliente.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoENAC", cliente.EstadoENAC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteID", cliente.ClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@NombreCompleto", cliente.NombreCompleto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@GrupoClienteID", cliente.GrupoClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CondicionPagoID", cliente.CondicionPagoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaRegistro", cliente.FechaRegistro ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SucursalID", cliente.SucursalID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EmpresaID", cliente.EmpresaID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoClienteID", cliente.TipoClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DiasCredito", cliente.DiasCredito ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@LimiteCredito", cliente.LimiteCredito ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoDocumentoID", cliente.TipoDocumentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DocumentoValor", cliente.DocumentoValor ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", cliente.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", cliente.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", cliente.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", cliente.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@inter", cliente.inter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@nac", cliente.nac ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Aereo", cliente.Aereo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Imagen", cliente.Imagen ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Telefono", cliente.Telefono ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Correo", cliente.Correo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoCosto", cliente.TipoCosto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", cliente.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaNacimiento", cliente.FechaNacimiento ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_Clientes";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_PermisosEspecificos(dynamic permiso)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            // Limpiar la tabla si ya tiene datos
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            // Agregar parámetros
            comando.Parameters.AddWithValue("@Opcion", permiso.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PermisoID", permiso.PermisoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UsuarioID", permiso.UsuarioID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@NombreFormulario", permiso.NombreFormulario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@NombreElemento", permiso.NombreElemento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@AccionElemento", permiso.AccionElemento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@IconoElemento", permiso.IconoElemento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Visible", permiso.Visible ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UbicacionID", permiso.UbicacionID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", permiso.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", permiso.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", permiso.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", permiso.Estado ?? (object)DBNull.Value);

            // Configurar comando
            comando.CommandText = "RCCONFIG.Empresa.SP_PermisosEspecificos";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_EstadoENAC(dynamic activar)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            // Limpiar la tabla si ya tiene datos
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            // Agregar parámetros
            comando.Parameters.AddWithValue("@Opcion", activar.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ID", activar.ID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", activar.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", activar.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", activar.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", activar.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", activar.PC ?? (object)DBNull.Value);

            // Configurar comando
            comando.CommandText = "RCCONFIG.Empresa.SP_ActivarENAC";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_GrupoClientes(dynamic grupo)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            // Limpiar la tabla si ya tiene datos
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", grupo.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@GrupoClientesID", grupo.GrupoClientesID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", grupo.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EmpresaID", grupo.EmpresaID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", grupo.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", grupo.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", grupo.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", grupo.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_GrupoClientes";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_TBLClientesTipoPago(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            // Limpiar la tabla si ya tiene datos
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Valor", a.Valor ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_TBLClientesTipoPago_GET";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CondicionPago(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            comando.Connection = Conexion.AbrirConexion(3); // Ajustá el índice de conexión si aplica
            comando.CommandText = "RC.Clientes.SP_CondicionPago";
            comando.CommandType = CommandType.StoredProcedure;

            // Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion);
            comando.Parameters.AddWithValue("@CondicionPagoID", (object)a.CondicionPagoID ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", (object)a.Descripcion ?? DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", (object)a.UPosteo ?? DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", (object)a.FPosteo ?? DBNull.Value);
            comando.Parameters.AddWithValue("@PC", (object)a.PC ?? DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", (object)a.Estado ?? DBNull.Value);

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ClientesDocumentos(dynamic documento)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", documento.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DocumentoID", documento.DocumentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", documento.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", documento.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", documento.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", documento.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", documento.PC ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_ClientesDocumentos";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ObtenerCiudades(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            // Limpiar la tabla si ya tiene datos
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@valor", a.valor ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_ObtenerCiudades";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ObtenerPaises(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            // Limpiar la tabla si ya tiene datos
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@valor", a.valor ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_ObtenerPaises";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_SeguimientoUsuario(dynamic seguimiento)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Operacion", seguimiento.Operacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@RegistroID", seguimiento.RegistroID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Usuario", seguimiento.Usuario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Modulo", seguimiento.Modulo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Formulario", seguimiento.Formulario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@AccionID", seguimiento.AccionID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", seguimiento.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", seguimiento.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", seguimiento.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", seguimiento.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCHISTORIAL.Registros.SP_AdministrarSeguimientoUsuario";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_AdministrarDiasCredito(dynamic credito)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", credito.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DiasCreditoID", credito.DiasCreditoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", credito.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", credito.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", credito.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", credito.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", credito.PC ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_AdministrarDiasCredito";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ClienteCiudades(dynamic ciudad)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", ciudad.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteCiudadesID", ciudad.ClienteCiudadesID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteID", ciudad.ClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadID", ciudad.CiudadID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Direccion", ciudad.Direccion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TelefonoPrincipal", ciudad.TelefonoPrincipal ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoNombre", ciudad.ContactoNombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoTelefono", ciudad.ContactoTelefono ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", ciudad.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", ciudad.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", ciudad.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", ciudad.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteCiudadIDAnterior", ciudad.ClienteCiudadIDAnterior ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PaisID", ciudad.PaisID ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_ClienteCiudades";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ClienteCiudadesENAC(dynamic ciudad)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", ciudad.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", ciudad.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Ciudad", ciudad.Ciudad ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Direccion", ciudad.Direccion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Inter", ciudad.Inter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Colonia", ciudad.Colonia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Telefono", ciudad.Telefono ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Fax", ciudad.Fax ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Correo", ciudad.Correo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoNombre", ciudad.ContactoNombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoCorreo", ciudad.ContactoCorreo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoTelefono", ciudad.ContactoTelefono ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoDepto", ciudad.ContactoDepto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", ciudad.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", ciudad.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", ciudad.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", ciudad.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Act", ciudad.Act ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_ClienteCiudades";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ClienteCiudadesINTER(dynamic ciudad)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(5);

            comando.Parameters.AddWithValue("@Opcion", ciudad.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", ciudad.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Ciudad", ciudad.Ciudad ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Direccion", ciudad.Direccion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Inter", ciudad.Inter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Colonia", ciudad.Colonia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Telefono", ciudad.Telefono ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Fax", ciudad.Fax ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Correo", ciudad.Correo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoNombre", ciudad.ContactoNombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoCorreo", ciudad.ContactoCorreo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoTelefono", ciudad.ContactoTelefono ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ContactoDepto", ciudad.ContactoDepto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", ciudad.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", ciudad.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", ciudad.FPosteo != DateTime.MinValue ? ciudad.FPosteo : (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", ciudad.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Act", ciudad.Act ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_ClienteCiudades";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ClientesCostos(dynamic cliente)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", cliente.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteCostosID", cliente.ClienteCostosID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", cliente.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", cliente.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", cliente.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", cliente.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", cliente.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RC.Clientes.SP_ClientesCostos";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_BodegasContadoresENAC(dynamic bodega)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", bodega.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Bodega", bodega.Bodega ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", bodega.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Caracter", bodega.Caracter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Contador", bodega.Contador ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", bodega.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", bodega.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Fposteo", bodega.Fposteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", bodega.PC ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_Bodegas";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_BodegasContadoresINTER(dynamic bodega)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(5);

            comando.Parameters.AddWithValue("@Opcion", bodega.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Bodega", bodega.Bodega ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", bodega.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Caracter", bodega.Caracter ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Contador", bodega.Contador ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", bodega.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", bodega.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Fposteo", bodega.Fposteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", bodega.PC ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_Bodegas";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_FacturasENAC(dynamic factura)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", factura.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Factura", factura.Factura ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteRemitente", factura.ClienteRemitente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteDestino", factura.ClienteDestino ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Guia", factura.Guia ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", factura.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaInicio", factura.FechaInicio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaFin", factura.FechaFin ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_Facturas";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_FactorDolar50(dynamic factor)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(5);

            comando.Parameters.AddWithValue("@Opcion", factor.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FactorDolar", factor.FactorDolar ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_FactorDolar";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_Perfiles(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PerfilID", a.PerfilID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_Perfiles";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_PerfilPermisos(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PerfilPermisoID", a.PerfilPermisoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PerfilID", a.PerfilID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@MenuID", a.MenuID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_PerfilPermisos";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_PerfilPermisosExtra(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PermisoExtraID", a.PermisoExtraID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UsuarioID", a.UsuarioID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@MenuID", a.MenuID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_PerfilPermisosExtra";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_Modulos(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ModuloID", a.ModuloID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Modulo", a.Modulo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PlataformaID", a.PlataformaID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_Modulos";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_PerfilModulos(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Accion", a.Accion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PerfilModuloID", a.PerfilModuloID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PerfilID", a.PerfilID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ModuloID", a.ModuloID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_PerfilModulos_CRUD";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_MenuDinamico(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@MenuID", a.MenuID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ModuloID", a.ModuloID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UbicacionID", a.UbicacionID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@AreaID", a.AreaID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Permiso", a.Permiso ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PadreID", a.PadreID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Tag", a.Tag ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ExcepcionCaja", a.ExcepcionCaja ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Comentario", a.Comentario ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_MenuDinamico";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CiudadesENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // Abrir conexión (ajusta el parámetro según tu configuración)
            //Conexion.CerrarConexion();
            comando.Connection = ConexionAlterna.AbrirConexion(4);

            // Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Ciudad", a.Ciudad ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", a.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Ruta", a.Ruta ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Sucursal", a.Sucursal ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadSucursalPrincipal", a.CiudadSucursalPrincipal ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Lunes", a.Lunes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Martes", a.Martes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Miercoles", a.Miercoles ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Jueves", a.Jueves ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Viernes", a.Viernes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Sabados", a.Sabados ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Domingos", a.Domingos ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Pais", a.Pais ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", a.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Observacion", a.Observacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Zona", a.Zona ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadPrincipal", a.CiudadPrincipal ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cabecera", a.Cabecera ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SucursalPago", a.SucursalPago ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@act", a.act ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Tabla", a.Tabla ?? (object)DBNull.Value);

            // Nombre del procedimiento
            comando.CommandText = "ENAC.dbo.SP_Ciudades";
            comando.CommandType = CommandType.StoredProcedure;

            // Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            ConexionAlterna.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ProductosENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // Abrir conexión (ajusta el parámetro según tu configuración)
            comando.Connection = Conexion.AbrirConexion(4);

            // Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Producto", a.Producto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", a.Nombre ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CostoSencillo", a.CostoSencillo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Activo", a.Activo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", a.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", a.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Listo", a.Listo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@act", a.Act ?? (object)DBNull.Value);

            // Nombre del procedimiento
            comando.CommandText = "ENAC.dbo.SP_Productos";
            comando.CommandType = CommandType.StoredProcedure;

            // Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ProductoCostosENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // Abrir conexión (ajusta el parámetro según tu configuración)
            comando.Connection = Conexion.AbrirConexion(3);

            // Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Producto", a.Producto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadRemitente", a.CiudadRemitente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadDestino", a.CiudadDestino ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Costo", a.Costo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Empresa", a.Empresa ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CostoRecolector", a.CostoRecolector ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@act", a.Act ?? (object)DBNull.Value);

            // Nombre del procedimiento
            comando.CommandText = "ENAC.dbo.SP_ProductoCostos";
            comando.CommandType = CommandType.StoredProcedure;

            // Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionEncabezado(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EncabezadoID", a.EncabezadoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoCotizacionID", a.TipoCotizacionID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteID", a.ClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Atencion", a.Atencion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ImpuestoID", a.ImpuestoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoSeguimiento", a.EstadoSeguimiento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Archivo", a.Archivo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_CotizacionEncabezado";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionDetalle(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DetalleID", a.DetalleID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EncabezadoID", a.EncabezadoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadOrigenID", a.CiudadOrigenID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadDestinoID", a.CiudadDestinoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoID", a.ProductoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Precio", a.Precio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_CotizacionDetalle";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionTerminos(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // 🔹 Abre la conexión (usa tu helper de conexión)
            comando.Connection = Conexion.AbrirConexion(3);

            // 🔹 Parámetros del SP
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TerminoID", a.TerminoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            // 🔹 Nombre del procedimiento
            comando.CommandText = "RCCONFIG.Empresa.SP_CotizacionTerminos";
            comando.CommandType = CommandType.StoredProcedure;

            // 🔹 Ejecuta y carga resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }

        public DataTable SP_CotizacionTipo(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoID", a.TipoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_CotizacionTipo";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionImpuesto(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ImpuestoID", a.ImpuestoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_CotizacionImpuesto";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ProductosGruposENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoGrupoID", a.ProductoGrupoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@GrupoID", a.GrupoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoID", a.ProductoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_ProductosGrupos";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ProductosPreciosENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoPrecioID", a.ProductoPrecioID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PromedioID", a.PromedioID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PromedioDesde", a.PromedioDesde ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PromedioHasta", a.PromedioHasta ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PromedioGDesde", a.PromedioGDesde ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PromedioGHasta", a.PromedioGHasta ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoID", a.ProductoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Precio", a.Precio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_ProductosPrecios";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ProductosDescripcionENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(4);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ID", a.ID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoID", a.ProductoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descripcion", a.Descripcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "ENAC.dbo.SP_ProductosDescripcion";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_AumentoPreciosEncabezado(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@AumentoID", a.AumentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoAumento", a.TipoAumento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ValorAumento", a.ValorAumento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoAprobacion", a.EstadoAprobacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Comentario", a.Comentario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_AumentoPreciosEncabezado";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_AumentoPreciosDetalle(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            comando.Connection = Conexion.AbrirConexion(3);

            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DetalleID", a.DetalleID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@AumentoID", a.AumentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteID", a.ClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ValorAumento", a.ValorAumento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Comentario", a.Comentario ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            comando.CommandText = "RCCONFIG.Empresa.SP_AumentoPreciosDetalle";
            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_TarifarioSucursales(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // Abrir conexión (ajusta el número según tu configuración)
            comando.Connection = Conexion.AbrirConexion(4);

            // Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TarifarioID", a.TarifarioID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@SucursalID", a.SucursalID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadDestino", a.CiudadDestino ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Orden", a.Orden ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            // Nombre del procedimiento
            comando.CommandText = "ENAC.dbo.SP_TarifarioSucursales";
            comando.CommandType = CommandType.StoredProcedure;

            // Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_ProductosCiudadesENAC(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // 🔹 Abrir conexión (usa tu helper Conexion.AbrirConexion)
            comando.Connection = Conexion.AbrirConexion(4);

            // 🔹 Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Productos", a.Productos ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadRemitente", a.CiudadRemitente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", a.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Impuesto", a.Impuesto ?? (object)DBNull.Value);

            // 🔹 Nombre del procedimiento
            comando.CommandText = "ENAC.dbo.SP_ProductosCiudades";
            comando.CommandType = CommandType.StoredProcedure;

            // 🔹 Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }

        public DataTable SP_ProductosClienteCostos(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // 🔹 Abrir conexión (usa tu helper Conexion.AbrirConexion)
            comando.Connection = Conexion.AbrirConexion(4);

            // 🔹 Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Productos", a.Productos ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadRemitente", a.CiudadRemitente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", a.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", a.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Impuesto", a.Impuesto ?? (object)DBNull.Value);

            // 🔹 Nombre del procedimiento
            comando.CommandText = "ENAC.dbo.SP_ProductosClienteCostos";
            comando.CommandType = CommandType.StoredProcedure;

            // 🔹 Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionDescuentoEncabezado(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // 🔹 Abrir conexión
            comando.Connection = Conexion.AbrirConexion(3);

            // 🔹 Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DescuentoID", a.DescuentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Tipo", a.Tipo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ClienteID", a.ClienteID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", a.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Impuesto", a.Impuesto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoAprobacion", a.EstadoAprobacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            // 🔹 Nombre del procedimiento
            comando.CommandText = "RCCONFIG.Empresa.dbo.SP_CotizacionDescuentoEncabezado";
            comando.CommandType = CommandType.StoredProcedure;

            // 🔹 Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionDescuentoDetalle(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // 🔹 Abrir conexión
            comando.Connection = Conexion.AbrirConexion(3);

            // 🔹 Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DetalleID", a.DetalleID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DescuentoID", a.DescuentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PrincipalAledano", a.PrincipalAledano ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ProductoID", a.ProductoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadRemitente", a.CiudadRemitente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadDestino", a.CiudadDestino ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Costo", a.Costo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoAprobacion", a.EstadoAprobacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);

            // 🔹 Nombre del procedimiento
            comando.CommandText = "RCCONFIG.Empresa.dbo.SP_CotizacionDescuentoDetalle";
            comando.CommandType = CommandType.StoredProcedure;

            // 🔹 Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable SP_CotizacionDescuento(dynamic a)
        {
            SqlDataReader leer = null;
            SqlCommand comando = new SqlCommand();
            DataTable tabla = new DataTable();

            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }

            // 🔹 Abrir conexión
            comando.Connection = Conexion.AbrirConexion(3);

            // 🔹 Parámetros del procedimiento
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@DescuentoID", a.DescuentoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@TipoCosto", a.TipoCosto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Cliente", a.Cliente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CiudadRemitente", a.CiudadRemitente ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Productos", a.Productos ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Descuento", a.Descuento ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Impuesto", a.Impuesto ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoAprobacion", a.EstadoAprobacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Estado", a.Estado ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaDesde", a.FechaDesde ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FechaHasta", a.FechaHasta ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Nombre", a.Nombre ?? (object)DBNull.Value);


            // 🔹 Nombre del procedimiento
            comando.CommandText = "RCCONFIG.Empresa.SP_CotizacionDescuento";
            comando.CommandType = CommandType.StoredProcedure;

            // 🔹 Ejecutar y cargar resultados
            leer = comando.ExecuteReader();
            tabla.Load(leer);

            Conexion.CerrarConexion();

            return tabla;
        }
        public DataTable DynamicSP_EmpleadosAutorizacion_Ins(dynamic a)
        {
            leer = null;
            comando.Parameters.Clear();
            DataTable tabla = new DataTable();
            if (tabla.Rows.Count > 0)
            {
                tabla.Rows.Clear();
                tabla.Clear();
            }
            tabla = new DataTable();

            comando.Connection = Conexion.AbrirConexion(3);
            comando.Parameters.AddWithValue("@Opcion", a.Opcion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@valor", a.valor ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EmpleadoID", a.EmpleadoID ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@CampoTabla", a.CampoTabla ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ValorAnterior", a.ValorAnterior ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@ValorNuevo", a.ValorNuevo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@EstadoAprobacion", a.EstadoAprobacion ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@UPosteo", a.UPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@FPosteo", a.FPosteo ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@PC", a.PC ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Anio", a.Anio ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Mes", a.Mes ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Quincena", a.Quincena ?? (object)DBNull.Value);
            comando.Parameters.AddWithValue("@Comodin", a.Comodin ?? (object)DBNull.Value);
            comando.CommandText = "RCRH.Empleados.SP_EmpleadosAutorizacion_Ins";

            comando.CommandType = CommandType.StoredProcedure;

            leer = comando.ExecuteReader();
            tabla.Load(leer);
            Conexion.CerrarConexion();

            return tabla;
        }

    }
}
