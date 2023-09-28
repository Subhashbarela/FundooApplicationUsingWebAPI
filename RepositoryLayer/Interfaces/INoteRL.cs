using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INoteRL
    {
        public NoteEntity AddNote(NoteModel model, int UserId);
        public List<NoteEntity> GetAllNotes(int UserId);
        public List<NoteEntity> GetNotesUsingRedisCache();
        public bool UpdateNote(long noteid, int userid, NoteModel node);
        public bool DeleteNote(long noteid, int userid);
        public bool IsPinOrNot(long noteid, int userid);
        public bool IsArchiveOrNot(long noteid, int userid);
        public bool DeleteForever(long noteid, int userid);
        public NoteEntity Color(long noteid, int userid, string color);
        public NoteEntity IsRememberOrNot(long noteid, int userid, DateTime reminder);
        public string UploadImage(long noteid, int userid, IFormFile img);


    }
}
