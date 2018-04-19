using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>()
            {
                new City(){
                    Name = "New york City", Description="has big park", PointsOfInterest= new List<PointOfInterest>()
                        { new PointOfInterest() { Name="Hyde PArk",Description= "jlkjl" } ,  new PointOfInterest() { Name="emperial state",Description= "sf sdf" } , } },
                new City(){
                    Name = "antwerp", Description="unfiinished cathedral", PointsOfInterest= new List<PointOfInterest>()
                        { new PointOfInterest() { Name="cathedral",Description= " khjfljkdsfkhasjkhfd lashdfljkh asjkdfh ;asf" } } },
                new City(){
                    Name = "Paris", Description="zczczxczx", PointsOfInterest= new List<PointOfInterest>()
                        { new PointOfInterest() { Name="tower",Description= "hlkjh djfkhsdj hfljksd" } } },
                new City(){
                    Name = "London", Description="hzcxsdfnymuiuikzxczxck"},
                new City(){
                    Name = "Torento", Description="czxczxc"},
                new City(){
                    Name = "Shiraz", Description="rgegu"},
                new City(){
                    Name = "Reading", Description="erfhtyjt"},
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
