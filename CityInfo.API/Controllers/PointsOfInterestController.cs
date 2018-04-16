using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CityInfo.API.Controllers
{

    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {

        public ILogger<PointsOfInterestController> _logger;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{CityId}/poinsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
               // throw new Exception("Amazoing error");

                var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == cityId);

                if (city == null)
                {
                    _logger.LogInformation($"city with id {cityId} wasn't found");
                    return NotFound();
                }

                return Ok(city.PointsOfInterest);
            }
            catch( Exception ex)
            {
                _logger.LogCritical($"Exception while getting pontt of interest , cityId = {cityId}", ex);
                return StatusCode(500, "A Problem happened while handdling your request");
            }
        }


        [HttpGet("{CityId}/poinsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            var poi = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

            if (poi == null)
            {
                return NotFound();
            }

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

            var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfinterestId = CitiesDataStore.current.cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);


            var finalPointOfinterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfinterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };


            city.PointsOfInterest.Add(finalPointOfinterest);

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

            var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var thePointOfinterestId = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);


            if (thePointOfinterestId == null)
            {
                return NotFound();
            }

            thePointOfinterestId.Name = pointOfInterest.Name;
            thePointOfinterestId.Description = pointOfInterest.Description;

            return NoContent();

        }



        [HttpPatch("{cityId}/pointofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }


            var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var thePointOfinterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);


            if (thePointOfinterestFromStore == null)
            {
                return NotFound();
            }


            var pointOfinterestToPatch =
                    new PointOfInterestForUpdateDto()
                    {
                        Name = thePointOfinterestFromStore.Name,
                        Description = thePointOfinterestFromStore.Description

                    };

            patchDoc.ApplyTo(pointOfinterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(pointOfinterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            thePointOfinterestFromStore.Name = pointOfinterestToPatch.Name;
            thePointOfinterestFromStore.Description = pointOfinterestToPatch.Description;

            return NoContent();
        }


        [HttpDelete("{cityId}/pointofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {

            var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var thePointOfinterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);


            if (thePointOfinterestFromStore == null)
            {
                return NotFound();
            }


            city.PointsOfInterest.Remove(thePointOfinterestFromStore);

            return NoContent();
        }
    }
}