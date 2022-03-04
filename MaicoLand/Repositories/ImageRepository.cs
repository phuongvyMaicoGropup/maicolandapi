using System;
using System.Collections.Generic;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
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
            //config = new AmazonS3Config();
            bucketName = settings.BucketName;
            config = new AmazonS3Config();
            config.ServiceURL = "https://hn.ss.bfcplatform.vn";
            config.RegionEndpoint = RegionEndpoint.APNortheast1;
            config.ForcePathStyle = true;
            client = new AmazonS3Client(
             accessKey,
             secretKey,
             config
           );
        }
        //public String UploadFile(Models.FileInfo uploadMeta)
        //{
        //    var fileTransferUtility = new TransferUtility(client);
        //    try
        //    {
        //        HeadersCollection headers = new HeadersCollection();

        //        headers.ContentType = "image/png";
        //        headers.Expires = DateTime.Now.AddHours(3);
        //        //                esponse Headers

        //        //HTTP / 1.1 200 OK
        //        //  Server: ASP.NET Development Server / 10.0.0.0
        //        //Date: Sun, 28 Aug 2011 13:54:50 GMT
        //        //X - AspNet - Version: 4.0.30319
        //        //Cache - Control: private
        //        //  Content-Type: image/jpeg
        //        //Content-Length: 24255
        //        //Connection: Close


        //        //Request Headersview source
        //        //Host localhost:50715
        //        //User-Agent Mozilla/5.0 (Windows NT 6.1; WOW64; rv:6.0) Gecko/20100101 Firefox/6.0
        //        //Accept image/png,image/*;q=0.8,*/*;q=0.5
        //        //Accept-Language en-us,en;q=0.5
        //        //Accept-Encoding gzip, deflate
        //        //Accept-Charset ISO-8859-1, utf-8; q=0.7,*;q=0.7
        //        //Connection keep-alive
        //        //Referer http://localhost:50715/MySite/SiteHome.html
        //        //Pragma no-cache
        //        //Cache-Control no-cache
        //        var fileName = "/images/" + Guid.NewGuid().ToString() + ".png";


        //        var fileTransferUtilityRequest = new TransferUtilityUploadRequest
        //        {
        //            BucketName = bucketName,
        //            FilePath = uploadMeta.Path,
        //            StorageClass = S3StorageClass.StandardInfrequentAccess,
        //            PartSize = 6291456, // 6 MB.  
        //            Key = fileName,
        //            CannedACL = S3CannedACL.PublicRead,
        //            ContentType = uploadMeta.ContentType,

        //        };
        //        fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
        //        fileTransferUtilityRequest.Metadata.Add("param2", "Value2");
        //        fileTransferUtility.Upload(fileTransferUtilityRequest);
        //        fileTransferUtility.Dispose();
        //        return client.GetPreSignedURL(fileTransferUtilityRequest);


        //    }

        //    catch (AmazonS3Exception amazonS3Exception)
        //    {

        //        if (amazonS3Exception.ErrorCode != null &&
        //            (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
        //            ||
        //            amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
        //        {
        //            return "Invalid";
        //        }
        //        else
        //        {
        //            var t = amazonS3Exception.ErrorType.ToString() + amazonS3Exception.Message;
        //            return (t + "-" + amazonS3Exception.ErrorCode.ToString());
        //        }
        //    }
        //}

        public void DeleteFile(String filePath)
        {
            //client.DeleteObject(bucketName, filePath);
        }
        public string GetUploadLink(Models.FileInfo uploadMeta)
        {
            var fileName = "images/" + Guid.NewGuid().ToString();
            GetPreSignedUrlRequest request_generate_url = new GetPreSignedUrlRequest();
            request_generate_url.ContentType = uploadMeta.ContentType;
            request_generate_url.BucketName = bucketName;
            request_generate_url.Key = fileName;
            request_generate_url.Expires = DateTime.Now.AddMinutes(30);
            request_generate_url.Verb = HttpVerb.PUT;
            //request_generate_url.= S3CannedACL.PublicRead;


            return client.GetPreSignedURL(request_generate_url);
            //return "";
           

        }

        public string UploadFile(Models.FileInfo uploadMeta)
        {
            throw new NotImplementedException();
        }
    }
}
