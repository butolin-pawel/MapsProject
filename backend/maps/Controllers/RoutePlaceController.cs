using maps.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Получение листов маршо=рутов
        /// </summary>
        /// <returns>Лист маршрутов </returns>
        [HttpGet("{rId}")]
        public ICollection<place> Get(int rId)
        {
            context cnt = new context();
            return  cnt.routes.Where(a => a.id == rId).Include(b => b.places).First().places;
        }

        [HttpPost("{rId}")]
        
        public JsonResult Post(int rId,[FromBody] int[] placesId)
        {
            string query = @"insert into routeplace (placesid, routesid) values (@placesid, @routeid)";
            DataTable table = new DataTable();
            NpgsqlDataReader reader;
            using (NpgsqlConnection connection = new NpgsqlConnection(context.connectionString))
            {
                connection.Open();
                foreach (int pId in placesId)
                {
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, connection))
                    {
                        myCommand.Parameters.AddWithValue("@placesid", pId);
                        myCommand.Parameters.AddWithValue("@routeid", rId);
                        reader = myCommand.ExecuteReader();
                        table.Load(reader);
                        reader.Close();
                    }
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

