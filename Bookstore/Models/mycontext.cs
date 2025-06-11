using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models
{
    public class mycontext:DbContext
    {
        public mycontext(DbContextOptions<mycontext>options):base(options) 
        {
            

        }
        public DbSet<User>tbl_users { get; set; }
        public DbSet<authors> tbl_authors { get; set; }
        public DbSet<genre> tbl_genres { get; set; }
        public DbSet<Book> tbl_books { get; set; }
        public DbSet<wishlist> tbl_wishlist {  get; set; }
        public DbSet<cart> tbl_cart { get; set; }
        public DbSet<order> tbl_order { get; set; }
        public DbSet<Contact> tbl_contact { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().
                HasOne(p => p.auth_name).
                WithMany(p => p.book_details).HasForeignKey(p => p.auth_id);
            modelBuilder.Entity<Book>().
                HasOne(p => p.genres_name)
                .WithMany(p => p.book_details).HasForeignKey(p => p.genres_id);
        }


    }
}
