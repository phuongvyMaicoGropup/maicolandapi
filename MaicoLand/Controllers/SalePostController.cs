using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaicoLand.Models;
using MaicoLand.Models.Requests;
using MaicoLand.Models.StructureType;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MaicoLand.Controllers
{
    [Route("api/salepost")]
    [ApiController]
    public class SalePostController : ControllerBase
    {
        
        private readonly IPostRepository<SalePost> _salePostRepository;
        private readonly IFileRepository _fileRepository;

        public SalePostController(IPostRepository<SalePost> salePostRepository, IFileRepository fileRepository)
        {
            _salePostRepository = salePostRepository;
            _fileRepository = fileRepository;
        }
        [HttpGet("read")]
        public async Task<IActionResult> Get([FromQuery] PagingParameter pagingParameter)
        {
            var list = _salePostRepository.Get(pagingParameter);

            var metaData = new
            {
                list.TotalCount,
                list.PageSize,
                list.CurrentPage,
                list.HasNext,
                list.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
            return Ok(list);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<SalePost>> Get(string id)
        {
            var item = await _salePostRepository.GetAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }
        [HttpGet("search")]
        public async Task<List<SalePost>> Search(string searchKey)
        {
            List<SalePost> newsList = _salePostRepository.GetItemByKeyword(searchKey);
            //foreach (var item in newsList)
            //{
            //    item.Images.ForEach(async i => i = await _fileRepository.GetLinkFileAsync(i));

            //}


            return newsList;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(SalePostRequest item)
        {
            var post = new SalePost()
            {
                Title = item.Title,
                Content = item.Content,
                Address = item.Address,
                IsAvailable = item.IsAvailable,
                IsNegotiable = item.IsNegotiable,
                Point = item.Point,
                Area = item.Area,
                Cost = item.Cost,
                Images = item.Images,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedBy = item.CreatedBy,
                Type = item.Type,
                IsPrivate= item.IsPrivate,
            };
            await _salePostRepository.CreateAsync(post);

            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<bool> Update(string id, SalePost updatedItem)
        {
            var news = await _salePostRepository.GetAsync(id);

            if (news is null)
            {
                return false; 
            }

            updatedItem.Id = news.Id;

            await _salePostRepository.UpdateAsync(id, updatedItem);

            return true; 
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<bool> Delete(string id)
        {
            var news = await _salePostRepository.GetAsync(id);

            if (news is null)
            {
                return false; 
            }

            await _salePostRepository.RemoveAsync(id);

            return true; 
        }
        [HttpGet("author/{id:length(24)}")]
        public async Task<List<SalePost>> SearchNewsByAuthorId(string id)
        => _salePostRepository.GetItemByAuthorId(id);
          

    }
}
