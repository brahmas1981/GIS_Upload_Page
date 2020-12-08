using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using GIS_Upload_Page.Models;

namespace GIS_Upload_Page.Data
{
    public interface IDataService
    {
        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        EntityEntry Entry(object entity);
        void AddRange(IEnumerable<object> entities);
        void AddRange(params object[] entities);
        EntityEntry Add(object entity);
        EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class;
        IEnumerable<TEntity> FromSql<TEntity>(string sql) where TEntity : class;
        int ExecuteSqlCommand(string sql);

        DbSet<T> GetValues<T>() where T : class;
        IEnumerable<object> GetValues(string T);
        User FindUser(string userName = null);
        DbContext DbContext { get; }
    }

    public interface IDataService<TModel> : IDataService
        where TModel : class
    {
        IEnumerable<TModel> Read(bool showDeleted = false);
        void Create(TModel model);
        void Update(TModel model);
        void Destroy(TModel model);
    }
}
