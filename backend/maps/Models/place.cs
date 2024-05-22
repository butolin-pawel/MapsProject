using System.ComponentModel.DataAnnotations;

namespace maps.Models
{
    public class place
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string adress { get; set; }
        public string description { get; set; }
        public DateTime dateofcreation { get; set; }
        public place()
        {
            routes = new List<route>();
        }
        public ICollection<route> routes { get; set; }
    }
}
