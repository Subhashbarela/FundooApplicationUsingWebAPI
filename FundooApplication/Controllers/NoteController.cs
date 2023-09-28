using BussinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System.Drawing;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBL _inoteBL;
        private readonly IDistributedCache _distributedCache;
        public NoteController(INoteBL inoteBL, IDistributedCache distributedCache)
        {
            _inoteBL = inoteBL;
            _distributedCache = distributedCache;
        }
        [Authorize]
        [HttpPost("Add")]
        public IActionResult AddNotes(NoteModel addnote)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);

                var result = _inoteBL.AddNote(addnote, userId);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Notes Added Successfully", result = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Unable to Added Notes" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("Get")]
        public IActionResult GetNotes()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _inoteBL.GetAllNotes(userId);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Get User Notes Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = " Unable To Get Notes " });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllNoteUsingRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            try
            {
                var cacheKey = "notesList";
                string serializedNotesList;
                var notesList = new List<NoteEntity>();
                var redisNotesList=await _distributedCache.GetAsync(cacheKey);
                if(redisNotesList != null)
                {
                    serializedNotesList=Encoding.UTF8.GetString(redisNotesList);
                    notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializedNotesList);
                }
                else
                {
                    notesList = _inoteBL.GetNotesUsingRedisCache().ToList();
                    serializedNotesList=JsonConvert.SerializeObject(notesList);
                    redisNotesList=Encoding.UTF8.GetBytes(serializedNotesList);
                    var options = new DistributedCacheEntryOptions()
                       .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await _distributedCache.SetAsync(cacheKey,redisNotesList,options);
                }
                return Ok(notesList);


            }
            catch(Exception ex)
            {
                return NotFound(new {success=false,message=ex.Message});
            }
        }
        [HttpPut("Update/{id}")]      
        public IActionResult UpdateNotes(long noteid,NoteModel model)
        {
            try
            {
                        
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result=_inoteBL.UpdateNote( noteid, userId,model);
                if (result == true)
                {
                    return Ok(new { Success = true, message = "Notes Update Successfully", result = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Unable to Update Notes" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete("Delete/{id}")]       
        public IActionResult DeleteNotes(long noteid)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _inoteBL.DeleteNote(noteid, userId);
                if (result == true)
                {
                    return Ok(new { Success = true, message = "Notes Update Successfully"});
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Note id is not match" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("IsPin/{id}")]
        public IActionResult TogglePin(long noteid)
        {
            try
            {
                var userid=Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type =="UserId").Value);   
                var result=_inoteBL.IsPinOrNot(noteid, userid);
                if(result == true)
                {
                    return Ok(new { Success = true, message = "Notes is Pin" });
                }
                else
                {
                    return Ok(new { Success = false, message = "Notes is UnPin" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         [HttpPut("IsAchive/{id}")]
        public IActionResult ToggleArchive(long noteid)
        {
            try
            {
                var userid=Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type =="UserId").Value);   
                var result=_inoteBL.IsArchiveOrNot(noteid, userid);
                if(result == true)
                {
                    return Ok(new { Success = true, message = "Notes is Archive" });
                }
                else
                {
                    return Ok(new { Success = false, message = "Notes is Not Archive" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        [HttpDelete("DeleteForever/{id}")]
        public IActionResult DeleteForevers(long noteid)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _inoteBL.DeleteForever(noteid, userId);
                if (result == true)
                {
                    return Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Note id is not match" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("AddColor/{id}")]
        public IActionResult AddColor(long noteid, string color)
        {
            try
            {

                int userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _inoteBL.Color(noteid, userid, color);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Color  Added Successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Color not added into notes" });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("Remember/{id}")]
        public IActionResult IsReminder(long noteid, DateTime reminder)
        {
            try
            {
                var userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _inoteBL.IsRememberOrNot(noteid, userid, reminder);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Remember is added", data = result });

                }
                else
                {
                    return Ok(new { Success = false, message = "Remember is not added" });

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UploadImg/{noteid}")]
        public IActionResult UploadImages(long noteid, IFormFile img)
        {
            try
            {
                int userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _inoteBL.UploadImage(noteid, userid, img);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Image Upload  Successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Error while uploading image" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = "Error while uploading image", Error = ex.Message });
            }
        }

    }
}
