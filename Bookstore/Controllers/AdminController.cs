
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;

namespace Bookstore.Controllers
{
    public class AdminController : Controller
    {
        private mycontext _con;
        private IWebHostEnvironment _evn;
        public AdminController(mycontext con, IWebHostEnvironment evn)
        {
            _evn = evn;
            _con = con;
        }
        public IActionResult Index()
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                var admin_count = _con.tbl_users.Where(e => e.is_admin == 1).Count();
                var user_count = _con.tbl_users.Where(e => e.is_admin == 0).Count();
              
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                TempData["admin_count"] = admin_count;
                TempData["User_count"] = user_count;
                TempData["genres_count"]= _con.tbl_genres.Count();
                TempData["authors_count"] = _con.tbl_authors.Count();
                TempData["Books_count"] = _con.tbl_books.Count();
                TempData["orders_count"] = _con.tbl_order.Count();
             
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        [HttpGet]
        public IActionResult user_details(string textsreach)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                List<User>  user_data = new List<User>();

                if (string.IsNullOrEmpty(textsreach))
                {
                    user_data = _con.tbl_users.Where(u => u.is_admin == 0).ToList();

                }
                else
                {
                    user_data = _con.tbl_users.FromSqlInterpolated($"select * from tbl_users where is_admin=0 and Name like'%'+{textsreach}+'%' or Email like'%'+{textsreach}+'%' or email like'%'+{textsreach}+'%' ").ToList();
                }
                if (user_data.Count == 0)
                {
                    TempData["count"] = $"not any record is founded {textsreach}";
                }

              
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {user_details=user_data,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        public IActionResult staus_user(int id)
        {
            var find = _con.tbl_users.Find(id);
            if (find.status == 1)
            {
                find.status = 0;
            }
            else
            {
                find.status = 1;
            }
            _con.SaveChanges();
            return RedirectToAction("user_details");
        }
        public IActionResult delete_user(int id)
        {
            var find = _con.tbl_users.Find(id);
            _con.tbl_users.Remove(find);
            _con.SaveChanges();
            return RedirectToAction("user_details");
        }
        public IActionResult admin_profile(int id)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                var data_admin = _con.tbl_users.Find(id);
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    admin_profiles = data_admin,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }


        }
        [HttpPost]
        public IActionResult admin_profile(User admin)
        {
            _con.tbl_users.Update(admin);
            _con.SaveChanges();
            return RedirectToAction("index");
        }

        /*for the authors details*/
        [HttpGet]
        public IActionResult author_details(string textsreach)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                List<authors> auth_detail = new List<authors>();

