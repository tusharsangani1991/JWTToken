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

        public DbSet<UserTable> Users { get; set; }
        public DbSet<UserRoleTable> UserRoles { get; set; }
        public DbSet<RoleTable> Roles { get; set; }
        public DbSet<PermissionTable> Permissions { get; set; }
        public DbSet<ApiTokenTable> ApiTokens { get; set; }
        public DbSet<ProductTable> Products { get; set; }

       
    }
}
