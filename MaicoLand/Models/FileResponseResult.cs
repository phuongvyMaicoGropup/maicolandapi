using System;
using Microsoft.AspNetCore.Mvc;

namespace MaicoLand.Models
{
    public class FileResponseResult
    {
        public bool isSuccess;
        public FileContentResult file;
        public FileResponseResult(bool result)
        {
            isSuccess = false; 

        }
        public FileResponseResult(bool result, FileContentResult _file)
        {
            isSuccess = false;
            file = _file; 

        }
    }
}
