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
        public PagedList<T> Get(PagingParameter pagingParameter);

        public Task<T> GetAsync(string id);
        public Task CreateAsync(T newItems);

        public Task UpdateAsync(string id, T updatedItem);
        public List<T> GetItemByKeyword(string key); 
        public Task RemoveAsync(string id);
        public List<T> GetItemByAuthorId(string id);

    }
}
