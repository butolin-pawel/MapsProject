using System.ComponentModel.DataAnnotations;
using static Npgsql.PostgresTypes.PostgresCompositeType;

namespace maps.Models
{
    public class feedback
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
        public int score { get; set;
        }
        public int? userid { get; set; }
        public user? user { get; set; }
        public int? routeid { get; set; }
        public route? route { get; set; }

        public feedback()
        {
        }
    }
}
