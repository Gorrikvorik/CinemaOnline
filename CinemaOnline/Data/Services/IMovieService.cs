using CinemaOnline.Models.CinemaModels;
using CinemaOnline.ViewModels;

namespace CinemaOnline.Data.Services
{
    public interface IMovieService
    {
        Task AddNewMovieAsync(NewMovieVM model);
        Task UpdateMovieAsync(NewMovieVM model);
        Task<Movie> GetMovieById(int id);

        Task DeleteMovieById(int id);
        Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues();
 

    }
}