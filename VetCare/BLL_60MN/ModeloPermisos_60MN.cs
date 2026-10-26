using DAL_60MN;
using Interfaces_60MN;
using Servicios_60MN.Composite;

namespace BLL_60MN
{
    /// <summary>
    /// Foto en memoria del modelo Usuario - Familia - Patente: todas las patentes,
    /// las familias con su árbol (Composite) y las asignaciones de cada usuario.
    /// Se usa para armar los permisos de la sesión y para simular un cambio
    /// antes de persistirlo (por ejemplo, que no quede ningún administrador).
    /// </summary>
    public class ModeloPermisos_60MN
    {
        public Dictionary<int, Patente_60MN> Patentes { get; }

        public Dictionary<int, Familia_60MN> Familias { get; }

        public Dictionary<int, PermisosUsuario_60MN> Asignaciones { get; }

        /// <summary>
        /// Relaciones FamiliaFamilia de la base que se descartaron al cargar porque
        /// formaban un ciclo (la tabla fue modificada por fuera del sistema).
        /// </summary>
        public IReadOnlyList<(int Padre, int Hija)> RelacionesConCiclo { get; }

        private ModeloPermisos_60MN(Dictionary<int, Patente_60MN> patentes, Dictionary<int, Familia_60MN> familias,
                                    Dictionary<int, PermisosUsuario_60MN> asignaciones, IReadOnlyList<(int, int)> relacionesConCiclo)
        {
            Patentes = patentes;
            Familias = familias;
            Asignaciones = asignaciones;
            RelacionesConCiclo = relacionesConCiclo;
        }

        public static ModeloPermisos_60MN Cargar()
        {
            PermisoDAL_60MN mapper = new PermisoDAL_60MN();
            Dictionary<int, Patente_60MN> patentes = mapper.CargarPatentes();
            Dictionary<int, Familia_60MN> familias = mapper.CargarFamilias(patentes);
            return new ModeloPermisos_60MN(patentes, familias, mapper.CargarPermisosDeUsuarios(), mapper.RelacionesConCiclo.ToList());
        }

        /// <summary>Texto de las relaciones descartadas, para la bitácora ("Administrador → Vendedor").</summary>
        public string DescribirRelacionesConCiclo() =>
            string.Join(", ", RelacionesConCiclo.Select(r =>
                $"{(Familias.TryGetValue(r.Padre, out Familia_60MN? p) ? p.Nombre : r.Padre.ToString())} → " +
                $"{(Familias.TryGetValue(r.Hija, out Familia_60MN? h) ? h.Nombre : r.Hija.ToString())}"));

        /// <summary>Permisos directos del usuario: sus familias (con su árbol) y sus patentes individuales.</summary>
        public List<IPermiso_60MN> PermisosDe(int usuarioId)
        {
            List<IPermiso_60MN> permisos = new List<IPermiso_60MN>();
            if (!Asignaciones.TryGetValue(usuarioId, out PermisosUsuario_60MN? asignacion))
                return permisos;
            foreach (int familia in asignacion.Familias)
                if (Familias.TryGetValue(familia, out Familia_60MN? f)) permisos.Add(f);
            foreach (int patente in asignacion.Patentes)
                if (Patentes.TryGetValue(patente, out Patente_60MN? p)) permisos.Add(p);
            return permisos;
        }

        public string NombresFamiliasDe(int usuarioId) =>
            string.Join(", ", PermisosDe(usuarioId).OfType<Familia_60MN>().Select(f => f.Nombre).OrderBy(n => n));

        public int CantidadPatentesIndividualesDe(int usuarioId) =>
            Asignaciones.TryGetValue(usuarioId, out PermisosUsuario_60MN? a) ? a.Patentes.Count : 0;

        /// <summary>Simula (solo en memoria) una nueva asignación para un usuario.</summary>
        public void SimularAsignacion(int usuarioId, IEnumerable<int> familias, IEnumerable<int> patentes)
        {
            PermisosUsuario_60MN nueva = new PermisosUsuario_60MN();
            nueva.Familias.AddRange(familias);
            nueva.Patentes.AddRange(patentes);
            Asignaciones[usuarioId] = nueva;
        }

        /// <summary>Simula (solo en memoria) una nueva composición para una familia existente.</summary>
        public void SimularComposicion(int familiaId, IEnumerable<int> patentes, IEnumerable<int> subfamilias)
        {
            // ReemplazarHijos valida cada hijo: si la composición generara un ciclo, lanza CicloFamiliasException_60MN.
            Familia_60MN familia = Familias[familiaId];
            familia.ReemplazarHijos(patentes.Select(p => (IPermiso_60MN)Patentes[p])
                                            .Concat(subfamilias.Select(h => (IPermiso_60MN)Familias[h])));
        }
    }
}
