using System;
using System.Collections.Generic;
using System.Linq;

namespace Seguridad_60MN.Integridad
{
    /// <summary>
    /// Describe una tabla protegida por dígitos verificadores: qué columnas
    /// entran en el DVH y cuáles forman la clave primaria (siempre las
    /// primeras de la lista). Los nombres son fijos del sistema, nunca
    /// provienen de lo que escribe el usuario.
    /// </summary>
    public sealed class TablaDV_60MN
    {
        public string Nombre { get; }

        public string[] ClavePrimaria { get; }

        public string[] Columnas { get; }

        private TablaDV_60MN(string nombre, string[] clavePrimaria, params string[] columnas)
        {
            Nombre = nombre;
            ClavePrimaria = clavePrimaria;
            Columnas = columnas;
        }

        public const string Usuario = "Usuario";
        public const string Patente = "Patente";
        public const string Familia = "Familia";
        public const string FamiliaPatente = "FamiliaPatente";
        public const string FamiliaFamilia = "FamiliaFamilia";
        public const string UsuarioFamilia = "UsuarioFamilia";
        public const string UsuarioPatente = "UsuarioPatente";
        public const string Bitacora = "Bitacora";
        public const string Productos = "Productos";
        public const string MovimientoStock = "MovimientoStock";
        public const string Cliente = "Cliente";
        public const string Mascota = "Mascota";
        public const string HistoriaClinica = "HistoriaClinica";
        public const string Atencion = "Atencion";
        public const string Proveedor = "Proveedor";
        public const string VentaEncabezado = "VentaEncabezado";
        public const string VentaDetalle = "VentaDetalle";
        public const string SolicitudReposicion = "SolicitudReposicion";
        public const string SolicitudReposicionDetalle = "SolicitudReposicionDetalle";
        public const string OrdenCompra = "OrdenCompra";
        public const string OrdenCompraDetalle = "OrdenCompraDetalle";
        public const string PagoProveedor = "PagoProveedor";
        public const string Recepcion = "Recepcion";
        public const string RecepcionDetalle = "RecepcionDetalle";

        /// <summary>Tablas del modelo de permisos Usuario - Familia - Patente.</summary>
        public static readonly string[] TablasDePermisos =
            { Patente, Familia, FamiliaPatente, FamiliaFamilia, UsuarioFamilia, UsuarioPatente };

