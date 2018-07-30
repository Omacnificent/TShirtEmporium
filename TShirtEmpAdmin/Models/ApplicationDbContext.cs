using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TShirtEmpAdmin.Models
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<LogUserInfo> LogUserInfos { get; set; }
        public DbSet<ShirtSize> ShirtSizes { get; set; }
        public DbSet<ShirtQuantity> ShirtQuantities { get; set; }
        public DbSet<Shirt> Shirts { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<DeliveryOption> DeliveryOptions { get; set; }
        public DbSet<Markup> Markups { get; set; }
        public DbSet<ShirtToSize> ShirtToSizes { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}