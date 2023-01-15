using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using TaskPlanner.Model;
using TaskPlanner.UserDbContext;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskPlanner.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]

    public class FileAttachment : ControllerBase
    {
        private readonly UserdbContext dataContext;
        private string photoFileName;
        private string photoPathForDb;
     

        public FileAttachment(UserdbContext _dataContext)
        {
            dataContext = _dataContext;
        }
        [HttpPost("Attachment")]

        public IActionResult UploadFileAttachment(IFormFile files)
        {

            try
            {
                var file = Request.Form.Files[0];
               
                var date = DateTime.Now.Date.Month.ToString() + " " + DateTime.Now.Date.Year.ToString() + " " + DateTime.Now.Day.ToString();
                var folderName = Path.Combine("Resource", "Images", date);
                var pathtoSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    Directory.CreateDirectory(pathtoSave);
                    var fileName = file.FileName.Trim('"');
                    var fullPath = Path.Combine(pathtoSave, fileName).ToString();
                    var fileExtension = Path.GetExtension(fileName);
                    var dbpath = Path.Combine(folderName, fileName);
                    var filePathAttachment = Path.Combine(folderName, fileName).ToString();
                    using (var stream = new FileStream(fullPath, FileMode.Append))
                    {
                        file.CopyTo(stream);
                    }
                    var fileDetails = SaveFileToDB(fileName, filePathAttachment);

                    return Ok(fileDetails);

                }

                return BadRequest();
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        private FileAttachmentModel SaveFileToDB(string photoName, string photoPath)
        {
            var objFiles = new FileAttachmentModel()
            {
                AttachmentId = 0,
                PhotoName = photoName,
                PhotoPath = photoPath,
             
            };

            dataContext.FileAttachment.Add(objFiles);
            dataContext.SaveChanges();
            return objFiles;
        }

        [HttpGet("atttchmentFile")]
        public IActionResult GetAttachmentPath()
        {
            var user = dataContext.FileAttachment.AsQueryable();
            return Ok(user);
        }

       

        [HttpGet("Download")]
        public IActionResult DownloadFileAttachment(int id)
        {
            var file = dataContext.FileAttachment.Where(n => n.AttachmentId == id).FirstOrDefault();
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), file.PhotoPath);
            if (!System.IO.File.Exists(filepath))
                return NotFound();
            var memory = new MemoryStream();
            using (var stream = new FileStream(filepath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filepath), filepath);
        }
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {

                contentType = "application/octet-stream";
            }

            return contentType;
        }

    }

}