                if (string.IsNullOrEmpty(textsreach))
                {
                    auth_detail = _con.tbl_authors.ToList();

                }
                else
                {
                    auth_detail = _con.tbl_authors.FromSqlInterpolated($"select * from tbl_authors where name like'%'+{textsreach}+'%' or email like'%'+{textsreach}+'%' ").ToList();
                }
                if (auth_detail.Count == 0)
                {
                    TempData["count"] = $"not any record is founded {textsreach}";
                }


                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {

                    auth_details = auth_detail,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }

        }
        [HttpPost]
        public IActionResult add_authors(authors auth)
        {
            var authorexisting = _con.tbl_authors.FirstOrDefault(e => e.email == auth.email);
            if (authorexisting != null)
            {

                TempData["msg"] = "author email is already registred" + "" + auth.email;
                return RedirectToAction("author_details");
            }
            else
            {
                _con.tbl_authors.Add(auth);
                _con.SaveChanges();
                TempData["msg2"] = "The author is added suceesfully ";
                return RedirectToAction("author_details");
            }

        }
        public IActionResult delete_auth(int id)
        {
            var find_id = _con.tbl_authors.Find(id);
            _con.tbl_authors.Remove(find_id);
            _con.SaveChanges();
            return RedirectToAction("author_details");
        }
       
        public IActionResult auth_edit(int id)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {

                var auth = _con.tbl_authors.Find(id);
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    author_data = auth,

                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        [HttpPost]
        public IActionResult auth_edit(authors auth)
        {
            _con.tbl_authors.Update(auth);
            _con.SaveChanges();
            return RedirectToAction("author_details");
        }

        /*for the genres*/
        [HttpGet]
        public IActionResult genres_details(string textsreach)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                List<genre> genres = new List<genre>();

                if (textsreach == null)
                {
                    genres = _con.tbl_genres.ToList();
                }
                else
                {
                    genres = _con.tbl_genres.FromSqlInterpolated($"select * from tbl_genres where name like '%'+{textsreach}+ '%'").ToList();
                }
                if (genres.Count == 0)
                {
                    TempData["count"] = $"not record found of {textsreach}";
                }
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    genres_details = genres,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        [HttpPost]
        public IActionResult add_genres(genre genres)
        {
            var genres_existing = _con.tbl_genres.FirstOrDefault(e => e.name == genres.name);
            if (genres_existing == null)
            {
                Random random = new Random();
                var num = random.Next(0000, 9999).ToString();
                genres.genres_id = "Ebook -" + "" + num;
                _con.tbl_genres.Add(genres);
                _con.SaveChanges();
                TempData["msg2"] = "The genre is added successfully";
                return RedirectToAction("genres_details");
            }
            else
            {
                TempData["msg"] = "The genres is already added " + "" + genres.name;
                return RedirectToAction("genres_details");
            }
        }
        public IActionResult delete_genre(int id)
        {
            var del = _con.tbl_genres.Find(id);
            _con.tbl_genres.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("genres_details");
        }

        public IActionResult update_genre(int id)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                var data = _con.tbl_genres.Find(id);
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    genres_data = data,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        [HttpPost]
        public IActionResult update_genre(genre genres)
        {
            _con.tbl_genres.Update(genres);
            _con.SaveChanges();
            return RedirectToAction("genres_details");
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("login_form", "home");
        }
        [HttpGet]
        public IActionResult books_details(string textsreach)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                List<Book> Books = new List<Book>();

                if (textsreach == null)
                {
                    Books = _con.tbl_books.ToList();
                }
                else
                {
                    Books = _con.tbl_books.FromSqlInterpolated($"select * from tbl_books where name like '%'+{textsreach}+ '%'").ToList();
                }
                if (Books.Count == 0)
                {
                    TempData["count"] = $"not record found of {textsreach}";
                }
                var auth = _con.tbl_authors.ToList();
                var genres = _con.tbl_genres.ToList();
     
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    book_details = Books,
                    auth_details = auth,
                    genres_details = genres,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        [HttpPost]
        public IActionResult add_books(IFormFile book_cover, Book book)
        {
            string file_extension = Path.GetExtension(book_cover.FileName);
            Random ran = new Random();
            var name_exsiting = _con.tbl_books.FirstOrDefault(b => b.name == book.name);
            if (name_exsiting == null)
            {
                if (file_extension == ".jpg" || file_extension == ".png" || file_extension == ".jpeg")
                {
                    string file_path = Path.GetFileName(book_cover.FileName);
                    var R_num = ran.Next(0000, 9999).ToString();
                    var name = file_path = R_num;
                    string file_name = name + file_extension;
                    string directory_path = Path.Combine(_evn.WebRootPath, "books_covers", file_name);
                    FileStream fs = new FileStream(directory_path, FileMode.Create);
                    book_cover.CopyTo(fs);
                    book.book_cover = file_name;
                    book.total = book.price * book.qunatiy;
                    _con.tbl_books.Add(book);
                    _con.SaveChanges();
                    TempData["msg2"] = "The Book is added successfully";
                    return RedirectToAction("books_details");
                }
                else
                {
                    TempData["msg"] = "Oops this file is not supported ";
                    return RedirectToAction("books_details");
                }
            }
            else
            {
                TempData["msg"] = "The name of book is already registered!  with the name of" + "" + book.name;
                return RedirectToAction("books_details");
            }


        }
        public IActionResult books_data(int id)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {

                var book = _con.tbl_books.Find(id);
                var auth = _con.tbl_authors.Where(e => e.id == book.auth_id).ToList();
                var genres = _con.tbl_genres.Where(p => p.id == book.genres_id).ToList();
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    auth_details = auth,
                    genres_details = genres,
                    book_data = book,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        public IActionResult books_delete(int id)
        {
            var del = _con.tbl_books.FirstOrDefault(e => e.id == id);

            string direcorty_path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/books_covers", del.book_cover);
            if (System.IO.File.Exists(direcorty_path))
            {
                System.IO.File.Delete(direcorty_path);
            }
            _con.tbl_books.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("books_details");
        }

        public IActionResult books_update(int id)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                var auth = _con.tbl_authors.ToList();
                var genres = _con.tbl_genres.ToList();
                var book = _con.tbl_books.Find(id);
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    auth_details = auth,
                    genres_details = genres,
                    book_data = book,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }

        }
        [HttpPost]
        public IActionResult books_update(Book book)
        {
            book.total = book.price * book.qunatiy;
            _con.tbl_books.Update(book);
            _con.SaveChanges();
            return RedirectToAction("books_details");
        }
        [HttpPost]
        public IActionResult update_picture(IFormFile book_cover, Book book)
        {
            string file_extension = Path.GetExtension(book_cover.FileName);
            Random ran = new Random();
           
                if (file_extension == ".jpg" || file_extension == ".png" || file_extension == ".jpeg")
                {
                    string file_path = Path.GetFileName(book_cover.FileName);
                    var R_num = ran.Next(0000, 9999).ToString();
                    var name = file_path = R_num;
                    string file_name = name + file_extension;
                    string directory_path = Path.Combine(_evn.WebRootPath, "books_covers", file_name);
                    FileStream fs = new FileStream(directory_path, FileMode.Create);
                    book_cover.CopyTo(fs);
                    book.book_cover = file_name;

                    _con.tbl_books.Update(book);
                    _con.SaveChanges();
                    TempData["msg2"] = "The Book is updated sucessfully";
                    return RedirectToAction("books_details");
                }
                else
                {
                    TempData["msg"] = "Oops this file is not supported ";
                    return RedirectToAction("books_details");
                }
               
        }
        [HttpGet]
        public IActionResult orders(string textsreach)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                List<order> orders = new List<order>();

                if (textsreach == null)
                {
                    orders = _con.tbl_order.ToList();
                }
                else
                {
                    orders = _con.tbl_order.FromSqlInterpolated($"select * from tbl_order where user_name like '%'+{textsreach}+ '%'").ToList();
                }
                if (orders.Count == 0)
                {
                    TempData["count"] = $"not record found of {textsreach}";
                }

                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();

                
                mainmodel mydata = new mainmodel()
                {orders_Details=orders,
                    
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
     
        public IActionResult order_data(int id)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {

                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
            


                var payment_order = _con.tbl_order.Where(p=>p.order_id==id).ToList();
                mainmodel mydata = new mainmodel()
                {
                    orders_Details = payment_order,

                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        public IActionResult staus_order(int id)
        {
            var find_id = _con.tbl_order.Find(id);
            if (find_id.status == 1)
            {
                find_id.status = 0;
            }
            _con.SaveChanges();
            return RedirectToAction("orders");
        
        
        }
        public IActionResult order_delete(int id)
        {
            var find_id = _con.tbl_order.Find(id);
            _con.tbl_order.Remove(find_id);
            _con.SaveChanges();
            return RedirectToAction("orders");


        }
        [HttpGet]
        public IActionResult contact_details(string textsreach)
        {
            var login = HttpContext.Session.GetString("admin_session");
            if (login != null)
            {
                List<Contact> contact = new List<Contact>();

                if (textsreach == null)
                {
                    contact = _con.tbl_contact.ToList();
                }
                else
                {
                    contact = _con.tbl_contact.FromSqlInterpolated($"select * from tbl_contact where name like '%'+{textsreach}+ '%'").ToList();
                }
                if (contact.Count == 0)
                {
                    TempData["count"] = $"not record found of {textsreach}";
                }
                var auth = _con.tbl_authors.ToList();
                var genres = _con.tbl_genres.ToList();

                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                contact_detail=contact,
                    admin_data = admin_details
                };
                TempData["mgs"] = "admin";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        public IActionResult contact_delete(int id)
        {
            var del= _con.tbl_contact.Find(id);
            _con.tbl_contact.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("contact_details");
        }
    }


}

    
