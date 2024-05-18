using System.ComponentModel.DataAnnotations;

namespace maps.Models
{
    public class route
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int length { get; set; }
        public string description { get; set; }
        public TimeOnly time { get; set; }
        public route()
        {
            feedbacks = new List<feedback>();
            places = new List<place>();
        }
        public ICollection<feedback> feedbacks { get; set; }
        public ICollection<place> places { get; set; }

    }
}
