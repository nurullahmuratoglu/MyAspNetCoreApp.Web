using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MyAspNetCoreApp.Web.ViewModel
{
    public class ProductUpdateViewModel
    {

        public int Id { get; set; }
        public int CategoryId { get; set; }


        [StringLength(50, ErrorMessage = "En fazla 50 karekter girilebilir")]
        [Required(ErrorMessage = "İsim Alanı Boş Olamaz.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Fiyat Alanı Boş Olamaz.")]
        [Range(1, 1000, ErrorMessage = "1 ile 1000 arası bir değer giriniz")]

        public decimal? Price { get; set; }//default değeri olduğundan dolayı hepsini nullable yapmamız lazım
        [Required(ErrorMessage = "Stock Alanı Boş Olamaz.")]
        [Range(1, 200, ErrorMessage = "1 ile 200 arası bir değer giriniz")]
        public int? Stock { get; set; }
        [StringLength(300, MinimumLength = 50, ErrorMessage = "Açıklama alanı 50 ile 300 karekter arasında olabilir")]

        [Required(ErrorMessage = "Açıklama Alanı Boş Olamaz.")]

        public string Description { get; set; }
        [Required(ErrorMessage = "Renk Alanı Boş Olamaz.")]
        public string? Color { get; set; }
        [Required(ErrorMessage = "Tarih Alanı Boş Olamaz.")]
        public DateTime? PublishDate { get; set; }
        public bool IsPublish { get; set; }
        [Required(ErrorMessage = "Süre Alanı Boş Olamaz.")]
        public int? Expire { get; set; }

        [ValidateNever]
        public IFormFile? Image { get; set; }
        [ValidateNever]
        public string? ImagePath { get; set; }
    }
}
