using Microsoft.EntityFrameworkCore;
using Shopping_tutorial.Models;

namespace Shopping_tutorial.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                CategoryModel laptop = new CategoryModel { Name = "Laptop", Slug = "laptop", Description = "Máy tính xách tay, laptop chất lượng cao của nhiều thương hiệu lớn Dell, HP, Asus, Acer, trả góp lãi suất thấp, bảo hành lâu dài, giao hàng toàn quốc, nhiều quà tặng.", Status = 1 };
                /*CategoryModel phukiendidong = new CategoryModel { Name = "Phụ kiện di động", Slug = "Phụ kiện di động", Description = "Mua phụ kiện tai nghe, loa, chuột máy tính, ốp lưng, bàn phím, sạc dự phòng giá rẻ, chính hãng, giao hàng nhanh chóng.", Status = 1 };*/
                BrandModel apple = new BrandModel { Name = "Apple", Slug = "apple", Description = "Apple is the largest brand in the world", Status = 1 };
                BrandModel dell = new BrandModel { Name = "Dell", Slug = "dell", Description = "Explore Dell's latest computers & technology solutions. Laptops, Desktops, Gaming PCs, Monitors, Workstations, Storage & Servers.", Status = 1 };
                _context.Products.AddRange(
                    new ProductModel { Name = "Macbook Air M1", Slug = "Macbook Air M1", Description = "Macbook Air M1", Brand = apple, Category = laptop, Image = "macbook_air_m1.jpg", Price = 1110 },
                    new ProductModel { Name = "Laptop Dell XPS 14", Slug = "Laptop Dell XPS 14", Description = "Laptop Dell XPS 14 9440 U7 155H/AI/64GB/1TB/14.5\"3.2K Touch/Nvidia RTX4050 6GB/Win11/Office HS22", Brand = dell, Category = laptop, Image = "macbook_air_m1.jpg", Price = 1110 }
                );
                _context.SaveChanges();
            }
        }
    }
}
