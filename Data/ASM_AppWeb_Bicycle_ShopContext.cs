using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ASM_Bicycle_Shops.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ASM_AppWeb_Bicycle_Shop.Models;

namespace ASM_AppWeb_Bicycle_Shop.Data
{
    public class ASM_AppWeb_Bicycle_ShopContext : IdentityDbContext<IdentityUser>
    {
        public ASM_AppWeb_Bicycle_ShopContext (DbContextOptions<ASM_AppWeb_Bicycle_ShopContext> options)
            : base(options)
        {
        }

        public DbSet<ASM_Bicycle_Shops.Models.Product> Product { get; set; } = default!;

        public DbSet<ASM_Bicycle_Shops.Models.CategoryProduct> CategoryProduct { get; set; } = default!;

        public DbSet<ASM_AppWeb_Bicycle_Shop.Models.Shop> Shop { get; set; } = default!;
        public DbSet<ASM_AppWeb_Bicycle_Shop.Models.News> News { get; set; } = default!;
        public DbSet<ASM_AppWeb_Bicycle_Shop.Models.CalendarEvent> CalendarEvent { get; set; } = default!;
    }
}
