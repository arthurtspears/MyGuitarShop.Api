using MyGuitarShop.Common.Dtos;
using MyGuitarShop.Data.Ado.Entities;

namespace MyGuitarShop.Data.Ado.Mappers
{
    public static partial class Mapper
    {
        public static ProductEntity ToEntity(ProductDto dto)
        {
            dto.ProductID ??= 0;
            dto.DateAdded ??= DateTime.UtcNow;

            return new ProductEntity()
            {
                ProductID = dto.ProductID.Value,
                CategoryID = dto.CategoryID,
                ProductName = dto.ProductName,
                ProductCode = dto.ProductCode,
                ListPrice = dto.ListPrice,
                DiscountPercent = dto.DiscountPercent,
                Description = dto.Description,
                DateAdded = dto.DateAdded
            };
        }

        public static ProductDto ToDto(ProductEntity entity)
        {
            return new ProductDto()
            {
                ProductID = entity.ProductID,
                CategoryID = entity.CategoryID,
                ProductCode = entity.ProductCode,
                ProductName = entity.ProductName,
                Description = entity.Description,
                ListPrice = entity.ListPrice,
                DiscountPercent = entity.DiscountPercent,
                DateAdded = entity.DateAdded
            };
        }
    }
}