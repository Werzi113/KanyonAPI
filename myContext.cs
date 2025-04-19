using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MyContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryParameter> CategoryParameters { get; set; }
        public DbSet<CategoryParameterValue> CategoryParametersValues { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductPicture> ProductPictures { get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserRight> UserRights { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses {  get; set; }
        public DbSet<WishlistedProduct> WishlistedProducts { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=mysqlstudenti.litv.sssvt.cz;database=sibrava_db1;user=sibravaread;password=123456;SslMode=none");
        }

    }

}
