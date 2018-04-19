using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Model;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{

    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _repo;

        public CitiesController(ICityInfoRepository repo)
        {
            _repo = repo;
        }
        [HttpGet()]
        public IActionResult GetCities()
        {
            // return Ok(CitiesDataStore.current.cities);
            var cityEntities = _repo.GetCities();
            var result = new List<CityWithoutPointOfInterestDto>();

            foreach (var ce in cityEntities)
            {
                result.Add(new CityWithoutPointOfInterestDto()
                {
                    Id = ce.Id,
                    Description = ce.Description,
                    Name = ce.Name
                });

            }


            return Ok(result);
        }


        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePOI = false)
        {
            var city = _repo.GetCity(id, includePOI);
            if (city == null)
            {
                return NotFound();
            }

            if (includePOI)
            {
                var cityResult = new CityDto()
                {
                    Id = city.Id,
                    Description = city.Description,
                    Name = city.Name
                };

              var poiDtoList =   city.PointsOfInterest.Select(x => new PointOfInterestDto()
                {
                    Id = x.Id,
                    Description= x.Description,
                    Name = x.Name,
                }).ToList();

                cityResult.PointsOfInterest = poiDtoList;

                return Ok(cityResult);
            }

            var cityWithoutPointOfInterestResult = new CityWithoutPointOfInterestDto()
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };

            return Ok(cityWithoutPointOfInterestResult);

            //var city = CitiesDataStore.current.cities.FirstOrDefault(c => c.Id == id);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //return Ok(city);


        }

    }
}