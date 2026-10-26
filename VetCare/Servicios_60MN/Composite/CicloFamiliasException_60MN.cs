namespace Servicios_60MN.Composite
{
    /// <summary>
    /// Se intentó agregar a una familia un permiso que ya la contiene (directa o
    /// indirectamente). El árbol del Composite tiene que ser acíclico: con un ciclo,
    /// preguntar si una familia otorga una patente sería una recursión infinita.
    /// </summary>
    public class CicloFamiliasException_60MN : InvalidOperationException
    {
        public CicloFamiliasException_60MN(string familia, string hijo)
            : base($"No se puede incluir \"{hijo}\" en la familia \"{familia}\" porque se generaría un ciclo.")
        {
        }
    }
}
