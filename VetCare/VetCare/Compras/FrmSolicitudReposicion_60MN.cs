using System.ComponentModel;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN10 Generar Solicitud de Reposición (encargado de depósito): parte de los productos en alerta
    /// de stock mínimo, permite agregar otros y deja la solicitud pendiente para el analista de compras.
    /// </summary>
    public partial class FrmSolicitudReposicion_60MN : FormBase_60MN
    {
        private readonly SolicitudReposicionBLL_60MN solicitudBLL = new SolicitudReposicionBLL_60MN();
        private BindingList<SolicitudDetalle_60MN> items = new BindingList<SolicitudDetalle_60MN>();

        public FrmSolicitudReposicion_60MN()
        {
            InitializeComponent();
            dgvProductos.AutoGenerateColumns = false;
            txtObservaciones.MaxLength = SolicitudReposicionBLL_60MN.LargoObservaciones;
            TextoGuia_60MN.Asignar(txtBuscar, "Código o nombre");
            cboResultados.DisplayMember = nameof(SolicitudDetalle_60MN.Producto);
            dgvProductos.CellEndEdit += (s, e) => MostrarResumen();
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmSolicitudReposicion_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            Cargar();
        }

        protected override void AlCambiarIdioma() => MostrarResumen();

        /// <summary>Paso 1: productos en alerta que todavía no se pidieron, con la cantidad sugerida.</summary>
        private void Cargar()
        {
            try
            {
                items = new BindingList<SolicitudDetalle_60MN>(solicitudBLL.ListarProductosEnAlerta());
                dgvProductos.DataSource = items;
                txtObservaciones.Clear();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            MostrarResumen();
        }

        private void MostrarResumen()
        {
            if (items.Count == 0)
            {
                lblResumen.Text = T("msg.SinProductosEnAlerta", "No hay productos en alerta de stock mínimo pendientes de pedir. Puede agregar un producto con el buscador.");
                return;
            }
            List<SolicitudDetalle_60MN> marcados = items.Where(i => i.Incluido).ToList();
            lblResumen.Text = string.Format(T("msg.ResumenSolicitud", "{0} producto(s) marcado(s) · {1} unidad(es) a pedir"),
                marcados.Count, marcados.Sum(i => i.CantidadPedida));
        }

        /// <summary>El check se aplica apenas se marca (sin esperar a salir de la celda).</summary>
        private void dgvProductos_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvProductos.IsCurrentCellDirty && dgvProductos.CurrentCell is DataGridViewCheckBoxCell)
            {
                dgvProductos.CommitEdit(DataGridViewDataErrorContexts.Commit);
                MostrarResumen();
            }
        }

        private void dgvProductos_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Mensajes_60MN.Advertir(T("msg.CantidadEntera", "Ingrese una cantidad entera mayor a cero."));
            e.Cancel = true;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                List<SolicitudDetalle_60MN> encontrados = solicitudBLL.BuscarProductos(txtBuscar.Text);
                cboResultados.DataSource = encontrados;
                if (encontrados.Count == 0)
                    Mensajes_60MN.Informar(T("msg.SinResultados", "No se encontraron productos activos con ese criterio."));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Flujo alternativo: agregar un producto que no está en alerta.</summary>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (cboResultados.SelectedItem is not SolicitudDetalle_60MN elegido)
            {
                Mensajes_60MN.Advertir(T("msg.ElegirProducto", "Busque y elija un producto."));
                return;
            }
            SolicitudDetalle_60MN? existente = items.FirstOrDefault(i => i.ProductoId == elegido.ProductoId);
            if (existente != null)
            {
                existente.Incluido = true;
                dgvProductos.Refresh();
                Mensajes_60MN.Informar(T("msg.ProductoYaEnLista", "El producto ya está en la lista: quedó marcado para pedir."));
            }
            else
                items.Add(elegido);
            MostrarResumen();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            dgvProductos.EndEdit();
            Enviar(confirmado: false);
        }

        /// <summary>Pasos 4 a 7. Si un producto ya está en una compra en curso, la BLL pide confirmación.</summary>
        private void Enviar(bool confirmado)
        {
            try
            {
                SolicitudReposicion_60MN solicitud = solicitudBLL.Registrar(items, txtObservaciones.Text, confirmado);
                Mensajes_60MN.Informar(string.Format(T("msg.SolicitudGenerada",
                    "Se generó la solicitud {0} con {1} producto(s). Queda pendiente para el analista de compras."),
                    solicitud.Numero, solicitud.Detalle.Count));
                Cargar();
            }
            catch (ConfirmacionRequeridaException_60MN ex)
            {
                if (Mensajes_60MN.Confirmar(ex.Message))
                    Enviar(confirmado: true);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();
    }
}
