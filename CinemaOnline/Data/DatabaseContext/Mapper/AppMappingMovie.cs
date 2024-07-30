using AutoMapper;
using CinemaOnline.Models.CinemaModels;
using CinemaOnline.ViewModels;

namespace CinemaOnline.Data.DatabaseContext.Mapper
{
    public class AppMappingMovie : Profile
    {
        public AppMappingMovie()
        {
            CreateMap<NewMovieVM, Movie>().ReverseMap();
        }
    }

    
}
