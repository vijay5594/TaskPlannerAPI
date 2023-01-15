using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskPlanner.Model
{
    public class LoginModel
    {
        [Key]
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public bool IsFirstLogin { get; set; }
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
       
    }
}
