using System.ComponentModel;
using System.Globalization;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN09 Registrar Medicación en Carrito: se eligen productos con stock y se confirma
    /// la entrega, que genera los egresos de stock («include» CUN14) asociados a la atención.
    /// Se abre desde la atención (con la atención fija) o desde VENTAS > Carrito.
    /// </summary>
    public partial class FrmCarrito_60MN : FormBase_60MN
    {
        private readonly AtencionBLL_60MN atencionBLL = new AtencionBLL_60MN();
        private readonly int? atencionId;
        private readonly BindingList<ItemCarrito_60MN> carrito = new BindingList<ItemCarrito_60MN>();

        public FrmCarrito_60MN() : this(null) { }

        public FrmCarrito_60MN(int? atencionId)
        {
            InitializeComponent();
            this.atencionId = atencionId;
            dgvProductos.AutoGenerateColumns = false;
            dgvCarrito.AutoGenerateColumns = false;
            dgvCarrito.DataSource = carrito;
        }

        private void FrmCarrito_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                if (atencionId.HasValue)
                {
                    Atencion_60MN? atencion = atencionBLL.ObtenerAtencion(atencionId.Value);
                    if (atencion != null) cboAtencion.Items.Add(atencion);
                    cboAtencion.Enabled = false;
                }
                else
                {
                    // Paso 2: atenciones de los últimos días a las que asociar la medicación.
                    foreach (Atencion_60MN a in atencionBLL.ListarAtencionesRecientes())
                        cboAtencion.Items.Add(a);
                }
                if (cboAtencion.Items.Count > 0)
                    cboAtencion.SelectedIndex = 0;
                else
                    Mensajes_60MN.Advertir("No hay atenciones recientes. Registre primero la atención del paciente.");
                Buscar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                BeginInvoke(Close);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e) => Buscar();

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            Buscar();
        }

        /// <summary>Pasos 3 y 4: productos activos con stock, precio y vencimiento.</summary>
        private void Buscar()
        {
            try
            {
                dgvProductos.DataSource = atencionBLL.BuscarProductos(txtBuscar.Text, null)
                                                     .Where(p => p.StockActual > 0)
                                                     .ToList();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Resalta los productos vencidos (rojo) y los próximos a vencer (amarillo).</summary>
        private void dgvProductos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvProductos.Rows[e.RowIndex].DataBoundItem is not Producto_60MN p || e.CellStyle == null) return;
            if (p.Vencido)
                e.CellStyle.BackColor = Color.MistyRose;
            else if (p.DiasParaVencer <= AtencionBLL_60MN.DiasProximoAVencer)
                e.CellStyle.BackColor = Color.LightYellow;
        }

        private void dgvProductos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) btnAgregar_Click(sender, e);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (dgvProductos.CurrentRow?.DataBoundItem is not Producto_60MN p)
            {
                Mensajes_60MN.Advertir("Seleccione un producto.");
                return;
            }
            int cantidad = (int)nudCantidad.Value;
            // 5.1: cantidad menor o igual a cero.
            if (cantidad <= 0)
            {
                Mensajes_60MN.Advertir($"La cantidad de {p.Nombre} debe ser mayor a cero.");
                return;
            }
            // 4.2: medicación vencida o próxima a vencer.
            if (p.Vencido)
            {
                Mensajes_60MN.Advertir($"El producto {p.Nombre} (lote {p.Lote}) está vencido y no puede entregarse.");
                return;
            }
            if (p.DiasParaVencer <= AtencionBLL_60MN.DiasProximoAVencer &&
                !Mensajes_60MN.Confirmar($"El lote {p.Lote} de {p.Nombre} vence el {p.FechaVencimiento:dd/MM/yyyy} " +
                                         $"({p.DiasParaVencer} día(s)). ¿Desea entregarlo igualmente?"))
                return;

            // 4.1: stock insuficiente (contando lo que ya está en el carrito).
            ItemCarrito_60MN? existente = carrito.FirstOrDefault(i => i.ProductoId == p.Id);
            int total = cantidad + (existente?.Cantidad ?? 0);
            if (total > p.StockActual)
            {
                Mensajes_60MN.Advertir($"Stock insuficiente de {p.Nombre}: disponible {p.StockActual}, solicitado {total}.");
                return;
            }

            if (existente != null)
            {
                existente.Cantidad = total;
                carrito.ResetBindings();
            }
            else
            {
                carrito.Add(new ItemCarrito_60MN
                {
                    ProductoId = p.Id,
                    Codigo = p.Codigo,
                    Producto = p.Nombre,
                    Lote = p.Lote,
                    Cantidad = cantidad,
                    PrecioUnitario = p.PrecioVenta
                });
            }
            ActualizarTotal();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow?.DataBoundItem is ItemCarrito_60MN item)
            {
                carrito.Remove(item);
                ActualizarTotal();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        protected override void AlCambiarIdioma() => ActualizarTotal();

        private void ActualizarTotal() =>
            lblTotal.Text = $"{TraductorFormularios_60MN.T("msg.Total", "Total")}: $ {carrito.Sum(i => i.Subtotal).ToString("N2", CultureInfo.GetCultureInfo("es-AR"))}";

        /// <summary>Paso 9: confirma la entrega; los egresos y el detalle se registran en una sola transacción.</summary>
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (cboAtencion.SelectedItem is not Atencion_60MN atencion)
            {
                Mensajes_60MN.Advertir("Seleccione la atención a la que se asocia la medicación.");
                return;
            }
            if (carrito.Count == 0)
            {
                Mensajes_60MN.Advertir("El carrito está vacío.");
                return;
            }
            if (!Mensajes_60MN.Confirmar($"¿Confirma la entrega de {carrito.Count} producto(s) para {atencion.Paciente}?\n{lblTotal.Text}"))
                return;
            try
            {
                List<string> alertas = atencionBLL.RegistrarMedicacion(atencion.Id, carrito.ToList());
                string mensaje = "Medicación registrada y stock actualizado correctamente.";
                if (alertas.Count > 0)
                    mensaje += "\n\nAlerta de stock mínimo:\n- " + string.Join("\n- ", alertas);
                Mensajes_60MN.Informar(mensaje);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                Buscar();
            }
        }
    }
}
