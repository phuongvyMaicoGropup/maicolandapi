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
            var meta = new FIleInfo
            {
                Path = path,
                ContentType = contentType,
            };
            return _imageRepository.GetUploadLink(meta);
        }


        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }
}
