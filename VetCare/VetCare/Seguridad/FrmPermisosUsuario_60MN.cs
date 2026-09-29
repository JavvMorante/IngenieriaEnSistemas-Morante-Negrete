using BLL_60MN;
using Entidades_60MN;
using Interfaces_60MN;
using Servicios_60MN.Composite;

namespace VetCare
{
    /// <summary>
    /// Asignación de permisos a un usuario (modelo Usuario - Familia - Patente):
    ///  - Familias (UsuarioFamilia): el tipo de usuario, por ejemplo Vendedor.
    ///  - Patentes individuales (UsuarioPatente): permisos extra que se suman a
    ///    los de su familia, sin tener que crear una familia nueva.
    /// </summary>
    public partial class FrmPermisosUsuario_60MN : FormBase_60MN
    {
        private readonly PermisoBLL_60MN permisoBLL = new PermisoBLL_60MN();
        private readonly Usuario_60MN? usuario;
        private bool cargando;

        public FrmPermisosUsuario_60MN() : this(null) { }

        public FrmPermisosUsuario_60MN(Usuario_60MN? usuario)
        {
            InitializeComponent();
            this.usuario = usuario;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        protected override void AlCambiarIdioma()
        {
            if (usuario != null)
                lblTitulo.Text = $"{T("FrmPermisosUsuario_60MN.lblTitulo", "Permisos del usuario")}: {usuario.NombreUsuario} ({usuario.Apellido}, {usuario.Nombre})";
        }

        private void FrmPermisosUsuario_60MN_Load(object sender, EventArgs e)
        {
            if (usuario == null) return;
            try
            {
                cargando = true;
                List<IPermiso_60MN> asignados = permisoBLL.ObtenerPermisosUsuario(usuario.Id);
                HashSet<int> familiasAsignadas = asignados.OfType<Familia_60MN>().Select(f => f.Id).ToHashSet();
                HashSet<int> patentesAsignadas = asignados.OfType<Patente_60MN>().Select(p => p.Id).ToHashSet();

                foreach (Familia_60MN familia in permisoBLL.ListarFamilias())
                    clbFamiliasUsuario.Items.Add(familia, familiasAsignadas.Contains(familia.Id));
                foreach (Patente_60MN patente in permisoBLL.ListarPatentes())
                    clbPatentesUsuario.Items.Add(patente, patentesAsignadas.Contains(patente.Id));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            finally
            {
                cargando = false;
            }
            ActualizarArbol();
        }

        private void clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // ItemCheck ocurre antes de aplicar el cambio: se actualiza el árbol al terminar el evento.
            if (!cargando)
                BeginInvoke(ActualizarArbol);
        }

        /// <summary>
        /// Muestra el árbol de permisos efectivos: cada familia con sus patentes y
        /// subfamilias, más las patentes individuales del usuario.
        /// </summary>
        private void ActualizarArbol()
        {
            List<Familia_60MN> familias = clbFamiliasUsuario.CheckedItems.Cast<Familia_60MN>().ToList();
            List<Patente_60MN> patentes = clbPatentesUsuario.CheckedItems.Cast<Patente_60MN>().ToList();

            tvwPermisos.BeginUpdate();
            tvwPermisos.Nodes.Clear();
            TreeNode nodoFamilias = tvwPermisos.Nodes.Add(T("msg.NodoFamilias", "Familias (UsuarioFamilia)"));
            foreach (Familia_60MN familia in familias)
                nodoFamilias.Nodes.Add(FrmFamiliasPatentes_60MN.CrearNodo(familia));
            TreeNode nodoPatentes = tvwPermisos.Nodes.Add(T("msg.NodoPatentesIndividuales", "Patentes individuales (UsuarioPatente)"));
            foreach (Patente_60MN patente in patentes)
                nodoPatentes.Nodes.Add(FrmFamiliasPatentes_60MN.CrearNodo(patente));
            tvwPermisos.ExpandAll();
            tvwPermisos.EndUpdate();

            int efectivas = familias.SelectMany(f => f.ObtenerPatentes()).Concat(patentes).Select(p => p.Id).Distinct().Count();
            lblResumen.Text = string.Format(T("msg.PatentesEfectivas", "Patentes efectivas: {0}"), efectivas);
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (usuario == null) return;
            List<int> familias = clbFamiliasUsuario.CheckedItems.Cast<Familia_60MN>().Select(f => f.Id).ToList();
            List<int> patentes = clbPatentesUsuario.CheckedItems.Cast<Patente_60MN>().Select(p => p.Id).ToList();

            if (familias.Count == 0)
            {
                Mensajes_60MN.Advertir("El usuario debe tener asignada al menos una familia (tipo de usuario).");
                return;
            }
            try
            {
                permisoBLL.AsignarPermisosUsuario(usuario.Id, familias, patentes);
                Mensajes_60MN.Informar("Permisos del usuario actualizados correctamente. Se aplicarán en su próximo inicio de sesión.");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
