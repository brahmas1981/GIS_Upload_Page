using GIS_Upload_Page.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using GIS_Upload_Page.Extensions;

namespace GIS_Upload_Page.Data
{
    public class BaseDataService
    {
        private readonly static object userLock = new object();

        protected readonly IHttpContextAccessor _httpContextAccessor;

        protected readonly DbContext _dbContext;

        public BaseDataService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BaseDataService(DbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public DbContext DbContext { get { return _dbContext; } }

        public User FindUser(string userName = null)
        {
            lock (userLock)
            {
                var dbContext = _dbContext as Upload_PageContext;

                var update = false;
                dynamic userInfo = null;

                if (string.IsNullOrEmpty(userName))
                {
                    userInfo = _httpContextAccessor.HttpContext.User.GetLoggedInUserInfo();
                    userName = userInfo.UserName;
                    update = true;
                }

                var user = dbContext.User.FirstOrDefault(u => u.UserName == userName);

                if (update)
                {
                    if (user != null)
                    {
                        if (!string.Equals(user.Email, userInfo.UserEmail, StringComparison.OrdinalIgnoreCase) || !string.Equals(user.FullName, userInfo.FullName, StringComparison.OrdinalIgnoreCase))
                        {
                            user.Email = !string.IsNullOrEmpty(userInfo.UserEmail) ? userInfo.UserEmail : null;
                            user.FullName = !string.IsNullOrEmpty(userInfo.FullName) ? userInfo.FullName : null;
                            dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        var newUser = new User()
                        {
                            UserName = userInfo.UserName,
                            Email = !string.IsNullOrEmpty(userInfo.UserEmail) ? userInfo.UserEmail : null,
                            FullName = !string.IsNullOrEmpty(userInfo.FullName) ? userInfo.FullName : null
                        };

                        dbContext.User.Add(newUser);
                        dbContext.Entry(newUser).State = EntityState.Added;
                        dbContext.SaveChanges();

                        user = newUser;
                    }
                }

                return user;
            }
        }

        public DbSet<T> GetValues<T>() where T : class
        {
            System.Reflection.PropertyInfo property = _dbContext.GetType().GetProperty(typeof(T).Name);
            return property.GetValue(_dbContext) as DbSet<T>;
        }

        public IEnumerable<object> GetValues(string T)
        {
            System.Reflection.PropertyInfo property = _dbContext.GetType().GetProperty(T);
            return property.GetValue(_dbContext) as IEnumerable<object>;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return _dbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return _dbContext.Entry<TEntity>(entity);
        }

        public EntityEntry Entry(object entity)
        {
            return _dbContext.Entry(entity);
        }

        public void AddRange(IEnumerable<object> entities)
        {
            _dbContext.AddRange(entities);
        }

        public void AddRange(params object[] entities)
        {
            _dbContext.AddRange(entities);
        }

        public EntityEntry Add(object entity)
        {
            return _dbContext.Add(entity);
        }

        public EntityEntry<TEntity> Add<TEntity>(TEntity entity) where TEntity : class
        {
            return _dbContext.Add(entity);
        }

        public IEnumerable<TEntity> FromSql<TEntity>(string sql) where TEntity : class
        {
            return GetValues<TEntity>().FromSqlRaw(sql);
        }

        public int ExecuteSqlCommand(string sql)
        {
            return _dbContext.Database.ExecuteSqlCommand(sql);
        }
    }
}
