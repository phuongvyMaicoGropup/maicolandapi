using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository _newsRepository;
        private readonly IFileRepository _fileRepository;

        public NewsController(INewsRepository newsRepository, IFileRepository fileRepository)
        {
            _newsRepository = newsRepository;
            _fileRepository = fileRepository; 
        }
        [HttpGet("read")]
        public async Task<IActionResult> Get([FromQuery]PagingParameter pagingParameter) {


            var newsList = _newsRepository.Get(pagingParameter);

            foreach (var item in newsList)
            {
                item.ImageUrl = await _fileRepository.GetLinkFileAsync(item.ImageUrl);
            }

            var metaData = new
            {
                newsList.TotalCount,
                newsList.PageSize,
                newsList.CurrentPage,
                newsList.HasNext,
                newsList.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
            return Ok(newsList);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<News>> Get(string id)
        {
            var news = await _newsRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            return news;
        }
        [HttpGet("search")]
        public async Task<List<News>> Search(string searchKey)
        {
            List<News> newsList = _newsRepository.GetNewsByKeyword(searchKey);
            foreach (var item in newsList)
            {
                item.ImageUrl = await _fileRepository.GetLinkFileAsync(item.ImageUrl);
            }


            return newsList;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(NewsRequest newNews)
        {
            var newsInfo = new News()
            {
                Title = newNews.Title,
                Content = newNews.Content,
                HashTags = newNews.HashTags,
                ImageUrl = newNews.ImageUrl,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedBy = newNews.CreateBy,
            }; 
            await _newsRepository.CreateAsync(newsInfo);

            return CreatedAtAction(nameof(Get), new { id = newsInfo.Id }, newNews);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, News updatedNews)
        {
            var news = await _newsRepository.GetAsync(id);

            if ( news is null)
            {
                return NotFound();
            }

            updatedNews.Id = news.Id;

            await _newsRepository.UpdateAsync(id, updatedNews);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var news = await _newsRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            await _newsRepository.RemoveAsync(id);

            return NoContent();
        }
    }
}
