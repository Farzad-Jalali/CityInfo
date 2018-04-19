using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
            var cityEntities = _repo.GetCities();
            var result = Mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities);

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
                var cityResult = Mapper.Map<CityDto>(city);

                return Ok(cityResult);
            }

            var cityWithoutPointOfInterestResult = Mapper.Map<CityWithoutPointOfInterestDto>(city);

            return Ok(cityWithoutPointOfInterestResult);
        }
    }
}