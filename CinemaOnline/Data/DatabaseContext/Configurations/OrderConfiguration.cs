using CinemaOnline.Data.DatabaseContext.Configurations.ConfigurationHelpers;
using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaOnline.Data.DatabaseContext.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.User)
                .IsRequired()
                .HasColumnName("OrderOwnerUser");

            builder.Property(o => o.OrderCreatedTime).
                HasValueGenerator<DateTimeValueGenerator>();
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order);

            builder.Ignore(o => o.User); // Исключаем свойство User из модели базы данных
        }
    }

}
