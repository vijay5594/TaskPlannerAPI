using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskPlanner.Model;
using TaskPlanner.UserDbContext;

namespace TaskPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EmployeeController : ControllerBase
    {
        private readonly UserdbContext dataContext;
        public EmployeeController(UserdbContext _dataContext)
        {
            dataContext = _dataContext;
        }
        [HttpGet("getIndividualEmployeeDetailsById")]
        public IActionResult getIndividualEmployeeDetailsById(int id)
        {
            var details = dataContext.EmployeeModel.Where(a => a.EmployeeId == id).AsNoTracking().FirstOrDefault();
            return Ok(details);
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody] EmployeeModel employeeData)
        {
            if (dataContext.EmployeeModel.Any(x => x.EmployeeRefNo != 0))
            {
                var user = dataContext.EmployeeModel.Select(x => x.EmployeeRefNo).Max();
                employeeData.EmployeeRefNo = user + 1;
            }
            else
            {
                employeeData.EmployeeRefNo = 1000;

            }
                dataContext.EmployeeModel.Add(employeeData);
                dataContext.SaveChanges();
                string RandomPass = RandomString();
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(RandomPass);
                var data = new LoginModel { EmployeeId = employeeData.EmployeeId, Password = passwordHash, EmailId = employeeData.EmailId, IsFirstLogin = true };
                dataContext.LoginModel.Add(data);
                dataContext.SaveChanges();
                SendMail(data.EmailId, RandomPass);
                return Ok(employeeData);
            }
        private void SendMail(string to, string password)
        {
            string from = "info.rsinfosolution.in@gmail.com"; //From address
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Thank you for registering, use your registered mail and below password for access " + "<b>" + password + "</b><br/>Use Below link to Access for access <br/>http://3.110.106.202/app/user/login";
            message.Subject = "Login Credentials for Registered User";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("info.rsinfosolution.in@gmail.com", "mrpkyxbcdcyjdezy");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("ValidateEmail")]

        public IActionResult ValidateEmail(string data)
        {
            var email = dataContext.LoginModel.Where(e => e.EmailId == data).FirstOrDefault();
            if (email == null) return Ok("No User Found");
            return Ok("User Found");

        }

      
        public static string RandomString()
        {
            Random random = new Random();
            const string chars = "AhghghgkBCDjEjShFWrGwHFvHmIlJpKuUWtsLaWqQxMvNnGhBgLtOLjPQPKaQfQdfRSsBTUYIUVWIAXCYZS01Q2C34H56789";
            return new string(Enumerable.Repeat(chars, 12)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        [HttpGet("GetEmployeeDetailsById")]
        public object GetEmployeeDetailsById(int id)
        {
            var res = (from a in dataContext.EmployeeModel
                      
                       where a.EmployeeId == id
                       select new
                       {
                           a.EmployeeId,
                           a.FirstName,
                           a.LastName,
                           a.Role,
                           a.Gender,
                           a.Address,
                           a.DateOfBirth,
                           a.EmailId,
                           a.DateOfJoining,
                           a.MobileNumber,
                           a.EmployeeRefNo,
                           

                       });
            return res;
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(int data)
        {
            var user = dataContext.LoginModel.Where(x => x.EmployeeId == data).FirstOrDefault();
            var employee = dataContext.EmployeeModel.Where(a => a.EmployeeId == data).FirstOrDefault();
            if (user != null && employee.Role == "Admin")
            {
                var res = (from a in dataContext.EmployeeModel
                         
                           where a.IsDeleted == false
                           orderby a.EmployeeId ascending
                           select new
                           {
                               a.EmployeeId,
                               a.FirstName,
                               a.LastName,
                               a.Role,
                               a.Gender,
                               a.Address,
                               a.DateOfBirth,
                               a.EmailId,
                               a.DateOfJoining,
                               a.MobileNumber,
                               a.EmployeeRefNo,
                              
                               

                           }).ToList();
               
                return Ok(res);
            }
            return BadRequest();
        }
        [HttpPut("Update")]
        public IActionResult UpdateEmployee([FromBody] EmployeeModel EmployeeData)
        {
            var res = dataContext.EmployeeModel.AsNoTracking().FirstOrDefault(a => a.EmployeeId == EmployeeData.EmployeeId);
            if (res == null)
            {
                return NotFound();
            }
            else
            {
                EmployeeData.EmployeeId = res.EmployeeId;
                EmployeeData.EmployeeRefNo = res.EmployeeRefNo;
                dataContext.Entry(EmployeeData).State = EntityState.Modified;
                dataContext.SaveChanges();
                return Ok(EmployeeData);
            }
        }
        [HttpDelete("DeleteEmployee")]
        public IActionResult DeleteEmployee(int Id)
        {
            var employee = dataContext.EmployeeModel.Where(a => a.EmployeeId == Id).FirstOrDefault();
            var user = dataContext.LoginModel.Where(a => a.EmployeeId == Id).FirstOrDefault();
            if ((employee != null && employee.IsDeleted == false) && user == null)
            {
                employee.IsDeleted = true;
                dataContext.SaveChanges();
                return Ok();
            }
            if ((employee != null && employee.IsDeleted == false))
            {
                employee.IsDeleted = true;
               
                dataContext.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}

