using Api.GRRInnovations.SmartInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.GRRInnovations.SmartInventory.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        DbSet<ProductModel> Products {  get; set; }
        DbSet<CategoryModel> Categories { get; set; }
        DbSet<StockEntryModel> StockEntries { get; set; }
        DbSet<StockOutputModel> StockOutputs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            DefaultModelSetup<ProductModel>(modelBuilder);
            modelBuilder.Entity<ProductModel>().Ignore(m => m.Category);
            modelBuilder.Entity<ProductModel>().Ignore(m => m.StockEntries);
            modelBuilder.Entity<ProductModel>().Ignore(m => m.StockOutputs);
            modelBuilder.Entity<ProductModel>().Ignore(m => m.Supplier);
            modelBuilder.Entity<ProductModel>().HasMany(m => m.DbStockEntries).WithOne(m => m.DbProduct).HasForeignKey(p => p.ProductUid).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<ProductModel>().HasMany(m => m.DbStockOutputs).WithOne(m => m.DbProduct).HasForeignKey(p => p.ProductUid).OnDelete(DeleteBehavior.NoAction);

            DefaultModelSetup<CategoryModel>(modelBuilder);
            modelBuilder.Entity<CategoryModel>().Ignore(m => m.Products);
            modelBuilder.Entity<CategoryModel>().HasMany(m => m.DbProducts).WithOne(m => m.DbCategory).HasForeignKey(p => p.CategoryUid).OnDelete(DeleteBehavior.NoAction);

            DefaultModelSetup<StockEntryModel>(modelBuilder);
            modelBuilder.Entity<StockEntryModel>().Ignore(m => m.Product);
            modelBuilder.Entity<StockEntryModel>().HasOne(m => m.DbProduct).WithMany(m => m.DbStockEntries).HasForeignKey(p => p.ProductUid).OnDelete(DeleteBehavior.NoAction);

            DefaultModelSetup<StockOutputModel>(modelBuilder);
            modelBuilder.Entity<StockOutputModel>().Ignore(m => m.Product);
            modelBuilder.Entity<StockOutputModel>().HasOne(m => m.DbProduct).WithMany(m => m.DbStockOutputs).HasForeignKey(p => p.ProductUid).OnDelete(DeleteBehavior.NoAction);

            DefaultModelSetup<SupplierModel>(modelBuilder);
            modelBuilder.Entity<SupplierModel>().Ignore(m => m.Products);
            modelBuilder.Entity<SupplierModel>().HasMany(m => m.DbProducts).WithOne(m => m.DbSupplier).HasForeignKey(p => p.SupplierUid).OnDelete(DeleteBehavior.NoAction);
        }

        public override int SaveChanges()
        {
            int result;
            try
            {
                AdjustChanges();
                result = base.SaveChanges();
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result;
            try
            {
                AdjustChanges();
                result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception)
            {
                RollBack();
                throw;
            }

            return result;
        }

        public void RollBack()
        {
            var changedEntries = ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }

        private void AdjustChanges()
        {
            var changes = ChangeTracker.Entries<BaseModel>().Where(p => p.State == EntityState.Modified || p.State == EntityState.Added);

            foreach (var entry in changes)
            {
                entry.Property(p => p.UpdatedAt).CurrentValue = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entry.Property(p => p.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
            }
        }

        public void DefaultModelSetup<T>(ModelBuilder modelBuilder) where T : BaseModel
        {
            modelBuilder.Entity<T>().Property(m => m.CreatedAt).IsRequired();
            modelBuilder.Entity<T>().Property(m => m.UpdatedAt).IsRequired();

            modelBuilder.Entity<T>().HasKey(m => m.Uid);
            modelBuilder.Entity<T>().Property((m) => m.Uid).IsRequired().HasValueGenerator<GuidValueGenerator>();
        }
    }
}
