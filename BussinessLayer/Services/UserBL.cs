using BussinessLayer.Interfaces;
using CommonLayer.Models;
using MassTransit.Contracts;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;

namespace BussinessLayer.Services
{
    public class UserBL: IUserBL
    {
        private readonly IUserRepo _userRepo;                 
        public UserBL(IUserRepo userRepo)
        {
            _userRepo=userRepo;        
          
        }
        public UserEntity UserRegister(RegisterModel register)
        {      
            return _userRepo.UserRegister(register);
        }
        public string Login(UserLogin login)
        {
            return _userRepo.Login(login);
        }
        public ForgotPasswordModel UserForgotPassword(string email)
        {
            return _userRepo.UserForgotPassword(email);
        }
        public bool ResetPassword(UserResetPasswordModel model, string email)
        {
            return _userRepo.ResetPassword(model, email);
        }
    }
}
