using Api.GRRInnovations.SmartInventory.Interfaces.Entities;

namespace Api.GRRInnovations.SmartInventory.Domain.Entities
{
    public class CategoryModel : BaseModel, ICategoryModel
    {
        public string Name { get; set; }

        public List<ProductModel>? DbProducts { get; set; }

        public List<IProductModel>? Products
        {
            get => DbProducts?.Cast<IProductModel>()?.ToList();
            set => DbProducts = value?.Cast<ProductModel>()?.ToList();
        }

        public CategoryModel()
        {
            DbProducts = [];
        }
    }
}
