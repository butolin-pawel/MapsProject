using System.ComponentModel.DataAnnotations;

namespace maps.Models
{
    public class admin
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }
}
