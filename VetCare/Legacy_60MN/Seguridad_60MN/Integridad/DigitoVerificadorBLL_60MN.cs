using System;
using System.Collections.Generic;
using Seguridad_60MN.Configuracion;

namespace Seguridad_60MN.Integridad
{
    /// <summary>
    /// Reemplaza a DAL_60MN.DigitosVerificadores_60MN. Calcula y guarda los
    /// digitos verificadores (DVH por fila, DVV por tabla) usando
    /// HMAC-SHA256 en vez de la suma ponderada de bytes anterior.
    ///
    /// Tambien corrige dos problemas puntuales que tenia la version vieja:
    ///  - RecalcularDVH() lanzaba un Thread suelto (fire-and-forget) sin
    ///    esperarlo ni loguear si fallaba. Aca es sincronico: se llama y
    ///    se sabe si termino bien o exploto.
    ///  - El bug de "item[1].ToString" sin parentesis en el foreach de
    ///    Bitacora (que no compilaba) desaparece porque ahora se usan
    ///    objetos tipados (BitacoraParaDV) en vez de leer un DataRow por indice.
    /// </summary>
    public sealed class DigitoVerificadorBLL_60MN
    {
        private static readonly string[] TablasProtegidas =
            { "Usuario", "UsuarioOperacion", "Bitacora", "PerfilUsuario", "Operacion" };

        private readonly DigitoVerificadorDAL_60MN mapper;
        private readonly DigitoVerificadorCalculadora_60MN calculadora;

        public DigitoVerificadorBLL_60MN(string cadenaConexion = null)
        {
            mapper = new DigitoVerificadorDAL_60MN(cadenaConexion);
            byte[] clave = ConfiguracionSeguridad_60MN.ObtenerClaveDigitoVerificador();
            calculadora = new DigitoVerificadorCalculadora_60MN(clave);
        }

        /// <summary>Recalcula DVH y DVV de una sola tabla. Llamar despues de cada insert/update/delete sobre esa tabla.</summary>
        public void RecalcularTabla(string tabla)
        {
            switch (tabla)
            {
                case "Usuario": RecalcularUsuario(); break;
                case "UsuarioOperacion": RecalcularUsuarioOperacion(); break;
                case "Bitacora": RecalcularBitacora(); break;
                case "PerfilUsuario": RecalcularPerfilUsuario(); break;
                case "Operacion": RecalcularOperacion(); break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tabla), "Tabla no protegida por digitos verificadores: " + tabla);
            }
        }

        /// <summary>Recalcula las cinco tablas protegidas de punta a punta (equivalente al viejo RecalcularDVH(), pero sincronico).</summary>
        public void RecalcularTodo()
        {
            foreach (string tabla in TablasProtegidas) RecalcularTabla(tabla);
        }

        private void RecalcularUsuario()
        {
            List<string> digestos = new List<string>();
            foreach (UsuarioParaDV fila in mapper.LeerUsuarios())
            {
                string dvh = calculadora.CalcularDVH("Usuario", fila.UsuarioId, fila.Usuario, fila.Clave);
                mapper.ActualizarDVHUsuario(fila.UsuarioId, dvh);
                digestos.Add(dvh);
            }
            GuardarDVV("Usuario", digestos);
        }

        private void RecalcularUsuarioOperacion()
        {
            List<string> digestos = new List<string>();
            foreach (UsuarioOperacionParaDV fila in mapper.LeerUsuarioOperacion())
            {
                string dvh = calculadora.CalcularDVH("UsuarioOperacion", fila.UsuarioId, fila.OperacionId, fila.Habilitado);
                mapper.ActualizarDVHUsuarioOperacion(fila.UsuarioId, fila.OperacionId, dvh);
                digestos.Add(dvh);
            }
            GuardarDVV("UsuarioOperacion", digestos);
        }

        private void RecalcularBitacora()
        {
            List<string> digestos = new List<string>();
            foreach (BitacoraParaDV fila in mapper.LeerBitacora())
            {
                string dvh = calculadora.CalcularDVH("Bitacora", fila.BitacoraId, fila.UsuarioId, fila.FechaYHora);
                mapper.ActualizarDVHBitacora(fila.BitacoraId, dvh);
                digestos.Add(dvh);
            }
            GuardarDVV("Bitacora", digestos);
        }

        private void RecalcularPerfilUsuario()
        {
            List<string> digestos = new List<string>();
            foreach (PerfilUsuarioParaDV fila in mapper.LeerPerfilUsuario())
            {
                string dvh = calculadora.CalcularDVH("PerfilUsuario", fila.PerfilUsuarioId, fila.NombrePerfil, fila.DescPerfil);
                mapper.ActualizarDVHPerfilUsuario(fila.PerfilUsuarioId, dvh);
                digestos.Add(dvh);
            }
            GuardarDVV("PerfilUsuario", digestos);
        }

        private void RecalcularOperacion()
        {
            List<string> digestos = new List<string>();
            foreach (OperacionParaDV fila in mapper.LeerOperacion())
            {
                string dvh = calculadora.CalcularDVH("Operacion", fila.OperacionId, fila.Descripcion, fila.PatenteEscencial);
                mapper.ActualizarDVHOperacion(fila.OperacionId, dvh);
                digestos.Add(dvh);
            }
            GuardarDVV("Operacion", digestos);
        }

        private void GuardarDVV(string tabla, List<string> digestosHorizontales)
        {
            string dvv = calculadora.CalcularDVV(tabla, digestosHorizontales);
            mapper.GuardarDVV(tabla, dvv, calculadora.IdentificadorClave);
        }

        /// <summary>
        /// Verifica que el DVV guardado en la base coincida con el
        /// recalculado a partir de los DVH actuales de la tabla. Si no
        /// coincide, hubo una edicion directa en la base que no paso por
        /// la aplicacion (o la clave de firma cambio).
        /// </summary>
        public bool VerificarTabla(string tabla, IEnumerable<string> digestosHorizontalesActuales)
        {
            DVVGuardado guardado = mapper.LeerDVV(tabla);
            if (guardado == null) return false;
            if (!string.Equals(guardado.ClaveId, calculadora.IdentificadorClave, StringComparison.Ordinal))
                return false; // firmado con otra clave: no es "invalido", pero no se puede confiar sin la clave original
            string recalculado = calculadora.CalcularDVV(tabla, digestosHorizontalesActuales);
            return string.Equals(guardado.DVV, recalculado, StringComparison.Ordinal);
        }
    }
}
