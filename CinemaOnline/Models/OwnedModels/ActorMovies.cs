using CinemaOnline.Data.Base;
using CinemaOnline.Models.CinemaModels;

namespace CinemaOnline.Models.OwnedModels
{
    public class ActorMovies : IEntityBase
    {
        public int Id { get; set; }
        public int MovieId { get; set; }


        public int ActorId { get; set; }

        public Actor Actor { get; set; }
        public string Role { get; set; }

        public int MovieCount { get; }

    }
}
