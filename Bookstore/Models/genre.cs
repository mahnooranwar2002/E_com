using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class genre
    {
        [Key]
        public int id {  get; set; }
        public string genres_id { get; set; }
        public string name { get; set; }
        public List<Book> book_details { get; set; }
    }
}

