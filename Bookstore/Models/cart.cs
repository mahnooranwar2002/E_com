using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class cart

    {
        [Key]
        public int cart_id { get; set; }
        public int product_id { get; set; }
        public int user_id { get; set; }
        public int qunatity { get; set; }
        public int price { get; set; }
        public int total { get; set; }
        [ForeignKey("product_id")]
        public Book  book_data{get;set;}
    }
}
