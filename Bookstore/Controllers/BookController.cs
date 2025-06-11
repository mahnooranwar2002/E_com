using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private mycontext _con;
        public BookController(mycontext con)
        {
            _con = con;
            
        }
        public IActionResult Index()
        {
            var login = HttpContext.Session.GetString("user_session");
            var book_data = _con.tbl_books.ToList();
            if (login != null)
            {
            
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    book_details = book_data,
                    admin_data = admin_details
                };
                TempData["registered"] = "login";
                return View(mydata);
            }
            else
            {
                TempData["not_registered"] = "not_login";
            }
            return View();
            
        }
    }
}
