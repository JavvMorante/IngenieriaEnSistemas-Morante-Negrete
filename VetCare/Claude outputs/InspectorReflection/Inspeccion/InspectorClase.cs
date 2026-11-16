using System;
using System.Collections.Generic;
using System.Reflection;

namespace InspectorReflection.Inspeccion
{

    public class InspectorClase
    {
        private const BindingFlags Filtro =
            BindingFlags.Instance | BindingFlags.Public |
            BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private readonly ManejadorMiembro _cadena;

        public InspectorClase()
        {
            _cadena = new ManejadorAtributo();
            _cadena.EstablecerSiguiente(new ManejadorPropiedad())
                   .EstablecerSiguiente(new ManejadorMetodo());
        }

        public List<InfoMiembro> Inspeccionar(Type tipo)
        {
            List<InfoMiembro> resultado = new List<InfoMiembro>();

            foreach (MemberInfo miembro in tipo.GetMembers(Filtro))
            {
                InfoMiembro info = _cadena.Procesar(miembro);

                if (info != null)
                {
                    resultado.Add(info);
                }
            }

            return resultado;
        }
    }
}
