using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaicoLand.Models;
using MaicoLand.Models.StructureType;
using MaicoLand.Repositories.InterfaceRepositories;
using MongoDB.Driver;

namespace MaicoLand.Repositories
{
    public class SalePostRepository : IPostRepository<SalePost>
    {

        private readonly IMongoCollection<SalePost> collection;

        public SalePostRepository(
            IMaicoLandDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            collection = database.GetCollection<SalePost>("SalePost");
        }
            
        public async Task CreateAsync(SalePost newItems)=>
            await collection.InsertOneAsync(newItems);

        public PagedList<String> Get(PagingParameter pagingParameter)
        {
            return PagedList<String>.ToSalePostPagedList(collection.AsQueryable<SalePost>(), pagingParameter.pageNumber, pagingParameter.pageSize);

        }

        public async Task<SalePost> GetAsync(string id) =>
            await collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public List<String> GetItemByAuthorId(string id)=> collection.AsQueryable<SalePost>().Where(a => a.CreatedBy == id).Select((a=> a.Id)).ToList();
        public List<String> GetItemByKeyword(string key)=> collection.AsQueryable<SalePost>().Where(a => (a.Title.Contains(key))).Select((a => a.Id)).ToList();

        public async Task RemoveAsync(string id)=> await collection.DeleteOneAsync(x => x.Id == id);

        public async Task UpdateAsync(string id, SalePost updatedItem)=>await collection.ReplaceOneAsync(x => x.Id == id, updatedItem);

    }
}
