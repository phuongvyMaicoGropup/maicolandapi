using MaicoLand.Models.Entities;
using MaicoLand.Models.Requests;
using MaicoLand.Models.StructureType;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Controllers
{
    [Route("api/landplanning")]
    [ApiController]
    public class LandPlanningController : ControllerBase
    {
        private readonly ILandPlanningRepository _landPlanningRepository;
        private readonly IFileRepository _fileRepository;

        public LandPlanningController(ILandPlanningRepository landPlanningRepository, IFileRepository fileRepository)
        {
            _landPlanningRepository = landPlanningRepository;
            _fileRepository = fileRepository; 
        }
        [HttpGet("read")]
        public async Task<IActionResult> Get([FromQuery] PagingParameter pagingParameter)
        {
            var landPlanningList = _landPlanningRepository.Get(pagingParameter);
            //foreach (var item in landPlanningList)
            //{
            //    item.ImageUrl = await _fileRepository.GetLinkFileAsync(item.ImageUrl);
            //    item.DetailInfo = await _fileRepository.GetLinkFileAsync(item.DetailInfo);

            //}
            var metaData = new
            {
                landPlanningList.TotalCount,
                landPlanningList.PageSize,
                landPlanningList.CurrentPage,
                landPlanningList.HasNext,
                landPlanningList.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metaData));
            return Ok(landPlanningList);
            
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<LandPlanning>> Get(string id)
        {
            var item = await _landPlanningRepository.GetAsync(id);
            item.ImageUrl = await _fileRepository.GetLinkFileAsync(item.ImageUrl);
            item.DetailInfo = await _fileRepository.GetLinkFileAsync(item.DetailInfo);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }
        [HttpGet("search")]
        public async Task<List<String>> Search(string searchKey, string idAddress1 , string idAddress2)
        {
            if (idAddress1 is null) idAddress1 = "";
            if (idAddress2 is null) idAddress2 = "";
            if (searchKey is null) searchKey = "";


            List<String> landPlanningList = _landPlanningRepository.GetLandByKeyword(searchKey,idAddress1,idAddress2);
            //foreach (var item in landPlanningList)
            //{
            //    item.ImageUrl = await _fileRepository.GetLinkFileAsync(item.ImageUrl);
            //    item.DetailInfo = await _fileRepository.GetLinkFileAsync(item.DetailInfo);

            //}



            return landPlanningList;
        }
        [HttpGet("author/{id:length(24)}")]
        public async Task<List<string>> SearchLandByAuthorId(string id)
        {

            List<string> landPlanningList = _landPlanningRepository.GetLandByAuthorId(id);
            //foreach (var item in landPlanningList)
            //{
            //    item.ImageUrl = await _fileRepository.GetLinkFileAsync(item.ImageUrl);
            //    item.DetailInfo = await _fileRepository.GetLinkFileAsync(item.DetailInfo);

            //}

            return landPlanningList;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(LandPlanningRequest newLandPlanning)
        {
            var landInfo = new LandPlanning()
            {
                ImageUrl = newLandPlanning.ImageUrl,
                Title = newLandPlanning.Title,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                ExpirationDate = DateTime.Parse(newLandPlanning.ExpirationDate),
                CreatedBy= newLandPlanning.CreatedBy,
                Content = newLandPlanning.Content,
                DetailInfo = newLandPlanning.DetailInfo,
                Area = newLandPlanning.Area,
                LeftTop = newLandPlanning.LeftTop,
                LeftBottom = newLandPlanning.LeftBottom,
                RightBottom = newLandPlanning.RightBottom,
                RightTop = newLandPlanning.RightTop,
                Address = newLandPlanning.Address,
               

    };
            await _landPlanningRepository.CreateAsync( landInfo);

            return CreatedAtAction(nameof(Get), new { id = landInfo.Id }, newLandPlanning);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, LandPlanning updatedLandPlanning)
        {
            var land = await _landPlanningRepository.GetAsync(id);

            if (land is null)
            {
                return NotFound();
            }

            updatedLandPlanning.Id = land.Id;

            await _landPlanningRepository.UpdateAsync(id, updatedLandPlanning);

            return NoContent();
        }
        [HttpPut("viewed/{id:length(24)}")]
        public async Task<IActionResult> UpdateViewed(string id)
        {
            var land = await _landPlanningRepository.GetAsync(id);

            if (land is null)
            {
                return NotFound();
            }

            land.Viewed++;

            await _landPlanningRepository.UpdateAsync(id, land);

            return NoContent();
        }
        [HttpPut("saved/{id:length(24)}")]
        public async Task<IActionResult> UpdateSaved(string id)
        {
            var news = await _landPlanningRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            news.Saved++;

            await _landPlanningRepository.UpdateAsync(id, news);

            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var news = await _landPlanningRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            await _landPlanningRepository.RemoveAsync(id);

            return NoContent();
        }
    }
}
