using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bookstore.Models
{
    public class wishlist
    {
        [Key]
        public int wish_id { get; set; }
        public int product_id { get; set; }
        public int user_id { get; set; }
        public string product_name { get; set; }

        [ForeignKey("product_id")]
        public Book book_data { get; set; }
        
    }
}
