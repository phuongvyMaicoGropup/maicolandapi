using MaicoLand.Models;
using MaicoLand.Models.StructureType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Repositories.InterfaceRepositories
{
    public interface IPostRepository<T>
    {
        public PagedList<String> Get(PagingParameter pagingParameter);

        public Task<T> GetAsync(string id);
        public Task CreateAsync(T newItems);

        public Task UpdateAsync(string id, T updatedItem);
        public List<String> GetItemByKeyword(string key); 
        public Task RemoveAsync(string id);
        public List<String> GetItemByAuthorId(string id);

    }
}
