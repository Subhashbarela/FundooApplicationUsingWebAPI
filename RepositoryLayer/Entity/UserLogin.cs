using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class UserLogin
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}
