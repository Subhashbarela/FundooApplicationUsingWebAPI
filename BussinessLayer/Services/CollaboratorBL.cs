using BussinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Services
{
    public class CollaboratorBL:ICollaboratorBL
    {
        private readonly ICollaboratorRepo _collaboratorRepo;
        public CollaboratorBL(ICollaboratorRepo collaboratorRepo)
        {
            _collaboratorRepo = collaboratorRepo;
        }
        public CollaboratorEntity AddCollaborator(CollaboratorModel model, int userId, long noteid)
        {
            return _collaboratorRepo.AddCollaborator(model, userId, noteid);
        }
        public bool UpdateCollaborator(int id, int userid, CollaboratorModel model)
        {
            return _collaboratorRepo.UpdateCollaborator(id, userid, model);
        }
        public bool DeleteCollaborator(int Colaboratorid, int userid)
        {
            return _collaboratorRepo.DeleteCollaborator(Colaboratorid, userid);
        }
        public List<CollaboratorEntity> GetAllCollaborator(int userid)
        {
            return _collaboratorRepo.GetAllCollaborator(userid);
        }
    }
}
