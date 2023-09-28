using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        private readonly IConfiguration _config;
        private readonly FundooContext _fundooContext;
        public NoteRL(FundooContext fundooContext, IConfiguration config)
        {
            _fundooContext = fundooContext;
            _config = config;
        }
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
                _fundooContext.Notes.Add(noteEntity);
                int result = _fundooContext.SaveChanges();
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
        public List<NoteEntity> GetAllNotes(int UserId)
        {
            try
            {
                var result = _fundooContext.Notes.Where(x => x.UserId == UserId).ToList();
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
                var result = _fundooContext.Notes.ToList();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (node.Title != null)
                    {
                        result.Title = node.Title;
                    }
                    result.Modifiedat = DateTime.Now;
                    _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    _fundooContext.Notes.Remove(result);
                    _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (result.IsPin == true)
                    {
                        result.IsPin = false;
                        _fundooContext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsPin = true;
                        _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (result.IsArchive == true)
                    {
                        result.IsArchive = false;
                        _fundooContext.SaveChanges();
                        return false;
                    }
                    else
                    {
                        result.IsArchive = true;
                        _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid);
                if (result.IsTrash == true)
                {
                    _fundooContext.Remove(result.NoteID);
                    _fundooContext.SaveChanges();
                    return false;
                }
                else
                {
                    result.IsTrash = true;
                    _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result.Color != null)
                {
                    result.Color = color;
                    _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteid && e.UserId == userid);
                if (result != null)
                {
                    if (result.Reminder == null)
                    {
                        result.Reminder = reminder;
                        _fundooContext.SaveChanges();
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
                var result = _fundooContext.Notes.FirstOrDefault(e => e.NoteID == noteId && e.UserId == userId);

                if (result != null)
                {
                    Account account = new Account(
                         "dzd5e7nky",
                         "448453623633985",
                        "***************************");
                    Cloudinary cloudinary = new Cloudinary(account);


                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(img.FileName, img.OpenReadStream())
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);
                    string imagePath = uploadResult.Url.ToString();
                    result.Image = imagePath;
                    this._fundooContext.SaveChanges();
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
    }
}
