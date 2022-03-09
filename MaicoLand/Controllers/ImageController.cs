using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3.Model;
using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MaicoLand.Controllers
{
    [Route("api/[controller]")]
    public class ImageController : Controller
    {

        private IImageRepository _imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost(nameof(Upload))]  
        public IActionResult Upload(List<IFormFile> formFiles,  string subDirectory)
        {
            try
            {
                _imageRepository.UploadFile(formFiles, subDirectory);

                return Ok(new { formFiles.Count, Size = _imageRepository.SizeConverter(formFiles.Sum(f => f.Length)) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetFile")]
        public async Task<String> GetFile(string path , string key)
        {
            HttpResponseMessage result = null;
            try
            {
            var file = await _imageRepository.getFile(path,key);

                using (var stream = file)
                {
                    Console.WriteLine(stream.Key);
                    Console.WriteLine($"Kích thước stream {stream.ContentLength} bytes / Vị trí {stream.BucketName}");
                    
                    Console.WriteLine($"{stream.C}");
                    //Console.WriteLine($"Stream có thể : Đọc {stream.CanRead} -  Ghi {stream.CanWrite} - Seek {stream.CanSeek} - Timeout {stream.CanTimeout} ");
                }
                return file.ToString(); 
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }



    }
}
