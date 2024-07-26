using CinemaOnline.Data.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CinemaOnline.Models.OwnedModels;

namespace CinemaOnline.Models.CinemaModels
{
    public class Movie : IEntityBase
    {
        
        public int Id { get; set; }

        
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public double Price { get; set; }
        public string ImageURL { get; set; } =null!;
        public ScreeningPeriod ScreeningPeriod { get; set; }
        public MovieCategory MovieCategory { get; set; }

        //Связи
        public List<Actor> Actors { get; set; }

        public List<Cinema> Cinemas { get; set; }

       
        //Producer
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }
    }
}
