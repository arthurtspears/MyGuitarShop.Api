namespace MyGuitarShop.Common.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity?> GetByIdAsync(int id);

        Task<int> InsertAsync(TEntity entity);

        Task<int> UpdateAsync(int id, TEntity entity);

        Task<int> DeleteAsync(int id);

    }
}
