using System;
using System.Collections.Generic;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Http;

namespace MaicoLand.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private AmazonS3Client client;
        public string accessKey;
        public string secretKey;
        public string bucketName; 

        public ImageRepository(IMaicoLandDatabaseSettings settings)
        {
             accessKey = settings.AccessKey;
             secretKey = settings.SecretKey;
            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = "https://hn.ss.bfcplatform.vn";
            config.ForcePathStyle = true;
            client = new AmazonS3Client(
              accessKey,
              secretKey,
              config
            );
        }
        public String UploadFile(String filePath)
        {
            String key = "images/"+Guid.NewGuid().ToString()+".png";
            PutObjectRequest request_put_object = new PutObjectRequest();
            request_put_object.BucketName = bucketName;
            request_put_object.Key = key.ToString();
            request_put_object.FilePath = filePath;
            request_put_object.CannedACL = S3CannedACL.PublicRead; 
            PutObjectResponse respone_put = client.PutObject(request_put_object);
            return key; 
        }

        public void DeleteFile(String filePath)
        {
            client.DeleteObject(bucketName,filePath);
        }
        public string GetUploadLink(FIleInfo uploadMeta)
        {
            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.ContentType = uploadMeta.ContentType;
            request_generate_url.BucketName = bucketName;
            request_generate_url.Key = uploadMeta.Path;
            request_generate_url.Expires = DateTime.Now.AddMinutes(30);
            request_generate_url.Verb = HttpVerb.PUT;
            return client.GetPreSignedURL(request_generate_url);

        }
    }
}
