// <copyright file="CollaboratorRepo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using CommonLayer.Models;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entity;
    using RepositoryLayer.Interfaces;

    public class CollaboratorRepo : ICollaboratorRepo
    {
        private readonly FundooContext fundooContext;

        public CollaboratorRepo(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public CollaboratorEntity AddCollaborator(CollaboratorModel model, int userId, long noteid)
        {
            try
            {
                CollaboratorEntity collaboratorEntity = new CollaboratorEntity
                {
                    Email = model.Email,
                    UserId = userId,
                    NoteId = noteid,
                };
                this.fundooContext.Collaborator.Add(collaboratorEntity);
                this.fundooContext.SaveChanges();
                return collaboratorEntity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateCollaborator(int id, int userid, CollaboratorModel model)
        {
            try
            {
                var result = this.fundooContext.Collaborator.FirstOrDefault(e => e.Id == id && e.UserId == userid);
                if (result != null)
                {
                    result.Email = model.Email;
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

        public bool DeleteCollaborator(int Colaboratorid, int userid)
        {
            try
            {
                var result = this.fundooContext.Collaborator.FirstOrDefault(e => e.Id == Colaboratorid && e.UserId == userid);
                if (result != null)
                {
                    this.fundooContext.Collaborator.Remove(result);
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

        public List<CollaboratorEntity> GetAllCollaborator(int userid)
        {
            try
            {
                var result = this.fundooContext.Collaborator.Where(e => e.UserId == userid).ToList();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
