using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { 
                Id= "1bf15d66-1b01-4735-bc39-30954a8fb0e5",
                Name ="Manager",
                NormalizedName="MANAGER"
            },
            new IdentityRole
            {
                Id= "45726e29-68d3-4d73-a7a7-8c81036491d4",
                Name ="Administrator",
                NormalizedName="ADMINISTRATOR"
            });
        }
    }
}
