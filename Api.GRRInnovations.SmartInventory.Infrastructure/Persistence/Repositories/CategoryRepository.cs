using Api.GRRInnovations.SmartInventory.Domain.Entities;
using Api.GRRInnovations.SmartInventory.Interfaces.Entities;
using Api.GRRInnovations.SmartInventory.Interfaces.Repositories;

namespace Api.GRRInnovations.SmartInventory.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ICategoryModel> CreateAsync(ICategoryModel dto)
        {
            if (dto is not CategoryModel categoryM) return null;

            await _context.Categories.AddAsync(categoryM).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return categoryM;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICategoryModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICategoryModel> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, ICategoryModel dto)
        {
            throw new NotImplementedException();
        }
    }
}
