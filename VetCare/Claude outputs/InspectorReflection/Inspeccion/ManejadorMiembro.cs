using System.Reflection;

namespace InspectorReflection.Inspeccion
{
    public abstract class ManejadorMiembro
    {
        private ManejadorMiembro _siguiente;

        public ManejadorMiembro EstablecerSiguiente(ManejadorMiembro siguiente)
        {
            _siguiente = siguiente;
            return siguiente;
        }

        public InfoMiembro Procesar(MemberInfo miembro)
        {
            if (PuedeProcesar(miembro))
            {
                return ObtenerInformacion(miembro);
            }

            return _siguiente?.Procesar(miembro);
        }

        protected abstract bool PuedeProcesar(MemberInfo miembro);

        protected abstract InfoMiembro ObtenerInformacion(MemberInfo miembro);
    }
}
