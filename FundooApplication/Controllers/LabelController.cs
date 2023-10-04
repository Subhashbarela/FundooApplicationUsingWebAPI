using BussinessLayer.Interfaces;
using BussinessLayer.Services;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL _ilabelBL;
        private readonly IDistributedCache _distributedCache;
        public LabelController(ILabelBL ilabelBL, IDistributedCache distributedCache)
        {
            _ilabelBL = ilabelBL;
            _distributedCache = distributedCache;
        }

        [Authorize]
        [HttpPost("AddLable")]
        public IActionResult AddLabels(LabelModel model,long noteid)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _ilabelBL.AddLabel(model, userId, noteid);
                if(result !=null)
                {
                    return Ok(new{ Success = true, message = "Notes Added Successfully", result = result });
                }
                return Ok(new { Success = false, message = "Notes Added Successfully" });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [HttpPut("UpdateLabel/{id:int}")]
        public IActionResult UpdatesLabel(int id, LabelModel model)
        {
            try
            {
                    int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                    var result = _ilabelBL.UpdateLabel(id,userId, model);
                    if (result == true)
                    {
                        return Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                    }
                    return Ok(new { Success = false, message = "Note  not Update " });               
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteLabel(int id)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _ilabelBL.DeleteLabel(id, userId);
                if (result == true)
                {
                    return Ok(new { Success = true, message = "Label Delete Successfully" });
                }
                else
                {
                    return BadRequest(new { Success = false, message = "Label id is not match" });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("GetLabel")]
        public IActionResult GetLabel()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                var result = _ilabelBL.GetAllLabels(userId);
                if (result != null)
                {
                    return Ok(new { Success = true, message = " Label get Successfully", Data = result });
                }
                else
                {
                    return BadRequest(new { Success = false, message = " Unable To Get Lable " });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("GetAllLabelsUsingRedis")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLabelUsingRedisCache()
        {
            try
            {
                var cacheKey = "LabelList";
                string serializedLabelsList;
                var labelList = new List<LabelEntity>();
                var redislabelsList = await _distributedCache.GetAsync(cacheKey);
                if (redislabelsList != null)
                {
                    serializedLabelsList = Encoding.UTF8.GetString(redislabelsList);
                    labelList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelsList);
                }
                else
                {
                    labelList = _ilabelBL.GetLabelsUsingRedisCache().ToList();
                    serializedLabelsList = JsonConvert.SerializeObject(labelList);
                    redislabelsList = Encoding.UTF8.GetBytes(serializedLabelsList);
                    var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await _distributedCache.SetAsync(cacheKey, redislabelsList, options);
                }
                return Ok(labelList);


            }
            catch (Exception ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
