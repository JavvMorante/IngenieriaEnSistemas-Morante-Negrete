using System.Globalization;
using System.Text;
using DAL_60MN;
using Entidades_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;
using Servicios_60MN.Sesion;

namespace BLL_60MN
{
    /// <summary>CUN08 Registrar Atención, CUN09 Registrar Medicación en Carrito y CUN10 Emitir Informe de Atención.</summary>
    public class AtencionBLL_60MN
    {
        /// <summary>CUN08 4.1: diferencia de peso respecto del último valor que pide confirmación.</summary>
        public const decimal DiferenciaPesoSignificativa = 0.20m;

        /// <summary>CUN09 4.2: días antes del vencimiento en que se advierte "próximo a vencer".</summary>
        public const int DiasProximoAVencer = 30;

        private readonly AtencionDAL_60MN mapper = new AtencionDAL_60MN();
        private readonly HistoriaClinicaDAL_60MN historias = new HistoriaClinicaDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        // =====================================================================
        // CUN08 - Registrar Atención
        // =====================================================================

        /// <summary>Paso 3: profesional de la sesión activa.</summary>
        public static string ProfesionalActual()
        {
            var usuario = SessionManager_60MN.Instancia.Usuario;
            return usuario == null ? string.Empty : $"{usuario.Nombre} {usuario.Apellido}".Trim();
        }

        /// <summary>Flujo 4.1: true si el peso difiere más de un 20% del último registrado en la historia.</summary>
        public bool PesoDifiereSignificativamente(int historiaClinicaId, decimal peso)
        {
            HistoriaClinica_60MN ? hc = historias.ObtenerPorId(historiaClinicaId);//el signo de pregunta es para evitar los valores null

            if (hc == null)
                return false;               // no existe la historia clínica

            if (hc.PesoActual <= 0)
                return false;               // no hay peso anterior para comparar

            decimal diferencia = Math.Abs(peso - hc.PesoActual);
            decimal porcentaje = diferencia / hc.PesoActual;

            return porcentaje > DiferenciaPesoSignificativa;
        }

        public Atencion_60MN Registrar(Atencion_60MN atencion)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.AtencionRegistrar);
            // 1.1: la mascota debe tener historia clínica.
            HistoriaClinica_60MN hc = historias.ObtenerPorId(atencion.HistoriaClinicaId)
                ?? throw new NegocioException_60MN("La mascota no posee historia clínica: créela antes de registrar la atención.");
            //si lo de la izquierda es null, usá lo de la derecha

            atencion.Motivo = (atencion.Motivo ?? string.Empty).Trim();
            atencion.Diagnostico = (atencion.Diagnostico ?? string.Empty).Trim();
            if (atencion.Motivo.Length == 0 || atencion.Diagnostico.Length == 0)
                throw new NegocioException_60MN("Complete el motivo de consulta y el diagnóstico.");
            if (atencion.Peso <= 0 || atencion.Peso > 200)
                throw new NegocioException_60MN("El peso debe ser mayor a 0 y menor a 200 kg.");
            if (atencion.Temperatura < 30 || atencion.Temperatura > 45)
                throw new NegocioException_60MN("La temperatura debe estar entre 30 y 45 °C.");
            if (atencion.FrecuenciaCardiaca < 20 || atencion.FrecuenciaCardiaca > 400)
                throw new NegocioException_60MN("La frecuencia cardíaca debe estar entre 20 y 400 latidos por minuto.");

            atencion.FechaHora = DateTime.Now;
            atencion.UsuarioId = SesionActual_60MN.UsuarioId;
            atencion.Profesional = ProfesionalActual();
            bool cambiaPeso = atencion.Peso != hc.PesoActual;

            atencion.Id = mapper.Insertar(atencion);
            dv.ActualizarFila(TablaDV_60MN.Atencion, atencion.Id);
            if (cambiaPeso)
                dv.ActualizarFila(TablaDV_60MN.HistoriaClinica, hc.Id);

