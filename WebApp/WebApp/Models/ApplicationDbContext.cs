using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserTokenCache> UserTokenCacheList { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Beach> Beaches { get; set; }

        public DbSet<DeviceUser> DeviceUsers { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<DeviceLog> DeviceLogs { get; set; }

        public DbSet<BeachAccess> BeachAccesses { get; set; }
    }

    public class UserTokenCache
    {
        [Key]
        public int UserTokenCacheId { get; set; }
        public string webUserUniqueId { get; set; }
        public byte[] cacheBits { get; set; }
        public DateTime LastWrite { get; set; }
    }
}
