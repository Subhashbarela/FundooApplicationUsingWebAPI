// <copyright file="UserRepo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace RepositoryLayer.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using CommonLayer.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using RepositoryLayer.Context;
    using RepositoryLayer.Entity;
    using RepositoryLayer.Interfaces;

    public class UserRepo : IUserRepo
    {
        private readonly IConfiguration config;
        private readonly FundooContext fundooContext;

        public UserRepo(FundooContext fundooContext, IConfiguration config)
        {
            this.fundooContext = fundooContext;
            this.config = config;
        }

        public UserEntity UserRegister(RegisterModel register)
        {
            UserEntity userEntity = new UserEntity();
            userEntity.FirstName = register.FirstName;
            userEntity.LastName = register.LastName;
            userEntity.EmailId = register.EmailId;
            userEntity.Password = EncryptPass(register.Password);

            this.fundooContext.User.AddAsync(userEntity);
            this.fundooContext.SaveChanges();
            return userEntity;
        }

        public string Login(UserLogin login)
        {
            UserEntity user = this.fundooContext.User.SingleOrDefault(e => e.EmailId == login.EmailId);
            if (this.IsValidPass(DecryptPass(user.Password), login.Password))
            {
                var token = this.GenerateJWToken(login.EmailId, user.UserId);
                return token;
            }

            return null;
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

        public string GenerateJWToken(string emailId, int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email", emailId),
                new Claim("UserId", userId.ToString()),
            };
            var token = new JwtSecurityToken(
                this.config["Jwt:Issuer"],
                this.config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ForgotPasswordModel UserForgotPassword(string email)
        {
            try
            {
                var result = this.fundooContext.User.Where(e => e.EmailId == email).FirstOrDefault();
                ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                forgotPasswordModel.Email = result.EmailId;
                forgotPasswordModel.Token = this.GenerateJWToken(result.EmailId, result.UserId);
                forgotPasswordModel.UserID = result.UserId;

                return forgotPasswordModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ResetPassword(UserResetPasswordModel model, string email)
        {
            try
            {
                var result = this.fundooContext.User.Where(e => e.EmailId == email).FirstOrDefault();
                result.Password = EncryptPass(model.ConfirmPassword);
                this.fundooContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        private bool IsValidPass(string storePass, string prevPass)
        {
            return storePass == prevPass;
        }
    }
}
