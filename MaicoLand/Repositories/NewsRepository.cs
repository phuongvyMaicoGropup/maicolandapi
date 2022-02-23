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
    public class NewsRepository  : INewsRepository
    {
        private readonly IMongoCollection<News> _newsCollection;

        public NewsRepository(
            IOptions<MaicoLandDatabaseSettings> maicoLandDatabaseSettings)
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            //maicoLandDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase("MaicoLand");
            //maicoLandDatabaseSettings.Value.DatabaseName);

            _newsCollection = mongoDatabase.GetCollection<News>("News");
                //maicoLandDatabaseSettings.Value.NewsCollectionName);
        }

        public async Task<List<News>> GetAsync() =>
            await _newsCollection.Find(_ => true).ToListAsync();

        public async Task<News?> GetAsync(string id) =>
            await _newsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(News newNews) =>
            await _newsCollection.InsertOneAsync(newNews);

        public async Task UpdateAsync(string id, News updatedNews) =>
            await _newsCollection.ReplaceOneAsync(x => x.Id == id, updatedNews);
 
        public async Task RemoveAsync(string id) =>
            await _newsCollection.DeleteOneAsync(x => x.Id == id);
       
    }
}
