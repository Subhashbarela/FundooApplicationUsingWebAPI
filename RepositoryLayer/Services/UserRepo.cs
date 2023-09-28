using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepo:IUserRepo
    {  
        private readonly IConfiguration _config;
        private readonly FundooContext _fundooContext;
        public UserRepo(FundooContext fundooContext, IConfiguration config)
        {
            _fundooContext = fundooContext;
            _config = config;
        }
        public UserEntity UserRegister(RegisterModel register)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.FirstName= register.FirstName;
            userEntity.LastName= register.LastName;
            userEntity.EmailId= register.EmailId;
            userEntity.Password = EncryptPass(register.Password);

           _fundooContext.User.AddAsync(userEntity);
            _fundooContext.SaveChanges();
            return userEntity;

        }
        public string  Login(UserLogin login)
        {
            UserEntity user = _fundooContext.User.SingleOrDefault(e => e.EmailId == login.EmailId);
            if (IsValidPass(DecryptPass(user.Password), login.Password))
            {
               
                var token = GenerateJWToken(login.EmailId, user.UserId);
                return token;
            }
            return null;
        }
        private bool IsValidPass(string storePass,string prevPass)
        {
            return storePass==prevPass;
        }
        public static string EncryptPass(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] storePass = ASCIIEncoding.ASCII.GetBytes(password);
                string encryptedPass = Convert.ToBase64String(storePass);
                return encryptedPass;
            }
           
        } 
        public static string DecryptPass(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] encryptedPass = Convert.FromBase64String(password);
                string decryptedPass = ASCIIEncoding.ASCII.GetString(encryptedPass);
                return decryptedPass;
            }           
        }
        public string GenerateJWToken(string  EmailId,int UserId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",EmailId),
                new Claim("UserId",UserId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public ForgotPasswordModel UserForgotPassword(string email)
        {
            try
            {
                var result = _fundooContext.User.Where(e => e.EmailId == email).FirstOrDefault();
                ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                forgotPasswordModel.Email = result.EmailId;
                forgotPasswordModel.Token = GenerateJWToken(result.EmailId, result.UserId);
                forgotPasswordModel.UserID = result.UserId;

                return forgotPasswordModel;
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public bool ResetPassword(UserResetPasswordModel model,string email)
        {
            try
            {
                var result= _fundooContext.User.Where(e =>e.EmailId == email).FirstOrDefault();
                result.Password = EncryptPass(model.ConfirmPassword);
                _fundooContext.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
                throw ex;
            }
        }

       
    }
}
