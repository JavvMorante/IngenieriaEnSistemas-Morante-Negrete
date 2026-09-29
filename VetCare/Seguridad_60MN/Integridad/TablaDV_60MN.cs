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

        /// <summary>Tablas del modelo de permisos Usuario - Familia - Patente.</summary>
        public static readonly string[] TablasDePermisos =
            { Patente, Familia, FamiliaPatente, FamiliaFamilia, UsuarioFamilia, UsuarioPatente };

        public static readonly IReadOnlyList<TablaDV_60MN> Todas = new List<TablaDV_60MN>
        {
            new TablaDV_60MN(Usuario, new[] { "UsuarioID" },
                "UsuarioID", "Usuario", "Clave", "Nombre", "Apellido", "DNI", "Email",
                "Activo", "Bloqueado", "IntentosFallidos", "PrimerIngreso", "EnSesion"),
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
                "PrecioCosto", "PrecioVenta", "FechaVencimiento", "Lote", "Activo"),
            new TablaDV_60MN(MovimientoStock, new[] { "MovimientoID" },
                "MovimientoID", "ProductoID", "Tipo", "Cantidad", "StockAnterior", "StockResultante",
                "Motivo", "FechaHora", "UsuarioID"),
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
