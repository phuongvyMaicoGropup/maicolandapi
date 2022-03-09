using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface IFileRepository
    {
        public Task<HttpResponseMessage> Upload(FileInfo uploadMeta);
        public void DeleteFile(string filePath);
        public void UploadFile(List<IFormFile> files, string subDirectory);
        public string SizeConverter(long bytes);
        public string GetUploadLinkAsync(string path, string contentType);
        public  Task<GetObjectResponse> getFile(string path, string key); 
    }
}
