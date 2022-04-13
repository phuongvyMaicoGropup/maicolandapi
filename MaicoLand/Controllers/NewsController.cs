using MaicoLand.Models;
using MaicoLand.Models.Entities;
using MaicoLand.Models.Requests;
using MaicoLand.Models.StructureType;
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
        private readonly IPostRepository<News> _newsRepository;
        private readonly IFileRepository _fileRepository;

        public NewsController(IPostRepository<News> newsRepository, IFileRepository fileRepository)
        {
            _newsRepository = newsRepository;
            _fileRepository = fileRepository; 
        }
        [HttpGet("read")]
        public async Task<IActionResult> Get([FromQuery]PagingParameter pagingParameter) {


            var newsList = _newsRepository.Get(pagingParameter);

            

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
            var listFile = new List<String>();
           
                foreach (var i in news.Images)
                {
                    listFile.Add(await _fileRepository.GetLinkFileAsync(i));
                    news.Images = listFile;
                }



            

            if (news is null)
            {
                return NotFound();
            }

            return news;
        }
        [HttpGet("search")]
        public async Task<List<String>> Search(string searchKey)
        {
            List<String> newsList = _newsRepository.GetItemByKeyword(searchKey);
            //var listFile = new List<String>();
            //foreach (var item in newsList)
            //{
            //    listFile = new List<String>();
            //    foreach (var i in item.Images)
            //    {
            //        listFile.Add(await _fileRepository.GetLinkFileAsync(i));
            //        Console.WriteLine(listFile);
            //        item.Images = listFile;
            //    }



            //}

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
                Images = newNews.Images,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedBy = newNews.CreatedBy,
                Type = newNews.Type,
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
        [HttpPut("viewed/{id:length(24)}")]
        public async Task<IActionResult> UpdateViewed(string id)
        {
            var news = await _newsRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            news.Viewed++;

            await _newsRepository.UpdateAsync(id, news);

            return NoContent();
        }
        [HttpPut("saved/{id:length(24)}")]
        public async Task<IActionResult> UpdateSaved(string id)
        {
            var news = await _newsRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            news.Saved++;

            await _newsRepository.UpdateAsync(id, news);

            return NoContent();
        }
        //[HttpGet("commenthashtags")]
        //public async Task<List<String>> GetCommonHashTags()
        //{
        //    var news = await _newsRepository.GetAsync(id);

        //    if (news is null)
        //    {
        //        return NotFound();
        //    }

        //    updatedNews.Id = news.Id;

        //    await _newsRepository.UpdateAsync(id, updatedNews);

        //    return NoContent();
        //}

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
        [HttpGet("author/{id:length(24)}")]
        public  List<String> SearchNewsByAuthorId(string id)
        {
            return _newsRepository.GetItemByAuthorId(id);
          
        }
        [HttpGet("topviewed")]
        public  List<String> GetTopViewedNews()
        {
            return _newsRepository.GetTopViewedNews();

        }

        [HttpGet("topsaved")]
        public List<String> GetTopSavedNews()
        {
            return _newsRepository.GetTopSavedNews();

        }


    }
}
