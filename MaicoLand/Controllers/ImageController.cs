using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
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
        //[HttpPost]

        //public Task<bool> Upload(Fi le file)
        //{

        //}

        [HttpGet("GetUploadLink")]

        public string GetUploadLink(string path, string contentType)
        {
            var meta = new FileInfo
            {
                Path = path,
                ContentType = contentType,
            };
            return _imageRepository.GetUploadLink(meta);
        }




    }
}
