using Microsoft.AspNetCore.Mvc;
using MyAspNetCoreApp.Web.Filters;

namespace MyAspNetCoreApp.Web.Controllers
{

    public class Product2
    {
        public int Id { get; set; }
        public string Name { get; set; }



    }
    
    [CustomResultFilter("x-version","1.0")]
    [Route("/[controller]/[action]")]
    public class OrnekController : Controller
    {
        public IActionResult Index()
        {

            ViewData["age"] = 30;

            ViewBag.name = "Asp.net core";
            ViewData["names"] = new List<string>() { "ahmet", "mehmet", "hasan" };

            ViewBag.person = new { Id = 1, Name = "ahmet", Age = 100 };



            TempData["surname"] = "Muratoğlu";//farklı actionresult'a veri göndermeye yarar (önce index çalışması lazım) 



            var productList = new List<Product2>()
            {
                new(){ Id=1, Name="kalem" },
                new(){ Id=2, Name="Defter" },
                new(){ Id=3, Name="Silgi" },

            };

            return View(productList);
        }

        public IActionResult Index3() 
        {
            return View();
        
        }
        public IActionResult Index2()
        {

            return RedirectToAction("Index", "Ornek");// aynı controller içerisinde controller ismini yazmaya gerek yok
        }
        public IActionResult ParametreView(int id)
        {
            return RedirectToAction("JsonResultParametre", "Ornek", new { id = id });
        }
        public IActionResult JsonResultParametre(int id)
        {
            return Json(new { Id = id});

        }
        public IActionResult ContentResult()
        {
            return Content("Content Result");
        }

        public IActionResult JsonResult()
        {
            return Json(new { id = 1, name = "kalem", price = 100 });

        }
        public IActionResult EmptyResult()
        {
            return new EmptyResult();

        }
    }
}