namespace MyAspNetCoreApp.Web.Models
{
    public class ProductRepository
    {
        private static List<Product> _products = new List<Product>()
        {

              new() { Id = 1, Name = "kalem 1", Price = 100, Stock = 200 },
              new () { Id = 2, Name = "kalem 2", Price = 200, Stock = 300 },
              new () { Id = 3, Name = "kalem 3", Price = 300, Stock = 400 }

        };


        public List<Product> GetAll() => _products;/* return _products fonksiyonuna karşılık gelir*/

        public void Add(Product newProduct) => _products.Add(newProduct);


        public void Remove(int id)
        {
            var hasProduct = _products.FirstOrDefault(x => x.Id == id);
            if (hasProduct == null)
            {
                throw new Exception($"bu id({id})ye sahip ürün bulunmamaktadır");
            }

            _products.Remove(hasProduct);

        }


        public void Uptade(Product updateProduct)
        {
            var hasProduct = _products.FirstOrDefault(x => x.Id == updateProduct.Id);
            if (hasProduct == null)
            {
                throw new Exception($"bu id({updateProduct.Id})ye sahip ürün bulunmamaktadır");
            }
            _products.Remove(hasProduct);
            hasProduct.Name = updateProduct.Name;
            hasProduct.Price = updateProduct.Price;
            hasProduct.Stock = updateProduct.Stock;

            var index = _products.FindIndex(x => x.Id == updateProduct.Id);
            _products[index] = hasProduct;


        }

    }
}
