// <copyright file="NoteRL.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using CommonLayer.Models;
    using HtmlAgilityPack;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entity;
    using RepositoryLayer.Interfaces;
    using RepositoryLayer.Migrations;

    public class NoteRL : INoteRL
    {
        private readonly IConfiguration config;
        private readonly FundooContext fundooContext;

        public NoteRL(FundooContext fundooContext, IConfiguration config)
        {
            this.fundooContext = fundooContext;
            this.config = config;
        }

        /// <inheritdoc/>
        public NoteEntity AddNote(NoteModel model, int UserId)
        {
            try
            {
                NoteEntity noteEntity = new NoteEntity();
                noteEntity.Title = model.Title;
                noteEntity.Note = model.Note;
                noteEntity.Reminder = model.Reminder;
                noteEntity.Color = model.Color;
                noteEntity.Image = model.Image;
                noteEntity.IsArchive = model.IsArchive;
                noteEntity.IsPin = model.IsPin;
                noteEntity.IsTrash = model.IsTrash;
                noteEntity.UserId = UserId;
                noteEntity.Createat = model.Createat;
                noteEntity.Modifiedat = model.Modifiedat;
                this.fundooContext.Notes.Add(noteEntity);
                int result = this.fundooContext.SaveChanges();
                if (result > 0)
                {
                    return noteEntity;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <inheritdoc/>
        public List<NoteEntity> GetAllNotes(int UserId)
        {
            try
            {
                var result = this.fundooContext.Notes.Where(x => x.UserId == UserId).ToList();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NoteEntity> GetNotesUsingRedisCache()
        {
            try
            {
                var result = this.fundooContext.Notes.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateNote(long noteid, int userid, NoteModel node)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (node.Title != null)
                    {
                        result.Title = node.Title;
                    }

                    result.Modifiedat = DateTime.Now;
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

        public bool DeleteNote(long noteid, int userid)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                   this.fundooContext.Notes.Remove(result);
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

        public bool IsPinOrNot(long noteid, int userid)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (result.IsPin == true)
                    {
                        result.IsPin = false;
                        this.fundooContext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsPin = true;
                        this.fundooContext.SaveChanges();
                        return true;
                    }
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

        public bool IsArchiveOrNot(long noteid, int userid)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (result.IsArchive == true)
                    {
                        result.IsArchive = false;
                        this.fundooContext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsArchive = true;
                        this.fundooContext.SaveChanges();
                        return true;
                    }
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

        public bool DeleteForever(long noteid, int userid)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid);
                if (result.IsTrash == true)
                {
                    this.fundooContext.Remove(result.NoteID);
                    this.fundooContext.SaveChanges();
                    return false;
                }
                else
                {
                    result.IsTrash = true;
                    this.fundooContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public NoteEntity Color(long noteid, int userid, string color)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result.Color != null)
                {
                    result.Color = color;
                    this.fundooContext.SaveChanges();
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

        public NoteEntity IsRememberOrNot(long noteid, int userid, DateTime reminder)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (result.Reminder != null)
                    {
                        result.Reminder = reminder;
                        this.fundooContext.SaveChanges();
                        return result;
                    }
                    else
                    {
                        return null;
                    }
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

        public string UploadImage(long noteId, int userId, IFormFile img)
        {
            try
            {
                var result = this.fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteId && e.UserId == userId);

                if (result != null)
                {
                    Account account = new Account(
                       this.config["CloudinarySettings:CloudName"],
                       this.config["CloudinarySettings:ApiKey"],
                       this.config["CloudinarySettings:ApiSecret"]);
                    Cloudinary cloudinary = new Cloudinary(account);

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(img.FileName, img.OpenReadStream()),
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = imagePath;
                    this.fundooContext.SaveChanges();
                    return "Image uploaded successfully";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<NoteEntity>> SearchNote(string title, int userid)
        {
            try
            {
                IQueryable<NoteEntity> query = this.fundooContext.Notes.AsQueryable();
                if (query != null)
                {
                    if (!string.IsNullOrEmpty(title))
                    {
                        query = query.Where(e => e.Title == title);
                        return await query.ToListAsync();
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
