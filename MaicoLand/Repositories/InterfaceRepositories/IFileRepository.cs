using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3.Model;
using MaicoLand.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface IFileRepository
    {
        public void DeleteFile(string filePath);
        public string GetUploadLinkAsync(string path, string contentType);
        public string GetLinkFile(string path);
    }
}
