using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class authors
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }    
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public List<Book> book_details { get; set; }
    }
}
