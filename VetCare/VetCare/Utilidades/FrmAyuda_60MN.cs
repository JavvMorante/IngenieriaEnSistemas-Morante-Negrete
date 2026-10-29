namespace VetCare
{
    /// <summary>
    /// Ayuda en línea (G05): F1 en cualquier pantalla o AYUDA > Ayuda en línea. Abre el tema
    /// de la pantalla activa y ofrece el índice de todos los temas, en el idioma actual.
    /// </summary>
    public partial class FrmAyuda_60MN : FormBase_60MN
    {
        private readonly string? formulario;

        public FrmAyuda_60MN() : this(null) { }

        /// <param name="formulario">Nombre del formulario desde el que se pidió la ayuda.</param>
        public FrmAyuda_60MN(string? formulario)
        {
            InitializeComponent();
            this.formulario = formulario;
        }

        /// <summary>
        /// Muestra la ayuda de la pantalla activa: en el menú principal, la de la ventana hija
        /// activa. Es modal para que funcione también desde los diálogos (alta de cliente, etc.).
        /// </summary>
        public static void Mostrar(Form origen)
        {
            Form activo = origen.IsMdiContainer && origen.ActiveMdiChild != null ? origen.ActiveMdiChild : origen;
            using FrmAyuda_60MN ayuda = new FrmAyuda_60MN(activo.Name);
            ayuda.ShowDialog(origen);
        }

        private void FrmAyuda_60MN_Load(object sender, EventArgs e)
        {
            CargarTemas(TemasAyuda_60MN.DeFormulario(formulario));
            lstTemas.Focus();
        }

        /// <summary>Al cambiar el idioma se vuelven a cargar los títulos del índice y el texto.</summary>
        protected override void AlCambiarIdioma()
        {
            if (lstTemas.Items.Count > 0)
                CargarTemas(lstTemas.SelectedItem as TemaAyuda_60MN ?? TemasAyuda_60MN.Todos[0]);
        }

        private void CargarTemas(TemaAyuda_60MN seleccionado)
        {
            lstTemas.BeginUpdate();
            lstTemas.Items.Clear();
            foreach (TemaAyuda_60MN tema in TemasAyuda_60MN.Todos)
                lstTemas.Items.Add(tema);
            lstTemas.EndUpdate();
            lstTemas.SelectedItem = seleccionado;
        }

        private void lstTemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTemas.SelectedItem is not TemaAyuda_60MN tema) return;
            lblTema.Text = tema.Titulo;
            txtTexto.Text = tema.Texto;
            txtTexto.SelectionLength = 0;
        }
    }
}
