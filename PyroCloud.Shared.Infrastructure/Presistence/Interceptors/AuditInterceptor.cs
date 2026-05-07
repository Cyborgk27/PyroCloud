using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PyroCloud.Core.Domain.Common;
using PyroCloud.Core.Domain.Interfaces;

namespace PyroCloud.Shared.Infrastructure.Presistence.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IDateTime _dateTime;

        public AuditInterceptor(ICurrentUserProvider currentUserProvider, IDateTime dateTime)
        {
            _currentUserProvider = currentUserProvider;
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            var userId = _currentUserProvider.UserId?.ToString();
            var now = _dateTime.UtcNow;

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedDate = now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedDate = now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedBy = userId;
                        entry.Entity.DeletedDate = now;
                        entry.Entity.IsDeleted = true;
                        break;
                }
            }

            foreach (var entry in context.ChangeTracker.Entries<ITenantEntity>())
            {
                if (entry.State == EntityState.Added && (entry.Entity.TenantId == Guid.Empty || entry.Entity.TenantId == default))
                {
                    entry.Entity.TenantId = _currentUserProvider.TenantId ?? Guid.Empty;
                }
            }

        }
    }
}