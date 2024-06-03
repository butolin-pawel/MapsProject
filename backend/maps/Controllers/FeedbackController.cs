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
        
        [HttpGet("{id}")]
        public List<feedback> GetByRoute(int id)
        {
            return context.feedbacks.Where(a => a.routeid == id).ToList();
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
