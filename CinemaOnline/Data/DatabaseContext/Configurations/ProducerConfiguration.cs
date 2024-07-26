using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaOnline.Data.DatabaseContext.Configurations
{
    public class ProducerConfiguration : IEntityTypeConfiguration<Producer>
    {
        public void Configure(EntityTypeBuilder<Producer> builder)
        {
            builder.Property(p => p.ProfilePictureURL)
               .IsRequired();
            builder.Property(p => p.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Bio)
                .IsRequired();
            builder.HasMany(p => p.Movies)
                .WithOne(m => m.Producer);
        }
    }

}
