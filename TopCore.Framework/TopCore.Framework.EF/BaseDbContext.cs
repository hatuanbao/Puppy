﻿#region	License
//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> TopCore.Framework.EF </Project>
//     <File>
//         <Name> BaseDbContext </Name>
//         <Created> 06 Apr 17 1:09:03 AM </Created>
//         <Key> 698007b1-38ad-45b0-b7d0-f22069fe2fab </Key>
//     </File>
//     <Summary>
//         BaseDbContext
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------
#endregion License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TopCore.Framework.EF.Interfaces;

namespace TopCore.Framework.EF
{
    public class BaseDbContext : DbContext, IBaseDbContext, IBaseDbEntityContext
    {
        protected BaseDbContext()
        {
        }

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        #region Database

        public bool EnsureDatabaseCreated()
        {
            return Database.EnsureCreated();
        }

        public Task<bool> EnsureDatabaseCreatedAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Database.EnsureCreatedAsync(cancellationToken);
        }

        public bool EnsureDatabaseDeleted()
        {
            return Database.EnsureDeleted();
        }

        public Task<bool> EnsureDatabaseDeletedAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Database.EnsureDeletedAsync(cancellationToken);
        }

        public void MigrateDatabase()
        {
            Database.Migrate();
        }

        public Task MigrateDatabaseAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Database.MigrateAsync(cancellationToken);
        }

        #endregion

        #region Save Changes

        public override int SaveChanges()
        {
            StandardizeSaveChangeData(ChangeTracker);
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            StandardizeSaveChangeData(ChangeTracker);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeSaveChangeData(ChangeTracker);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            StandardizeSaveChangeData(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void StandardizeSaveChangeData(ChangeTracker changeTracker)
        {
            var entities = changeTracker.Entries().Where(x => x.Entity is IBaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            DateTime utcNow = DateTime.UtcNow;

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        {
                            entity.Property(nameof(IBaseEntity.CreatedOnUtc)).CurrentValue = utcNow;
                            entity.Property(nameof(IBaseEntity.LastUpdatedOnUtc)).CurrentValue = null;
                            entity.Property(nameof(IBaseEntity.IsDeleted)).CurrentValue = false;
                            entity.Property(nameof(IBaseEntity.DeletedOnUtc)).CurrentValue = null;
                            break;
                        }
                    case EntityState.Modified:
                        {
                            entity.Property(nameof(IBaseEntity.LastUpdatedOnUtc)).CurrentValue = utcNow;
                            break;
                        }
                }
            }
        }

        #endregion

        #region Get

        public IQueryable<T> AllIncluding<T>(params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            IQueryable<T> query = Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Set<T>().Where(predicate);
        }

        public IQueryable<T> Get<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return AllIncluding(includeProperties).Where(predicate);
        }

        public T GetSingle<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Get(predicate).FirstOrDefault();
        }

        public Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return Get(predicate).FirstOrDefaultAsync();
        }

        public T GetSingle<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return Get(predicate, includeProperties).FirstOrDefault();
        }

        public Task<T> GetSingleAsync<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties) where T : class
        {
            return Get(predicate, includeProperties).FirstOrDefaultAsync();
        }

        #endregion

        #region Get Entity

        public IQueryable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isIncludeDeleted = false) where TEntity : class, IBaseEntity
        {
            IQueryable<TEntity> query = Set<TEntity>().Where(predicate);
            return isIncludeDeleted ? query : query.Where(x => !x.IsDeleted);
        }

        public IQueryable<TEntity> GetEntity<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isIncludeDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
        {
            IQueryable<TEntity> query = GetEntity(predicate, isIncludeDeleted);
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public TEntity GetSingleEntity<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isIncludeDeleted = false) where TEntity : class, IBaseEntity
        {
            return GetEntity(predicate, isIncludeDeleted).FirstOrDefault();
        }

        public Task<TEntity> GetSingleEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isIncludeDeleted = false) where TEntity : class, IBaseEntity
        {
            return GetEntity(predicate, isIncludeDeleted).FirstOrDefaultAsync();
        }

        public TEntity GetSingleEntity<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isIncludeDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
        {
            return GetEntity(predicate, isIncludeDeleted, includeProperties).FirstOrDefault();
        }

        public Task<TEntity> GetSingleEntityAsync<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isIncludeDeleted = false, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
        {
            return GetEntity(predicate, isIncludeDeleted, includeProperties).FirstOrDefaultAsync();
        }

        #endregion

        #region Add

        public new T Add<T>(T entity) where T : class
        {
            entity = Set<T>().Add(entity).Entity;
            return entity;
        }

        public new Task AddAsync<T>(T entity, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            return Set<T>().AddAsync(entity, cancellationToken);
        }

        public void AddRange<T>(params T[] entities) where T : class
        {
            Set<T>().AddRange(entities);
        }

        public void AddRange<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().AddRange(entities);
        }

        public Task AddRangeAsync<T>(params T[] entities) where T : class
        {
            return Set<T>().AddRangeAsync(entities);
        }

        public Task AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            return Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        #region Update

        public new T Update<T>(T entity) where T : class
        {
            entity = Set<T>().Update(entity).Entity;
            return entity;
        }

        public void UpdateRange<T>(params T[] entities) where T : class
        {
            Set<T>().UpdateRange(entities);
        }

        public void UpdateRange<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().UpdateRange(entities);
        }

        #endregion

        #region Delete

        public void Delete<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        public void RemoveRange<T>(ICollection<T> entities, bool isPhysicalDelete = false) where T : class, IBaseEntity
        {
            if (!isPhysicalDelete)
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                    entity.DeletedOnUtc = DateTime.UtcNow;
                    Set<T>().Update(entity);
                }
            }
            else
            {
                Set<T>().RemoveRange(entities);
            }
        }

        public void RemoveRange<T>(bool isPhysicalDelete = false, params T[] entities) where T : class, IBaseEntity
        {
            RemoveRange(entities, isPhysicalDelete);
        }

        public void DeleteRange<T>(params T[] entities) where T : class
        {
            Set<T>().RemoveRange(entities);
        }

        public void DeleteRange<T>(IEnumerable<T> entities) where T : class
        {
            Set<T>().RemoveRange(entities);
        }

        public void DeleteWhere<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var query = Get(predicate).AsEnumerable();
            DeleteRange(query);
        }

        #endregion

        #region Delete Entity

        public void DeleteEntity<TEntity>(TEntity entity, bool isPhysicalDelete = false) where TEntity : class, IBaseEntity
        {
            if (!isPhysicalDelete)
            {
                entity.IsDeleted = true;
                entity.DeletedOnUtc = DateTime.UtcNow;
                Update(entity);
            }
            Set<TEntity>().Remove(entity);
        }

        public void DeleteEntityRange<TEntity>(IEnumerable<TEntity> entities, bool isPhysicalDelete = false) where TEntity : class, IBaseEntity
        {
            if (!isPhysicalDelete)
            {
                foreach (var entity in entities)
                {
                    DeleteEntity(entity);
                }
            }
            else
            {
                Set<TEntity>().RemoveRange(entities);
            }
        }

        public void DeleteEntityRange<TEntity>(bool isPhysicalDelete = false, params TEntity[] entities) where TEntity : class, IBaseEntity
        {
            DeleteEntityRange(entities, isPhysicalDelete);
        }

        public void DeleteEntityWhere<TEntity>(Expression<Func<TEntity, bool>> predicate, bool isPhysicalDelete = false) where TEntity : class, IBaseEntity
        {
            var query = GetEntity(predicate, isPhysicalDelete).AsEnumerable();
            DeleteEntityRange(query, isPhysicalDelete);
        }

        #endregion
    }
}