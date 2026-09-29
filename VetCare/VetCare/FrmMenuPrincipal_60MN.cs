using System.Globalization;
using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Idioma;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>
    /// Menú principal horizontal (ADMIN, MAESTROS, USUARIO, VENTAS, COMPRAS,
    /// REPORTES, AYUDA). Los ítems están definidos en la vista de diseño y cada
    /// uno lleva en la propiedad Tag el código de la patente que lo habilita:
    /// se muestran solo los que otorga la familia/patentes del usuario, y un
    /// menú sin opciones visibles se oculta completo.
    /// </summary>
    public partial class FrmMenuPrincipal_60MN : FormBase_60MN
    {
        private readonly UsuarioBLL_60MN usuarioBLL = new UsuarioBLL_60MN();

        // Evita pedir una segunda confirmación al cerrar cuando ya se confirmó Re-Login o Logout.
        private bool salidaConfirmada;

        /// <summary>True si se cerró por Re-Login (volver al Login) y no por salir de la aplicación.</summary>
        public bool SesionCerrada { get; private set; }

        public FrmMenuPrincipal_60MN()
        {
            InitializeComponent();
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmMenuPrincipal_60MN_Load(object sender, EventArgs e)
        {
            AplicarPermisosMenu();

            SessionManager_60MN sesion = SessionManager_60MN.Instancia;
            if (sesion.IntegridadComprometida && sesion.TienePermiso(mnuDigitosVerificadores.Tag as string ?? string.Empty))
                Abrir<FrmIntegridad_60MN>();
        }

        /// <summary>Textos de la barra de estado (los arma el código, por eso se re-traducen aquí).</summary>
        protected override void AlCambiarIdioma()
        {
            SessionManager_60MN sesion = SessionManager_60MN.Instancia;
            string familias = sesion.Usuario is Usuario_60MN u
                ? u.Familias + (u.PatentesIndividuales > 0 ? $" + {u.PatentesIndividuales} {T("estado.PatentesIndividuales", "patente(s) individual(es)")}" : string.Empty)
                : string.Empty;
            lblEstadoUsuario.Text = $"{T("estado.Usuario", "Usuario")}: {sesion.Usuario?.NombreUsuario} ({sesion.Usuario?.Apellido}, {sesion.Usuario?.Nombre})";
            lblEstadoFamilias.Text = $"{T("estado.Familias", "Familias")}: {familias}";
            lblEstadoIdioma.Text = $"{T("estado.Idioma", "Idioma")}: {GestorIdioma_60MN.Instancia.IdiomaActual?.Nombre}";

            string codigoIdioma = GestorIdioma_60MN.Instancia.IdiomaActual?.Codigo ?? "es";
            CultureInfo cultura = codigoIdioma == "en" ? CultureInfo.GetCultureInfo("en-US") : CultureInfo.GetCultureInfo("es-AR");
            lblEstadoFecha.Text = DateTime.Now.ToString("dddd dd/MM/yyyy", cultura);

            if (sesion.EsEmergencia)
                Text += $"  [{T("estado.Emergencia", "USUARIO DE EMERGENCIA")}]";
            ActualizarEstadoIntegridad();
        }

        public void ActualizarEstadoIntegridad()
        {
            lblEstadoIntegridad.Text = SessionManager_60MN.Instancia.IntegridadComprometida
                ? "⚠ " + T("estado.Integridad", "Inconsistencias de integridad detectadas")
                : string.Empty;
        }

        /// <summary>
        /// Muestra cada ítem solo si el usuario tiene alguna de las patentes de
        /// su Tag (ítems sin Tag, como los de USUARIO, están siempre disponibles).
        /// </summary>
        private void AplicarPermisosMenu()
        {
            foreach (ToolStripMenuItem menu in menuPrincipal.Items.OfType<ToolStripMenuItem>())
            {
                bool algunoVisible = false;
                foreach (ToolStripMenuItem item in menu.DropDownItems.OfType<ToolStripMenuItem>())
                {
                    bool visible = !(item.Tag is string codigos && codigos.Length > 0) || Mensajes_60MN.TieneAlguno(codigos);
                    item.Visible = visible;
                    algunoVisible |= visible;
                }
                menu.Visible = algunoVisible;
            }
        }

        /// <summary>Abre un formulario como hijo MDI, o lo activa si ya estaba abierto.</summary>
        private T Abrir<T>() where T : Form, new()
        {
            T? abierto = MdiChildren.OfType<T>().FirstOrDefault();
            if (abierto != null)
            {
                if (abierto.WindowState == FormWindowState.Minimized)
                    abierto.WindowState = FormWindowState.Normal;
                abierto.Activate();
                return abierto;
            }
            T formulario = new T { MdiParent = this };
            formulario.Show();
            return formulario;
        }

        // ---------------- ADMIN ----------------
        private void mnuUsuarios_Click(object sender, EventArgs e) => Abrir<FrmGestionUsuarios_60MN>();

        private void mnuPerfiles_Click(object sender, EventArgs e) => Abrir<FrmFamiliasPatentes_60MN>();

        private void mnuBitacoraEventos_Click(object sender, EventArgs e) => Abrir<FrmBitacoraEventos_60MN>();

        private void mnuDigitosVerificadores_Click(object sender, EventArgs e) => Abrir<FrmIntegridad_60MN>();

        // ---------------- MAESTROS ----------------
        private void mnuProductos_Click(object sender, EventArgs e) => Abrir<FrmProductos_60MN>();

        private void mnuMovimientosStock_Click(object sender, EventArgs e) => Abrir<FrmMovimientoStock_60MN>();

        private void mnuBitacoraCambios_Click(object sender, EventArgs e) => Abrir<FrmBitacoraCambios_60MN>();

        // ---------------- USUARIO ----------------

        /// <summary>Re-Login (CUS06): cierra la sesión y vuelve a la pantalla de Login.</summary>
        private void mnuReLogin_Click(object sender, EventArgs e)
        {
            if (!ConfirmarCierre(T("msg.ConfirmarReLogin", "¿Desea cerrar la sesión y volver a iniciar sesión?")))
                return;
            SesionCerrada = true;
            Close();
        }

        /// <summary>Logout: cierra la sesión (queda registrada en la bitácora) y sale de la aplicación.</summary>
        private void mnuLogout_Click(object sender, EventArgs e)
        {
            if (!ConfirmarCierre(T("msg.ConfirmarLogout", "¿Desea cerrar la sesión y salir de VetCare?")))
                return;
            SesionCerrada = false;
            Close();
        }

        /// <summary>CUS06 pasos 2 y 4.1: confirmación y advertencia de ventanas abiertas.</summary>
        private bool ConfirmarCierre(string pregunta)
        {
            if (!Mensajes_60MN.Confirmar(pregunta))
                return false;
            if (MdiChildren.Length > 0 &&
                !Mensajes_60MN.Confirmar(string.Format(T("msg.VentanasAbiertas",
                    "Hay {0} ventana(s) abiertas. Las operaciones sin guardar se perderán. ¿Continuar?"), MdiChildren.Length)))
                return false;
            salidaConfirmada = true;
            return true;
        }

        private void mnuCambiarClave_Click(object sender, EventArgs e)
        {
            if (SessionManager_60MN.Instancia.EsEmergencia)
            {
                Mensajes_60MN.Advertir("La contraseña del usuario de emergencia no se administra desde el sistema.");
                return;
            }
            using FrmCambiarClave_60MN formulario = new FrmCambiarClave_60MN();
            formulario.ShowDialog(this);
        }

        private void mnuCambiarIdioma_Click(object sender, EventArgs e)
        {
            using FrmCambiarIdioma_60MN formulario = new FrmCambiarIdioma_60MN();
            formulario.ShowDialog(this);
        }

        // ---------------- VENTAS / COMPRAS / REPORTES / AYUDA ----------------

        /// <summary>Opciones del menú todavía no desarrolladas ("TD").</summary>
        private void mnuPendiente_Click(object sender, EventArgs e)
        {
            string opcion = (sender as ToolStripItem)?.Text ?? string.Empty;
            Mensajes_60MN.Informar($"{opcion}: {T("msg.EnDesarrollo", "Funcionalidad en desarrollo (TD).")}");
        }

        private void mnuAcercaDe_Click(object sender, EventArgs e)
        {
            Mensajes_60MN.Informar("VetCare - Sistema de gestión integral para veterinarias.\n\n" +
                                   "Trabajo de diploma - Ingeniería en Sistemas.\nMorante Javier - Negrete Mario.");
        }

        private void FrmMenuPrincipal_60MN_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!salidaConfirmada && e.CloseReason == CloseReason.UserClosing &&
                !Mensajes_60MN.Confirmar(T("msg.ConfirmarSalir", "¿Desea salir de VetCare?")))
            {
                e.Cancel = true;
                return;
            }

            foreach (Form hijo in MdiChildren)
                hijo.Close();

            // En cualquier caso la sesión se cierra y queda registrada en la bitácora (CUS06).
            if (!usuarioBLL.Logout())
                Mensajes_60MN.Advertir("La sesión se cerró, pero no se pudo registrar el evento en la bitácora.");
        }
    }
}
