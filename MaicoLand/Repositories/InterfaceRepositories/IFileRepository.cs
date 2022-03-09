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
        public Task<HttpResponseMessage> Upload(Models.FileInfo uploadMeta);
        public void DeleteFile(string filePath);
        public string SizeConverter(long bytes);
        public string GetUploadLinkAsync(string path, string contentType);
        public  Task<FileResponseResult> GetFile(string path, string key);
        public string GetLinkFile(string path);
    }
}
