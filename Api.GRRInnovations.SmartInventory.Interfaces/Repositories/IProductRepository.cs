using Api.GRRInnovations.SmartInventory.Interfaces.Entities;

namespace Api.GRRInnovations.SmartInventory.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<IProductModel>> GetAllAsync(ProductOptions productOptions);
        Task<IProductModel> GetByIdAsync(Guid id);
        Task<IProductModel> CreateAsync(IProductModel dto);
    }

    public class ProductOptions
    {
        public List<Guid> FilterUids { get; set; }
        public List<string> FilterNames { get; set; }
        public List<Guid> FilterCategoriesUids { get; set; }
        public List<Guid> FilterSupplierUids { get; set; }
        public bool IncludeCategory { get; set; }
        public bool IncludeSupplier { get; set; }
    }
}
