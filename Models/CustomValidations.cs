using System;
using System.ComponentModel.DataAnnotations;
namespace belt_exam.Models
{

    public class CheckFuture: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext){
            DateTime Today = DateTime.Now;
            
            if((DateTime)value<Today )
            {
                return new ValidationResult("Date must be in the Future");
            }
            return ValidationResult.Success;
        }
    }
}