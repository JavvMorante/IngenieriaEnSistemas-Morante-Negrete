using DAL_60MN;
using Interfaces_60MN;
using Seguridad_60MN.Auditoria;
using Seguridad_60MN.Integridad;
using Servicios_60MN.Composite;
using Servicios_60MN.Excepciones;

namespace BLL_60MN
{
    /// <summary>
    /// Modelo de permisos Usuario - Familia - Patente.
    ///  - CUS04 Gestionar Familias y Permisos: una familia (tipo de usuario, p. ej.
    ///    Vendedor) agrupa patentes y opcionalmente otras familias.
    ///  - Asignación a un usuario: una o más familias + patentes individuales
    ///    que se suman a las de su familia.
    /// </summary>
    public class PermisoBLL_60MN
    {
        private readonly PermisoDAL_60MN mapper = new PermisoDAL_60MN();
        private readonly GestorDigitoVerificador_60MN dv = new GestorDigitoVerificador_60MN();
        private readonly BitacoraBLL_60MN bitacora = new BitacoraBLL_60MN();

        public List<Familia_60MN> ListarFamilias() =>
            ModeloPermisos_60MN.Cargar().Familias.Values.OrderBy(f => f.Nombre).ToList();

        public List<Patente_60MN> ListarPatentes() =>
            mapper.CargarPatentes().Values.OrderBy(p => p.Id).ToList();

        public int ContarUsuarios(int familiaId) => mapper.ContarUsuariosConFamilia(familiaId, soloActivos: true);

        /// <summary>Permisos del usuario (familias + patentes individuales), con los árboles de cada familia.</summary>
        public List<IPermiso_60MN> ObtenerPermisosUsuario(int usuarioId) => ModeloPermisos_60MN.Cargar().PermisosDe(usuarioId);

        // =====================================================================
        // CUS04 - Familias
        // =====================================================================

        /// <summary>
        /// Crea (id = 0) o modifica una familia con sus patentes y subfamilias.
        /// Flujos 3.1 (nombre repetido), ciclos y 6.2 (no dejar el sistema sin
        /// administrador). La advertencia 6.1 (usuarios impactados) la muestra la
        /// GUI con ContarUsuarios antes de llamar a este método.
        /// </summary>
        public int GuardarFamilia(int id, string nombre, IList<int> patentes, IList<int> subfamilias)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.RolesPermisos);

            nombre = (nombre ?? string.Empty).Trim();
            if (nombre.Length == 0)
                throw new NegocioException_60MN("Debe ingresar el nombre de la familia.");
            if (mapper.ExisteNombreFamilia(nombre, id))
                throw new NegocioException_60MN("Ya existe una familia con ese nombre. Ingrese otro nombre.");
            if (id != 0 && subfamilias.Contains(id))
                throw new NegocioException_60MN("Una familia no puede contenerse a sí misma.");
            ExigirTablasIntegras(TablaDV_60MN.Familia, TablaDV_60MN.FamiliaPatente, TablaDV_60MN.FamiliaFamilia);

            ModeloPermisos_60MN modelo = ModeloPermisos_60MN.Cargar();
            if (patentes.Any(p => !modelo.Patentes.ContainsKey(p)) || subfamilias.Any(f => !modelo.Familias.ContainsKey(f)))
                throw new NegocioException_60MN("Uno de los permisos seleccionados ya no existe. Actualice la pantalla.");

            if (id != 0)
            {
                if (!modelo.Familias.ContainsKey(id))
                    throw new NegocioException_60MN("La familia seleccionada ya no existe. Actualice la pantalla.");

                // Ninguna subfamilia puede contener (directa o indirectamente) a esta familia.
                foreach (int hija in subfamilias)
                    if (modelo.Familias[hija].ContieneFamilia(id))
                        throw new NegocioException_60MN($"No se puede incluir \"{modelo.Familias[hija].Nombre}\" porque ya contiene a esta familia (se generaría un ciclo).");

                // 6.2: simular el cambio y verificar que siga existiendo un administrador operativo.
                modelo.SimularComposicion(id, patentes, subfamilias);
                if (!new UsuarioBLL_60MN().ExisteAdministradorOperativo(modelo))
                    throw new NegocioException_60MN(
                        "El cambio quitaría los permisos administrativos al último administrador activo. Operación cancelada.");
            }

            bool esAlta = id == 0;
            if (esAlta)
                id = mapper.InsertarFamilia(nombre);
            else
                mapper.RenombrarFamilia(id, nombre);
            dv.ActualizarFila(TablaDV_60MN.Familia, id);

            mapper.ReemplazarComposicion(id, patentes, subfamilias);
            dv.RecalcularTabla(TablaDV_60MN.FamiliaPatente);
            dv.RecalcularTabla(TablaDV_60MN.FamiliaFamilia);

