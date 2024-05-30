using maps.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private context context;
        public RouteController()
        {
            context = new context();
        }
        //Получение всех маршрутов
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/Route' \ -H 'accept: */*')
        //Response body [ {"id": 1, "name": "Проб", "length": 30, "description": "Проб", "time": "2020-05-22T12:49:39.488"} ]
        [HttpGet]
        public JsonResult Get()
        {
            var routes = context.routes.Select(r => new { r.id, r.name, r.length, r.description, r.time }).ToList();

            var table = new DataTable();
            table.Columns.Add("id", typeof(int));
            table.Columns.Add("name", typeof(string));
            table.Columns.Add("length", typeof(int));
            table.Columns.Add("description", typeof(string));
            table.Columns.Add("time", typeof(DateTime));

            foreach (var route in routes)
            {
                table.Rows.Add(route.id, route.name, route.length, route.description, route.time);
            }
            return new JsonResult(table);
        }

        //Добавление маршрута
        //Пример Request body { "name": "Маршрут1", "length": 50, "description": "Крутой", "time": "2024-05-22T09:47:57.378Z"}
        //Response body "Added Successfully"
        [HttpPost]
        public int Post(route r)
        {
            var newRoute = new route
            {
                name = r.name,
                length = r.length,
                description = r.description,
                time = r.time
            };
            context.routes.Add(newRoute);
            
            context.SaveChanges();
            return context.routes.OrderBy(a =>a.id).Last().id;
        }

        //Изменение маршрута
        //Пример Request body {"id": 1, "name": "Проб", "length": 30, "description": "Проб", "time": "2020-05-22T09:49:39.488Z"}
        //Response body "Updated Successfully"
        [HttpPut]
        public JsonResult Put(route r)
        {
            var existingRoute = context.routes.FirstOrDefault(route => route.id == r.id);
            if (existingRoute != null)
            {
                existingRoute.name = r.name;
                existingRoute.length = r.length;
                existingRoute.description = r.description;
                existingRoute.time = r.time;
                context.SaveChanges();
                return new JsonResult("Updated Successfully");
            }
            else
            {
                return new JsonResult("Route not found");
            }
        }

        //Удаление маршрута
        //Пример Request body 2 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/Route/2' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var routeToDelete = context.routes.FirstOrDefault(route => route.id == id);

            if (routeToDelete != null)
            {
                context.routes.Remove(routeToDelete);
                context.SaveChanges();
                return new JsonResult("Deleted Successfully");
            }
            else
            {
                return new JsonResult("Route not found");
            }
        }
    }
}
