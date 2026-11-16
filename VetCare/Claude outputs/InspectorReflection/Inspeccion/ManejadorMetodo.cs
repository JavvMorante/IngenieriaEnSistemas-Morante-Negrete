using System.Reflection;

namespace InspectorReflection.Inspeccion
{
    public class ManejadorMetodo : ManejadorMiembro
    {
        protected override bool PuedeProcesar(MemberInfo miembro)
        {

            return miembro is MethodInfo metodo && !metodo.IsSpecialName;
        }

        protected override InfoMiembro ObtenerInformacion(MemberInfo miembro)
        {
            MethodInfo metodo = (MethodInfo)miembro;

            return new InfoMiembro
            {
                Nombre = metodo.Name,
                Categoria = "Método",
                TipoDato = metodo.ReturnType.Name,
                CantidadParametros = metodo.GetParameters().Length
            };
        }
    }
}
