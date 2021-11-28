
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Web.Models.Base
{
    public class AppUserBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }

    public class GetInfoBindingModel
    {
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
    }
}