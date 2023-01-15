using System.ComponentModel.DataAnnotations;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskPlanner.Model
{
    public class FileAttachmentModel
    {
        [Key]

        public int AttachmentId { get; set; }
        [MaxLength(100)]
        public string PhotoName { get; set; }
        [MaxLength(100)]
        public string PhotoPath { get; set; }
        
    }
}
