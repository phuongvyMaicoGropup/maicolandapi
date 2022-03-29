using MaicoLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface INewsRepository
    {
        public PagedList<News> Get(PagingParameter pagingParameter);

        public Task<News> GetAsync(string id);
        public Task CreateAsync(News newNews);

        public Task UpdateAsync(string id, News updatedNews);
        public List<News> GetNewsByKeyword(string key); 
        public Task RemoveAsync(string id);
        public List<News> GetNewsByAuthorId(string id);

    }
}
