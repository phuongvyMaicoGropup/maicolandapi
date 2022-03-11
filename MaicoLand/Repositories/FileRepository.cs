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
    public class FileRepository : IFileRepository
    {
         string url  = "https://localhost:5001/api/Image/UpFile";
        private AmazonS3Client client;
        public string accessKey;
        public string secretKey;
        public string bucketName;
        private AmazonS3Config config;
        private static string _bucketSubdirectory = String.Empty;

        public FileRepository(IMaicoLandDatabaseSettings settings)
        {
            accessKey = settings.AccessKey;
            secretKey = settings.SecretKey;
             //config = new AmazonS3Config();
            bucketName = settings.BucketName;
            config = new AmazonS3Config();
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
            string extension = "";
            switch (contentType)
            {
                case "image/png":
                    extension = ".png";
                    break;
                 default:
                    break; 
            }
            string fileName =path + "/"+ Guid.NewGuid().ToString() + extension;  
            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.ContentType = contentType;
            request_generate_url.BucketName = bucketName;
            request_generate_url.Key = fileName     ;
            request_generate_url.Expires = DateTime.Now.AddMinutes(60);
            request_generate_url.Verb = HttpVerb.PUT;


            return client.GetPreSignedURL(request_generate_url);



        }
        public string GetLinkFile(string path)
        {
            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.BucketName = bucketName; 
            request_generate_url.Key = path;
            request_generate_url.Expires = DateTime.Now.AddMinutes(60);
            return client.GetPreSignedURL(request_generate_url); 
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
        public async Task<FileResponseResult>  GetFile(string path , string key) 
        {
            try
            {

                GetObjectRequest request_download = new GetObjectRequest();
                request_download.BucketName = bucketName;
                request_download.Key = path + key;
                GetObjectResponse response = await client.GetObjectAsync(request_download);
                //if (response.Headers.ContentType == "text/plain")
                //{
                using (Stream responseStream = response.ResponseStream)
                {
                    Console.WriteLine("Success");
                    var bytes = ReadStream(responseStream);
                    var download = new FileContentResult(bytes, response.Headers.ContentType);
                    download.FileDownloadName = key;

                    var result = new FileResponseResult(true, download);
                    //Console.WriteLine(result.file.ContentType)
                    Console.WriteLine(result.file.FileContents);
                    return result;
                }
            }

            //}

            catch (Exception e)
            {
                return new FileResponseResult(false);
            }
            return new FileResponseResult(false);

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

        public Task<HttpResponseMessage> Upload(Models.FileInfo uploadMeta)
        {
            throw new NotImplementedException();
        }
    }
}
