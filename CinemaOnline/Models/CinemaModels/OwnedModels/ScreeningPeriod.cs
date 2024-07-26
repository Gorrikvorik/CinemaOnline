using Microsoft.EntityFrameworkCore;

namespace CinemaOnline.Models.CinemaModels.OwnedModels
{
    [Owned]
    public class ScreeningPeriod
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public double ScreenTimeInDays { get => (EndDate - StartDate).TotalDays; }
    }
}
