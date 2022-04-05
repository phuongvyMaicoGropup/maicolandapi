using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models.StructureType
{
    public class MaicoLandDatabaseSettings : IMaicoLandDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }


    public interface IMaicoLandDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string AccessKey { set; get; }
        string SecretKey { set; get; }
        string BucketName { set; get; }       
    }
}
