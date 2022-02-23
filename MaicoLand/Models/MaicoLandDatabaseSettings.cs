using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Models
{
    public class MaicoLandDatabaseSettings : IMaicoLandDatabaseSettings
    {
            public string ConnectionString { get; set; } = null!;

            public string DatabaseName { get; set; } = null!;

            public string NewsCollectionName { get; set; } = null!;
    }
   

    public interface IMaicoLandDatabaseSettings
    {
        string NewsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
