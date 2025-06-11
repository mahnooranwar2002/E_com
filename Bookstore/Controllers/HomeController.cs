
using Bookstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Numerics;

namespace Bookstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private mycontext _con;

        public HomeController(ILogger<HomeController> logger, mycontext con)
        {
            _con = con;
            _logger = logger;
        }
        

        public IActionResult Index()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login !=null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                   
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
        public IActionResult about()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {

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
        public IActionResult Privacy()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {

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
      
        public IActionResult blog()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {

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
        public IActionResult contact()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {

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
        [HttpPost]
        public IActionResult add_contact(Contact _cont)
        {
            _con.tbl_contact.Add(_cont);
            _con.SaveChanges();
            TempData["MSG"] = "Thankyou for massage us ! ";
            return RedirectToAction("contact", "Home");

        }
        public IActionResult login_form()
        {
            return View();
        }
        [HttpPost]
        public IActionResult login_form(string email, string password)
        {
            var data = _con.tbl_users.FirstOrDefault(e => e.Email == email);

            if (data != null && data.Password == password)
            {
                if (data.status == 1) // Active User
                {
                    if (data.is_admin == 1) // Admin User
                    {
                        HttpContext.Session.SetString("admin_session", data.Id.ToString());
                        return RedirectToAction("Index", "Admin");
                    }
                    else // Regular User
                    {
                        HttpContext.Session.SetString("user_session", data.Id.ToString());
                        return RedirectToAction("Index"); // Redirect to user dashboard or relevant page
                    }
                }
               
                else // Handle unexpected status values
                {
                    return RedirectToAction("WaitedPage"); // Optionally, redirect to an error page
                }
            }
            else
            {
                // Invalid login attempt (email not found or incorrect password)
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("login_form");
            }
        }
        
        public IActionResult register_form()
        {
            return View();
        }
        [HttpPost]

        public IActionResult register_form(User myuser)
        {
         
           var emailExisting=_con.tbl_users.FirstOrDefault(e=>e.Email==myuser.Email);
            if (emailExisting != null)
            {
                TempData["MSG"] = "This email is already registered!";
                return RedirectToAction("register_form");
            }
            else
            {

                if (ModelState.IsValid)
                {
                    myuser.status = 1;
                    myuser.is_admin = 0;
                    _con.tbl_users.Add(myuser);
                    _con.SaveChanges();
                   
                    return RedirectToAction("index");

                }
                else
                {

                    return RedirectToAction("register_form");
                }
                
            }
           
        }
        public IActionResult logout()
        {
            HttpContext.Session.Remove("user_session");
            return RedirectToAction("index");
        }
        public IActionResult profile(int id)
        {
            var login = HttpContext.Session.GetString("user_session");
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
        public IActionResult profile(User user)
        {
            _con.tbl_users.Update(user);
            _con.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult books_details(string search_text, string mydatass)
        {
            var login = HttpContext.Session.GetString("user_session");
            var authors = _con.tbl_authors.ToList();
            var category = _con.tbl_genres.ToList();
        
            List<Book> book_data = new List<Book>();
            if(search_text == null)
            {
                book_data = _con.tbl_books.ToList();
            }
            else
            {
               
                book_data = _con.tbl_books.FromSqlInterpolated($"select * from tbl_books where name like '%'+{search_text}+'%'").ToList();
            }
            if (book_data.Count == 0)
            {
                TempData["msg"] = $"we didn't have this book yet {search_text}";
            }

            if (login != null)
            {
               
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                    auth_details=authors,
                    genres_details=category,
                   book_details=book_data,
                    admin_data = admin_details
                };
                TempData["registered"] = "login";
                return View(mydata);
            }
            else
            {
                mainmodel mydats = new mainmodel()
                {
                    auth_details = authors,
                    genres_details = category,
                    book_details = book_data,

                };
                TempData["not_registered"] = "not_login";
                return View(mydats);
            
               
            }
           
        }
        public IActionResult book_data(int id)
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var purchase_data = _con.tbl_books.Find(id);
               var gnres = _con.tbl_genres.Where(e => e.id == purchase_data.genres_id).ToList();
                var auth = _con.tbl_authors.Where(e => e.id == purchase_data.auth_id).ToList();
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
                mainmodel mydata = new mainmodel()
                {
                   auth_details=auth,
                    genres_details=gnres,
                    book_data = purchase_data,
                    admin_data = admin_details
                };
                TempData["registered"] = "login";
                return View(mydata);


            }
            else
            {
                return RedirectToAction("login_form", "home");

            }
        }
        [HttpPost]
      public IActionResult add_wishlist(int p_id,wishlist wish)
        {
            var product = _con.tbl_books.Find(p_id);
            var login = HttpContext.Session.GetString("user_session");
            var p_o = _con.tbl_wishlist.FirstOrDefault(p => p.product_id == product.id);
           
           
            if (login != null && p_o ==null)
            {
                wish.product_name = product.name;
                wish.product_id=product.id;
                wish.user_id = int.Parse(login);
                _con.tbl_wishlist.Add(wish);
                _con.SaveChanges();
                TempData["msg2"] = "book has been added in  wishlist!";

                return RedirectToAction("books_details");
            }
            if(p_o != null)
            {
                TempData["msg1"] = "book already  added in  wishlist!";

                return RedirectToAction("books_details");
            }

            else
            {
                TempData["msg1"] = "please login first  to add the book in your wishlist!";
                return RedirectToAction("books_details");
            }
        }
        [HttpPost]
        public IActionResult add_cart(int pt_id,cart book_pro)
        {
            var login = HttpContext.Session.GetString("user_session");
            var findid = _con.tbl_books.Find(pt_id);

            var p = _con.tbl_books.FirstOrDefault(e=>e.id==book_pro.product_id);
            var cart_data = _con.tbl_cart.FirstOrDefault(p => p.product_id == findid.id);
            if (cart_data == null || login==null)
            {
            if (book_pro.qunatity <= findid.qunatiy)
            {
             book_pro.user_id=int.Parse(login);
                book_pro.product_id = findid.id;
                book_pro.price = findid.price;
                book_pro.total = book_pro.price * book_pro.qunatity;
                findid.qunatiy = findid.qunatiy - book_pro.qunatity;
                findid.total = findid.price * findid.qunatiy;
                _con.tbl_cart.Add(book_pro);

                _con.SaveChanges();
                TempData["msg1"] = "book has been added in your cart";
                return RedirectToAction("books_details");
            }
            else
            {
                TempData["msg1"] = "sorry we do not have much quantity of" +""+book_pro.qunatity;
                return RedirectToAction("books_details");
            }
            }
            else
            {
                TempData["msg1"] = "This Book is already add";
                return RedirectToAction("books_details");
            }
        }
        public IActionResult carts_views()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();

                var cart_count = _con.tbl_cart.Include(p => p.book_data).Where(i => i.user_id == int.Parse(login));
               
                if (cart_count == null)
                {
                    TempData["mydata"] = "your cart is empty";
                    mainmodel mydata = new mainmodel()
                    {

                        admin_data = admin_details
                    };
                    TempData["registered"] = "login";
                    return View(mydata);
                }
                else
                {
                    var cart_data=_con.tbl_cart.Include(p=>p.book_data).Where(p=>p.user_id==int.Parse(login)).ToList();
                    var count = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Count();
                    if (count == 0)
                    {
                        TempData["mydata"] = "your cart is empty";
                    }
                    else
                    {
                        TempData["cart_count"] = count;
                    }
                   
                    var income = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Sum(p => p.total);
                    TempData["income"] = income;
                    mainmodel mydata = new mainmodel()
                    {cart_details=cart_data,

                        admin_data = admin_details
                    };
                    TempData["registered"] = "login";
                    return View(mydata);
                }
            }
            else
            {
                return RedirectToAction("login_form");
            }
        }
       
        public IActionResult wishlist_view()
        {
            var login = HttpContext.Session.GetString("user_session");
            if (login != null)
            {
                var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();

                var cart_count = _con.tbl_wishlist.Include(p => p.book_data).Where(i => i.user_id == int.Parse(login));

                if (cart_count == null)
                {
                    TempData["mydata"] = "your wishlist is empty";
                    mainmodel mydata = new mainmodel()
                    {

                        admin_data = admin_details
                    };
                    TempData["registered"] = "login";
                    return View(mydata);
                }
                else
                {
                    var cart_data = _con.tbl_wishlist.Include(p => p.book_data).Where(p => p.user_id == int.Parse(login)).ToList();
                    var count = _con.tbl_wishlist.Where(p => p.user_id == int.Parse(login)).Count();
                    if (count == 0)
                    {
                        TempData["mydata"] = "your cart is empty";
                    }
                    else
                    {
                        TempData["cart_count"] = count;
                    }

                    var income = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Sum(p => p.total);
                    TempData["income"] = income;
                    mainmodel mydata = new mainmodel()
                    {
                        wishlist_detail=cart_data,

                        admin_data = admin_details
                    };
                    TempData["registered"] = "login";
                    return View(mydata);
                }
            }
            else
            {
                return RedirectToAction("login_form");
            }

        }
        public IActionResult remove_cartProduct(int id)
        {
            var del= _con.tbl_cart.Find(id);
            var quantity = _con.tbl_books.FirstOrDefault(i => i.id == del.product_id);
            quantity.qunatiy = quantity.qunatiy + del.qunatity;
            quantity.total = quantity.price * quantity.qunatiy;
            _con.tbl_cart.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("carts_views");
        }
        public IActionResult remove_productWishlist(int id)
        {
            var del = _con.tbl_wishlist.Find(id);
            _con.tbl_wishlist.Remove(del);
            _con.SaveChanges();
            return RedirectToAction("wishlist_view");
        }
        public IActionResult paymentGetway()
        {
            var login = HttpContext.Session.GetString("user_session");
            var count = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Count();
            var income = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Sum(p => p.total);
            TempData["income"] = income;
            TempData["cart_count"] = count; 
            var products = _con.tbl_cart.Include(p=>p.book_data).Where(e => e.user_id == int.Parse(login)).ToList();
            var user_data = _con.tbl_users.Find(int.Parse(login));
            mainmodel mydata = new mainmodel()
            {    cart_details=products,
                admin_profiles=user_data,
            };
           
            return View(mydata);
     
        }
        [HttpPost]
        public IActionResult paymentGetway(order cus_order)
        {
            var login = HttpContext.Session.GetString("user_session");
            var count = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Count();
            var products = _con.tbl_cart.Where(e => e.user_id == int.Parse(login));
            var income = _con.tbl_cart.Where(p => p.user_id == int.Parse(login)).Sum(p => p.total);
            var productss = _con.tbl_cart.Include(p => p.book_data).Where(e => e.user_id == int.Parse(login)).ToList();
            cus_order.product_name = string.Join(", ", productss.Select(p => p.book_data.name));
            cus_order.product_price = string.Join(",", productss.Select(p => p.price));
            cus_order.product_quantity = string.Join(",", productss.Select(p => p.qunatity));
            cus_order.status = 1;
            Random random = new Random();
           var days= random.Next(00, 11).ToString();
            cus_order.day_counts = days;
            cus_order.sum = income.ToString();
            cus_order.product_count = count.ToString();
            _con.tbl_order.Add(cus_order);
            _con.SaveChanges();
            return RedirectToAction("paymentGetway");

        }
        public IActionResult order_views(int id)
        {
            var login = HttpContext.Session.GetString("user_session");
            var order_data = _con.tbl_order.Where(o => o.user_ids == int.Parse(login)).ToList();
            if (order_data.Count == 0)
            {
                TempData["product"] = "you didn't ordered something yet";
            }
            var admin_details = _con.tbl_users.Where(e => e.Id == int.Parse(login)).ToList();
            mainmodel mydata = new mainmodel()
            { 
                orders_Details=order_data,

                admin_data = admin_details
            };
            TempData["registered"] = "login";
            return View(mydata);

        }
    }
}
    

