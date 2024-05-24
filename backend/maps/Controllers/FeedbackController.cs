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
        private context context;
        public FeedbackController()
        {
            context = new context();
        }
        //Получение всех отзывов с создателем и маршрутом
        //Пример Request body (просто api c get curl -X 'GET' \ 'https://localhost:????/api/Feedback' \ -H 'accept: */*')
        //Response body [ { "id": 1, "description": "Проб", "score": 0, "id1": 2, "name": "Петр", "city": "Киров", "id2": 1, "name1": "Проб", "length": 30, "description1": "Проб", "time": "2020-05-22T12:49:39.488" } ]
        [HttpGet]
        public JsonResult Get()
        {
                var feedbacksWithUsersAndRoutes = context.feedbacks.Join(context.users, f => f.userid,u => u.id, (f, u) => new { feedback = f, user = u }).Join(context.routes,fu => fu.feedback.routeid, r => r.id, (fu, r) => new { id = fu.feedback.id, description = fu.feedback.description, score = fu.feedback.score, id1 = fu.user.id, name = fu.user.name, city = fu.user.city, id2 = r.id, name1 = r.name, length = r.length, description1 = r.description, time = r.time}).ToList();
                var table = new DataTable();
                table.Columns.Add("id", typeof(int));
                table.Columns.Add("description", typeof(string));
                table.Columns.Add("score", typeof(int));
                table.Columns.Add("id1", typeof(int));
                table.Columns.Add("name", typeof(string));
                table.Columns.Add("city", typeof(string));
                table.Columns.Add("id2", typeof(int));
                table.Columns.Add("name1", typeof(string));
                table.Columns.Add("length", typeof(int));
                table.Columns.Add("description1", typeof(string));
                table.Columns.Add("time", typeof(DateTime));

                foreach (var item in feedbacksWithUsersAndRoutes)
                {
                    table.Rows.Add(item.id, item.description, item.score, item.id1, item.name, item.city, item.id2, item.name1, item.length, item.description1, item.time);
                }
                return new JsonResult(table);
        }

        //Добавление отзыва
        //Пример Request body {"description": "норм", "score": 0, "userid": 2, "routeid": 1}
        //Response body "Added Successfully"
        [HttpPost]
        public JsonResult Post(feedback f)
        {
            var newFeedback = new feedback
            {
                description = f.description,
                score = f.score,
                userid = f.userid,
                routeid = f.routeid
            };
            context.feedbacks.Add(newFeedback);
            context.SaveChanges();
            return new JsonResult("Added Successfully");
        }

        //Изменение отзыва
        //Пример Request body {"id": 1, "description": "Проб", "score": 0, "userid": 2, "routeid": 1}
        //Response body "Updated Successfully"
        [HttpPut]
        public JsonResult Put(feedback f)
        {
            var existingFeedback = context.feedbacks.FirstOrDefault(feedback => feedback.id == f.id);
            if (existingFeedback != null)
            {
                existingFeedback.description = f.description;
                existingFeedback.score = f.score;
                existingFeedback.userid = f.userid;
                existingFeedback.routeid = f.routeid;
                context.SaveChanges();
                return new JsonResult("Updated Successfully");
            }
            else
            {
                return new JsonResult("Feedback not found");
            }
        }

        //Удаление отзыва
        //Пример Request body 2 (Просто передаешь id на нужное api при delete curl -X 'DELETE' \ 'https://localhost:????/api/Feedback/2' \ -H 'accept: */*')
        //Response body "Deleted Successfully"
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            var feedbackToDelete = context.feedbacks.FirstOrDefault(feedback => feedback.id == id);

            if (feedbackToDelete != null)
            {
                context.feedbacks.Remove(feedbackToDelete);
                context.SaveChanges();
                return new JsonResult("Deleted Successfully");
            }
            else
            {
                return new JsonResult("Feedback not found");
            }
        }
    }
}
