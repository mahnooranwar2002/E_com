using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Contact
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string massage { get; set; }
    }
}
