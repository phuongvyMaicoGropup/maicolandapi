using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories
{
    public class LandPlanningRepository : ILandPlanningRepository
    {
        private readonly IMongoCollection<LandPlanning> _landPlanningCollection;

        public LandPlanningRepository(
            IMaicoLandDatabaseSettings settings)
        {
            

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _landPlanningCollection = database.GetCollection<LandPlanning>("LandPlanning");

        }

        public async Task<List<LandPlanning>> GetAsync() =>
            await _landPlanningCollection.Find(_ => true).ToListAsync();

        public async Task<LandPlanning> GetAsync(string id) =>
            await _landPlanningCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(LandPlanning newLandPlanning) =>
            await _landPlanningCollection.InsertOneAsync(newLandPlanning);

        public async Task UpdateAsync(string id, LandPlanning updatedLandPlanning) =>
            await _landPlanningCollection.ReplaceOneAsync(x => x.Id == id, updatedLandPlanning);

        public async Task RemoveAsync(string id) =>
            await _landPlanningCollection.DeleteOneAsync(x => x.Id == id);

        
    }
}
