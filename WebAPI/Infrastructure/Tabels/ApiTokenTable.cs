using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Infrastructure.Tabels
{
    public class ApiTokenTable
    {
        public Guid Id { get; set; }
        public Guid UserGuid { get; set; }
        public Guid GroupId { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpiryTime { get; set; }

        //Config
        public class DbConfiguration : IEntityTypeConfiguration<ApiTokenTable>
        {
            public void Configure(EntityTypeBuilder<ApiTokenTable> builder)
            {
                builder.HasKey(v => v.Id);
                builder.Property(v => v.Id).ValueGeneratedNever();
                builder.Property(v => v.UserGuid).ValueGeneratedNever();
                builder.HasIndex(v => v.UserGuid).HasDatabaseName("Member");
                builder.Property(v => v.GroupId).ValueGeneratedNever();
            }
        }
    }
}
