using Api.GRRInnovations.SmartInventory.Domain.Entities;
using Api.GRRInnovations.SmartInventory.Interfaces.Entities;
using Api.GRRInnovations.SmartInventory.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Api.GRRInnovations.SmartInventory.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext Context;

        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            this.Context = applicationDbContext;
        }

        public async Task<IProductModel> CreateAsync(IProductModel dto)
        {
            if (dto is not ProductModel productM) return null;

            await Context.Products.AddAsync(productM).ConfigureAwait(false);
            await Context.SaveChangesAsync().ConfigureAwait(false);

            return productM;
        }

        public async Task<IEnumerable<IProductModel>> GetAllAsync(ProductOptions productOptions)
        {
            return await Query(productOptions).ToListAsync<IProductModel>();
        }

        public async Task<IProductModel> GetByIdAsync(Guid id)
        {
            return await Context.Products.FirstOrDefaultAsync(x => x.Uid == id);
        }

        private IQueryable<ProductModel> Query(ProductOptions options)
        {
            var query = Context.Products.AsQueryable();

            if (options.FilterNames != null) query = query.Where(p => options.FilterNames.Contains(p.Name));

            return query;
        }

    }
}
