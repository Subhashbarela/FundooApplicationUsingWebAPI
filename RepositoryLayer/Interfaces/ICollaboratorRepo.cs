using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface ICollaboratorRepo
    {
        public CollaboratorEntity AddCollaborator(CollaboratorModel model, int userId, long noteid);
        public bool UpdateCollaborator(int id, int userid, CollaboratorModel model);
        public bool DeleteCollaborator(int Colaboratorid, int userid);
        public List<CollaboratorEntity> GetAllCollaborator(int userid);

    }
}
