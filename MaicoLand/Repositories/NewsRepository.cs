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
            IMaicoLandDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _newsCollection = database.GetCollection<News>("News");
        }

        public  PagedList<News> Get(PagingParameter pagingParameter)
        {
            return PagedList<News>.ToPagedList(_newsCollection.AsQueryable<News>(), pagingParameter.pageNumber, pagingParameter.pageSize);
        }

        public async Task<News> GetAsync(string id) =>
            await _newsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();    

        public async Task CreateAsync(News newNews) =>
            await _newsCollection.InsertOneAsync(newNews);

        public async Task UpdateAsync(string id, News updatedNews) =>

            await _newsCollection.ReplaceOneAsync(x => x.Id == id, updatedNews);
 
        public async Task RemoveAsync(string id) =>
            await _newsCollection.DeleteOneAsync(x => x.Id == id);
        public List<News> GetNewsByKeyword(string key)
        {

            return _newsCollection.AsQueryable<News>().Where(a => (a.Title.Contains(key))).ToList(); 


        }

    }
}
