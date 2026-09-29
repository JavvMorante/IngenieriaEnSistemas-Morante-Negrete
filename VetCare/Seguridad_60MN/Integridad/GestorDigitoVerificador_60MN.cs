using System;
using System.Collections.Generic;
using System.Linq;
using Seguridad_60MN.Configuracion;

namespace Seguridad_60MN.Integridad
{
    /// <summary>Una diferencia detectada al verificar los dígitos verificadores.</summary>
    public sealed class InconsistenciaDV_60MN
    {
        public string Tabla { get; set; }

        /// <summary>"DVH" (fila alterada) o "DVV" (filas agregadas/eliminadas o DVH alterado).</summary>
        public string Tipo { get; set; }

        public string Registro { get; set; }

        public string Detalle { get; set; }
    }

    /// <summary>
    /// Calcula, persiste y verifica los dígitos verificadores horizontales
    /// (DVH, uno por fila) y verticales (DVV, uno por tabla en la tabla DVV)
    /// de todas las tablas protegidas.
    /// </summary>
    public sealed class GestorDigitoVerificador_60MN
    {
        private readonly DigitoVerificadorDAL_60MN mapper = new DigitoVerificadorDAL_60MN();
        private readonly DigitoVerificadorCalculadora_60MN calculadora =
            new DigitoVerificadorCalculadora_60MN(ConfiguracionSeguridad_60MN.ObtenerClaveDigitoVerificador());

        /// <summary>
        /// Recalcula el DVH de una fila recién insertada/modificada por el sistema
        /// y el DVV de su tabla.
        ///
        /// El DVV solo se actualiza si la tabla estaba consistente ANTES del
        /// cambio: los DVH guardados distintos de NULL representan el estado
        /// previo (una fila recién insertada todavía no tiene DVH y una fila
        /// modificada conserva el anterior). Si ese estado previo no coincide
        /// con el DVV guardado, alguien alteró la tabla por fuera del sistema
        /// y se deja el DVV como está para que la inconsistencia se siga
        /// detectando, en lugar de "taparla" con esta escritura legítima.
        /// </summary>
        public void ActualizarFila(string tabla, params object[] clave)
        {
            TablaDV_60MN definicion = TablaDV_60MN.Obtener(tabla);

            List<string> dvhPrevios = mapper.LeerDVHs(definicion);
            bool estabaConsistente = mapper.LeerDVV(definicion.Nombre) ==
                                     calculadora.CalcularDVV(definicion.Nombre, dvhPrevios.Where(d => d != null));

            FilaDV_60MN fila = mapper.LeerFila(definicion, clave);
            if (fila != null)
                mapper.ActualizarDVH(definicion, clave, calculadora.CalcularDVH(definicion.Nombre, fila.Valores));

            if (estabaConsistente)
                ActualizarDVV(definicion);
        }

        /// <summary>True si todos los DVH y el DVV de la tabla son correctos.</summary>
        public bool TablaConsistente(string tabla) => VerificarTabla(TablaDV_60MN.Obtener(tabla)).Count == 0;

        /// <summary>Recalcula todos los DVH y el DVV de una tabla.</summary>
        public void RecalcularTabla(string tabla)
        {
            TablaDV_60MN definicion = TablaDV_60MN.Obtener(tabla);
            List<string> digitos = new List<string>();
            foreach (FilaDV_60MN fila in mapper.LeerFilas(definicion))
            {
                string dvh = calculadora.CalcularDVH(definicion.Nombre, fila.Valores);
                if (dvh != fila.DVHGuardado)
                    mapper.ActualizarDVH(definicion, fila.ValoresClave, dvh);
                digitos.Add(dvh);
            }
            mapper.GuardarDVV(definicion.Nombre, calculadora.CalcularDVV(definicion.Nombre, digitos));
        }

        public void RecalcularTodo()
        {
            foreach (TablaDV_60MN tabla in TablaDV_60MN.Todas)
                RecalcularTabla(tabla.Nombre);
        }

        /// <summary>Verifica todas las tablas y devuelve la lista de inconsistencias (vacía si la base está íntegra).</summary>
        public List<InconsistenciaDV_60MN> VerificarTodo()
        {
            List<InconsistenciaDV_60MN> resultado = new List<InconsistenciaDV_60MN>();
            foreach (TablaDV_60MN tabla in TablaDV_60MN.Todas)
                resultado.AddRange(VerificarTabla(tabla));
            return resultado;
        }

        /// <summary>True si el DVH guardado de esa fila coincide con sus datos actuales.</summary>
        public bool VerificarFila(string tabla, params object[] clave)
        {
            TablaDV_60MN definicion = TablaDV_60MN.Obtener(tabla);
            FilaDV_60MN fila = mapper.LeerFila(definicion, clave);
            return fila != null && calculadora.CalcularDVH(definicion.Nombre, fila.Valores) == fila.DVHGuardado;
        }

        private List<InconsistenciaDV_60MN> VerificarTabla(TablaDV_60MN tabla)
        {
            List<InconsistenciaDV_60MN> resultado = new List<InconsistenciaDV_60MN>();
            List<string> digitosGuardados = new List<string>();

            foreach (FilaDV_60MN fila in mapper.LeerFilas(tabla))
            {
                digitosGuardados.Add(fila.DVHGuardado);
                if (calculadora.CalcularDVH(tabla.Nombre, fila.Valores) != fila.DVHGuardado)
                    resultado.Add(new InconsistenciaDV_60MN
                    {
                        Tabla = tabla.Nombre,
                        Tipo = "DVH",
                        Registro = fila.ClaveTexto,
                        Detalle = "El contenido de la fila no coincide con su dígito verificador horizontal."
                    });
            }

            string dvvGuardado = mapper.LeerDVV(tabla.Nombre);
            string dvvCalculado = calculadora.CalcularDVV(tabla.Nombre, digitosGuardados);
            if (dvvGuardado != dvvCalculado)
                resultado.Add(new InconsistenciaDV_60MN
                {
                    Tabla = tabla.Nombre,
                    Tipo = "DVV",
                    Registro = "(tabla completa)",
                    Detalle = dvvGuardado == null
                        ? "No existe dígito verificador vertical registrado para la tabla."
                        : "Se agregaron, eliminaron o alteraron filas por fuera del sistema."
                });

            return resultado;
        }

        private void ActualizarDVV(TablaDV_60MN tabla)
        {
            mapper.GuardarDVV(tabla.Nombre, calculadora.CalcularDVV(tabla.Nombre, mapper.LeerDVHs(tabla)));
        }

        /// <summary>Cálculo sin base de datos, usado por el generador del script inicial.</summary>
        public string CalcularDVH(string tabla, params object[] valores) => calculadora.CalcularDVH(tabla, valores);

        public string CalcularDVV(string tabla, IEnumerable<string> dvhs) => calculadora.CalcularDVV(tabla, dvhs);
    }
}
