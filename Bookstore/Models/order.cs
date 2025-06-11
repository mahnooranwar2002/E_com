using System.ComponentModel.DataAnnotations;

namespace Bookstore.Models
{
    public class order
    {
        [Key]
        public int order_id { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string address { get; set; }
        public string cvv { get; set; }
        public string sum { get; set; }
        public string product_count { get; set; }
        public string payment { get; set; }
        public string card_num { get; set; }
        public string card_holder { get; set; }
        public int user_ids { get; set; }
        public string product_name { get; set; }
        public string product_price { get; set; }

        public string product_quantity { get; set; }
        public  int status { get; set; }
        public string day_counts { get; set; }
     
    }
}
