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

        public List<StockEntryModel>? DbStockEntries { get; set; }

        public List<IStockEntryModel>? StockEntries
        {
            get => DbStockEntries?.Cast<IStockEntryModel>()?.ToList();
            set => DbStockEntries = value?.Cast<StockEntryModel>()?.ToList();
        }

        public List<StockOutputModel>? DbStockOutputs { get; set; }
        public List<IStockOutputModel>? StockOutputs
        {
            get => DbStockOutputs?.Cast<IStockOutputModel>()?.ToList();
            set => DbStockOutputs = value?.Cast<StockOutputModel>()?.ToList();
        }

        public SupplierModel? DbSupplier { get; set; }
        public ISupplierModel? Supplier
        {
            get => DbSupplier;
            set => DbSupplier = value as SupplierModel;
        }

        public Guid? SupplierUid { get; set; }

        public ProductModel()
        {
            DbStockEntries = new List<StockEntryModel>();
            DbStockOutputs = new List<StockOutputModel>();
        }
    }
}
