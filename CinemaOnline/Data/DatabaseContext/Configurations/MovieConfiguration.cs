using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaOnline.Data.DatabaseContext.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.ToTable("Movies")
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(m => m.Description)
                .IsRequired();
            builder.Property(m => m.Price)
                .IsRequired();
            builder.Property(m => m.ImageURL)
                .HasDefaultValue("image not found");
            builder.OwnsOne(m => m.ScreeningPeriod, sp =>
            {
                sp.Property(sp => sp.StartDate)
                .IsRequired();
                sp.Property(sp => sp.EndDate)
                .IsRequired();
                sp.Property(sp => sp.ScreenTimeInDays)
                .HasComment("Период времени в днях, когда фильм показывают в кинотеатре");
            });
            
                
            builder.Property(m => m.MovieCategory)
                .IsRequired();
            builder.HasMany(m => m.Actors);

            builder.HasOne(m => m.Producer)
                 .WithMany(p => p.Movies);
                
              
 

        }
    }

}
