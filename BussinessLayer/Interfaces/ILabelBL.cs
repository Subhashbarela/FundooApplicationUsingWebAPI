using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface ILabelBL
    {
        public LabelEntity AddLabel(LabelModel label, int userId, long noteid);
        public bool UpdateLabel(int id,int userid, LabelModel model);
        public bool DeleteLabel(int labelid, int userid);
        public List<LabelEntity> GetAllLabels(int userid);
        public List<LabelEntity> GetLabelsUsingRedisCache();

    }
}
