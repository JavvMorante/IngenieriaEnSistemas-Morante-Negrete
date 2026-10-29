using System.Globalization;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN05 Registrar Medicación en Carrito: se eligen productos con stock y, al confirmar, se
    /// graba la venta con sus egresos de stock («include» CUN09). El carrito vive en memoria
    /// (Carrito_60MN) hasta la confirmación. Se abre desde la atención (cliente y atención
    /// fijos) o desde VENTAS > Carrito eligiendo un cliente, con o sin atención. No se factura.
    /// </summary>
    public partial class FrmCarrito_60MN : FormBase_60MN
    {
        private readonly VentaBLL_60MN ventaBLL = new VentaBLL_60MN();
        private readonly int? atencionId;
        private readonly Carrito_60MN carrito = new Carrito_60MN();
        private Cliente_60MN? cliente;

        public FrmCarrito_60MN() : this(null) { }

        public FrmCarrito_60MN(int? atencionId)
        {
            InitializeComponent();
            this.atencionId = atencionId;
            dgvProductos.AutoGenerateColumns = false;
            dgvCarrito.AutoGenerateColumns = false;
            TextoGuia_60MN.Asignar(txtBuscar, "Código o nombre");
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmCarrito_60MN_Load(object sender, EventArgs e)
        {
            try
            {
                if (atencionId.HasValue)
                {
                    // Desde la atención: el cliente es el dueño de la mascota y la atención queda fija.
                    Atencion_60MN atencion = ventaBLL.ObtenerAtencion(atencionId.Value)
                        ?? throw new InvalidOperationException("La atención no existe.");
                    MostrarCliente(ventaBLL.ObtenerCliente(atencion.ClienteId));
                    cboAtencion.Items.Clear();
                    cboAtencion.Items.Add(atencion);
                    cboAtencion.SelectedIndex = 0;
                    cboAtencion.Enabled = false;
                    btnElegirCliente.Visible = false;
                }
                else
                {
                    MostrarCliente(null);
                }
                RefrescarCarrito();
                Buscar();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
                BeginInvoke(new Action(Close));
            }
        }

        /// <summary>Paso 2 (desde VENTAS > Carrito): el cliente de la venta y, si corresponde, la atención.</summary>
        private void btnElegirCliente_Click(object sender, EventArgs e)
        {
            using FrmBuscarCliente_60MN selector = new FrmBuscarCliente_60MN("msg.ElegirClienteVenta", "Elija el cliente de la venta");
            if (selector.ShowDialog(this) != DialogResult.OK || selector.Seleccionado == null) return;
            try
            {
                MostrarCliente(ventaBLL.ObtenerCliente(selector.Seleccionado.Id));
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>
        /// Muestra el cliente y carga sus atenciones recientes. Desde VENTAS > Carrito la
        /// primera opción es "sin atención": también se vende a un cliente sin atención.
        /// </summary>
        private void MostrarCliente(Cliente_60MN? elegido)
        {
            cliente = elegido;
            txtCliente.Text = cliente == null
                ? T("msg.SinClienteVenta", "(elija el cliente de la venta)")
                : $"{cliente.NombreCompleto} - {cliente.Documento}";
            if (atencionId.HasValue) return;

            cboAtencion.Items.Clear();
            cboAtencion.Enabled = cliente != null;
            if (cliente == null) return;
            cboAtencion.Items.Add(T("msg.VentaSinAtencion", "(venta sin atención)"));
            foreach (Atencion_60MN a in ventaBLL.ListarAtencionesRecientes(cliente.Id))
                cboAtencion.Items.Add(a);
            cboAtencion.SelectedIndex = 0;
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
                dgvProductos.DataSource = ventaBLL.BuscarProductos(txtBuscar.Text, null);
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
            else if (p.DiasParaVencer <= VentaBLL_60MN.DiasProximoAVencer)
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
            if (p.DiasParaVencer <= VentaBLL_60MN.DiasProximoAVencer &&
                !Mensajes_60MN.Confirmar($"El lote {p.Lote} de {p.Nombre} vence el {p.FechaVencimiento:dd/MM/yyyy} " +
                                         $"({p.DiasParaVencer} día(s)). ¿Desea entregarlo igualmente?"))
                return;

            // 4.1: stock insuficiente (contando lo que ya está en el carrito).
            int total = cantidad + (carrito.Buscar(p.Id)?.Cantidad ?? 0);
            if (total > p.StockActual)
            {
                Mensajes_60MN.Advertir($"Stock insuficiente de {p.Nombre}: disponible {p.StockActual}, solicitado {total}.");
                return;
            }

            carrito.Agregar(new ItemCarrito_60MN
            {
                ProductoId = p.Id,
                Codigo = p.Codigo,
                Producto = p.Nombre,
                Lote = p.Lote,
                Cantidad = cantidad,
                PrecioUnitario = p.PrecioVenta
            });
            RefrescarCarrito();
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow?.DataBoundItem is ItemCarrito_60MN item)
            {
                carrito.Quitar(item.ProductoId);
                RefrescarCarrito();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e) => Close();

        protected override void AlCambiarIdioma()
        {
            ActualizarTotal();
            if (cliente == null)
                txtCliente.Text = T("msg.SinClienteVenta", "(elija el cliente de la venta)");
        }

        private void RefrescarCarrito()
        {
            dgvCarrito.DataSource = null;
            dgvCarrito.DataSource = carrito.Items.ToList();
            ActualizarTotal();
        }

        private void ActualizarTotal() =>
            lblTotal.Text = $"{T("msg.Total", "Total")}: $ {carrito.Total.ToString("N2", CultureInfo.GetCultureInfo("es-AR"))}";

        /// <summary>Paso 9: confirma la venta; encabezado, detalle y egresos se registran en una sola transacción.</summary>
        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (cliente == null)
            {
                Mensajes_60MN.Advertir("Elija el cliente de la venta.");
                return;
            }
            if (carrito.Items.Count == 0)
            {
                Mensajes_60MN.Advertir("El carrito está vacío.");
                return;
            }
            Atencion_60MN? atencion = cboAtencion.SelectedItem as Atencion_60MN;
            string destino = atencion != null ? $"{atencion.Paciente} (atención Nº {atencion.Id})" : cliente.NombreCompleto;
            if (!Mensajes_60MN.Confirmar($"¿Confirma la venta de {carrito.Items.Count} producto(s) para {destino}?\n{lblTotal.Text}"))
                return;

            carrito.ClienteId = cliente.Id;
            carrito.AtencionId = atencion?.Id;
            try
            {
                ResultadoVenta_60MN resultado = ventaBLL.RegistrarVenta(carrito);
                string mensaje = $"Venta V-{resultado.VentaId:00000} registrada y stock actualizado correctamente.";
                if (resultado.Alertas.Count > 0)
                    mensaje += "\n\nAlerta de stock mínimo:\n- " + string.Join("\n- ", resultado.Alertas);
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
