using DatosVentasAdmin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaVentasAdmin
{
    public class clsLogica
    {
        private readonly EnviarConsultas enviar = new EnviarConsultas();
        public DataTable SP_MenuDinamico_GET(TBLMenuDinamicoLista a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_MenuDinamico_GET(a);
            return tabla;
        }
        public DataTable SP_Clientes(Cliente a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_Clientes(a);
            return tabla;
        }
        public DataTable SP_ClientesENAC(ClienteENAC a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClientesENAC(a);
            return tabla;
        }
        public DataTable SP_ClientesINTER(ClienteINTER a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClientesINTER(a);
            return tabla;
        }
        public DataTable SP_ClienteCiudades(ClienteCiudad a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClienteCiudades(a);
            return tabla;
        }
        public DataTable SP_ClienteCiudadesENAC(ClienteCiudadENAC a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClienteCiudadesENAC(a);
            return tabla;
        }
        public DataTable SP_ClienteCiudadesINTER(ClienteCiudadINTER a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClienteCiudadesINTER(a);
            return tabla;
        }
        public DataTable SP_PermisosEspecificos(TBLPermisosEspecificos a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_PermisosEspecificos(a);
            return tabla;
        }
        public DataTable SP_EstadoENAC(EstadoENAC a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_EstadoENAC(a);
            return tabla;
        }
        public DataTable SP_GrupoClientes(TBLGrupoClientes a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_GrupoClientes(a);
            return tabla;
        }
        public DataTable SP_TBLClientesTipoPago(TipoPago a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_TBLClientesTipoPago(a);
            return tabla;
        }
        public DataTable SP_CondicionPago(CondicionPago a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CondicionPago(a);
            return tabla;
        }
        public DataTable SP_ClientesDocumentos(DocumentoCliente a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClientesDocumentos(a);
            return tabla;
        }
        public DataTable SP_ObtenerCiudades(CiudadesClientes a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ObtenerCiudades(a);
            return tabla;
        }
        public DataTable SP_ObtenerPaises(PaisesClientes a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ObtenerPaises(a);
            return tabla;
        }
        public DataTable SP_SeguimientoUsuario(SeguimientoUsuario a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_SeguimientoUsuario(a);
            return tabla;
        }
        public DataTable SP_AdministrarDiasCredito(DiasCreditoCliente a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_AdministrarDiasCredito(a);
            return tabla;
        }
        public DataTable SP_ClientesCostos(ClienteCosto a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ClientesCostos(a);
            return tabla;
        }
        public DataTable SP_BodegasContadoresENAC(BodegaDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_BodegasContadoresENAC(a);
            return tabla;
        }
        public DataTable SP_BodegasContadoresINTER(BodegaDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_BodegasContadoresINTER(a);
            return tabla;
        }
        public DataTable SP_FacturasENAC(FacturaDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_FacturasENAC(a);
            return tabla;
        }
        public DataTable SP_FactorDolar50(FactorDolar50DTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_FactorDolar50(a);
            return tabla;
        }
        public DataTable SP_Perfiles(PerfilDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_Perfiles(a);
            return tabla;
        }
        public DataTable SP_PerfilPermisos(PerfilPermisoDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_PerfilPermisos(a);
            return tabla;
        }
        public DataTable SP_PerfilPermisosExtra(PerfilPermisosExtraDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_PerfilPermisosExtra(a);
            return tabla;
        }
        public DataTable SP_Modulos(ModulosDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_Modulos(a);
            return tabla;
        }
        public DataTable SP_PerfilModulos(PerfilModuloDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_PerfilModulos(a);
            return tabla;
        }
        public DataTable SP_MenuDinamico(MenuDinamicoDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_MenuDinamico(a);
            return tabla;
        }
        public DataTable SP_CiudadesENAC(CiudadesENAC a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CiudadesENAC(a);
            return tabla;
        }
        public DataTable SP_ProductosENAC(ProductosENAC a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ProductosENAC(a);
            return tabla;
        }
        public DataTable SP_ProductoCostosENAC(ProductoCostosENAC a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_ProductoCostosENAC(a);
            return tabla;
        }
        public DataTable SP_CotizacionEncabezado(CotizacionEncabezadoDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CotizacionEncabezado(a);
            return tabla;
        }
        public DataTable SP_CotizacionDetalle(CotizacionDetalleDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CotizacionDetalle(a);
            return tabla;
        }
        public DataTable SP_CotizacionTerminos(CotizacionTerminoDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CotizacionTerminos(a);
            return tabla;
        }
        public DataTable SP_CotizacionTipo(CotizacionTipoDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CotizacionTipo(a);
            return tabla;
        }
        public DataTable SP_CotizacionImpuesto(CotizacionImpuestoDTO a)
        {
            DataTable tabla = new DataTable();
            tabla = enviar.SP_CotizacionImpuesto(a);
            return tabla;
        }
    }
}
