using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Infrastructure.Tabels
{
    [Table("Group")]
    public class GroupTable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        //Relationships
        public virtual ICollection<GroupRoleTable> Roles { get; set; }

        //Config
        public class DbConfiguration : IEntityTypeConfiguration<GroupTable>
        {
            public void Configure(EntityTypeBuilder<GroupTable> builder)
            {
                builder.HasKey(v => new { v.Id });


                builder.Property(v => v.Id).ValueGeneratedNever();

                builder.HasMany(v => v.Roles).WithOne(v => v.Group).HasForeignKey(v => new { v.GroupId }).HasPrincipalKey(v => new { v.Id });

                builder.HasIndex(v => new { v.Name }).HasDatabaseName("By GroupName");

            }
        }
    }

    [Table("GroupRole")]
    public class GroupRoleTable
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public Guid RoleId { get; set; }
        public bool Inherited { get; set; }

        //Relationships
        public virtual GroupTable Group { get; set; }
        // public virtual RoleTable Role { get; set; }

        //Config
        public class DbConfiguration : IEntityTypeConfiguration<GroupRoleTable>
        {
            public void Configure(EntityTypeBuilder<GroupRoleTable> builder)
            {
                builder.HasKey(v => new { v.Id });
                builder.Property(v => v.Id).HasDefaultValueSql("NEWID()");

                builder.Property(v => v.GroupId).ValueGeneratedNever();
                builder.Property(v => v.RoleId).ValueGeneratedNever();
                builder.HasIndex(v => new { v.GroupId, v.RoleId }).HasDatabaseName("By GroupRole");

            }
        }
    }

    [Table("Role")]
    public class RoleTable
    {
        public Guid Id { get; set; }
        public string Role { get; set; }

        // Relation Ship
        //   public virtual ICollection<GroupRoleTable> GroupRole { get; set; }
        public virtual ICollection<PermissionTable> Permissions { get; set; }

        public class DbConfiguration : IEntityTypeConfiguration<RoleTable>
        {
            public void Configure(EntityTypeBuilder<RoleTable> builder)
            {
                builder.HasKey(v => new { v.Id });
                builder.Property(v => v.Id).HasDefaultValueSql("NEWID()");
                //  builder.HasMany(v => v.GroupRole).WithOne(v => v.Role).HasForeignKey(v => new { v.RoleId }).HasPrincipalKey(v => new { v.Id });
                builder.HasMany(v => v.Permissions).WithOne(v => v.Role).HasForeignKey(v => new { v.RoleId }).HasPrincipalKey(v => new { v.Id });
                builder.HasIndex(v => new { v.Role }).HasDatabaseName("By Role");
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
