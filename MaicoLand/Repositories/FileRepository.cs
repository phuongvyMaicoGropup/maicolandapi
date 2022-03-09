using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MaicoLand.Repositories
{
    public class ImageRepository : IImageRepository
    {
         string url  = "https://localhost:5001/api/Image/UpFile";
        private IHostingEnvironment _hostingEnvironment;
        private AmazonS3Client client;
        public string accessKey;
        public string secretKey;
        public string bucketName;
        private AmazonS3Config config;
        private static string _bucketSubdirectory = String.Empty;

        public ImageRepository(IMaicoLandDatabaseSettings settings, IHostingEnvironment hostingEnvironment)
        {
            accessKey = settings.AccessKey;
            secretKey = settings.SecretKey;
            _hostingEnvironment = hostingEnvironment; 
             //config = new AmazonS3Config();
            bucketName = settings.BucketName;
            config = new AmazonS3Config();
            //config.ServiceURL = "https://ss-hn-1.bizflycloud.vn";
            config.ServiceURL= "https://hn.ss.bfcplatform.vn";
            config.ForcePathStyle = true;
            client = new AmazonS3Client(
            settings.AccessKey,
            settings.SecretKey,
             config
           ) ;
        }
        public Task<HttpResponseMessage> Upload(System.IO.FileInfo fileInfo)
        {
            
                try
                {
                    var httpClient = new HttpClient();
                    var multipartFormDataContent = new MultipartFormDataContent();
                    httpClient.BaseAddress = new Uri(url);
                    var fileContent = new ByteArrayContent(File.ReadAllBytes(fileInfo.FullName));
                    multipartFormDataContent.Add(fileContent, "file", fileInfo.Name);
                    return httpClient.PostAsync("upload", multipartFormDataContent);
                }
                catch
                {
                    return null;
                }
            
        }

        public void DeleteFile(String filePath)
        {
            //client.DeleteObject(bucketName, filePath);
        }
        public  string GetUploadLinkAsync(string path , string contentType) 
        {
           
            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.ContentType = contentType;
            request_generate_url.BucketName = bucketName;
            request_generate_url.Key = path;
            request_generate_url.Expires = DateTime.Now.AddMinutes(30);
            request_generate_url.Verb = HttpVerb.PUT;


            return client.GetPreSignedURL(request_generate_url);



        }
        
        public void UploadFile(List<IFormFile> files, string subDirectory)
        {
            subDirectory = subDirectory ?? string.Empty;
            var target = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);
            
            Directory.CreateDirectory(target);
            using (HttpClient client = new HttpClient())
            {

            files.ForEach(async file =>
            {
                
                string fileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);
                if (file.Length <= 0) return;
                var filePath = Path.Combine(target, fileName);
                var link = GetUploadLinkAsync(target, file.ContentType);
                using (var stream = file.OpenReadStream())
                {
                    var contentData = new StreamContent(stream);
                    contentData.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                    var result = await client.PutAsync(link, contentData).ConfigureAwait(false);

                    await file.CopyToAsync(stream);
                    Console.WriteLine(result);
                }
            });
            }

            

        }
        public string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            switch (fileSize)
            {
                case var _ when fileSize < kilobyte:
                    return $"Less then 1KB";
                case var _ when fileSize < megabyte:
                    return $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB";
                case var _ when fileSize < gigabyte:
                    return $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB";
                case var _ when fileSize >= gigabyte:
                    return $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB";
                default:
                    return "n/a";
            }
        }
        public async Task<GetObjectResponse>  getFile(string path , string key) 
        {
            GetObjectRequest request_download = new GetObjectRequest();
            request_download.BucketName = bucketName;
            request_download.Key = path + key;
            GetObjectResponse response = await client.GetObjectAsync(request_download);

            //await response.WriteResponseStreamToFileAsync("/images/",true,System.Threading.CancellationToken.None);
            return response; 
        }
        public static byte[] ReadStream(Stream responseStream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
