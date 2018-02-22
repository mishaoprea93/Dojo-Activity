using System;
using System.Collections.Generic;
namespace belt_exam.Models{
    public class Activity{
        public int ActivityId{get;set;}
        public int UserId{get;set;}
        public User User{get;set;}
        public string Title{get;set;}
        public string Description{get;set;}
        public DateTime DateTime{get;set;}

        public string Duration{get;set;}
        public DateTime CreatedAt{get;set;}
        public DateTime UpdatedAt{get;set;}
        public List<Join> Joins {get;set;}

        public Activity(){
            Joins=new List<Join>();
            CreatedAt=DateTime.Now;
            UpdatedAt=DateTime.Now;
        }
    }
}