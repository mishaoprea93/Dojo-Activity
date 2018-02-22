using System.ComponentModel.DataAnnotations;
namespace belt_exam.Models{
    public class RegisterViews{
        [Required]
        [MinLength(2)]
        [RegularExpression(@"[a-zA-Z''-'\s]{1,40}$",ErrorMessage="First Name can contain only letter characters!")]
        public string FirstName{get;set;}
        [Required]
        [MinLength(2)]
        [RegularExpression(@"[a-zA-Z''-'\s]{1,40}$",ErrorMessage="Last Name can contain only letter characters!")]        
        public string LastName{get;set;}
        [Required]
        [EmailAddress]
        public string Email{get;set;}

        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,12}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase, and one numeric digit.Password Length showl be no less than 8 characters & no more than 12")]
        [DataType(DataType.Password)]
        public string Password{get;set;}

        [DataType(DataType.Password)] 
        [Compare ("Password",ErrorMessage="Password and Password confirmation do not match")]
        public string PasswordConfirmation{get;set;}
    }
    
}