using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebsterWebApp.Models
{
    public class RegistrationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        //Label relevant field on the front end as 'User Name' instead of default 'Email'
        [Display(Name = "User Name")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[\d])(?=.*[\W])(?=.*[A-Z]).*$", ErrorMessage = "Min 8 chars, min 1 capital letter, min 1 digit, min 1 special char")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        //public string Avatar { get; set; }

        public bool AcceptUserAgreement { get; set; }

        //Reponse from server if registration is invalid
        public string RegistrationInValid { get; set; }
    }
}
