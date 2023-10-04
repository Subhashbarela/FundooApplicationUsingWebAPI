using BussinessLayer.Interfaces;
using BussinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollaboratorController : ControllerBase
    {
        private readonly ICollaboratorBL _icollaboratorBL;
        public CollaboratorController(ICollaboratorBL icollaboratorBL)
        {
            _icollaboratorBL = icollaboratorBL;
        }
        [Authorize]
        [HttpPost("AddEmail")]
        public IActionResult AddCollaborator(CollaboratorModel model, int noteid)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result=_icollaboratorBL.AddCollaborator(model, userId,noteid);
                if (result != null)
                {
                    return Ok(new { Success = true, message = "Email Added Successfully", result = result });
                }
                return Ok(new { Success = false, message = "Email Not Added " });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateCollaborator/{id:int}")]
        public IActionResult UpdatesCollaborator(int id, CollaboratorModel model)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result=_icollaboratorBL.UpdateCollaborator(id,userId, model);
                if (result == true)
                {
                    return Ok(new { Success = true, message = "Collaborator Updated Successfully", data = result });
                }
                return Ok(new { Success = false, message = "Collaborator  not Update " });

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteCollaborator(int id)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _icollaboratorBL.DeleteCollaborator(id, userId);
                if (result == true)
                {
                    return Ok(new { Success = true, message = "Collaborator Delete Successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Collaborator id is not match" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetCollaborator")]
        public IActionResult GetLabel()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _icollaboratorBL.GetAllCollaborator(userId);
                if (result != null)
                {
                    return Ok(new { Success = true, message = " Collaborator get Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = " Unable To Get Collaborator " });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
