using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Data;
using Web.Models.Base;

namespace Web.Models.Manager
{
    public static class AppUserManager
    {
        private static WebDBContext db = new WebDBContext();

        public static bool Create(AppUser appUser)
        {
            try
            {
                db.AppUsers.Add(appUser);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }
    }
}