using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        public  PagedList<LandPlanning> Get(PagingParameter pagingParameter)
        {
            return PagedList<LandPlanning>.ToLandPagedList(_landPlanningCollection.AsQueryable<LandPlanning>(), pagingParameter.pageNumber, pagingParameter.pageSize);

        } 

        public async Task<LandPlanning> GetAsync(string id) =>
            await _landPlanningCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(LandPlanning newLandPlanning) =>
            await _landPlanningCollection.InsertOneAsync(newLandPlanning);

        public async Task UpdateAsync(string id, LandPlanning updatedLandPlanning) =>
            await _landPlanningCollection.ReplaceOneAsync(x => x.Id == id, updatedLandPlanning);

        public async Task RemoveAsync(string id) =>
            await _landPlanningCollection.DeleteOneAsync(x => x.Id == id);
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        public List<LandPlanning> GetLandByKeyword(string key , string addressId1="",string addressId2="")
        {
           
            return _landPlanningCollection.AsQueryable<LandPlanning>().Where(a =>

            (a.Title.Contains(key))&& (a.Address.IdLevel1.Contains(addressId1))&&(a.Address.IdLevel2.Contains(addressId2)) ).ToList();


        }

    }
}
