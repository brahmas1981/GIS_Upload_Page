using GIS_Upload_Page.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GIS_Upload_Page.Data
{
    public class MasterCcDataService : BaseDataService, IDataService<MasterCcViewModel>
    {
        public MasterCcDataService(Upload_PageContext dbContext) : base(dbContext)
        {
        }

        public void Create(List<MasterCcViewModel> models)
        {
            var lastIndex = models.Count - 1;
            for (var index = 0; index < models.Count; index++)
            {
                if (index == lastIndex)
                    Create(models[index], true);
                else
                    Create(models[index], false);
            }
        }

        public void Create(MasterCcViewModel model, bool save)
        {
            //var masterCcDbContext = _dbContext as ExxonCDMSContext;

            //var dateTimeUtcNow = DateTime.UtcNow;
            //var entity = new MasterCc();

            //entity.SourceId = model.SourceID;
            //entity.CcStatement = model.VerbatimStatement;
            //entity.ApprovalId = 2;
            //entity.CreateUserId = 4;
            //// Calculated
            //entity.CreatedDateTime = entity.ModifiedDateTime = dateTimeUtcNow;

            //masterCcDbContext.MasterCc.Add(entity);
            //masterCcDbContext.Entry(entity).State = EntityState.Added;

            //if (save)
            //{
            //    masterCcDbContext.SaveChanges();
            //    model.ID = (int)entity.Id;
            //}
        }

        public void Create(MasterCcViewModel model)
        {
            Create(model, false);
        }

        public void Destroy(MasterCcViewModel model)
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteSqlCommand(string sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FromSql<TEntity>(string sql) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<MasterCcViewModel> Read(bool showDeleted = false)
        {
            throw new System.NotImplementedException();
        }

        public void Update(MasterCcViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
