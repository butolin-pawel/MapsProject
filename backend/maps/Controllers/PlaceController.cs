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
        private context context;
        public PlaceController()
        {
            context = new context();
        }
        //Получение всех мест
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/Place' \ -H 'accept: */*')
        //Response body [ {"id": 1, "name": "Проб", "adress": "Проб", "description": "Проб", "dateofcreation": "1996-05-22T13:38:20.314"} ] пример без longitude и latitude
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

        //Добавление нового места
        //Пример Request body { "name": "Площадь", "adress": "Московская 1", "description": "проб", "dateofcreation": "2024-05-22T09:34:03.914Z"} пример без longitude и latitude
        //Response body "Added Successfully"
        [HttpPost]
        public JsonResult Post([FromBody] place p)
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
            return new JsonResult("Added Successfully");
        }

        //Редактирование места
        //Пример Request body {"id": 1, "name": "Проб", "adress": "Проб", "description": "Проб", "dateofcreation": "1996-05-22T09:38:20.314Z"} пример без longitude и latitude
        //Response body "Updated Successfully"
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

        //Удаление места
        //Пример Request body 2 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/Place/2' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
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