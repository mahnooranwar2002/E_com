using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class Book
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public string book_cover { get; set; }
        public int qunatiy { get; set; }
        public int price { get; set; }
        public int total { get; set; }
        public int  auth_id {get;set;}
        public authors  auth_name { get; set; }
        public int genres_id { get; set; }
        public genre genres_name { get; set; }

    }
}
