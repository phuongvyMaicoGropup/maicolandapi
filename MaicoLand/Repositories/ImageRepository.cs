using System;
using System.Collections.Generic;
using System.IO;
using Amazon.S3;
using Amazon.S3.Transfer;
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
        private AmazonS3Config config;

        public ImageRepository(IMaicoLandDatabaseSettings settings)
        {
            accessKey = settings.AccessKey;
            secretKey = settings.SecretKey;
            config = new AmazonS3Config();
            bucketName = settings.BucketName; 
            config.ServiceURL = "https://hn.ss.bfcplatform.vn";
            config.ForcePathStyle = true;
            client = new AmazonS3Client(
             accessKey,
             secretKey,
             config
           );
        }
        public String UploadFile(Models.FileInfo uploadMeta)
        {
            var fileTransferUtility = new TransferUtility(client);
            try
            {
                var fileName = "/images/"+Guid.NewGuid().ToString(); 
              
                   
                    var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        FilePath = uploadMeta.Path,
                        StorageClass = S3StorageClass.StandardInfrequentAccess,
                        PartSize = 6291456, // 6 MB.  
                        Key = fileName,
                        CannedACL = S3CannedACL.PublicRead,
                        ContentType = uploadMeta.ContentType
                    };
                    //fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                    //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
                    fileTransferUtility.Upload(fileTransferUtilityRequest);
                    fileTransferUtility.Dispose();
                return "https://maico-hub-record.ss-hn-1.bizflycloud.vn/maicoland/" + fileName; 


            }

            catch (AmazonS3Exception amazonS3Exception)
            {

                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    return "Invalid";
                }
                else
                {
                    return (amazonS3Exception.ErrorCode.ToString());
                }
            }
        }

        public void DeleteFile(String filePath)
        {
            //client.DeleteObject(bucketName, filePath);
        }
        public string GetUploadLink(Models.FileInfo uploadMeta)
        {
            //GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            //request_generate_url.ContentType = uploadMeta.ContentType;
            //request_generate_url.BucketName = bucketName;
            //request_generate_url.Key = uploadMeta.Path;
            //request_generate_url.Expires = DateTime.Now.AddMinutes(30);
            //request_generate_url.Verb = HttpVerb.PUT;
            //return client.GetPreSignedURL(request_generate_url);
            return "";
           

        }
    }
}
