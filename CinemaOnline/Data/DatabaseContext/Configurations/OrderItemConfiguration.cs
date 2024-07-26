using CinemaOnline.Models.CinemaModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaOnline.Data.DatabaseContext.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price)
                .IsRequired();
            builder.Property(oi => oi.Amount)
                .IsRequired();                                        
            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems);

        }
         
        
    }

}
