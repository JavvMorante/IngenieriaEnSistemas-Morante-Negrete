using System.Reflection;
using System.Runtime.CompilerServices;

namespace InspectorReflection.Inspeccion
{
    public class ManejadorAtributo : ManejadorMiembro
    {
        protected override bool PuedeProcesar(MemberInfo miembro)
        {

            return miembro is FieldInfo
                && !miembro.IsDefined(typeof(CompilerGeneratedAttribute), false);
        }

        protected override InfoMiembro ObtenerInformacion(MemberInfo miembro)
        {
            FieldInfo campo = (FieldInfo)miembro;

            return new InfoMiembro
            {
                Nombre = campo.Name,
                Categoria = "Atributo",
                TipoDato = campo.FieldType.Name
            };
        }
    }
}
