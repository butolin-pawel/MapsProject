using maps.Models;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutePlaceController : ControllerBase
    {
        //Получение списка мест маршрутов
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/RoutePlace' \ -H 'accept: */*')
        //Response body [ { "placesid": 1, "routesid": 1, "id": 1, "name": "Проб", "length": 30, "description": "Проб", "time": "2020-05-22T12:49:39.488", "id1": 1, "name1": "Проб", "adress": "Проб", "description1": "Проб", "dateofcreation": "1996-05-22T13:38:20.314"} ]
        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select o.placesid, o.routesid, r.id, r.name, r.length, r.description, r.time, p.id, p.name, p.adress, p.description, p.dateofcreation from routeplace o join routes r ON o.routesid = r.id join places p ON  o.placesid = p.id ";
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

        //Добавление места к маршруту
        //Пример Request body RoutePlace?placesid=1&routesid=1 (Просто передаешь два id на нужное api при post curl -X 'POST' \ 'https://localhost:7084/api/RoutePlace?placesid=1&routesid=1' \ -H 'accept: */*' \ -d '')
        //Response body "Added Successfully"
        [HttpPost]
        public JsonResult Post(int placesid, int routesid)
        {
            string query = @"insert into routeplace (placesid, routesid) values (@placesid, @routesid)";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@placesid", placesid);
                    myCommand.Parameters.AddWithValue("@routesid", routesid);
                    reader = myCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return new JsonResult("Added Successfully");
        }


        //Удаление места у маршрута
        //Пример Request body 1 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/RoutePlace/1' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
        [HttpDelete("{placesid}")]
        public JsonResult Delete(int placesid)
        {
            string query = @"delete from routeplace where placesid=@placesid";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
            {
                connection.Open();
                using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                {
                    myCommand.Parameters.AddWithValue("@placesid", placesid);
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

