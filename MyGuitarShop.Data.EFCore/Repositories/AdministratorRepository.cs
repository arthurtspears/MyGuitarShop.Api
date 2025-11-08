using MyGuitarShop.Data.EFCore.Abstract;
using MyGuitarShop.Data.EFCore.Entities;
using MyGuitarShop.Data.EFCore.Context;

namespace MyGuitarShop.Data.EFCore.Repositories
{
    public class AdministratorRepository(MyGuitarShopContext dbContext) : RepositoryBase<Administrator>(dbContext) { }
}
