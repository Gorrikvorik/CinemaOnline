using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaOnline.Data.DatabaseContext.Configurations
{
    public class ActorConfiguration : IEntityTypeConfiguration<Actor>
    {
        public void Configure(EntityTypeBuilder<Actor> builder)
        {
            builder.ToTable("Actors")
                .Property(a => a.ProfilePictureURL)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a=> a.FullName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(a => a.Bio)
                .IsRequired();
            builder.HasMany(a => a.ActorsMovies)
                .WithOne(m => m.Actor)
                .HasForeignKey(m => m.ActorId);
            builder.OwnsOne(a => a.Rating, rating =>
            {
                rating.Property(r => r.Score)
                .HasComment("Рейтинг пользователей даного фильма");
            });
     
                
        }
    }
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
                .IsRequired()
                .HasDefaultValue(0D);
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

            builder.Property(m => m.Producer)
                .IsRequired();
                
                
 

        }
    }

}
