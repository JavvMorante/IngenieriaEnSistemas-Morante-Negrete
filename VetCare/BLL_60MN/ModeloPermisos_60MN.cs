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

        private ModeloPermisos_60MN(Dictionary<int, Patente_60MN> patentes, Dictionary<int, Familia_60MN> familias,
                                    Dictionary<int, PermisosUsuario_60MN> asignaciones)
        {
            Patentes = patentes;
            Familias = familias;
            Asignaciones = asignaciones;
        }

        public static ModeloPermisos_60MN Cargar()
        {
            PermisoDAL_60MN mapper = new PermisoDAL_60MN();
            Dictionary<int, Patente_60MN> patentes = mapper.CargarPatentes();
            return new ModeloPermisos_60MN(patentes, mapper.CargarFamilias(patentes), mapper.CargarPermisosDeUsuarios());
        }

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
            Familia_60MN familia = Familias[familiaId];
            familia.ObtenerHijos().Clear();
            foreach (int patente in patentes) familia.ObtenerHijos().Add(Patentes[patente]);
            foreach (int hija in subfamilias) familia.ObtenerHijos().Add(Familias[hija]);
        }
    }
}
