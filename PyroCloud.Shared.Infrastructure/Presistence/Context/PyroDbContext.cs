using Microsoft.EntityFrameworkCore;
using PyroCloud.Core.Domain.Entities.Identity;
using PyroCloud.Core.Domain.Entities.Inventory;
using PyroCloud.Core.Domain.Interfaces;

namespace PyroCloud.Shared.Infrastructure.Presistence.Context
{
    public class PyroDbContext : DbContext
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public PyroDbContext(DbContextOptions<PyroDbContext> options, ICurrentUserProvider currentUserProvider) : base(options)
        {
            _currentUserProvider = currentUserProvider;
        }

        #region Identity schema
        public DbSet<Tenant> Tenants => Set<Tenant>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<UserCompany> UserCompanies => Set<UserCompany>();
        #endregion

        #region Inventory schema
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductBatch> ProductBatches => Set<ProductBatch>();
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PyroDbContext).Assembly);
        }
    }
}
