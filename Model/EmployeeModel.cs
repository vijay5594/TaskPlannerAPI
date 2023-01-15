using System;
using System.ComponentModel.DataAnnotations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskPlanner.Model
{
    public class EmployeeModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int EmployeeRefNo { get; set; }
        public bool IsDeleted { get; set; } = false;
       // public int AttachmentId { get; set; }

    }
}
