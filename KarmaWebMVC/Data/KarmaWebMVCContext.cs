using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KarmaWebMVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace KarmaWebMVC.Data
{
    public class KarmaWebMVCContext : IdentityDbContext<AppUserModel>
    {
        public KarmaWebMVCContext (DbContextOptions<KarmaWebMVCContext> options)
            : base(options)
        {
        }

        //public DbSet<KarmaWebMVC.Models.User> User { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.Brand> Brand { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.Category> Category { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.Size> Size { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.Product> Product { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.Order> Order { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.OrderStatus> OrderStatus { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.OrderItems> OrderItem { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.ProductStatus> ProductStatus { get; set; } = default!;
        public DbSet<KarmaWebMVC.Models.Cart> Cart { get; set; } = default!;


    }
}
