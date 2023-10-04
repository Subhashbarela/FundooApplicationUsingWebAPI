using BussinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class LabelBL: ILabelBL
    {
        private readonly ILabelRepo _repo;

        public LabelBL(ILabelRepo repo)
        {
            _repo = repo;
        }
        public LabelEntity AddLabel(LabelModel label, int userId, long noteid)
        {
            try
            {
                return _repo.AddLabel(label, userId, noteid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool UpdateLabel(int id,int userid, LabelModel model)
        {
            try
            {
                return _repo.UpdateLabel(id,userid, model);
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
                return _repo.DeleteLabel(labelid, userid);
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
                return _repo.GetAllLabels( userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<LabelEntity> GetLabelsUsingRedisCache()
        {
            try
            {
                return _repo.GetLabelsUsingRedisCache();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
