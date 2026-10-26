namespace Entidades_60MN
{
    /// <summary>Archivo de backup de la base de datos (CUS12 Gestión de Backup).</summary>
    public class RespaldoBD_60MN
    {
        /// <summary>Ruta completa del archivo .bak.</summary>
        public string Ruta { get; set; } = string.Empty;

        public string Archivo => Path.GetFileName(Ruta);

        /// <summary>Base de datos contenida en el archivo (cabecera del backup).</summary>
        public string BaseDatos { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public long TamanioBytes { get; set; }

        public string Tamanio => TamanioBytes >= 1024 * 1024
            ? $"{TamanioBytes / 1024d / 1024d:0.0} MB"
            : $"{TamanioBytes / 1024d:0} KB";

        /// <summary>Login de SQL Server que generó el backup.</summary>
        public string Usuario { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

        /// <summary>True si es un backup completo de base de datos.</summary>
        public bool TipoCompleto { get; set; } = true;

        /// <summary>True si RESTORE VERIFYONLY con checksum fue correcto.</summary>
        public bool Verificado { get; set; }
    }
}
