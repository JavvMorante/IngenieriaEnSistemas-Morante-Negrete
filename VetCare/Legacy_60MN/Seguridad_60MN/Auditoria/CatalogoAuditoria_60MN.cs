using System;
using System.Collections.Generic;

namespace Seguridad_60MN.Auditoria
{
    /// <summary>Tipo de evento que se registra en la bitácora. Reemplaza a los strings sueltos ("Login", "Alta Usuario", etc.) que hoy se arman a mano en cada formulario.</summary>
    public enum TipoEventoBitacora_60MN
    {
        Login, LoginFallido, Logout, UsuarioBloqueado,
        UsuarioCreado, UsuarioModificado, UsuarioEliminado, ClaveModificada,
        PerfilCreado, PerfilModificado, PerfilEliminado,
        OperacionAsignada, OperacionDesasignada,
        BackupRealizado, RestoreIniciado, RestoreCompletado, RestoreFallido,
        DigitosRecalculados, BitacoraConsultada
    }

    public enum ModuloBitacora_60MN { Usuarios, Perfiles, Operaciones, Seguridad, Bitacora }

    public sealed class DefinicionEventoBitacora_60MN
    {
        public TipoEventoBitacora_60MN Tipo { get; }
        public ModuloBitacora_60MN Modulo { get; }

        /// <summary>1 = mas critico ... 5 = informativo. Mismo criterio de numeros que ya usaba el proyecto (Criticidad = 1, 2, 4, 5 sueltos en cada formulario), pero ahora declarado una sola vez.</summary>
        public int Criticidad { get; }

        internal DefinicionEventoBitacora_60MN(TipoEventoBitacora_60MN tipo, ModuloBitacora_60MN modulo, int criticidad)
        {
            Tipo = tipo;
            Modulo = modulo;
            Criticidad = criticidad;
        }
    }

    /// <summary>
    /// Reemplaza los numeros de criticidad "magicos" (1, 2, 4, 5) que
    /// estaban sueltos y repetidos en Login.cs, ModificarUsuario.cs,
    /// AltaFamilia.cs, etc. Cada evento de bitacora se declara una sola
    /// vez aca, junto con su modulo y su criticidad. Si mañana cambia el
    /// criterio de criticidad de un evento, se cambia en un solo lugar.
    /// </summary>
    public static class CatalogoAuditoria_60MN
    {
        private static readonly Dictionary<TipoEventoBitacora_60MN, DefinicionEventoBitacora_60MN> Definiciones = Construir();

        public static DefinicionEventoBitacora_60MN Obtener(TipoEventoBitacora_60MN tipo)
        {
            if (!Definiciones.TryGetValue(tipo, out DefinicionEventoBitacora_60MN definicion))
                throw new ArgumentOutOfRangeException(nameof(tipo), "No hay una definicion de auditoria para " + tipo);
            return definicion;
        }

        private static Dictionary<TipoEventoBitacora_60MN, DefinicionEventoBitacora_60MN> Construir()
        {
            Dictionary<TipoEventoBitacora_60MN, DefinicionEventoBitacora_60MN> resultado =
                new Dictionary<TipoEventoBitacora_60MN, DefinicionEventoBitacora_60MN>();

            void Agregar(ModuloBitacora_60MN modulo, int criticidad, params TipoEventoBitacora_60MN[] tipos)
            {
                foreach (TipoEventoBitacora_60MN tipo in tipos)
                    resultado.Add(tipo, new DefinicionEventoBitacora_60MN(tipo, modulo, criticidad));
            }

            Agregar(ModuloBitacora_60MN.Usuarios, 4,
                TipoEventoBitacora_60MN.Login, TipoEventoBitacora_60MN.LoginFallido, TipoEventoBitacora_60MN.Logout);
            Agregar(ModuloBitacora_60MN.Usuarios, 1,
                TipoEventoBitacora_60MN.UsuarioBloqueado, TipoEventoBitacora_60MN.UsuarioCreado,
                TipoEventoBitacora_60MN.UsuarioModificado, TipoEventoBitacora_60MN.UsuarioEliminado,
                TipoEventoBitacora_60MN.ClaveModificada);
            Agregar(ModuloBitacora_60MN.Perfiles, 2,
                TipoEventoBitacora_60MN.PerfilCreado, TipoEventoBitacora_60MN.PerfilModificado, TipoEventoBitacora_60MN.PerfilEliminado);
            Agregar(ModuloBitacora_60MN.Operaciones, 2,
                TipoEventoBitacora_60MN.OperacionAsignada, TipoEventoBitacora_60MN.OperacionDesasignada);
            Agregar(ModuloBitacora_60MN.Seguridad, 1,
                TipoEventoBitacora_60MN.BackupRealizado, TipoEventoBitacora_60MN.RestoreIniciado,
                TipoEventoBitacora_60MN.RestoreCompletado, TipoEventoBitacora_60MN.RestoreFallido,
                TipoEventoBitacora_60MN.DigitosRecalculados);
            Agregar(ModuloBitacora_60MN.Bitacora, 5, TipoEventoBitacora_60MN.BitacoraConsultada);

            return resultado;
        }
    }
}
