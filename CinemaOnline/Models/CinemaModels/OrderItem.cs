using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CinemaOnline.Data.Base;

namespace CinemaOnline.Models.CinemaModels
{
    public class OrderItem : IEntityBase
    {
       
        public int Id { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
