using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Tabels
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid GroupId { get; set; }
        public DateTime CreatedOn { get; set; }

        public class DbConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.HasKey(v => new { v.Id });
                builder.Property(v => v.Email).HasMaxLength(150).IsRequired();
                builder.Property(v => v.Password).HasMaxLength(500).IsRequired();
                builder.Property(v => v.PhoneNumber).HasMaxLength(15).IsRequired();

            }
        }
    }
}