            string detalle = string.Join(", ", patentes.Select(p => modelo.Patentes[p].Codigo)
                                               .Concat(subfamilias.Select(f => "[" + modelo.Familias[f].Nombre + "]")));
            bitacora.Registrar(esAlta ? EventoSistema_60MN.RolCreado : EventoSistema_60MN.RolModificado,
                $"Familia \"{nombre}\" (Id {id}) con permisos: {detalle}.");
            return id;
        }

        public void EliminarFamilia(int id)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.RolesPermisos);
            ExigirTablasIntegras(TablaDV_60MN.Familia, TablaDV_60MN.FamiliaPatente, TablaDV_60MN.FamiliaFamilia);

            if (mapper.ContarUsuariosConFamilia(id, soloActivos: false) > 0)
                throw new NegocioException_60MN("No se puede eliminar la familia porque está asignada a usuarios.");
            ModeloPermisos_60MN modelo = ModeloPermisos_60MN.Cargar();
            Familia_60MN? padre = modelo.Familias.Values.FirstOrDefault(f => f.FamiliasDirectas.Any(h => h.Id == id));
            if (padre != null)
                throw new NegocioException_60MN($"No se puede eliminar la familia porque forma parte de la familia \"{padre.Nombre}\".");

            string nombre = modelo.Familias.TryGetValue(id, out Familia_60MN? familia) ? familia.Nombre : id.ToString();
            mapper.EliminarFamilia(id);
            dv.RecalcularTabla(TablaDV_60MN.Familia);
            dv.RecalcularTabla(TablaDV_60MN.FamiliaPatente);
            dv.RecalcularTabla(TablaDV_60MN.FamiliaFamilia);
            bitacora.Registrar(EventoSistema_60MN.RolEliminado, $"Familia \"{nombre}\" (Id {id}) eliminada.");
        }

        // =====================================================================
        // Asignación de familias y patentes individuales a un usuario
        // =====================================================================

        /// <summary>
        /// Asigna a un usuario su(s) familia(s) (tipo de usuario) y las patentes
        /// individuales que tiene además de las de su familia.
        /// </summary>
        public void AsignarPermisosUsuario(int usuarioId, IList<int> familias, IList<int> patentes)
        {
            SesionActual_60MN.RequierePermiso(CodigosPatente_60MN.UsuarioModificar);

            // CUS02 5.1: nadie puede modificar sus propios permisos.
            if (usuarioId == SesionActual_60MN.UsuarioId)
                throw new NegocioException_60MN("No puede modificar sus propios permisos.");
            if (familias.Count == 0)
                throw new NegocioException_60MN("El usuario debe tener asignada al menos una familia (tipo de usuario).");
            ExigirTablasIntegras(TablaDV_60MN.UsuarioFamilia, TablaDV_60MN.UsuarioPatente);

            ModeloPermisos_60MN modelo = ModeloPermisos_60MN.Cargar();
            if (patentes.Any(p => !modelo.Patentes.ContainsKey(p)) || familias.Any(f => !modelo.Familias.ContainsKey(f)))
                throw new NegocioException_60MN("Uno de los permisos seleccionados ya no existe. Actualice la pantalla.");

            modelo.SimularAsignacion(usuarioId, familias, patentes);
            if (!new UsuarioBLL_60MN().ExisteAdministradorOperativo(modelo))
                throw new NegocioException_60MN("El cambio dejaría al sistema sin ningún administrador activo.");

            GuardarAsignacion(usuarioId, familias, patentes);

            string detalle = "familias: " + string.Join(", ", familias.Select(f => modelo.Familias[f].Nombre)) +
                             "; patentes individuales: " + (patentes.Count == 0 ? "(ninguna)" : string.Join(", ", patentes.Select(p => modelo.Patentes[p].Codigo)));
            bitacora.Registrar(EventoSistema_60MN.PermisosUsuarioModificados, $"Permisos del usuario Id {usuarioId} → {detalle}.");
        }

        /// <summary>Persiste la asignación y recalcula los dígitos verificadores (sin validaciones; uso interno).</summary>
        internal void GuardarAsignacion(int usuarioId, IEnumerable<int> familias, IEnumerable<int> patentes)
        {
            mapper.AsignarPermisosUsuario(usuarioId, familias, patentes);
            dv.RecalcularTabla(TablaDV_60MN.UsuarioFamilia);
            dv.RecalcularTabla(TablaDV_60MN.UsuarioPatente);
        }

        /// <summary>
        /// Las operaciones que borran filas recalculan la tabla completa, por lo
        /// que solo se permiten si no hay alteraciones previas sin revisar.
        /// </summary>
        internal void ExigirTablasIntegras(params string[] tablas)
        {
            if (tablas.Any(t => !dv.TablaConsistente(t)))
                throw new NegocioException_60MN(
                    "Las tablas de permisos presentan inconsistencias en sus dígitos verificadores. " +
                    "Revíselas desde ADMIN > Dígitos verificadores antes de modificar permisos.");
        }
    }
}
