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
        string connectionString = "Server=127.0.0.1;Port=5432;Database=K;User Id=postgres;Password=123;";

        //Получение всех маршрутов
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/Route' \ -H 'accept: */*')
        //Response body [ {"id": 1, "name": "Проб", "length": 30, "description": "Проб", "time": "2020-05-22T12:49:39.488"} ]
        [HttpGet]
        public JsonResult Get() 
        {
            string query = @"select id, name, length, description, time from routes";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    reader = command.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult(table);
        }

        //Добавление маршрута
        //Пример Request body { "name": "Маршрут1", "length": 50, "description": "Крутой", "time": "2024-05-22T09:47:57.378Z"}
        //Response body "Added Successfully"
        [HttpPost]
        public JsonResult Post(route r)
        {
            string query = @"insert into routes (name, length, description, time) values (@name, @length, @description, @time)";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@name", r.name);
                    myCommand.Parameters.AddWithValue("@length", r.length);
                    myCommand.Parameters.AddWithValue("@description", r.description);
                    myCommand.Parameters.AddWithValue("@time", r.time);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Added Successfully");
        }

        //Изменение маршрута
        //Пример Request body {"id": 1, "name": "Проб", "length": 30, "description": "Проб", "time": "2020-05-22T09:49:39.488Z"}
        //Response body "Updated Successfully"
        [HttpPut]
        public JsonResult Put(route r)
        {
            string query = @"update routes set name= @name, length= @length, description= @description, time= @time  where id=@id";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@id", r.id);
                    myCommand.Parameters.AddWithValue("@name", r.name);
                    myCommand.Parameters.AddWithValue("@length", r.length);
                    myCommand.Parameters.AddWithValue("@description", r.description);
                    myCommand.Parameters.AddWithValue("@time", r.time);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Updated Successfully");
        }

        //Удаление маршрута
        //Пример Request body 2 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/Route/2' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from routes where id=@id";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Deleted Successfully");
        }
    }
}
