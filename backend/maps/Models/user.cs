using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace maps.Models
{
    public class user
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public user()
        {
            feedbacks = new List<feedback>();
        }
        public ICollection<feedback> feedbacks { get; set; }
    }
}