            bitacora.Registrar(EventoSistema_60MN.AtencionRegistrada,
                $"Atención Nº {atencion.Id} en la historia {hc.Numero}: {atencion.Motivo}" +
                (cambiaPeso ? $" (peso {hc.PesoActual:0.##} → {atencion.Peso:0.##} kg)." : "."));
            return mapper.ObtenerPorId(atencion.Id)!;
        }

        public Atencion_60MN? ObtenerAtencion(int atencionId) => mapper.ObtenerPorId(atencionId);

        // =====================================================================
        // CUN09 - Registrar Medicación en Carrito («include» CUN14)
        // =====================================================================

        /// <summary>Atenciones de los últimos días para asociar la medicación (VENTAS > Carrito).</summary>
        public List<Atencion_60MN> ListarAtencionesRecientes(int dias = 30)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return mapper.ListarRecientes(dias);
        }

        /// <summary>Pasos 3 y 4: productos activos con stock, precio y vencimiento.</summary>
        public List<Producto_60MN> BuscarProductos(string? texto, int? categoriaId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            return new ProductoDAL_60MN().Buscar(texto, categoriaId, incluirInactivos: false);
        }

        /// <summary>
        /// Paso 9: genera los egresos de stock y asocia el detalle a la atención en una
        /// sola transacción. Devuelve los productos que quedaron con alerta de reposición.
        /// </summary>
        public List<string> RegistrarMedicacion(int atencionId, IList<ItemCarrito_60MN> items)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.Carrito);
            Atencion_60MN atencion = mapper.ObtenerPorId(atencionId) ?? throw new NegocioException_60MN("La atención no existe.");
            if (items.Count == 0)
                throw new NegocioException_60MN("El carrito está vacío.");

            ProductoDAL_60MN productos = new ProductoDAL_60MN();
            foreach (ItemCarrito_60MN item in items)
            {
                Producto_60MN p = productos.ObtenerPorId(item.ProductoId) ?? throw new NegocioException_60MN("Un producto del carrito ya no existe.");
                // 5.1: cantidad menor o igual a cero.
                if (item.Cantidad <= 0)
                    throw new NegocioException_60MN($"La cantidad de {p.Nombre} debe ser mayor a cero.");
                if (!p.Activo)
                    throw new NegocioException_60MN($"El producto {p.Codigo} está dado de baja.");
                // 4.2: no se entrega medicación vencida.
                if (p.Vencido)
                    throw new NegocioException_60MN($"El producto {p.Codigo} \"{p.Nombre}\" (lote {p.Lote}) está vencido.");
                item.PrecioUnitario = p.PrecioVenta;
                item.Codigo = p.Codigo;
                item.Producto = p.Nombre;
                item.Lote = p.Lote;
            }
            // 4.1: la suma por producto no puede superar el stock (también se controla dentro de la transacción).
            foreach (var grupo in items.GroupBy(i => i.ProductoId))
            {
                Producto_60MN p = productos.ObtenerPorId(grupo.Key)!;
                int total = grupo.Sum(i => i.Cantidad);
                if (total > p.StockActual)
                    throw new NegocioException_60MN($"Stock insuficiente de {p.Nombre}: disponible {p.StockActual}, solicitado {total}.");
            }

            List<EntregaCarrito_60MN> entregas;
            try
            {
                entregas = mapper.RegistrarCarrito(atencionId, items, SesionActual_60MN.UsuarioId,
                    $"Medicación entregada en la atención Nº {atencionId}");
            }
            catch (InvalidOperationException ex)
            {
                throw new NegocioException_60MN(ex.Message);
            }

            ProductoBLL_60MN productoBLL = new ProductoBLL_60MN();
            List<string> alertas = new List<string>();
            foreach (EntregaCarrito_60MN entrega in entregas)
            {
                dv.ActualizarFila(TablaDV_60MN.MovimientoStock, entrega.MovimientoId);
                dv.ActualizarFila(TablaDV_60MN.AtencionDetalle, entrega.DetalleId);
                dv.ActualizarFila(TablaDV_60MN.Productos, entrega.ProductoId);
            }
            foreach (int productoId in entregas.Select(e => e.ProductoId).Distinct())
            {
                Producto_60MN p = productos.ObtenerPorId(productoId)!;
                // Postcondición: si alguna existencia alcanza el stock mínimo se dispara la alerta (sin duplicarla).
                if (productoBLL.EvaluarAlerta(p) || p.BajoStockMinimo)
                    alertas.Add($"{p.Nombre} (stock {p.StockActual}, mínimo {p.StockMinimo})");
            }

            bitacora.Registrar(EventoSistema_60MN.MedicacionEntregada,
                $"Medicación de la atención Nº {atencionId} ({atencion.Paciente}): " +
                string.Join(", ", items.Select(i => $"{i.Cantidad} x {i.Producto}")) +
                $". Total $ {items.Sum(i => i.Subtotal).ToString("N2", CultureInfo.GetCultureInfo("es-AR"))}.");
            return alertas;
        }

        // =====================================================================
        // CUN10 - Emitir Informe de Atención
        // =====================================================================

        /// <summary>Paso 2: compone el informe con paciente, propietario, fecha, profesional, motivo, diagnóstico y medicación.</summary>
        public string ComponerInforme(int atencionId)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.InformeAtencion);
            Atencion_60MN a = mapper.ObtenerPorId(atencionId) ?? throw new NegocioException_60MN("La atención no existe.");

            // 3.1: diagnóstico incompleto.
            List<string> faltantes = new List<string>();
            if (string.IsNullOrWhiteSpace(a.Motivo)) faltantes.Add("motivo de consulta");
            if (string.IsNullOrWhiteSpace(a.Diagnostico)) faltantes.Add("diagnóstico");
            if (faltantes.Count > 0)
                throw new NegocioException_60MN("No se puede emitir el informe. Faltan: " + string.Join(", ", faltantes) + ".");

            HistoriaClinica_60MN hc = new HistoriaClinicaBLL_60MN().ObtenerConCabecera(historias.ObtenerPorId(a.HistoriaClinicaId)!.MascotaId)!;
            CultureInfo ar = CultureInfo.GetCultureInfo("es-AR");
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("VETCARE - INFORME DE ATENCIÓN");
            sb.AppendLine(new string('=', 60));
            sb.AppendLine($"Atención Nº:     {a.Id}");
            sb.AppendLine($"Fecha y hora:    {a.FechaHora:dd/MM/yyyy HH:mm}");
            sb.AppendLine($"Profesional:     {a.Profesional}");
            sb.AppendLine($"Historia:        {hc.Numero}");
            sb.AppendLine();
            sb.AppendLine($"Paciente:        {hc.Mascota.Nombre} ({hc.Mascota.Especie}, {hc.Mascota.Raza}, {hc.Mascota.SexoTexto})");
            sb.AppendLine($"Propietario:     {hc.Propietario.NombreCompleto} - {hc.Propietario.Documento}");
            sb.AppendLine($"Teléfono:        {hc.Propietario.Telefono}");
            sb.AppendLine($"Alergias:        {hc.Alergias}");
            sb.AppendLine();
            sb.AppendLine($"Signos vitales:  peso {a.Peso:0.##} kg · temperatura {a.Temperatura:0.0} °C · FC {a.FrecuenciaCardiaca} lpm");
            sb.AppendLine();
            sb.AppendLine("Motivo de consulta:");
            sb.AppendLine("  " + a.Motivo);
            sb.AppendLine();
            sb.AppendLine("Diagnóstico e indicaciones:");
            sb.AppendLine("  " + a.Diagnostico);
            sb.AppendLine();
            sb.AppendLine("Medicación prescripta:");
            if (a.Medicacion.Count == 0)
                sb.AppendLine("  (sin medicación)");
            foreach (ItemCarrito_60MN item in a.Medicacion)
                sb.AppendLine($"  {item.Cantidad,3} x {item.Producto} (lote {item.Lote})  $ {item.Subtotal.ToString("N2", ar)}");
            if (a.Medicacion.Count > 0)
                sb.AppendLine($"  Total: $ {a.Medicacion.Sum(i => i.Subtotal).ToString("N2", ar)}");
            sb.AppendLine(new string('=', 60));
            sb.AppendLine($"Emitido el {DateTime.Now:dd/MM/yyyy HH:mm} por {SesionActual_60MN.NombreUsuario}" +
                          (a.InformesEmitidos > 0 ? $" (reimpresión Nº {a.InformesEmitidos})" : string.Empty));
            return sb.ToString();
        }

        /// <summary>Paso 5 / postcondición: la emisión (o reimpresión, flujo 1.1) queda en la bitácora.</summary>
        public void RegistrarEmisionInforme(int atencionId, string destino)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.InformeAtencion);
            Atencion_60MN a = mapper.ObtenerPorId(atencionId) ?? throw new NegocioException_60MN("La atención no existe.");
            mapper.IncrementarInformes(atencionId);
            dv.ActualizarFila(TablaDV_60MN.Atencion, atencionId);
            bitacora.Registrar(EventoSistema_60MN.InformeAtencionEmitido,
                $"{(a.InformesEmitidos == 0 ? "Emisión" : $"Reimpresión Nº {a.InformesEmitidos}")} del informe de la atención Nº {atencionId} ({destino}).");
        }
    }
}
