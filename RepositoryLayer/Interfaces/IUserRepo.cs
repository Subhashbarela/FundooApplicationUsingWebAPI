using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepo
    {
        public UserEntity UserRegister(RegisterModel register);
        public string Login(UserLogin login);
        public ForgotPasswordModel UserForgotPassword(string email);
        public bool ResetPassword(UserResetPasswordModel model, string email);

    }
}
