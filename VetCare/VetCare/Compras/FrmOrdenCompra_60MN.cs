using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN11 Registrar Orden de Compra (analista de compras): elige una solicitud pendiente, carga las
    /// cotizaciones que mandaron los proveedores, las compara y genera la orden con la elegida.
    /// En "Mis órdenes" corrige las rechazadas o anula las que todavía no se pagaron.
    /// </summary>
    public partial class FrmOrdenCompra_60MN : FormBase_60MN
    {
        private readonly OrdenCompraBLL_60MN ordenBLL = new OrdenCompraBLL_60MN();
        private List<Cotizacion_60MN> cotizaciones = new List<Cotizacion_60MN>();
        private List<Proveedor_60MN> proveedores = new List<Proveedor_60MN>();

        public FrmOrdenCompra_60MN()
        {
            InitializeComponent();
            dgvSolicitud.AutoGenerateColumns = false;
            dgvCotizaciones.AutoGenerateColumns = false;
            dgvDetalle.AutoGenerateColumns = false;
            dgvMisOrdenes.AutoGenerateColumns = false;
            txtMotivo.MaxLength = OrdenCompraBLL_60MN.LargoMotivo;
            txtMotivoAnulacion.MaxLength = OrdenCompraBLL_60MN.LargoMotivo;
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private SolicitudReposicion_60MN? Solicitud => cboSolicitud.SelectedItem as SolicitudReposicion_60MN;

        private Cotizacion_60MN? Elegida => dgvCotizaciones.CurrentRow?.DataBoundItem as Cotizacion_60MN;

        private OrdenCompra_60MN? OrdenSeleccionada => dgvMisOrdenes.CurrentRow?.DataBoundItem as OrdenCompra_60MN;

        private void FrmOrdenCompra_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            try
            {
                proveedores = ordenBLL.ListarProveedores();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            CargarSolicitudes();
        }

        protected override void AlCambiarIdioma() => MostrarAviso();

        /// <summary>Paso 1: solicitudes PENDIENTE o EN_COMPRA a las que les falta pedir algo.</summary>
        private void CargarSolicitudes()
        {
            try
            {
                cboSolicitud.DataSource = ordenBLL.ListarSolicitudesPendientes();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            if (cboSolicitud.Items.Count == 0)
                MostrarSolicitud();
        }

        private void cboSolicitud_SelectedIndexChanged(object sender, EventArgs e) => MostrarSolicitud();

        /// <summary>Paso 2: lo que falta pedir de la solicitud. Las cotizaciones cargadas son de una solicitud.</summary>
        private void MostrarSolicitud()
        {
            dgvSolicitud.DataSource = Solicitud?.Detalle.ToList();
            cotizaciones = new List<Cotizacion_60MN>();
            txtMotivo.Clear();
            Recalcular();
        }

        private void btnCargarCotizacion_Click(object sender, EventArgs e)
        {
            SolicitudReposicion_60MN? solicitud = Solicitud;
            if (solicitud == null)
            {
                Mensajes_60MN.Advertir(T("msg.ElegirSolicitud", "Elija primero la solicitud de reposición."));
                return;
            }
            List<FrmCotizacion_60MN.FilaPrecio_60MN> filas = solicitud.Detalle.Where(d => d.PendienteDePedir > 0)
                .Select(d => new FrmCotizacion_60MN.FilaPrecio_60MN
                {
                    ProductoId = d.ProductoId, Producto = d.Producto, Cantidad = d.PendienteDePedir, CostoActual = d.PrecioCostoActual
                })
                .ToList();
            using FrmCotizacion_60MN dialogo = new FrmCotizacion_60MN(proveedores, filas, permiteCantidades: false, inicial: null);
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            Cotizacion_60MN nueva = dialogo.Cotizacion;
            if (cotizaciones.Any(c => c.ProveedorId == nueva.ProveedorId && c.NroCotizacion == nueva.NroCotizacion))
            {
                Mensajes_60MN.Advertir(T("msg.CotizacionRepetida", "Esa cotización ya está cargada."));
                return;
            }
            cotizaciones.Add(nueva);
            Recalcular();
        }

        private void btnQuitarCotizacion_Click(object sender, EventArgs e)
        {
            if (Elegida is Cotizacion_60MN cotizacion)
            {
                cotizaciones.Remove(cotizacion);
                Recalcular();
            }
        }

        /// <summary>Paso 4: compara las cotizaciones (la BLL calcula los totales y marca la más barata) y elige la más barata.</summary>
        private void Recalcular()
        {
            if (Solicitud is SolicitudReposicion_60MN solicitud && cotizaciones.Count > 0)
            {
                try
                {
                    ordenBLL.Comparar(solicitud, cotizaciones);
                }
                catch (Exception ex)
                {
                    Mensajes_60MN.MostrarError(ex);
                }
            }
            dgvCotizaciones.DataSource = null;
            dgvCotizaciones.DataSource = cotizaciones.ToList();
            Cotizacion_60MN? masBarata = cotizaciones.FirstOrDefault(c => c.EsLaMasBarata);
            foreach (DataGridViewRow fila in dgvCotizaciones.Rows)
                if (fila.DataBoundItem == masBarata)
                    dgvCotizaciones.CurrentCell = fila.Cells[0];
            MostrarDetalle();
        }

        private void dgvCotizaciones_SelectionChanged(object sender, EventArgs e) => MostrarDetalle();

        private void MostrarDetalle()
        {
            dgvDetalle.DataSource = Solicitud is SolicitudReposicion_60MN solicitud && Elegida is Cotizacion_60MN elegida
                ? ordenBLL.ArmarOrden(solicitud, elegida, null).Detalle
                : null;
            MostrarAviso();
        }

        /// <summary>Paso 5.1 y 6: avisa si la elegida no es la más barata o si algún precio supera mucho el costo.</summary>
        private void MostrarAviso()
        {
            List<string> avisos = new List<string>();
            if (Elegida is Cotizacion_60MN elegida && !elegida.EsLaMasBarata)
                avisos.Add(T("msg.NoEsLaMasBarata", "La cotización elegida no es la de menor costo: indique el motivo de la elección."));
            if (dgvDetalle.DataSource is List<OrdenCompraDetalle_60MN> detalle && detalle.Any(PrecioAlto))
                avisos.Add(string.Format(T("msg.PreciosAltos",
                    "Hay precios que superan en más de {0} % el costo actual (en rojo): el gerente lo verá al aprobar."),
                    OrdenCompraBLL_60MN.PorcentajeAdvertenciaPrecio));
            lblAviso.Text = string.Join("\n", avisos);
        }

        private static bool PrecioAlto(OrdenCompraDetalle_60MN d) =>
            d.PrecioCostoActual > 0 && d.PrecioUnitario > d.PrecioCostoActual * (1 + OrdenCompraBLL_60MN.PorcentajeAdvertenciaPrecio / 100m);

        private void dgvDetalle_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colDetPrecio.Index && dgvDetalle.Rows[e.RowIndex].DataBoundItem is OrdenCompraDetalle_60MN d && PrecioAlto(d))
            {
                e.CellStyle.BackColor = Color.MistyRose;
                e.CellStyle.ForeColor = Color.DarkRed;
            }
        }

        /// <summary>Pasos 5 a 8: genera la orden con la cotización elegida; queda pendiente de aprobación.</summary>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (Solicitud is not SolicitudReposicion_60MN solicitud || Elegida is not Cotizacion_60MN elegida)
            {
                Mensajes_60MN.Advertir(T("msg.ElegirCotizacion", "Cargue las cotizaciones y seleccione la elegida."));
                return;
            }
            try
            {
                OrdenCompra_60MN orden = ordenBLL.Registrar(ordenBLL.ArmarOrden(solicitud, elegida, txtMotivo.Text), cotizaciones);
                Mensajes_60MN.Informar(string.Format(T("msg.OrdenRegistrada",
                    "Se registró la orden {0} a {1} por $ {2:N2}. Queda pendiente de aprobación del gerente."),
                    orden.Numero, orden.Proveedor, orden.Total) +
                    (orden.TieneAdvertencias ? "\n\n" + T("msg.ConAdvertencias", "Advertencias") + ": " + orden.Advertencias : string.Empty));
                CargarSolicitudes();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        // ---------------- Mis órdenes ----------------

        private void tabOrdenes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabOrdenes.SelectedTab == tpMisOrdenes)
                CargarMisOrdenes();
        }

        private void CargarMisOrdenes()
        {
            try
            {
                dgvMisOrdenes.DataSource = ordenBLL.ListarMisOrdenes();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Las rechazadas (para corregir) se destacan; las anuladas y cerradas se ven en gris.</summary>
        private void dgvMisOrdenes_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow fila in dgvMisOrdenes.Rows)
            {
                if (fila.DataBoundItem is not OrdenCompra_60MN orden) continue;
                if (orden.Estado == EstadoOrden_60MN.Rechazada)
                    fila.DefaultCellStyle.BackColor = Color.MistyRose;
                else if (orden.Estado == EstadoOrden_60MN.Anulada || orden.Estado == EstadoOrden_60MN.Cerrada)
                    fila.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        /// <summary>Orden rechazada: se corrige la cotización (proveedor, precios y cantidades) y vuelve a aprobación.</summary>
        private void btnCorregir_Click(object sender, EventArgs e)
        {
            if (OrdenSeleccionada is not OrdenCompra_60MN orden) return;
            if (orden.Estado != EstadoOrden_60MN.Rechazada)
            {
                Mensajes_60MN.Advertir(T("msg.SoloRechazadas", "Solo se corrigen las órdenes rechazadas."));
                return;
            }
            List<FrmCotizacion_60MN.FilaPrecio_60MN> filas = orden.Detalle
                .Select(d => new FrmCotizacion_60MN.FilaPrecio_60MN
                {
                    ProductoId = d.ProductoId, Producto = d.Producto, Cantidad = d.Cantidad, Precio = d.PrecioUnitario, CostoActual = d.PrecioCostoActual
                })
                .ToList();
            Cotizacion_60MN actual = new Cotizacion_60MN
            {
                ProveedorId = orden.ProveedorId, NroCotizacion = orden.NroCotizacion,
                PlazoEntregaDias = orden.PlazoEntregaDias, CondicionPago = orden.CondicionPago
            };
            using FrmCotizacion_60MN dialogo = new FrmCotizacion_60MN(proveedores, filas, permiteCantidades: true, inicial: actual);
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            Cotizacion_60MN corregida = dialogo.Cotizacion;
            orden.ProveedorId = corregida.ProveedorId;
            orden.NroCotizacion = corregida.NroCotizacion;
            orden.PlazoEntregaDias = corregida.PlazoEntregaDias;
            orden.CondicionPago = corregida.CondicionPago;
            foreach (OrdenCompraDetalle_60MN d in orden.Detalle)
            {
                d.PrecioUnitario = corregida.Precios[d.ProductoId];
                d.Cantidad = dialogo.Cantidades[d.ProductoId];
            }
            try
            {
                OrdenCompra_60MN resultado = ordenBLL.Corregir(orden, new List<Cotizacion_60MN>());
                Mensajes_60MN.Informar(string.Format(T("msg.OrdenCorregida",
                    "La orden {0} se corrigió (total $ {1:N2}) y volvió a quedar pendiente de aprobación."), resultado.Numero, resultado.Total));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            CargarMisOrdenes();
        }

        private void btnAnular_Click(object sender, EventArgs e)
        {
            if (OrdenSeleccionada is not OrdenCompra_60MN orden) return;
            if (!Mensajes_60MN.Confirmar(string.Format(T("msg.ConfirmarAnularOrden", "¿Anular la orden {0} a {1} por $ {2:N2}?"),
                    orden.Numero, orden.Proveedor, orden.Total)))
                return;
            try
            {
                ordenBLL.Anular(orden.Id, txtMotivoAnulacion.Text);
                txtMotivoAnulacion.Clear();
                Mensajes_60MN.Informar(string.Format(T("msg.OrdenAnulada", "La orden {0} quedó anulada."), orden.Numero));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
            CargarMisOrdenes();
            CargarSolicitudes();
        }
    }
}
