using Azure;
using maps.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private readonly context context;

        public PlaceController(context context)
        {
            this.context = context;
        }


        [HttpGet]
        public JsonResult Get()
        {
            var places = context.places.Select(p => new { p.id, p.name, p.adress, p.longitude, p.latitude, p.description, p.dateofcreation }).ToList();

            var table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("adress", typeof(string));
            table.Columns.Add("longitude", typeof(double));
            table.Columns.Add("latitude", typeof(double));
            table.Columns.Add("description", typeof(string));
            table.Columns.Add("dateofcreation", typeof(DateTime));

            foreach (var place in places)
            {
                table.Rows.Add(place.id, place.name, place.adress, place.longitude, place.latitude, place.description, place.dateofcreation);
            }
            return new JsonResult(table);
        }

       
        [HttpPost]
        public int Post([FromBody] place p)
        {
            var newPlace = new place
            {
                name = p.name,
                adress = p.adress,
                longitude = p.longitude,
                latitude = p.latitude,
                description = p.description,
                dateofcreation = p.dateofcreation
            };
            context.places.Add(newPlace);
            context.SaveChanges();
            return context.places.OrderBy(a => a.id).Last().id;
        }

       
        [HttpPut]
        public JsonResult Put(place p)
        {
            var existingPlace = context.places.FirstOrDefault(place => place.id == p.id);
            if (existingPlace != null)
            {
                existingPlace.name = p.name;
                existingPlace.adress = p.adress;
                existingPlace.longitude = p.longitude;
                existingPlace.latitude = p.latitude;
                existingPlace.description = p.description;
                existingPlace.dateofcreation = p.dateofcreation;
                context.SaveChanges();
                return new JsonResult("Updated Successfully");
            }
            else
            {
                return new JsonResult("Place not found");
            }   
        }

      
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var placeToDelete = context.places.FirstOrDefault(place => place.id == id);

            if (placeToDelete != null)
            {
                context.places.Remove(placeToDelete);
                context.SaveChanges();
                return new JsonResult("Deleted Successfully");
            }
            else
            {
                return new JsonResult("Place not found");
            }
        }
    }
}