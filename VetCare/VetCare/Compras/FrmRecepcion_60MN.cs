using System.ComponentModel;
using System.Globalization;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// CUN14 Registrar Recepción de Mercadería (encargado de depósito): busca la orden pagada, carga el
    /// remito y lo recibido de cada producto (lote y vencimiento) y lo ingresa al stock en una transacción.
    /// </summary>
    public partial class FrmRecepcion_60MN : FormBase_60MN
    {
        /// <summary>Un renglón de la grilla: lo pedido y pendiente de la orden y lo que se recibe.</summary>
        public class FilaRecepcion_60MN
        {
            public int ProductoId { get; set; }

            public string Producto { get; set; } = string.Empty;

            public int Pedido { get; set; }

            public int Pendiente { get; set; }

            public int Recibido { get; set; }

            public string Lote { get; set; } = string.Empty;

            public DateTime Vencimiento { get; set; }

            public string? Observacion { get; set; }
        }

        private readonly RecepcionBLL_60MN recepcionBLL = new RecepcionBLL_60MN();
        private OrdenCompra_60MN? orden;
        private BindingList<FilaRecepcion_60MN> filas = new BindingList<FilaRecepcion_60MN>();

        public FrmRecepcion_60MN()
        {
            InitializeComponent();
            dgvDetalle.AutoGenerateColumns = false;
            txtRemito.MaxLength = RecepcionBLL_60MN.LargoRemito;
            colLote.MaxInputLength = ProductoBLL_60MN.LargoLote;
            colObservacion.MaxInputLength = RecepcionBLL_60MN.LargoObservacion;
            txtRemito.KeyPress += (s, e) =>
            {
                char c = char.ToUpperInvariant(e.KeyChar);
                if (!char.IsControl(c) && !((c >= 'A' && c <= 'Z') || char.IsDigit(c) || c == '-' || c == '/'))
                {
                    e.Handled = true;
                    System.Media.SystemSounds.Beep.Play();
                }
            };
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private void FrmRecepcion_60MN_Load(object sender, EventArgs e)
        {
            Mensajes_60MN.AplicarPermisos(this);
            MostrarOrden(null);
        }

        protected override void AlCambiarIdioma() => MostrarDatosOrden();

        /// <summary>Paso 1: busca la orden (PAGADA o RECIBIDA_PARCIAL) y propone recibir todo lo pendiente.</summary>
        private void btnBuscarOrden_Click(object sender, EventArgs e)
        {
            lblEstado.Text = string.Empty;
            try
            {
                MostrarOrden(recepcionBLL.ObtenerOrdenParaRecibir(txtOrden.Text));
                txtRemito.Focus();
            }
            catch (Exception ex)
            {
                MostrarOrden(null);
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void MostrarOrden(OrdenCompra_60MN? encontrada)
        {
            orden = encontrada;
            filas = new BindingList<FilaRecepcion_60MN>((orden?.Detalle ?? new List<OrdenCompraDetalle_60MN>())
                .Where(d => d.Pendiente > 0)
                .Select(d => new FilaRecepcion_60MN
                {
                    ProductoId = d.ProductoId,
                    Producto = d.Producto,
                    Pedido = d.Cantidad,
                    Pendiente = d.Pendiente,
                    Recibido = d.Pendiente,
                    Lote = d.LoteActual,
                    Vencimiento = d.VencimientoActual
                })
                .ToList());
            dgvDetalle.DataSource = filas;
            txtRemito.Clear();
            txtObservaciones.Clear();
            btnConfirmar.Enabled = orden != null;
            MostrarDatosOrden();
        }

        private void MostrarDatosOrden() =>
            lblDatosOrden.Text = orden == null
                ? string.Empty
                : $"{orden.Numero} · {orden.Proveedor} · {orden.EstadoTexto} · {T("msg.Solicitud", "Solicitud")} {orden.SolicitudNumero}";

        /// <summary>La fecha de vencimiento se escribe dd/MM/yyyy, sin depender de la configuración regional.</summary>
        private void dgvDetalle_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == colVencimiento.Index && e.Value is string texto &&
                DateTime.TryParseExact(texto.Trim(), new[] { "dd/MM/yyyy", "d/M/yyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha))
            {
                e.Value = fecha;
                e.ParsingApplied = true;
            }
        }

        private void dgvDetalle_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Mensajes_60MN.Advertir(e.ColumnIndex == colVencimiento.Index
                ? T("msg.FechaInvalida", "Ingrese la fecha de vencimiento con el formato dd/mm/aaaa.")
                : T("msg.CantidadEnteraCero", "Ingrese una cantidad entera (0 si no llegó)."));
            e.Cancel = true;
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (orden == null) return;
            dgvDetalle.EndEdit();
            Recepcion_60MN recepcion = new Recepcion_60MN
            {
                OrdenCompraId = orden.Id,
                NroRemito = txtRemito.Text,
                Observaciones = txtObservaciones.Text
            };
            foreach (FilaRecepcion_60MN f in filas)
                recepcion.Detalle.Add(new RecepcionDetalle_60MN
                {
                    ProductoId = f.ProductoId,
                    Producto = f.Producto,
                    CantidadRecibida = f.Recibido,
                    Lote = f.Lote,
                    FechaVencimiento = f.Vencimiento,
                    Observacion = f.Observacion
                });
            Confirmar(recepcion, confirmado: false);
        }

        /// <summary>Pasos 2 a 8. Un lote que vence en menos de 90 días pide confirmación.</summary>
        private void Confirmar(Recepcion_60MN recepcion, bool confirmado)
        {
            try
            {
                ResultadoRecepcion_60MN resultado = recepcionBLL.Registrar(recepcion, confirmado);
                string numero = orden!.Numero;
                lblEstado.Text = string.Format(T("msg.RecepcionRegistrada",
                    "Recepción registrada: ingresaron {0} unidad(es) al stock. La orden {1} quedó {2}."),
                    resultado.UnidadesIngresadas, numero, resultado.EstadoOrden.Descripcion().ToLower());
                Mensajes_60MN.Informar(lblEstado.Text);
                // Si quedó parcial se vuelve a mostrar con lo que falta; si se cerró, se limpia.
                MostrarOrden(resultado.EstadoOrden == EstadoOrden_60MN.RecibidaParcial ? recepcionBLL.ObtenerOrdenParaRecibir(numero) : null);
                if (orden == null) txtOrden.Clear();
            }
            catch (ConfirmacionRequeridaException_60MN ex)
            {
                if (Mensajes_60MN.Confirmar(ex.Message))
                    Confirmar(recepcion, confirmado: true);
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }
    }
}
