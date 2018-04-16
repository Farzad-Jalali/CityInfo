using CityInfo.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {

        public static CitiesDataStore current { get; } = new CitiesDataStore();

        public List<CityDto> cities { get; set; }

        public CitiesDataStore()
        {
            cities = new List<CityDto>()
            {
                new CityDto(){ Id = 1 , Name= "New york City", Description="has big park", PointsOfInterest= new List<PointOfInterestDto>()
                        { new PointOfInterestDto() {Id=1 , Name="Hyde PArk",Description= "jlkjl" } ,  new PointOfInterestDto() {Id=2 , Name="emperial state",Description= "sf sdf" } , } },
                new CityDto(){ Id = 2 , Name= "antwerp", Description="unfiinished cathedral", PointsOfInterest= new List<PointOfInterestDto>()
                        { new PointOfInterestDto() {Id=3 , Name="cathedral",Description= " khjfljkdsfkhasjkhfd lashdfljkh asjkdfh ;asf" } } },
                new CityDto(){ Id = 3 , Name= "Paris", Description="zczczxczx", PointsOfInterest= new List<PointOfInterestDto>()
                        { new PointOfInterestDto() {Id=4 , Name="tower",Description= "hlkjh djfkhsdj hfljksd" } } },
                new CityDto(){ Id = 4 , Name= "London", Description="hzcxsdfnymuiuikzxczxck"},
                new CityDto(){ Id = 5 , Name= "Tehran", Description="czxczxc"},
                new CityDto(){ Id = 6 , Name= "shiraz", Description="rgegu"},
                new CityDto(){ Id = 7 , Name= "Reading", Description="erfhtyjt"},
            };
        }
    }
}
