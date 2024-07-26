using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Models.OwnedModels
{
    [Owned]
    public class Rating
    {
        public double Score { get; set; }

        public string Description;
    }
}
