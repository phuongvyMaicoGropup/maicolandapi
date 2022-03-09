using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        private IFileRepository _fileRepository;
        public ImageController(IFileRepository fileRepository)
        {
            _fileRepository = fileRepository;
        }

        
        [HttpGet("getpresignedurl")]
        public string GetPresignedUrl(string path,string contentType)
        {
            return _fileRepository.GetUploadLinkAsync(path,contentType);
        }

        [HttpGet("GetLink")]
        public string GetLinkFromServer(string path)
        {
            return _fileRepository.GetLinkFile(path);
        }

        


    }
}
