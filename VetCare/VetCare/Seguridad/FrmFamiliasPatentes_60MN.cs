using BLL_60MN;
using Interfaces_60MN;
using Servicios_60MN.Composite;

namespace VetCare
{
    /// <summary>
    /// CUS04 Gestionar Familias y Patentes (ADMIN > Perfiles). Una familia es un
    /// tipo de usuario (Vendedor, Veterinario...) que agrupa patentes
    /// (FamiliaPatente) y, opcionalmente, otras familias (FamiliaFamilia).
    /// </summary>
    public partial class FrmFamiliasPatentes_60MN : FormBase_60MN
    {
        private readonly PermisoBLL_60MN permisoBLL = new PermisoBLL_60MN();
        private List<Familia_60MN> familias = new List<Familia_60MN>();
        private List<Patente_60MN> patentes = new List<Patente_60MN>();

        /// <summary>Id de la familia en edición (0 = familia nueva).</summary>
        private int familiaEnEdicion;
        private bool cargando;

        public FrmFamiliasPatentes_60MN()
        {
            InitializeComponent();
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmFamiliasPatentes_60MN_Load(object sender, EventArgs e)
        {
            CargarDatos(null);
        }

        private void CargarDatos(int? seleccionarId)
        {
            try
            {
                cargando = true;
                familias = permisoBLL.ListarFamilias();
                patentes = permisoBLL.ListarPatentes();

                clbPatentes.Items.Clear();
                foreach (Patente_60MN patente in patentes)
                    clbPatentes.Items.Add(patente);

                lstFamilias.DataSource = null;
                lstFamilias.DisplayMember = nameof(Familia_60MN.Nombre);
                lstFamilias.DataSource = familias;
                cargando = false;

                int indice = seleccionarId.HasValue ? familias.FindIndex(f => f.Id == seleccionarId.Value) : 0;
                if (familias.Count > 0)
                {
                    lstFamilias.SelectedIndex = Math.Max(indice, 0);
                    MostrarFamilia(familias[lstFamilias.SelectedIndex]);
                }
                else
                {
                    PrepararNueva();
                }
            }
            catch (Exception ex)
            {
                cargando = false;
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void lstFamilias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cargando && lstFamilias.SelectedItem is Familia_60MN familia)
                MostrarFamilia(familia);
        }

        /// <summary>Paso 4: muestra las patentes y familias que componen la familia.</summary>
        private void MostrarFamilia(Familia_60MN familia)
        {
            cargando = true;
            familiaEnEdicion = familia.Id;
            txtNombreFamilia.Text = familia.Nombre;

            HashSet<int> patentesDirectas = familia.PatentesDirectas.Select(p => p.Id).ToHashSet();
            for (int i = 0; i < clbPatentes.Items.Count; i++)
                clbPatentes.SetItemChecked(i, patentesDirectas.Contains(((Patente_60MN)clbPatentes.Items[i]).Id));

            // Solo se ofrecen familias que no contengan a esta (evita ciclos).
            HashSet<int> familiasDirectas = familia.FamiliasDirectas.Select(f => f.Id).ToHashSet();
            clbSubfamilias.Items.Clear();
            foreach (Familia_60MN otra in familias.Where(f => f.Id != familia.Id && !f.ContieneFamilia(familia.Id)))
                clbSubfamilias.Items.Add(otra, familiasDirectas.Contains(otra.Id));

            int usuarios = permisoBLL.ContarUsuarios(familia.Id);
            lblUsuariosFamilia.Text = string.Format(T("msg.UsuariosConFamilia", "Usuarios activos con esta familia: {0}"), usuarios);
            btnEliminarFamilia.Enabled = true;
            cargando = false;
            ActualizarArbol();
        }

        private void PrepararNueva()
        {
            cargando = true;
            familiaEnEdicion = 0;
            lstFamilias.ClearSelected();
            txtNombreFamilia.Clear();
            for (int i = 0; i < clbPatentes.Items.Count; i++)
                clbPatentes.SetItemChecked(i, false);
            clbSubfamilias.Items.Clear();
            foreach (Familia_60MN familia in familias)
                clbSubfamilias.Items.Add(familia, false);
            lblUsuariosFamilia.Text = T("msg.FamiliaNueva", "Familia nueva");
            btnEliminarFamilia.Enabled = false;
            cargando = false;
            ActualizarArbol();
            txtNombreFamilia.Focus();
        }

        private void clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // ItemCheck ocurre antes de aplicar el cambio: se actualiza el árbol al terminar el evento.
            if (!cargando)
                BeginInvoke(ActualizarArbol);
        }

