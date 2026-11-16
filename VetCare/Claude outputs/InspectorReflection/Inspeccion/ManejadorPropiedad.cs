using System.Reflection;

namespace InspectorReflection.Inspeccion
{

    public class ManejadorPropiedad : ManejadorMiembro
    {
        protected override bool PuedeProcesar(MemberInfo miembro)
        {
            return miembro is PropertyInfo;
        }

        protected override InfoMiembro ObtenerInformacion(MemberInfo miembro)
        {
            PropertyInfo propiedad = (PropertyInfo)miembro;

            return new InfoMiembro
            {
                Nombre = propiedad.Name,
                Categoria = "Propiedad",
                TipoDato = propiedad.PropertyType.Name
            };
        }
    }
}
