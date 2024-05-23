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
        //Получение всех мест
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/Place' \ -H 'accept: */*')
        //Response body [ {"id": 1, "name": "Проб", "adress": "Проб", "description": "Проб", "dateofcreation": "1996-05-22T13:38:20.314"} ]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select id, name, adress, description, dateofcreation from places";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
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

        //Добавление нового места
        //Пример Request body { "name": "Площадь", "adress": "Московская 1", "description": "проб", "dateofcreation": "2024-05-22T09:34:03.914Z"}
        //Response body "Added Successfully"
        [HttpPost]
        public JsonResult Post(place p)
        {
            string query = @"insert into places (name, adress, description, dateofcreation) values (@name, @adress, @description, @dateofcreation)";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@name", p.name);
                    myCommand.Parameters.AddWithValue("@adress", p.adress);
                    myCommand.Parameters.AddWithValue("@description", p.description);
                    myCommand.Parameters.AddWithValue("@dateofcreation", p.dateofcreation);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Added Successfully");
        }

        //Редактирование места
        //Пример Request body {"id": 1, "name": "Проб", "adress": "Проб", "description": "Проб", "dateofcreation": "1996-05-22T09:38:20.314Z"}
        //Response body "Updated Successfully"
        [HttpPut]
        public JsonResult Put(place p)
        {
            string query = @"update places set name= @name, adress= @adress, description= @description, dateofcreation= @dateofcreation  where id=@id";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@id", p.id);
                    myCommand.Parameters.AddWithValue("@name", p.name);
                    myCommand.Parameters.AddWithValue("@adress", p.adress);
                    myCommand.Parameters.AddWithValue("@description", p.description);
                    myCommand.Parameters.AddWithValue("@dateofcreation", p.dateofcreation);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Updated Successfully");
        }

        //Удаление места
        //Пример Request body 2 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/Place/2' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from places where id=@id";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
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
