using System.Globalization;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>Diferencia entre dos versiones de un producto (CUS10, flujo 6.1).</summary>
    public class DiferenciaVersion_60MN
    {
        public string Campo { get; set; } = string.Empty;

        public string VersionA { get; set; } = string.Empty;

        public string VersionB { get; set; } = string.Empty;

        public bool Cambio => VersionA != VersionB;
    }

    /// <summary>CUS10 Consultar Bitácora de Cambios (tabla Productos_C alimentada por trigger).</summary>
    public class BitacoraCambiosBLL_60MN
    {
        private readonly BitacoraCambiosDAL_60MN mapper = new BitacoraCambiosDAL_60MN();

        public List<ProductoHistorial_60MN> Consultar(DateTime? desde, DateTime? hasta, string? codigo, string? nombre)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.BitacoraCambios);
            if (desde.HasValue && hasta.HasValue && desde.Value.Date > hasta.Value.Date)
                throw new NegocioException_60MN("El rango de fechas es inválido: la fecha de inicio es posterior a la de fin.");

            List<ProductoHistorial_60MN> resultado = mapper.Consultar(desde, hasta, codigo, nombre);
            new BitacoraBLL_60MN().Registrar(EventoSistema_60MN.BitacoraCambiosConsultada,
                $"Consulta de bitácora de cambios de productos ({resultado.Count} registros).");
            return resultado;
        }

        public List<DiferenciaVersion_60MN> Comparar(ProductoHistorial_60MN a, ProductoHistorial_60MN b)
        {
            CultureInfo ar = CultureInfo.GetCultureInfo("es-AR");
            DiferenciaVersion_60MN Fila(string campo, Func<ProductoHistorial_60MN, string> valor) =>
                new DiferenciaVersion_60MN { Campo = campo, VersionA = valor(a), VersionB = valor(b) };

            return new List<DiferenciaVersion_60MN>
            {
                Fila("Fecha y hora", v => v.FechaHora.ToString("dd/MM/yyyy HH:mm:ss")),
                Fila("Operación", v => v.Operacion),
                Fila("Usuario", v => v.Usuario),
                Fila("Nombre", v => v.Nombre),
                Fila("Categoría", v => v.Categoria),
                Fila("Proveedor", v => v.Proveedor),
                Fila("Stock actual", v => v.StockActual.ToString()),
                Fila("Stock mínimo", v => v.StockMinimo.ToString()),
                Fila("Precio de costo", v => v.PrecioCosto.ToString("C2", ar)),
                Fila("Precio de venta", v => v.PrecioVenta.ToString("C2", ar)),
                Fila("Vencimiento", v => v.FechaVencimiento.ToString("dd/MM/yyyy")),
                Fila("Lote", v => v.Lote),
                Fila("Activo", v => v.ActivoProducto ? "Sí" : "No"),
                Fila("Vigente (Act)", v => v.Act ? "1" : "0"),
            };
        }
    }
}
