using Api.GRRInnovations.SmartInventory.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.SmartInventory.Domain.Entities
{
    public class ProductModel : BaseModel, IProductModel
    {
        public string Name { get; set; }
        public int StockQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public CategoryModel? DbCategory { get; set; }
        public ICategoryModel? Category
        {
            get => DbCategory;
            set => DbCategory = value as CategoryModel;
        }

        public Guid? CategoryUid { get; set; }
    }
}
