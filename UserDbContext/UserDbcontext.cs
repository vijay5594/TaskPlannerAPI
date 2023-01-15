using Microsoft.EntityFrameworkCore;
using TaskPlanner.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskPlanner.UserDbContext
{
    public class UserdbContext : DbContext
    {

        public UserdbContext(DbContextOptions<UserdbContext> Options) : base(Options) { }
        public DbSet<LoginModel> LoginModel { get; set; }
        public DbSet<EmployeeModel> EmployeeModel { get; set; }
        public DbSet<FileAttachmentModel> FileAttachment { get; set; }
        public DbSet<TokenRequest> TokenDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBulider)

        {
            modelBulider.Entity<LoginModel>().ToTable("user_login");
            modelBulider.Entity<EmployeeModel>().ToTable("employee_details");
            modelBulider.Entity<FileAttachmentModel>().ToTable("file_attachment");
            modelBulider.Entity<TokenRequest>().ToTable("token_details");

        }
    }
}

