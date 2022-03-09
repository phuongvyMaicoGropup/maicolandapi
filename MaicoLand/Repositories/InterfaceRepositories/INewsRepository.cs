using MaicoLand.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface INewsRepository
    {
        public Task<List<News>> GetAsync();

        public Task<News> GetAsync(string id);
        public Task CreateAsync(News newNews);

        public Task UpdateAsync(string id, News updatedNews);

        public Task RemoveAsync(string id);
    }
}
