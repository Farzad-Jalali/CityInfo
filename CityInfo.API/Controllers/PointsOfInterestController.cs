using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityInfo.API.Model;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{

    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private ICityInfoRepository _repo;


        public ILogger<PointsOfInterestController> _logger;
        public IMailService _mailService;


        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService
            , ICityInfoRepository repo)
        {
            _logger = logger;
            _mailService = mailService;
            _repo = repo;
        }

        [HttpGet("{CityId}/poinsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_repo.CityExists(cityId))
                {
                    _logger.LogInformation($"city with id {cityId} wasn't found when processing poi!");
                    return NotFound();
                }

                var poiList = _repo.GetPointsOfInterestForCity(cityId);

                if (poiList == null)
                {
                    return NotFound();
                }

                var result = Mapper.Map<IEnumerable<PointOfInterestDto>>(poiList);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting pontt of interest , cityId = {cityId}", ex);
                return StatusCode(500, "A Problem happened while handdling your request");
            }
        }


        [HttpGet("{CityId}/poinsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {

            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var poi = _repo.GetPointsOfInterestForCity(cityId);

            if (poi == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<PointOfInterestDto>(poi);

            return Ok(poi);
        }


        [HttpPost("{CityId}/poinsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "Should be different to name");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPointOfinterest = Mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _repo.AddPointOfInterestForCity(cityId , finalPointOfinterest);

            if ( ! _repo.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfinterest.Id }, finalPointOfinterest);
        }


        [HttpPut("{CityId}/poinsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "Should be different to name");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (! _repo.CityExists(cityId))
            {
                return NotFound();
            }


            var poiEntities = _repo.GetPointOfInterestForCity(cityId, id);
            if (poiEntities == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, poiEntities);

            _repo.Save();

            return NoContent();
        }



        [HttpPatch("{cityId}/pointofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntities = _repo.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntities == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntities );

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntities);
            _repo.Save();


            return NoContent();
        }


        [HttpDelete("{cityId}/pointofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {

            if (!_repo.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntities = _repo.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntities == null)
            {
                return NotFound();
            }

            _repo.DeletePointOfInterestForCity(pointOfInterestEntities);

            if (! _repo.Save() )
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }


            _mailService.Send("POI deleted", $"POI {pointOfInterestEntities.Name} with id {pointOfInterestEntities.Id} was deleted");

            return NoContent();
        }
    }
}