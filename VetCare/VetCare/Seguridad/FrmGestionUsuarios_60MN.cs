using BLL_60MN;
using Entidades_60MN;
using Servicios_60MN.Sesion;

namespace VetCare
{
    /// <summary>
    /// Módulo "Gestión de Usuarios": CUS01 Crear, CUS02 Modificar, CUS03 Dar de
    /// baja y CUS08 Desbloquear. Cada botón tiene en Tag la patente que requiere.
    /// </summary>
    public partial class FrmGestionUsuarios_60MN : FormBase_60MN
    {
        private readonly UsuarioBLL_60MN usuarioBLL = new UsuarioBLL_60MN();

        public FrmGestionUsuarios_60MN()
        {
            InitializeComponent();
            dgvUsuarios.AutoGenerateColumns = false;
        }

        private void FrmGestionUsuarios_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            CargarUsuarios();
        }

        /// <summary>Consultar Usuarios: recupera la lista y la muestra en la grilla con su estado.</summary>
        private void CargarUsuarios(int? seleccionarId = null)
        {
            try
            {
                List<Usuario_60MN> usuarios = usuarioBLL.ListarUsuarios();
                if (!chkMostrarBajas.Checked)
                    usuarios = usuarios.Where(u => u.Activo).ToList();
                dgvUsuarios.DataSource = usuarios;

                if (seleccionarId.HasValue)
                    foreach (DataGridViewRow fila in dgvUsuarios.Rows)
                        if (fila.DataBoundItem is Usuario_60MN u && u.Id == seleccionarId.Value)
                        {
                            fila.Selected = true;
                            dgvUsuarios.CurrentCell = fila.Cells[0];
                        }
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private Usuario_60MN? UsuarioSeleccionado()
        {
            if (dgvUsuarios.CurrentRow?.DataBoundItem is Usuario_60MN usuario)
                return usuario;
            Mensajes_60MN.Advertir("Debe seleccionar un usuario de la grilla.");
            return null;
        }

        private void dgvUsuarios_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvUsuarios.Rows)
                if (fila.DataBoundItem is Usuario_60MN u)
                    fila.DefaultCellStyle.BackColor = !u.Activo ? Color.Gainsboro : u.Bloqueado ? Color.MistyRose : Color.Empty;
        }

        // CUS01
        private void btnAnadir_Click(object sender, EventArgs e)
        {
            using FrmUsuario_60MN formulario = new FrmUsuario_60MN();
            if (formulario.ShowDialog(this) == DialogResult.OK)
                CargarUsuarios(formulario.UsuarioId);
        }

        // CUS02
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Usuario_60MN? usuario = UsuarioSeleccionado();
            if (usuario == null) return;
            if (!usuario.Activo)
            {
                Mensajes_60MN.Advertir("El usuario está dado de baja: reactívelo antes de modificarlo.");
                return;
            }

            using FrmUsuario_60MN formulario = new FrmUsuario_60MN(usuario);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                CargarUsuarios(usuario.Id);
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && btnModificar.Visible)
                btnModificar.PerformClick();
        }

        // CUS03
        private void btnDarBaja_Click(object sender, EventArgs e)
        {
            Usuario_60MN? usuario = UsuarioSeleccionado();
            if (usuario == null) return;

            // 3.1: no puede darse de baja a sí mismo.
            if (usuario.Id == SessionManager_60MN.Instancia.Usuario?.Id && !SessionManager_60MN.Instancia.EsEmergencia)
            {
                Mensajes_60MN.Advertir("No puede darse de baja a sí mismo.");
                return;
            }
            // 4 / 4.1: confirmación.
            if (!Mensajes_60MN.Confirmar($"¿Confirma la baja del usuario \"{usuario.NombreUsuario}\" ({usuario.NombreCompleto})?"))
                return;
            // 5.2: el usuario tiene una sesión activa.
            if (usuario.EnSesion &&
                !Mensajes_60MN.Confirmar("El usuario tiene una sesión activa, que quedará inhabilitada. ¿Desea continuar?"))
                return;

            try
            {
                usuarioBLL.DarDeBaja(usuario.Id);
                Mensajes_60MN.Informar("Usuario dado de baja correctamente.");
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnReactivar_Click(object sender, EventArgs e)
        {
            Usuario_60MN? usuario = UsuarioSeleccionado();
            if (usuario == null) return;
            if (!Mensajes_60MN.Confirmar($"¿Reactivar al usuario \"{usuario.NombreUsuario}\"?")) return;
            try
            {
                usuarioBLL.Reactivar(usuario.Id);
                Mensajes_60MN.Informar("Usuario reactivado correctamente.");
                CargarUsuarios(usuario.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // CUS08
        private void btnDesbloquear_Click(object sender, EventArgs e)
        {
            Usuario_60MN? usuario = UsuarioSeleccionado();
            if (usuario == null) return;
            try
            {
                usuarioBLL.Desbloquear(usuario.Id);
                Mensajes_60MN.Informar("Usuario desbloqueado correctamente.");
                CargarUsuarios(usuario.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnBlanquear_Click(object sender, EventArgs e)
        {
            Usuario_60MN? usuario = UsuarioSeleccionado();
            if (usuario == null) return;
            if (!Mensajes_60MN.Confirmar($"Se generará una nueva contraseña para \"{usuario.NombreUsuario}\", que deberá cambiarla al ingresar. ¿Continuar?"))
                return;
            try
            {
                string clave = usuarioBLL.BlanquearClave(usuario.Id);
                FrmUsuario_60MN.MostrarClaveInicial(this, usuario.NombreUsuario, clave);
                CargarUsuarios(usuario.Id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Asignar familias y patentes individuales al usuario seleccionado.</summary>
        private void btnPermisos_Click(object sender, EventArgs e)
        {
            Usuario_60MN? usuario = UsuarioSeleccionado();
            if (usuario == null) return;
            if (usuario.Id == SessionManager_60MN.Instancia.Usuario?.Id && !SessionManager_60MN.Instancia.EsEmergencia)
            {
                Mensajes_60MN.Advertir("No puede modificar sus propios permisos.");
                return;
            }
            using FrmPermisosUsuario_60MN formulario = new FrmPermisosUsuario_60MN(usuario);
            if (formulario.ShowDialog(this) == DialogResult.OK)
                CargarUsuarios(usuario.Id);
        }

        private void btnActualizar_Click(object sender, EventArgs e) => CargarUsuarios();

        private void chkMostrarBajas_CheckedChanged(object sender, EventArgs e) => CargarUsuarios();
    }
}
