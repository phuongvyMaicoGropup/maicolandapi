using MaicoLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface ILandPlanningRepository
    {
        public Task<List<LandPlanning>> GetAsync();

        public Task<LandPlanning> GetAsync(string id);
        public Task CreateAsync(LandPlanning newLandPlanning);

        public Task UpdateAsync(string id, LandPlanning updatedLandPlanning);

        public Task RemoveAsync(string id);

    }
}
