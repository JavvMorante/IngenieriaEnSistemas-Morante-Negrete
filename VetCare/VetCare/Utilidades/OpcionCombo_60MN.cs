namespace VetCare
{
    /// <summary>Ítem de ComboBox con texto visible y un valor asociado (null = "Todos").</summary>
    internal sealed class OpcionCombo_60MN
    {
        public string Texto { get; }

        public object? Valor { get; }

        public OpcionCombo_60MN(string texto, object? valor)
        {
            Texto = texto;
            Valor = valor;
        }

        public static OpcionCombo_60MN Todos => new OpcionCombo_60MN(TraductorFormularios_60MN.T("msg.Todos", "(Todos)"), null);

        public override string ToString() => Texto;

        /// <summary>Valor del ítem seleccionado del combo, convertido al tipo pedido, o null si es "(Todos)".</summary>
        public static T? ValorDe<T>(ComboBox combo) where T : struct =>
            combo.SelectedItem is OpcionCombo_60MN opcion && opcion.Valor is T valor ? valor : null;
    }
}