        private List<int> PatentesSeleccionadas() => clbPatentes.CheckedItems.Cast<Patente_60MN>().Select(p => p.Id).ToList();

        private List<int> SubfamiliasSeleccionadas() => clbSubfamilias.CheckedItems.Cast<Familia_60MN>().Select(f => f.Id).ToList();

        /// <summary>Vista previa del árbol Composite que resulta de la selección actual.</summary>
        private void ActualizarArbol()
        {
            tvwArbol.BeginUpdate();
            tvwArbol.Nodes.Clear();
            TreeNode raiz = tvwArbol.Nodes.Add("📁 " + (string.IsNullOrWhiteSpace(txtNombreFamilia.Text) ? T("msg.FamiliaNueva", "Familia nueva") : txtNombreFamilia.Text));
            foreach (Familia_60MN familia in clbSubfamilias.CheckedItems.Cast<Familia_60MN>())
                raiz.Nodes.Add(CrearNodo(familia));
            foreach (Patente_60MN patente in clbPatentes.CheckedItems.Cast<Patente_60MN>())
                raiz.Nodes.Add(CrearNodo(patente));
            tvwArbol.ExpandAll();
            tvwArbol.EndUpdate();
        }

        internal static TreeNode CrearNodo(IPermiso_60MN permiso)
        {
            TreeNode nodo = new TreeNode(permiso is Patente_60MN p ? $"🔑 {p.Nombre}  [{p.Codigo}]" : "📁 " + permiso.Nombre);
            foreach (IPermiso_60MN hijo in permiso.ObtenerHijos())
                nodo.Nodes.Add(CrearNodo(hijo));
            return nodo;
        }

        private void btnNuevaFamilia_Click(object sender, EventArgs e) => PrepararNueva();

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            List<int> patentesElegidas = PatentesSeleccionadas();
            List<int> subfamilias = SubfamiliasSeleccionadas();

            // 5.1: familia sin permisos.
            if (patentesElegidas.Count == 0 && subfamilias.Count == 0 &&
                !Mensajes_60MN.Confirmar("La familia no tiene ningún permiso asignado: los usuarios de esta familia no podrán operar. ¿Desea continuar?"))
                return;

            // 6.1: informar a cuántos usuarios impacta el cambio.
            if (familiaEnEdicion != 0)
            {
                int usuarios = permisoBLL.ContarUsuarios(familiaEnEdicion);
                if (usuarios > 0 &&
                    !Mensajes_60MN.Confirmar($"El cambio impacta a {usuarios} usuario(s) activo(s) de esta familia (se aplicará en su próximo inicio de sesión). ¿Confirmar?"))
                    return;
            }

            try
            {
                int id = permisoBLL.GuardarFamilia(familiaEnEdicion, txtNombreFamilia.Text, patentesElegidas, subfamilias);
                Mensajes_60MN.Informar("Permisos actualizados correctamente.");
                CargarDatos(id);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnEliminarFamilia_Click(object sender, EventArgs e)
        {
            if (familiaEnEdicion == 0) return;
            if (!Mensajes_60MN.Confirmar($"¿Eliminar la familia \"{txtNombreFamilia.Text}\"?")) return;
            try
            {
                permisoBLL.EliminarFamilia(familiaEnEdicion);
                CargarDatos(null);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnDescartar_Click(object sender, EventArgs e)
        {
            if (lstFamilias.SelectedItem is Familia_60MN familia)
                MostrarFamilia(familia);
            else
                PrepararNueva();
        }
    }
}
