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
        string url = "https://localhost:5001/api/Image/UpFile";
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
            config.ServiceURL = "https://hn.ss.bfcplatform.vn";
            config.ForcePathStyle = true;
            client = new AmazonS3Client(
            settings.AccessKey,
            settings.SecretKey,
             config
           );
        }


        public void DeleteFile(String filePath)
        {
            //client.DeleteObject(bucketName, filePath);
        }
        public string GetUploadLinkAsync(string path, string contentType)
        {

            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.ContentType = contentType;
            request_generate_url.BucketName = bucketName;
            request_generate_url.Key = path;
            request_generate_url.Expires = DateTime.Now.AddMinutes(60);
            request_generate_url.Verb = HttpVerb.PUT;
            return client.GetPreSignedURL(request_generate_url);



        }
        public async Task<string> GetLinkFileAsync(string path)
        {
            PutCORSConfigurationRequest request_put_cors = new PutCORSConfigurationRequest();
            request_put_cors.BucketName = bucketName;
            CORSRule cors_rule = new CORSRule();
            cors_rule.AllowedHeaders = new List<string> { "*" };
            cors_rule.AllowedMethods = new List<string> { "GET" };
            cors_rule.AllowedOrigins = new List<string> { "*" };
            cors_rule.MaxAgeSeconds = 6000;

            CORSConfiguration cors_config = new CORSConfiguration();
            cors_config.Rules = new List<CORSRule> { cors_rule };

            request_put_cors.Configuration = cors_config;

            PutCORSConfigurationResponse response_put_cors = new PutCORSConfigurationResponse();
            response_put_cors = await client.PutCORSConfigurationAsync(request_put_cors);
            Console.WriteLine("Put bucket CORS status " + response_put_cors.HttpStatusCode);

            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.BucketName = bucketName;
            request_generate_url.Key = path;
            request_generate_url.Expires = DateTime.Now.AddMinutes(60);
            return client.GetPreSignedURL(request_generate_url);
        }

        public Task<string> DownloadFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
