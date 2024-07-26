using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaOnline.Data.DatabaseContext.Configurations
{
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder.ToTable("Cinemis")
                .Property(c => c.Logo)
                .IsRequired();
            builder.Property(c => c.Name)
                .IsRequired();
            builder.Property(c => c.Description)
                .IsRequired();
            builder.HasMany(c => c.Movies)
                .WithMany(m => m.Cinemas);

        }
    }

}
