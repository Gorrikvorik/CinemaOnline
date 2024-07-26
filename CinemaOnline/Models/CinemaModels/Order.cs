using CinemaOnline.Data.Base;
using CinemaOnline.Models.ApplicationModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaOnline.Models.CinemaModels
{
    public class Order : IEntityBase
    {
        public int Id { get; set; }


        public DateTime OrderCreatedTime { get; set; }

        public DateTime OrderComplitedTime { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
