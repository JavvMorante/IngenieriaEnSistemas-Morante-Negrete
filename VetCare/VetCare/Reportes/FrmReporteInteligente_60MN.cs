using System.Globalization;
using System.Text;
using BLL_60MN;
using Entidades_60MN;

namespace VetCare
{
    /// <summary>
    /// Reporte Inteligente. Muestra en una sola pantalla los dos gráficos que cruzan
    /// los requerimientos funcionales del trabajo: la demanda estacional de insumos por
    /// categoría (RF2 Gestión de Stock) y el ranking de productos entregados en
    /// atenciones (RF1 Gestión de Pacientes). Los gráficos se dibujan con GDI+ en el
    /// evento Paint de cada panel, sin librerías externas.
    /// </summary>
    public partial class FrmReporteInteligente_60MN : FormBase_60MN
    {
        private readonly ReporteInteligenteBLL_60MN reporteBLL = new ReporteInteligenteBLL_60MN();
        private ResultadoReporteInteligente_60MN? resultado;
        private string criterios = string.Empty;

        public FrmReporteInteligente_60MN()
        {
            InitializeComponent();
        }

        private static string T(string clave, string porDefecto) => TraductorFormularios_60MN.T(clave, porDefecto);

        private string TituloDemanda => T("msg.DemandaPorCategoria", "Demanda de insumos por mes y categoría (unidades egresadas)");

        private string TituloTop => T("msg.TopProductosEntregados", "Productos más entregados en atenciones");

        /// <summary>Paso 2: el período arranca en los últimos doce meses.</summary>
        private void FrmReporteInteligente_60MN_Load(object sender, EventArgs e)
        {
            numTop.Minimum = 1;
            numTop.Maximum = ReporteInteligenteBLL_60MN.TopMaximo;
            numTop.Value = ReporteInteligenteBLL_60MN.TopPorDefecto;

            DateTime hoy = DateTime.Today;
            dtpHasta.Value = hoy;
            dtpDesde.Value = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-11);
        }

        /// <summary>Al cambiar el idioma los títulos viven dentro del dibujo: hay que repintar.</summary>
        protected override void AlCambiarIdioma()
        {
            pnlDemanda.Invalidate();
            pnlTop.Invalidate();
            if (resultado != null) MostrarResumen();
        }

        /// <summary>Paso 3: la BLL valida el rango y registra el evento en la bitácora.</summary>
        private void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                resultado = reporteBLL.Generar(dtpDesde.Value.Date, dtpHasta.Value.Date, (int)numTop.Value);
                criterios = $"{lblDesde.Text} {resultado.Desde:dd/MM/yyyy} {lblHasta.Text} {resultado.Hasta:dd/MM/yyyy} · " +
                            $"{lblTop.Text} {resultado.TopProductos.Count}";
                MostrarResumen();
                bool hayDatos = !resultado.SinDatos;
                btnImprimir.Enabled = btnExportar.Enabled = hayDatos;
                pnlDemanda.Invalidate();
                pnlTop.Invalidate();
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Paso 5: totales del período.</summary>
        private void MostrarResumen()
        {
            if (resultado == null) return;
            if (resultado.SinDatos)
            {
                lblResumen.Text = T("msg.SinMovimientos", "No se encontraron movimientos en el período indicado.");
                return;
            }
            CultureInfo ar = CultureInfo.GetCultureInfo("es-AR");
            lblResumen.Text = string.Format(
                T("msg.ResumenReporteInteligente", "{0} unidad(es) egresada(s) · {1} unidad(es) entregada(s) en atenciones · $ {2}"),
                resultado.UnidadesEgresadas, resultado.UnidadesEntregadas, resultado.ImporteEntregado.ToString("N2", ar));
        }

        /// <summary>Gráfico de demanda estacional: una barra apilada por mes.</summary>
        private void pnlDemanda_Paint(object sender, PaintEventArgs e)
        {
            Rectangle area = Rectangle.Inflate(pnlDemanda.ClientRectangle, -10, -8);
            if (resultado == null || resultado.Demanda.Count == 0)
            {
                Graficos_60MN.Vacio(e.Graphics, area, TituloDemanda,
                    T("msg.GenerarParaVer", "Elija un período y presione Generar."));
                return;
            }

            List<string> periodos = resultado.Periodos;
            List<string> categorias = resultado.Categorias;
            int[,] valores = new int[periodos.Count, categorias.Count];
            for (int p = 0; p < periodos.Count; p++)
                for (int c = 0; c < categorias.Count; c++)
                    valores[p, c] = resultado.Cantidad(periodos[p], categorias[c]);

            Graficos_60MN.BarrasApiladas(e.Graphics, area, TituloDemanda, periodos, categorias, valores);
        }

