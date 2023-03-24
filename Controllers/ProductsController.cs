using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAspNetCoreApp.Web.Filters;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModel;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MyAspNetCoreApp.Web.Controllers
{
    [Route("/[controller]/[action]")]
    [Authorize]
    public class ProductsController : Controller
    {

        private AppDbContext _context;

        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;
        private readonly IFileProvider _fileProvider;


        public ProductsController(AppDbContext context, IMapper mapper, IFileProvider fileProvider)
        {

            _productRepository = new ProductRepository();

            _context = context;// nesne örneği ürettik new anahtar sözcüğü kullanmadan buna DI Container denir , Dependency Injection Pattern
            _mapper = mapper;
            _fileProvider = fileProvider;
            //if (!_context.Products.Any())
            //{
            //    _context.Products.Add(new Product() { Name = "Kalem 1", Price = 100, Stock = 100, Color = "red", });
            //    _context.Products.Add(new Product() { Name = "Kalem 2", Price = 200, Stock = 200, Color = "red", });
            //    _context.Products.Add(new Product() { Name = "Kalem 3", Price = 300, Stock = 300, Color = "red", });
            //}
            //_context.SaveChanges();
        }
        //[CashResourceFilter]
        public IActionResult Index() /*DI containerdan gideceğini haber vermek için [FromServices] kullanılır*/
        {
            List<ProductViewModel> products = _context.Products.Include(x => x.Category).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
                CategoryName = x.Category.Name,
                Color = x.Color,
                Description = x.Description,
                Expire = x.Expire,
                ImagePath = x.ImagePath,
                IsPublish = x.IsPublish,
                PublishDate = x.PublishDate

            }).ToList();


            return View(products);
        }
        [ServiceFilter(typeof(NotFoundFilter))]//consturctur'da bir nesne parametresi aldığından dolayı bu şekilde tanımlandı
        [HttpGet("{id}")]
        public IActionResult Remove(int id)
        {

            var product = _context.Products.Find(id);
            _context.Products.Remove(product);//null olabildiği için altı çizili oluyor
            _context.SaveChanges();
            //_productRepository.Remove(id);
            return RedirectToAction("Index");


        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1.Ay",1 },
                {"3.Ay",3 },
                {"6.Ay",6 },
                {"12.Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>
            {
                new(){ Data="mavi",Value="Mavi" },
                new(){ Data="Kırmızı",Value="Kırmızı" },
                new(){ Data="Sarı",Value="Sarı" },
            }, "Value", "Data");
            var categories = _context.Category.ToList();

            ViewBag.categorySelect = new SelectList(categories, "Id", "Name");
            return View();

        }

        //[HttpPost]
        //public IActionResult SaveProduct()
        //{
        //    var name = HttpContext.Request.Form["Name"].ToString();
        //    var price = decimal.Parse(HttpContext.Request.Form["Price"]);
        //    var stock = int.Parse(HttpContext.Request.Form["Stock"]);
        //    var color = HttpContext.Request.Form["Color"].ToString();
        //    Product newProduct = new Product() { Name = name, Price = price, Stock = stock, Color = color };
        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");

        //}
        //[HttpPost]
        //public IActionResult Add(string Name, decimal Price, int Stock, string Color)
        //{

        //    Product newProduct = new Product() { Name = Name, Price = Price, Stock = Stock, Color = Color };
        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();
        //    return RedirectToAction("Index");

        //}


        [HttpPost]
        public IActionResult Add(ProductViewModel newProduct)//en uygun yöntem budur*******
        {
            //if (!string.IsNullOrEmpty(newProduct.Name) && newProduct.Name.StartsWith("A"))//name alanı A harfi ile başlamıyorsa
            //{
            //    ModelState.AddModelError(string.Empty, "ürün ismi A harfi ile başlayamaz");

            //}

            IActionResult result = null;




            if (ModelState.IsValid)
            {
                try
                {
                    var product = _mapper.Map<Product>(newProduct);
                    if (newProduct.Image != null && newProduct.Image.Length > 0)
                    {
                        var root = _fileProvider.GetDirectoryContents("wwwroot").ToList() ;

                        var images = root.First(x => x.Name == "images");


                        var randomImageName = Guid.NewGuid() + Path.GetExtension(newProduct.Image.FileName);


                        var path = Path.Combine(images.PhysicalPath, randomImageName);


                        using var stream = new FileStream(path, FileMode.Create);


                        newProduct.Image.CopyTo(stream);

                        product.ImagePath = randomImageName;
                    }


                    

                    _context.Products.Add(product);
                    
                    _context.SaveChanges();

                    TempData["status"] = "Ürün başarıyla eklendi.";
                    return RedirectToAction("Index");

                }
                catch (Exception)
                {

                    result = View();
                }
            }
            else
            {
                result = View();
            }

            var categories = _context.Category.ToList();

            ViewBag.categorySelect = new SelectList(categories, "Id", "Name");


            ViewBag.Expire = new Dictionary<string, int>()
            {
                { "1 Ay",1 },
                 { "3 Ay",3 },
                 { "6 Ay",6 },
                 { "12 Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {

                new(){ Data="Mavi" ,Value="Mavi" },
                  new(){ Data="Kırmızı" ,Value="Kırmızı" },
                    new(){ Data="Sarı" ,Value="Sarı" }


            }, "Value", "Data");

            return result;
        }


        //[HttpGet]
        //public IActionResult SaveProduct2(Product newProduct)//güvenli bir yöntem değil
        //{

        //    _context.Products.Add(newProduct);
        //    _context.SaveChanges();
        //    return View();

        //}
        [ServiceFilter(typeof(NotFoundFilter))]//consturctur'da  bir nesne parametresi aldığından dolayı bu şekilde tanımlandı
        [HttpGet]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Find(id);
            var categories = _context.Category.ToList();

            ViewBag.categorySelect = new SelectList(categories, "Id", "Name", product.CategoryId);
            ViewBag.radioExpireValue = product.Expire;
            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1.Ay",1 },
                {"3.Ay",3 },
                {"6.Ay",6 },
                {"12.Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>
            {
                new(){ Data="mavi",Value="Mavi" },
                new(){ Data="Kırmızı",Value="Kırmızı" },
                new(){ Data="Sarı",Value="Sarı" },
            }, "Value", "Data", product.Color);

            return View(_mapper.Map<ProductUpdateViewModel>(product));
        }
        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel updateProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.radioExpireValue = updateProduct.Expire;
                ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1.Ay",1 },
                {"3.Ay",3 },
                {"6.Ay",6 },
                {"12.Ay",12 }
            };
                ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>
            {
                new(){ Data="mavi",Value="Mavi" },
                new(){ Data="Kırmızı",Value="Kırmızı" },
                new(){ Data="Sarı",Value="Sarı" },
            }, "Value", "Data");
                var categories = _context.Category.ToList();
                ViewBag.categorySelect = new SelectList(categories, "Id", "Name", updateProduct.CategoryId);
            }
            if (updateProduct.Image != null && updateProduct.Image.Length > 0)
            {

                var root = _fileProvider.GetDirectoryContents("wwwroot");
                var images = root.First(x => x.Name == "images");
                var randomImageName = Guid.NewGuid() + Path.GetExtension(updateProduct.Image.FileName);
                var path = Path.Combine(images.PhysicalPath, randomImageName);
                using var stream = new FileStream(path, FileMode.Create);
                updateProduct.Image.CopyTo(stream);
                updateProduct.ImagePath = randomImageName;
            }
            var products = _mapper.Map<Product>(updateProduct);
            _context.Products.Update(products);
            _context.SaveChanges();
            TempData["status"] = "ürün başarıyla güncellendi";
            return RedirectToAction("Index");
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult HasProductName(string Name)
        {
            var anyProduct = _context.Products.Any(x => x.Name.ToLower() == Name.ToLower());
            if (anyProduct)
            {
                return Json("Ürün ismi veritabanında bulunmaktadır");
            }
            else
            {
                return Json(true);

            }

        }
        //[HttpGet("{page}/{pagesize}")]
        [Route("[action]/{page}/{pagesize}", Name = "productpage")]
        public IActionResult Pages(int page, int pagesize)
        {

            // page=1 pagesize=3 ilk 3 kayıt
            // page=2 pagesize=3 ikinci 3 kayıt
            //^page=3 pagesize=3 ücüncü 3 kayıts
            var products = _context.Products.Skip((page - 1) * pagesize).Take(pagesize).ToList();//skip fonksiyon atlama işlemi yapar take: kaç data alacağını yazar
            ViewBag.page = page;
            ViewBag.pagesize = pagesize;
            return View(_mapper.Map<List<ProductViewModel>>(products));
        }
        //[Route("[action]/{productid}")]
        //[Route("/urun/{productid}")]
        [Route("urunler/urun/{productid}", Name = "product")]


        [ServiceFilter(typeof(NotFoundFilter))]//consturctur'da  bir nesne parametresi aldığından dolayı bu şekilde tanımlandı
        public IActionResult GetById(int productid)
        {
            var product = _context.Products.Find(productid);
            return View(_mapper.Map<ProductViewModel>(product));


        }
    }
}