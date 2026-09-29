using System;
using System.IO;
using System.Xml.Linq;
using Seguridad_60MN.Criptografia;
using Seguridad_60MN.Integridad;

namespace Seguridad_60MN.Emergencia
{
    /// <summary>
    /// Usuario de emergencia guardado FUERA de la base de datos, en un archivo
    /// XML junto al ejecutable. Permite recuperar la administración del
    /// sistema si se borró o inutilizó al último administrador en la base.
    ///
    /// Protecciones:
    ///  - La contraseña está guardada con el mismo hash SHA-256 + sal que el
    ///    resto de los usuarios (no está en claro en el XML).
    ///  - El archivo lleva un DVH (HMAC-SHA256 con la clave VETCARE_CLAVE_DV):
    ///    si alguien edita el XML a mano, la firma deja de validar y el
    ///    usuario de emergencia queda inutilizable.
    ///  - La BLL solo lo acepta cuando NO existe un administrador operativo
    ///    en la base (activo, no bloqueado y con su registro íntegro).
    /// </summary>
    public sealed class UsuarioEmergencia_60MN
    {
        public const string NombreArchivo = "UsuarioEmergencia_60MN.xml";
        private const string TablaFirma = "UsuarioEmergencia";

        public string Usuario { get; private set; }

        public string Nombre { get; private set; }

        public string Apellido { get; private set; }

        private string ClaveHash { get; set; }

        public static string RutaPorDefecto => Path.Combine(AppContext.BaseDirectory, NombreArchivo);

        /// <summary>Lee y valida la firma del XML. Devuelve null si el archivo no existe.</summary>
        public static UsuarioEmergencia_60MN Cargar(string ruta = null)
        {
            ruta ??= RutaPorDefecto;
            if (!File.Exists(ruta)) return null;

            XElement raiz = XDocument.Load(ruta).Root;
            if (raiz == null) throw new InvalidDataException("El archivo del usuario de emergencia está vacío.");

            UsuarioEmergencia_60MN usuario = new UsuarioEmergencia_60MN
            {
                Usuario = (string)raiz.Element("Usuario") ?? string.Empty,
                Nombre = (string)raiz.Element("Nombre") ?? string.Empty,
                Apellido = (string)raiz.Element("Apellido") ?? string.Empty,
                ClaveHash = (string)raiz.Element("ClaveHash") ?? string.Empty
            };

            string dvhGuardado = (string)raiz.Element("DVH");
            if (dvhGuardado != usuario.CalcularFirma(new GestorDigitoVerificador_60MN()))
                throw new InvalidDataException("El archivo del usuario de emergencia fue alterado (firma inválida).");

            return usuario;
        }

        public bool EsUsuario(string nombreUsuario) =>
            string.Equals(Usuario, nombreUsuario?.Trim(), StringComparison.OrdinalIgnoreCase);

        public bool VerificarClave(string clave) => PasswordHasher_60MN.Verificar(clave, ClaveHash);

        /// <summary>Genera (o regenera) el archivo XML firmado. Lo usa el generador del script inicial.</summary>
        public static void Generar(string ruta, string usuario, string nombre, string apellido, string clave, GestorDigitoVerificador_60MN gestorDV)
        {
            UsuarioEmergencia_60MN datos = new UsuarioEmergencia_60MN
            {
                Usuario = usuario,
                Nombre = nombre,
                Apellido = apellido,
                ClaveHash = PasswordHasher_60MN.Hashear(clave)
            };

            new XDocument(
                new XComment(" Usuario de emergencia de VetCare. NO editar: el DVH invalida el archivo ante cualquier cambio. "),
                new XElement("UsuarioEmergencia_60MN",
                    new XElement("Usuario", datos.Usuario),
                    new XElement("Nombre", datos.Nombre),
                    new XElement("Apellido", datos.Apellido),
                    new XElement("ClaveHash", datos.ClaveHash),
                    new XElement("DVH", datos.CalcularFirma(gestorDV))))
                .Save(ruta);
        }

        private string CalcularFirma(GestorDigitoVerificador_60MN gestorDV) =>
            gestorDV.CalcularDVH(TablaFirma, Usuario, Nombre, Apellido, ClaveHash);
    }
}
