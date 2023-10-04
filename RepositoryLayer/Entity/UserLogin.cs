namespace RepositoryLayer.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class UserLogin
    {
        public string EmailId { get; set; }

        public string Password { get; set; }
    }
}