        /// <summary>Ranking de productos: barras horizontales de mayor a menor.</summary>
        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {
            Rectangle area = Rectangle.Inflate(pnlTop.ClientRectangle, -10, -8);
            if (resultado == null || resultado.TopProductos.Count == 0)
            {
                Graficos_60MN.Vacio(e.Graphics, area, TituloTop,
                    T("msg.GenerarParaVer", "Elija un período y presione Generar."));
                return;
            }

            List<string> etiquetas = resultado.TopProductos.Select(p => p.Nombre).ToList();
            List<int> valores = resultado.TopProductos.Select(p => p.Cantidad).ToList();
            List<string> detalles = resultado.TopProductos
                .Select(p => $"{p.Categoria} · {p.Atenciones} atención(es)").ToList();

            Graficos_60MN.BarrasHorizontales(e.Graphics, area, TituloTop, etiquetas, valores, detalles);
        }

        /// <summary>Un panel redimensionado tiene que volver a dibujarse completo.</summary>
        private void Panel_Resize(object sender, EventArgs e) => ((Control)sender).Invalidate();

        /// <summary>Paso 6: impresión o PDF. Se imprimen los datos, no la imagen.</summary>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (resultado == null) return;
            CultureInfo ar = CultureInfo.GetCultureInfo("es-AR");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VETCARE - REPORTE INTELIGENTE");
            sb.AppendLine(criterios);
            sb.AppendLine($"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}");

            sb.AppendLine();
            sb.AppendLine("DEMANDA DE INSUMOS POR MES Y CATEGORIA");
            sb.AppendLine(new string('=', 96));
            sb.AppendLine($"{"Período",-12}{"Categoría",-46}{"Unidades",10}");
            sb.AppendLine(new string('-', 96));
            foreach (DemandaMensual_60MN d in resultado.Demanda.OrderBy(x => x.ClavePeriodo).ThenBy(x => x.Categoria))
            {
                string categoria = d.Categoria.Length > 45 ? d.Categoria.Substring(0, 45) : d.Categoria;
                sb.AppendLine($"{d.Periodo,-12}{categoria,-46}{d.Cantidad,10}");
            }
            sb.AppendLine(new string('-', 96));
            sb.AppendLine($"{"Total",-58}{resultado.UnidadesEgresadas,10}");

            sb.AppendLine();
            sb.AppendLine("PRODUCTOS MAS ENTREGADOS EN ATENCIONES");
            sb.AppendLine(new string('=', 96));
            sb.AppendLine($"{"Código",-10}{"Nombre",-32}{"Categoría",-22}{"Cant.",7}{"Atenc.",8}{"Importe",13}");
            sb.AppendLine(new string('-', 96));
            foreach (ProductoEntregado_60MN p in resultado.TopProductos)
            {
                string nombre = p.Nombre.Length > 31 ? p.Nombre.Substring(0, 31) : p.Nombre;
                string categoria = p.Categoria.Length > 21 ? p.Categoria.Substring(0, 21) : p.Categoria;
                sb.AppendLine($"{p.Codigo,-10}{nombre,-32}{categoria,-22}{p.Cantidad,7}{p.Atenciones,8}" +
                              $"{p.Importe.ToString("N2", ar),13}");
            }
            sb.AppendLine(new string('-', 96));
            sb.AppendLine($"{"Total",-64}{resultado.UnidadesEntregadas,7}" +
                          $"{string.Empty,8}{resultado.ImporteEntregado.ToString("N2", ar),13}");
            sb.AppendLine(new string('=', 96));
            sb.AppendLine(lblResumen.Text);

            try
            {
                if (ImpresionTexto_60MN.Imprimir(this, "Reporte inteligente", sb.ToString()) != null)
                    Mensajes_60MN.Informar("Reporte enviado a la impresora.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        /// <summary>Exportación a CSV: las dos series en un único archivo, identificadas por bloque.</summary>
        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (resultado == null) return;
            using SaveFileDialog dialogo = new SaveFileDialog
            {
                FileName = $"ReporteInteligente_{DateTime.Now:yyyyMMdd_HHmm}.csv",
                Filter = "CSV (*.csv)|*.csv",
                DefaultExt = "csv"
            };
            if (dialogo.ShowDialog(this) != DialogResult.OK) return;

            CultureInfo ar = CultureInfo.GetCultureInfo("es-AR");
            static string Campo(string valor) => "\"" + valor.Replace("\"", "\"\"") + "\"";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Bloque;Periodo;Categoria;Codigo;Nombre;Cantidad;Atenciones;Importe");
            foreach (DemandaMensual_60MN d in resultado.Demanda.OrderBy(x => x.ClavePeriodo).ThenBy(x => x.Categoria))
                sb.AppendLine(string.Join(";", "DEMANDA", d.Periodo, Campo(d.Categoria), string.Empty, string.Empty,
                    d.Cantidad, string.Empty, string.Empty));
            foreach (ProductoEntregado_60MN p in resultado.TopProductos)
                sb.AppendLine(string.Join(";", "TOP", string.Empty, Campo(p.Categoria), Campo(p.Codigo), Campo(p.Nombre),
                    p.Cantidad, p.Atenciones, p.Importe.ToString("0.00", ar)));
            try
            {
                File.WriteAllText(dialogo.FileName, sb.ToString(), new UTF8Encoding(true));
                Mensajes_60MN.Informar("Reporte exportado correctamente.");
            }
            catch (Exception ex)
            {
                Mensajes_60MN.MostrarError(ex);
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e) => Close();
    }
}
