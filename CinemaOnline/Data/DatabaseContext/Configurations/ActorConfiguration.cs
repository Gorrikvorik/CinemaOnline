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

}
