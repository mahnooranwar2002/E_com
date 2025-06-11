namespace Bookstore.Models
{
    public class mainmodel
    {
        public List<User> admin_data { get; set; }
        public User admin_profiles { get; set; }
        public List<User> user_details { get; set; }
        public List<authors> auth_details { get; set; }
        public authors author_data { get; set; }
        public List<genre> genres_details { get; set; }
        public genre genres_data { get; set; }
        public List<Book> book_details { get; set; }
        public Book book_data { get; set; }
        public cart cart_data { get; set; }
        public List<cart> cart_details { get; set; }
        public List<wishlist> wishlist_detail { get; set; }
        public List<order> orders_Details { get; set; }
        public List<Contact> contact_detail { get; set; }
        public List<Contact> contact_Data { get; set; }
    }
}
