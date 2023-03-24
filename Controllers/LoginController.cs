using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Models;
using System.Security.Claims;

namespace MyAspNetCoreApp.Web.Controllers
{
    [Route("/[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Index(User users)
        {
            //var uservalue = _context.Users.FirstOrDefault(x => x.UserName == users.UserName && x.UserPassword == users.UserPassword);

            //if (uservalue != null)
            //{
            //    HttpContext.Session.SetString("username", users.UserName);
            //    return RedirectToAction("Index", "Home");

            //}

            //else
            //{
            //    return View();

            //}
            var uservalue = _context.Users.FirstOrDefault(x => x.UserName == users.UserName && x.UserPassword == users.UserPassword);
            if (uservalue != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,users.UserName)

                };
                var useridentity= new ClaimsIdentity(claims,"a");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home");

            }

            else 
            {
                return View();
            }

        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        
        }        
        [HttpPost]
        public IActionResult SignUp(User users)
        {
            _context.Users.Add(users);
            _context.SaveChanges();
            TempData["status"] = "Kayıt oldunuz lütfen giriş yapınız";
            return RedirectToAction("Index");

        }
        public async Task <IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();
            TempData["status"] = "başarıyla çıkış yapıldı";

            return RedirectToAction("Index");

        }
    }
}
