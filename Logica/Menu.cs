using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaVentasAdmin
{
    public class Menu
    {
    }
    public class TBLMenuDinamicoLista
    {
        public string Opcion { get; set; }
        public string Valor { get; set; }
        public string Valor2 { get; set; }
        public string Valor3 { get; set; }
    }
    public class PermisoAprobacionListado
    {
        public string Opcion { get; set; }
        public int? PermisoID { get; set; }
        public string Usuario { get; set; }
        public string NombreFormulario { get; set; }
        public string TipoObjeto { get; set; }
        public string NombreObjeto { get; set; }
        public string ValorObjeto { get; set; }
        public int? FormaValor { get; set; }
        public int? Estado { get; set; }
        public DateTime? FPosteo { get; set; }
        public string UPosteo { get; set; }
    }
    public class ClienteINTER
    {
        public string Opcion { get; set; }
        public string Cliente { get; set; }
        public string Nombre { get; set; }
        public string GrupoCliente { get; set; }
        public short? TipoPago { get; set; }
        public short? CreditoDia { get; set; }
        public decimal? CreditoLimite { get; set; }
        public short? TipoCosto { get; set; }
        public decimal? Descuento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public bool? SolicitaGuia { get; set; }
        public bool? PagoDestinatario { get; set; }
        public bool? Bloqueado { get; set; }
        public string SucursalPago { get; set; }
        public bool? Lunes { get; set; }
        public bool? Martes { get; set; }
        public bool? Miercoles { get; set; }
        public bool? Jueves { get; set; }
        public bool? Viernes { get; set; }
        public bool? Sabados { get; set; }
        public bool? Domingos { get; set; }
        public bool? ConfirmarRecoleccion { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public string RtnCedula { get; set; }
        public bool? Inter { get; set; }
        public string Act { get; set; }
        public string Pago { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string EnviosRecientes { get; set; }
        public string Aereo { get; set; }
        public string Password { get; set; }
    }

    public class ClienteENAC
    {
        public string Opcion { get; set; }
        public string Cliente { get; set; }
        public string Nombre { get; set; }
        public string GrupoCliente { get; set; }
        public short TipoPago { get; set; }
        public short CreditoDia { get; set; }
        public decimal CreditoLimite { get; set; }
        public short TipoCosto { get; set; }
        public decimal Descuento { get; set; }
        public DateTime? FechaIngreso { get; set; }
        public bool? SolicitaGuia { get; set; }
        public bool PagoDestinatario { get; set; }
        public bool Bloqueado { get; set; }
        public string SucursalPago { get; set; }
        public bool Lunes { get; set; }
        public bool Martes { get; set; }
        public bool Miercoles { get; set; }
        public bool Jueves { get; set; }
        public bool Viernes { get; set; }
        public bool Sabados { get; set; }
        public bool Domingos { get; set; }
        public bool ConfirmarRecoleccion { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public string RtnCedula { get; set; }
        public bool? Inter { get; set; }
        public string Act { get; set; }
        public string Semanales { get; set; }
        public string Mensuales { get; set; }
        public string Quincenales { get; set; }
    }

    public class Cliente
    {
        public string Opcion { get; set; }
        public bool EstadoENAC { get; set; }
        public int? ClienteID { get; set; }
        public string NombreCompleto { get; set; }
        public int? GrupoClienteID { get; set; }
        public int? CondicionPagoID { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? SucursalID { get; set; }
        public int? EmpresaID { get; set; }
        public int? TipoClienteID { get; set; }
        public int? DiasCredito { get; set; }
        public decimal? LimiteCredito { get; set; }
        public int? TipoDocumentoID { get; set; }
        public string DocumentoValor { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
        public int? inter { get; set; }
        public int? nac { get; set; }
        public int? Aereo { get; set; }
        public string Imagen { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public int? TipoCosto { get; set; }
        public decimal? Descuento { get; set; }
        public DateTime? FechaNacimiento { get; set; }
    }

    public class TBLPermisosEspecificos
    {
        public string Opcion { get; set; }               // Acción a ejecutar en el SP
        public int? PermisoID { get; set; }              // Clave primaria
        public int? UsuarioID { get; set; }              // Usuario al que se le asigna el permiso
        public string NombreFormulario { get; set; }     // Nombre del formulario
        public string NombreElemento { get; set; }       // Nombre del elemento
        public string AccionElemento { get; set; }       // Nombre visual para el usuario
        public string IconoElemento { get; set; }        // Ícono asociado (opcional)
        public bool? Visible { get; set; }               // Visibilidad del elemento
        public int? UbicacionID { get; set; }           // Ubicacion del elemento
        public string UPosteo { get; set; }              // Usuario que registra
        public DateTime? FPosteo { get; set; }           // Fecha de registro
        public string PC { get; set; }                   // Nombre del equipo
        public bool? Estado { get; set; }                // Estado activo/inactivo
    }
    public class EstadoENAC
    {
        public string Opcion { get; set; }           // Acción a ejecutar en el SP
        public int? ID { get; set; }                 // Clave primaria
        public string Descripcion { get; set; }      // Descripción del registro
        public bool? Estado { get; set; }            // Estado activo/inactivo
        public string UPosteo { get; set; }          // Usuario que registra
        public DateTime? FPosteo { get; set; }       // Fecha de registro
        public string PC { get; set; }               // Nombre del equipo
    }
    public class TBLGrupoClientes
    {
        public string Opcion { get; set; }           // Acción a ejecutar en el SP
        public int? GrupoClientesID { get; set; }    // Clave primaria
        public string Nombre { get; set; }           // Nombre del grupo
        public int? EmpresaID { get; set; }          // Empresa asociada
        public bool? Estado { get; set; }            // Estado activo/inactivo
        public string UPosteo { get; set; }          // Usuario que registra
        public DateTime? FPosteo { get; set; }       // Fecha de registro
        public string PC { get; set; }               // Nombre del equipo
    }
    public class TipoPago
    {
        public int Opcion { get; set; }           // Acción a ejecutar en el SP
        public string Valor { get; set; }           // Tipo de PAgo
    }
    public class CondicionPago
    {
        public string Opcion { get; set; }             // Acción a ejecutar en el SP
        public int? CondicionPagoID { get; set; }      // ID autogenerado
        public string Descripcion { get; set; }        // Nombre de la condición de pago
        public string UPosteo { get; set; }            // Usuario que realiza el cambio
        public DateTime? FPosteo { get; set; }         // Fecha del cambio
        public string PC { get; set; }                 // PC desde donde se hizo el cambio
        public bool? Estado { get; set; }              // true = activo, false = inactivo
    }
    public class DocumentoCliente
    {
        public string Opcion { get; set; }             // Acción a ejecutar en el SP
        public int? DocumentoID { get; set; }          // ID autogenerado del documento
        public string Descripcion { get; set; }        // Nombre o tipo del documento
        public bool? Estado { get; set; }              // true = activo, false = inactivo
        public string UPosteo { get; set; }            // Usuario que realiza el cambio
        public DateTime? FPosteo { get; set; }         // Fecha del cambio
        public string PC { get; set; }                 // PC desde donde se hizo el cambio
    }
    public class CiudadesClientes
    {
        public int Opcion { get; set; }           // Acción a ejecutar en el SP
        public string valor { get; set; }
    }
    public class PaisesClientes
    {
        public int Opcion { get; set; }           // Acción a ejecutar en el SP
        public string valor { get; set; }           // Tipo de PAgo
    }
    public class SeguimientoUsuario
    {
        public string Operacion { get; set; }             // Acción a ejecutar en el SP: INSERTAR, ACTUALIZAR, INACTIVAR, LISTAR
        public int? RegistroID { get; set; }              // ID autogenerado del registro
        public string Usuario { get; set; }               // Usuario que realizó la acción
        public string Modulo { get; set; }                // Módulo del sistema donde ocurrió
        public string Formulario { get; set; }            // Formulario específico
        public int? AccionID { get; set; }                // ID de la acción realizada
        public string UPosteo { get; set; }               // Usuario que realiza el cambio
        public DateTime? FPosteo { get; set; }            // Fecha del cambio
        public string PC { get; set; }                    // PC desde donde se hizo el cambio
        public bool? Estado { get; set; }                 // true = activo, false = inactivo
    }
    public class DiasCreditoCliente
    {
        public string Opcion { get; set; }           // Acción a ejecutar en el SP: INSERTAR, ACTUALIZAR, INACTIVAR, LISTAR
        public int? DiasCreditoID { get; set; }         // ID autogenerado del registro
        public string Descripcion { get; set; }         // Descripción del tipo de crédito (ej. "30 días", "Contado")
        public bool? Estado { get; set; }               // true = activo, false = inactivo
        public string UPosteo { get; set; }             // Usuario que realiza el cambio
        public DateTime? FPosteo { get; set; }          // Fecha del cambio
        public string PC { get; set; }                  // PC desde donde se hizo el cambio
    }
    public class ClienteCiudad
    {
        public string Opcion { get; set; }
        public int? ClienteCiudadesID { get; set; }
        public int? ClienteID { get; set; }
        public int? CiudadID { get; set; }
        public string Direccion { get; set; }
        public string TelefonoPrincipal { get; set; }
        public string ContactoNombre { get; set; }
        public string ContactoTelefono { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
        public string ClienteCiudadIDAnterior { get; set; }
        public int? PaisID { get; set; }
    }
    public class ClienteCiudadENAC
    {
        public string Opcion { get; set; } // Para operaciones tipo 'AGREGAR', 'ACTUALIZAR', etc.
        public string Cliente { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Inter { get; set; }
        public string Colonia { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Correo { get; set; }
        public string ContactoNombre { get; set; }
        public string ContactoCorreo { get; set; }
        public string ContactoTelefono { get; set; }
        public string ContactoDepto { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public string Act { get; set; }
    }
    public class ClienteCiudadINTER
    {
        public string Opcion { get; set; }
        public string Cliente { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public string Inter { get; set; }
        public string Colonia { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string Correo { get; set; }
        public string ContactoNombre { get; set; }
        public string ContactoCorreo { get; set; }
        public string ContactoTelefono { get; set; }
        public string ContactoDepto { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime FPosteo { get; set; }
        public string PC { get; set; }
        public string Act { get; set; }
    }
    public class ClienteCosto
    {
        public string Opcion { get; set; }
        public int? ClienteCostosID { get; set; }
        public string Descripcion { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
    }
    public class BodegaDTO
    {
        public string Opcion { get; set; }
        public string Bodega { get; set; }
        public string Nombre { get; set; }
        public string Caracter { get; set; }
        public long? Contador { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? Fposteo { get; set; }
        public string PC { get; set; }
    }
    public class FacturaDTO
    {
        public string Opcion { get; set; }
        public string Factura { get; set; }
        public string ClienteRemitente { get; set; }
        public string ClienteDestino { get; set; }
        public string Guia { get; set; }
        public string Empresa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
    public class FactorDolar50DTO
    {
        public string Opcion { get; set; }            // Acción a ejecutar en el SP (AGREGAR, LISTADO, RECUPERAR, etc.)
        public decimal? FactorDolar { get; set; }     // Valor del factor de cambio (USD)
    }
    public class PerfilModuloPermisoDTO
    {
        public string Opcion { get; set; }                // Acción a ejecutar en el SP (Agregar, Actualizar, Eliminar, Listado, etc.)
        public int? PerfilModuloPermisoID { get; set; }   // Clave primaria del registro
        public int? PerfilID { get; set; }                // Perfil asociado
        public int? ModuloID { get; set; }                // Módulo asociado
        public int? MenuID { get; set; }                  // Formulario/pantalla
        public int? InteraccionID { get; set; }           // Tipo de interacción (Leer, Escribir, Eliminar, etc.)
        public string UPosteo { get; set; }               // Usuario que registra
        public DateTime? FPosteo { get; set; }            // Fecha del registro
        public string PC { get; set; }                    // Equipo desde donde se registró
        public bool? Estado { get; set; }                 // Estado lógico (activo/inactivo)
    }
    public class PerfilDTO
    {
        public string Opcion { get; set; }          // Acción: Agregar, Actualizar, Eliminar, Listado, etc.
        public int? PerfilID { get; set; }          // Clave primaria
        public string Descripcion { get; set; }     // Nombre/Descripción del perfil
        public bool? Estado { get; set; }           // Activo/Inactivo
        public string UPosteo { get; set; }         // Usuario que registra
        public DateTime? FPosteo { get; set; }      // Fecha de registro
        public string PC { get; set; }              // Equipo desde donde se registró
    }

    public class PerfilPermisoDTO
    {
        public string Opcion { get; set; }                // Acción: Agregar, Actualizar, Eliminar, Listado, etc.
        public int? PerfilPermisoID { get; set; }         // Clave primaria
        public int? PerfilID { get; set; }                // Perfil asociado
        public int? MenuID { get; set; }                  // Formulario/pantalla
        public bool? Estado { get; set; }                 // Activo/Inactivo
        public string UPosteo { get; set; }               // Usuario que registra
        public DateTime? FPosteo { get; set; }            // Fecha de registro
        public string PC { get; set; }                    // Equipo desde donde se registró
    }
    public class PerfilPermisosExtraDTO
    {
        public string Opcion { get; set; }             // Acción: Agregar, Actualizar, Eliminar, Listado, etc.
        public int? PermisoExtraID { get; set; }       // Clave primaria
        public int? UsuarioID { get; set; }            // Usuario asociado
        public int? MenuID { get; set; }               // Formulario/pantalla
        public bool? Estado { get; set; }              // Activo/Inactivo
        public string UPosteo { get; set; }            // Usuario que registra
        public DateTime? FPosteo { get; set; }         // Fecha de registro
        public string PC { get; set; }                 // Equipo desde donde se registró
    }
    public class ModulosDTO
    {
        public string Opcion { get; set; }             // Acción: Agregar, Actualizar, Eliminar, Listado, etc.
        public int? ModuloID { get; set; }             // PK
        public string Modulo { get; set; }             // Nombre del módulo
        public string UPosteo { get; set; }            // Usuario que registra
        public DateTime? FPosteo { get; set; }         // Fecha de registro
        public int? PlataformaID { get; set; }         // Plataforma asociada
        public bool? Estado { get; set; }              // Activo/Inactivo
    }
    public class PerfilModuloDTO
    {
        public string Accion { get; set; }
        public int PerfilModuloID { get; set; }
        public int PerfilID { get; set; }
        public int ModuloID { get; set; }
        public bool? Estado { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
    }
    public class MenuDinamicoDTO
    {
        public string Opcion { get; set; }
        public int? MenuID { get; set; }
        public string Descripcion { get; set; }
        public int? ModuloID { get; set; }
        public int? UbicacionID { get; set; }
        public int? AreaID { get; set; }
        public bool? Permiso { get; set; }
        public int? PadreID { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
        public string Tag { get; set; }
        public bool? ExcepcionCaja { get; set; }
        public string Comentario { get; set; }
    }
    public class CiudadesENAC
    {
        public string Opcion { get; set; }
        public string Ciudad { get; set; }
        public string Nombre { get; set; }
        public string Ruta { get; set; }
        public string Sucursal { get; set; }
        public bool? CiudadSucursalPrincipal { get; set; }
        public bool? Lunes { get; set; }
        public bool? Martes { get; set; }
        public bool? Miercoles { get; set; }
        public bool? Jueves { get; set; }
        public bool? Viernes { get; set; }
        public bool? Sabados { get; set; }
        public bool? Domingos { get; set; }
        public string Pais { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public string Observacion { get; set; }
        public string Zona { get; set; }
        public string CiudadPrincipal { get; set; }
        public bool? Cabecera { get; set; }
        public string SucursalPago { get; set; }
        public string act { get; set; }
        public string Tabla { get; set; }
    }
    public class ProductosENAC
    {
        public string Opcion { get; set; }
        public string Producto { get; set; }
        public string Nombre { get; set; }
        public decimal? CostoSencillo { get; set; }
        public bool? Activo { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public string Cliente { get; set; }
        public string Listo { get; set; }
        public string Act { get; set; }
    }
    public class ProductoCostosENAC
    {
        public string Opcion { get; set; }
        public string Producto { get; set; }
        public string CiudadRemitente { get; set; }
        public string CiudadDestino { get; set; }
        public decimal? Costo { get; set; }
        public string Empresa { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public decimal? CostoRecolector { get; set; }
        public string Act { get; set; }
    }
    public class CotizacionEncabezadoDTO
    {
        public string Opcion { get; set; }
        public long? EncabezadoID { get; set; }
        public int? TipoCotizacionID { get; set; }
        public int? ClienteID { get; set; }
        public string Atencion { get; set; }
        public int? ImpuestoID { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
    }
    public class CotizacionDetalleDTO
    {
        public string Opcion { get; set; }
        public long? DetalleID { get; set; }
        public int? EncabezadoID { get; set; }
        public string CiudadOrigenID { get; set; }
        public string CiudadDestinoID { get; set; }
        public string ProductoID { get; set; }
        public decimal? Precio { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
    }
    public class CotizacionTerminoDTO
    {
        public string Opcion { get; set; } // Agregar, Actualizar, Eliminar, Listar
        public long? TerminoID { get; set; }
        public string Descripcion { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
    }
    public class CotizacionTipoDTO
    {
        public string Opcion { get; set; }
        public long? TipoID { get; set; }
        public string Descripcion { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
    }
    public class CotizacionImpuestoDTO
    {
        public string Opcion { get; set; }
        public long? ImpuestoID { get; set; }
        public string Descripcion { get; set; }
        public string UPosteo { get; set; }
        public DateTime? FPosteo { get; set; }
        public string PC { get; set; }
        public bool? Estado { get; set; }
    }
}