        public static readonly IReadOnlyList<TablaDV_60MN> Todas = new List<TablaDV_60MN>
        {
            new TablaDV_60MN(Usuario, new[] { "UsuarioID" },
                "UsuarioID", "Usuario", "Clave", "Nombre", "Apellido", "DNI", "Email",
                "Activo", "Bloqueado", "IntentosFallidos", "PrimerIngreso", "EnSesion", "IdiomaID"),
            new TablaDV_60MN(Patente, new[] { "PatenteID" },
                "PatenteID", "Nombre", "Codigo"),
            new TablaDV_60MN(Familia, new[] { "FamiliaID" },
                "FamiliaID", "Nombre"),
            new TablaDV_60MN(FamiliaPatente, new[] { "FamiliaID", "PatenteID" },
                "FamiliaID", "PatenteID"),
            new TablaDV_60MN(FamiliaFamilia, new[] { "FamiliaPadreID", "FamiliaHijaID" },
                "FamiliaPadreID", "FamiliaHijaID"),
            new TablaDV_60MN(UsuarioFamilia, new[] { "UsuarioID", "FamiliaID" },
                "UsuarioID", "FamiliaID"),
            new TablaDV_60MN(UsuarioPatente, new[] { "UsuarioID", "PatenteID" },
                "UsuarioID", "PatenteID"),
            new TablaDV_60MN(Bitacora, new[] { "BitacoraID" },
                "BitacoraID", "UsuarioID", "Usuario", "FechaHora", "Modulo", "Evento", "Criticidad", "Descripcion"),
            new TablaDV_60MN(Productos, new[] { "ProductoID" },
                "ProductoID", "Codigo", "Nombre", "CategoriaID", "ProveedorID", "StockActual", "StockMinimo",
                "PrecioCosto", "PrecioVenta", "Margen", "FechaVencimiento", "Lote", "Activo"),
            new TablaDV_60MN(MovimientoStock, new[] { "MovimientoID" },
                "MovimientoID", "ProductoID", "Tipo", "Cantidad", "StockAnterior", "StockResultante",
                "Motivo", "FechaHora", "UsuarioID"),
            new TablaDV_60MN(Cliente, new[] { "ClienteID" },
                "ClienteID", "TipoDocumento", "NroDocumento", "Nombre", "Apellido", "Direccion", "Telefono", "Email", "Activo"),
            new TablaDV_60MN(Mascota, new[] { "MascotaID" },
                "MascotaID", "ClienteID", "Nombre", "Especie", "Raza", "Sexo", "FechaNacimiento", "Activo"),
            new TablaDV_60MN(HistoriaClinica, new[] { "HistoriaClinicaID" },
                "HistoriaClinicaID", "Numero", "MascotaID", "FechaApertura", "PesoActual", "Alergias", "Observaciones"),
            new TablaDV_60MN(Atencion, new[] { "AtencionID" },
                "AtencionID", "HistoriaClinicaID", "FechaHora", "UsuarioID", "Profesional", "Peso", "Temperatura",
                "FrecuenciaCardiaca", "Motivo", "Diagnostico", "InformesEmitidos", "Indicaciones"),
            new TablaDV_60MN(Proveedor, new[] { "ProveedorID" },
                "ProveedorID", "RazonSocial", "CUIT", "Telefono", "Email", "Direccion", "Contacto", "Activo"),
            new TablaDV_60MN(VentaEncabezado, new[] { "VentaID" },
                "VentaID", "ClienteID", "AtencionID", "FechaHora", "UsuarioID", "Total"),
            new TablaDV_60MN(VentaDetalle, new[] { "VentaDetalleID" },
                "VentaDetalleID", "VentaID", "ProductoID", "Cantidad", "PrecioUnitario", "Subtotal", "MovimientoID"),
            new TablaDV_60MN(SolicitudReposicion, new[] { "SolicitudID" },
                "SolicitudID", "FechaHora", "UsuarioID", "Estado", "Observaciones"),
            new TablaDV_60MN(SolicitudReposicionDetalle, new[] { "SolicitudDetalleID" },
                "SolicitudDetalleID", "SolicitudID", "ProductoID", "StockActual", "StockMinimo", "CantidadPedida"),
            new TablaDV_60MN(OrdenCompra, new[] { "OrdenCompraID" },
                "OrdenCompraID", "SolicitudID", "ProveedorID", "FechaHora", "NroCotizacion", "PlazoEntregaDias",
                "CondicionPago", "MotivoEleccion", "Advertencias", "Total", "Estado", "UsuarioID", "AprobadaPor",
                "FechaAprobacion", "MotivoRechazo"),
            new TablaDV_60MN(OrdenCompraDetalle, new[] { "OrdenDetalleID" },
                "OrdenDetalleID", "OrdenCompraID", "ProductoID", "Cantidad", "PrecioUnitario", "Subtotal", "CantidadRecibida"),
            new TablaDV_60MN(PagoProveedor, new[] { "PagoID" },
                "PagoID", "OrdenCompraID", "Banco", "NroComprobante", "FechaPago", "Importe", "Observaciones", "UsuarioID"),
            new TablaDV_60MN(Recepcion, new[] { "RecepcionID" },
                "RecepcionID", "OrdenCompraID", "NroRemito", "FechaHora", "UsuarioID", "Observaciones"),
            new TablaDV_60MN(RecepcionDetalle, new[] { "RecepcionDetalleID" },
                "RecepcionDetalleID", "RecepcionID", "ProductoID", "CantidadRecibida", "Lote", "FechaVencimiento",
                "Observacion", "MovimientoID"),
        };

        public static TablaDV_60MN Obtener(string nombre)
        {
            TablaDV_60MN tabla = Todas.FirstOrDefault(t => string.Equals(t.Nombre, nombre, StringComparison.OrdinalIgnoreCase));
            if (tabla == null)
                throw new ArgumentOutOfRangeException(nameof(nombre), "Tabla no protegida por dígitos verificadores: " + nombre);
            return tabla;
        }

        internal string OrdenPorClave => string.Join(", ", ClavePrimaria);
    }
}
