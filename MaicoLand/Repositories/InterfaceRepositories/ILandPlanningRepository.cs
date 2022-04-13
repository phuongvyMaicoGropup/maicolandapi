using MaicoLand.Models;
using MaicoLand.Models.Entities;
using MaicoLand.Models.StructureType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface ILandPlanningRepository
    {
        public PagedList<String> Get(PagingParameter pagingParameter);

        public Task<LandPlanning> GetAsync(string id);
        public Task CreateAsync(LandPlanning newLandPlanning);

        public Task UpdateAsync(string id, LandPlanning updatedLandPlanning);

        public Task RemoveAsync(string id);
        public List<String> GetLandByKeyword(string key, string addressId1, string addressId2);
        public List<String> GetLandByAuthorId(string id); 



    }
}
