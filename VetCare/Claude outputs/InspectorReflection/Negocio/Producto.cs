namespace InspectorReflection.Negocio
{

    public class Producto
    {
        private decimal _precio;


        public string Nombre { get; set; }

        public Producto(string nombre, decimal precio)
        {
            Nombre = nombre;
            _precio = precio;
        }

        public decimal CalcularPrecioFinal(decimal impuesto)
        {
            return _precio + (_precio * impuesto / 100);
        }
    }
}
