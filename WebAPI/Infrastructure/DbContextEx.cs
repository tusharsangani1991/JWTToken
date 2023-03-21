using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebAPI.Infrastructure.Tabels;

namespace WebAPI.Infrastructure
{
    public class DbContextEx : DbContext
    {
        protected DbContextEx() : base() { }


        public DbContextEx(DbContextOptions<DbContextEx> options) : base(options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //{
        //    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<GroupTable> Groups { get; set; }
        public DbSet<GroupRoleTable> GroupRoles { get; set; }
        public DbSet<RoleTable> Roles { get; set; }
        public DbSet<PermissionTable> Permissions { get; set; }
        public DbSet<ApiTokenTable> ApiTokens { get; set; }
        public DbSet<ProductTable> Products { get; set; }

    }
}
