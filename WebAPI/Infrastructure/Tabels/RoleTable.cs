using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.Infrastructure.Tabels
{

    [Table("Role")]
    public class RoleTable
    {
        public Guid Id { get; set; }
        public string Role { get; set; }

        public virtual ICollection<UserRoleTable> UserRoles { get; set; }

        public virtual ICollection<PermissionTable> Permissions { get; set; }

        public class DbConfiguration : IEntityTypeConfiguration<RoleTable>
        {
            public void Configure(EntityTypeBuilder<RoleTable> builder)
            {
                builder.HasKey(v => new { v.Id });
                builder.Property(v => v.Id).HasDefaultValueSql("NEWID()");
                builder.HasMany(v => v.Permissions).WithOne(v => v.Role).HasForeignKey(v => new { v.RoleId }).HasPrincipalKey(v => new { v.Id });
                builder.HasMany(v => v.UserRoles).WithOne(v => v.Role).HasForeignKey(v => new { v.RoleId }).HasPrincipalKey(v => new { v.Id });
                builder.HasIndex(v => new { v.Role }).HasDatabaseName("By Role");
            }
        }
    }

    [Table("UserRole")]
    //[Keyless]
    public class UserRoleTable
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public virtual RoleTable Role { get; set; }
        public virtual UserTable User { get; set; }

        public class DbConfiguration : IEntityTypeConfiguration<UserRoleTable>
        {
            public void Configure(EntityTypeBuilder<UserRoleTable> builder)
            {
                builder.HasKey(v => new { v.Id });
                //builder.ToTable("UserRole");
            }
        }
    }


    [Table("Permission")]
    public class PermissionTable
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string Module { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool View { get; set; }
        public bool Delete { get; set; }

        //Relation Ship
        public virtual RoleTable Role { get; set; }

        public class DbConfiguration : IEntityTypeConfiguration<PermissionTable>
        {
            public void Configure(EntityTypeBuilder<PermissionTable> builder)
            {
                builder.HasKey(v => new { v.Id });
                builder.Property(v => v.Id).HasDefaultValueSql("NEWID()");
                builder.Property(v => v.RoleId).ValueGeneratedNever();

                // builder.HasMany(v => v.Role).WithOne(v => v.Permission).HasForeignKey(v => new { v.Id }).HasPrincipalKey(v => new { v.RoleId });
                builder.HasIndex(v => new { v.RoleId }).HasDatabaseName("By Role");

            }
        }
    }
}
