namespace Interfaces_60MN
{
    /// <summary>Operaciones básicas de persistencia que implementan los mappers de la DAL.</summary>
    public interface ICrud_60MN<T> where T : IEntity_60MN
    {
        int Insertar(T entidad);

        void Modificar(T entidad);

        IList<T> ListarTodos();

        T? ObtenerPorId(int id);
    }
}
