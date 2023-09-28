using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class ForgotPasswordModel
    {
        public int  UserID { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
