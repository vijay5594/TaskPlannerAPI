using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskPlanner.Model
{
    public class TokenRequest
    {
        [Key]
        public int TokenId { get; set; }
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
        public string Token { get; set; }
    }
}
