using MaicoLand.Models;
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
        public LandPlanningController(ILandPlanningRepository landPlanningRepository)
        {
            _landPlanningRepository = landPlanningRepository;
        }
        [HttpGet("read")]
        public IActionResult Get([FromQuery] PagingParameter pagingParameter)
        {
            var landPlanningList = _landPlanningRepository.Get(pagingParameter);
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

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<LandPlanning>>> Search(string searchKey, string idAddress1 , string idAddress2)
        {
            //if (idAddress1!="")
            List<LandPlanning> items =new List<LandPlanning>();

            if (searchKey != "")
            {
                items = _landPlanningRepository.GetLandByKeyword(searchKey);

            }

            
            return items;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post(LandPlanningRequest newLandPlanning)
        {
            var landInfo = new LandPlanning()
            {
                ImageUrl = newLandPlanning.ImageUrl,
                Title = newLandPlanning.Title,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                ExpirationDate = DateTime.Parse(newLandPlanning.ExpirationDate),
                CreatedBy= newLandPlanning.CreatedBy,
                Content = newLandPlanning.Content,
                FilePdfUrl = newLandPlanning.FilePdfUrl,
                LandArea = newLandPlanning.LandArea,
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
            var news = await _landPlanningRepository.GetAsync(id);

            if (news is null)
            {
                return NotFound();
            }

            updatedLandPlanning.Id = news.Id;

            await _landPlanningRepository.UpdateAsync(id, updatedLandPlanning);

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
