using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WebAPI.Tabels;

namespace WebAPI.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<GroupTable> Groups { get; set; }
        public DbSet<GroupRoleTable> GroupRoles { get; set; }
        public DbSet<RoleTable> Roles { get; set; }
        public DbSet<PermissionTable> Permissions { get; set; }

    }
}
