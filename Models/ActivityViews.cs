
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System;
namespace belt_exam.Models{
    public class ActivityViews{
        [Required]
        [MinLength(2)]
        public string Title{get;set;}
        [Required]
        [MinLength(10,ErrorMessage="Description should be at least 10 characters long!")]
        public string Description{get;set;}
        [Required]
        [CheckFuture]
        [DataType(DataType.DateTime)]
        public DateTime DateTime{get;set;}
        [Required]
        [DataType(DataType.Text)]
        public string Duration{get;set;}
    }
}