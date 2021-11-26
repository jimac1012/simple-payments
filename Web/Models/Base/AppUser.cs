using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Models.Base
{
    public class AppUser
    {
        public AppUser()
        {
            DateCreated = DateTime.UtcNow;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateCreated { get; set; }
    }
}