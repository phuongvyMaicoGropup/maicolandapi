using MaicoLand.Models;
using MaicoLand.Repositories.InterfaceRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MaicoLand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LandPlanningController : ControllerBase
    {
        private readonly ILandPlanningRepository _landPlanningRepository;
        public LandPlanningController(ILandPlanningRepository landPlanningRepository)
        {
            _landPlanningRepository = landPlanningRepository;
        }
        [HttpGet]
        public async Task<List<LandPlanning>> Get() =>
        await _landPlanningRepository.GetAsync();

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

        [HttpPost]
        public async Task<IActionResult> Post(LandPlanning newLandPlanning)
        {
            await _landPlanningRepository.CreateAsync(newLandPlanning);

            return CreatedAtAction(nameof(Get), new { id = newLandPlanning.Id }, newLandPlanning);
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
