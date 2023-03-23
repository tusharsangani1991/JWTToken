using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Infrastructure.Tabels
{
    public class UserTable
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }        
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<UserRoleTable> UserRoles { get; set; }

        public class DbConfiguration : IEntityTypeConfiguration<UserTable>
        {
            public void Configure(EntityTypeBuilder<UserTable> builder)
            {
                builder.HasKey(v => new { v.Id });
                builder.Property(v => v.Email).HasMaxLength(150).IsRequired();
                builder.Property(v => v.Password).HasMaxLength(500).IsRequired();
                builder.Property(v => v.PhoneNumber).HasMaxLength(15).IsRequired();
                builder.HasMany(v => v.UserRoles).WithOne(v => v.User).HasForeignKey(v => new { v.UserId }).HasPrincipalKey(v => new { v.Id });

            }
        }
    }
}
