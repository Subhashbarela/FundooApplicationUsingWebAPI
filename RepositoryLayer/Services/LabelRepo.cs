// <copyright file="LabelRepo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommonLayer.Models;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entity;
    using RepositoryLayer.Interfaces;

    public class LabelRepo : ILabelRepo
    {
        private readonly FundooContext fundooContext;

        public LabelRepo(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public LabelEntity AddLabel(LabelModel label, int userId, long noteid)
        {
            try
            {
                LabelEntity labelEntity = new LabelEntity();
                labelEntity.LabelName = label.LabelName;
                labelEntity.UserId = userId;
                labelEntity.NoteId = noteid;
                this.fundooContext.Label.Add(labelEntity);
                this.fundooContext.SaveChanges();
                return labelEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateLabel(int id, int userid, LabelModel model)
        {
            try
            {
                var result = this.fundooContext.Label.FirstOrDefault(e => e.UserId == userid && e.LableId == id);
                if (result != null)
                {
                    result.LabelName = model.LabelName;

                    this.fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteLabel(int labelid, int userid)
        {
            try
            {
                var result = this.fundooContext.Label.FirstOrDefault(e => e.LableId == labelid && e.UserId == userid);
                if (result != null)
                {
                    this.fundooContext.Label.Remove(result);
                    this.fundooContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<LabelEntity> GetAllLabels(int userid)
        {
            try
            {
                var result = this.fundooContext.Label.Where(x => x.UserId == userid).ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LabelEntity> GetLabelsUsingRedisCache()
        {
            try
            {
                var result = this.fundooContext.Label.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
