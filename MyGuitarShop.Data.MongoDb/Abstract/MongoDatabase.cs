

using MongoDB.Driver;
using MyGuitarShop.Common.Interfaces;

namespace MyGuitarShop.Data.MongoDb.Abstract
{
    public abstract class MongoDatabase<TEntity>(IMongoDatabase database)
    : IRepository<TEntity> where TEntity : class
    {
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(int id, TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
