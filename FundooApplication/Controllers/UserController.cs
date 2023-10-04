using BussinessLayer.Interfaces;
using CommonLayer;
using CommonLayer.Models;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
       
        private readonly IUserBL _userBL;
        private readonly FundooContext _fundooContext;
        private readonly IBus _bus;  
        private readonly ILogger<UserController> _logger;
        public UserController(IUserBL userBL, FundooContext fundooContext, IBus bus, ILogger<UserController> logger)
        {
            _userBL = userBL;
            _fundooContext = fundooContext;
            _bus = bus;
            _logger = logger;
           
        }
        [HttpPost("Register")]

        public IActionResult Register(RegisterModel registerModel) 
        {
            try
            {
                if (IsValidEmail(registerModel.EmailId))
                {
                    return BadRequest(new { Status = false,Message = "Email Already Registered"});
                }
                var result =  _userBL.UserRegister(registerModel);
                if (result != null)
                {
                    _logger.LogInformation("User Registration Succesfull");
                    return Ok(new ResponseModel<UserEntity>{ Status = true,Message = "User Registration SuccessFully", Data = result }); 
                }
                else
                {
                    _logger.LogInformation("User Registration UnSuccesfull");
                    return BadRequest(new { Status = false,Message = "User Registration UnSuccessFully" });                   
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Some exception are occure ");

                return BadRequest(new { Status = false, Message = ex.Message});
            }          
        }
        private bool IsValidEmail(string email)
        {
            return _fundooContext.User.Any(e => e.EmailId == email);
        }
        [HttpPost("Login")]
        public  IActionResult LoginUser(UserLogin login)
        {
            try
            {
                var result =  _userBL.Login(login);
                
                    if (result != null)
                    {
                    _logger.LogInformation("User Login Succesfully");
                    return Ok(new  {  Status = true, Message = "User Registration SuccessFully",Data = result });
                    }
                    else
                    {
                    _logger.LogError("Try again to login"); 
                    return BadRequest(new { Status = false,Message = "User Registration UnSuccessFully" });
                    }                
            }
            catch(Exception ex)
            {
                return BadRequest(new { Status = false,Message = ex.Message });
            }           
        }
  

        [HttpPost("forgot-password")]
        public async Task<IActionResult> UserForgotPasswords(string email)
        {
            try
            {
                if (email != null) 
                { 
                    Send send= new Send();
                    ForgotPasswordModel forgotPasswordModel =_userBL.UserForgotPassword(email);
                    send.SendingMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                    Uri uri = new Uri("rabbitmq://localhost//FundoNotesEmail_Queue");
                    var endPoint = await _bus.GetSendEndpoint(uri);

                    await endPoint.Send(forgotPasswordModel);

                    return Ok(new ResponseModel<string> { Status = true, Message = "Email  Send  SuccessFully", Data = forgotPasswordModel.Token });
                }
                return BadRequest((new { Status = true, Message = "Email Not Send  SuccessFully", Response = email }));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        [Authorize]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(UserResetPasswordModel userResetPasswordModel)
        {
            try
            {
                string email = User.FindFirst("Email").Value;
                if (userResetPasswordModel.Password == userResetPasswordModel.ConfirmPassword)
                {
                    bool result = _userBL.ResetPassword(userResetPasswordModel, email);
                    if (result)
                    {
                        return Ok(new ResponseModel<string> { Status = true, Message = "password change successfully", Data = result.ToString() });
                    }
                    return BadRequest(Ok(new ResponseModel<string> { Status = true, Message = "password Not change successfully" }));
                }
                return Ok(new { Status = false, Message = "ConfirmPassword  does not match" });
            }
            catch(Exception ex)
            {               
                throw ex;
            }
        }     
      
       
    }
}
