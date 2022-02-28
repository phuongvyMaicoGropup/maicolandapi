using System;
using MaicoLand.Models;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface IImageRepository
    {
        //public Task<List<LandPlanning>> GetAsync();

        //public Task<LandPlanning?> GetAsync(string id);
        //public Task CreateAsync(LandPlanning newLandPlanning);

        //public Task UpdateAsync(string id, LandPlanning updatedLandPlanning);

        //public Task RemoveAsync(string id);
        //public Task UpFile();
        public string UploadFile(FileInfo uploadMeta);
        public void DeleteFile(string filePath);
        public string GetUploadLink(FileInfo uploadMeta); 
    }
}
