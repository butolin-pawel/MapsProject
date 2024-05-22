using maps.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        string connectionString = "Server=127.0.0.1;Port=5432;Database=K;User Id=postgres;Password=123;";

        //Получение всех отзывов с создателем и маршрутом
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/Feedback' \ -H 'accept: */*')
        //Response body [ { "id": 1, "description": "Проб", "score": 0, "id1": 2, "name": "Петр", "city": "Киров", "id2": 1, "name1": "Проб", "length": 30, "description1": "Проб", "time": "2020-05-22T12:49:39.488" } ]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select f.id, f.description, f.score, u.id, u.name, u.city, r.id, r.name, r.length, r.description, r.time from feedbacks f join users u ON f.userid = u.id join routes r ON  f.routeid = r.id ";
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

        //Добавление отзыва
        //Пример Request body {"description": "норм", "score": 0, "userid": 2, "routeid": 1}
        //Response body "Added Successfully"
        [HttpPost]
        public JsonResult Post(feedback f)
        {
            string query = @"insert into feedbacks (description, score, userid, routeid) values (@description, @score, @userid, @routeid)";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@description", f.description);
                    myCommand.Parameters.AddWithValue("@score", f.score);
                    myCommand.Parameters.AddWithValue("@userid", f.userid);
                    myCommand.Parameters.AddWithValue("@routeid", f.routeid);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Added Successfully");
        }

        //Изменение отзыва
        //Пример Request body {"id": 1, "description": "Проб", "score": 0, "userid": 2, "routeid": 1}
        //Response body "Updated Successfully"
        [HttpPut]
        public JsonResult Put(feedback f)
        {
            string query = @"update feedbacks set description= @description, score= @score, userid= @userid, routeid= @routeid  where id=@id";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@id", f.id);
                    myCommand.Parameters.AddWithValue("@description", f.description);
                    myCommand.Parameters.AddWithValue("@score", f.score);
                    myCommand.Parameters.AddWithValue("@userid", f.userid);
                    myCommand.Parameters.AddWithValue("@routeid", f.routeid);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Updated Successfully");
        }

        //Удаление отзыва
        //Пример Request body 2 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/Feedback/2' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from feedbacks where id=@id";
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
