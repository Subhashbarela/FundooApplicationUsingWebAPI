using BussinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace BussinessLayer.Services
{
    public class NoteBL: INoteBL
    {
        INoteRL _iNoteRL;
        public NoteBL(INoteRL iNoteRL)
        {
            _iNoteRL = iNoteRL;
        }
        public NoteEntity AddNote(NoteModel model, int UserId)
        {
            try
            {
                return _iNoteRL.AddNote(model, UserId);

            }catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<NoteEntity> GetAllNotes(int userId)
        {
            try
            {
                return _iNoteRL.GetAllNotes(userId);

            }catch (Exception)
            {
                throw;
            }
        }
        public List<NoteEntity> GetNotesUsingRedisCache()
        {
            try
            {
                return _iNoteRL.GetNotesUsingRedisCache();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateNote(long noteid, int userid, NoteModel node)
        {
            try
            {
                return _iNoteRL.UpdateNote(noteid,userid, node);
            }
            catch(Exception )
            {
                throw;
            }
        }
        public bool DeleteNote(long noteid, int userid)
        {
            try
            {
                return _iNoteRL.DeleteNote(noteid, userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsPinOrNot(long noteid, int userid)
        {
            try
            {
                return _iNoteRL.IsPinOrNot(noteid, userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool IsArchiveOrNot(long noteid, int userid)
        {
            try
            {
                return _iNoteRL.IsArchiveOrNot(noteid,userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity IsRememberOrNot(long noteid, int userid, DateTime reminder)
        {
            try
            {
                return _iNoteRL.IsRememberOrNot(noteid, userid, reminder);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteForever(long noteid, int userid)
        {
            try
            {
                return _iNoteRL.DeleteForever(noteid, userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity Color(long noteid, int userid, string color)
        {
            try
            {
                return _iNoteRL.Color(noteid, userid, color);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string UploadImage(long noteid, int userid, IFormFile img)
        {
            try
            {
                return _iNoteRL.UploadImage(noteid, userid, img);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
