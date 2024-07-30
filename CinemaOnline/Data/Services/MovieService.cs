using AutoMapper;
using CinemaOnline.Data.Base;
using CinemaOnline.Models.CinemaModels;
using CinemaOnline.ViewModels;

namespace CinemaOnline.Data.Services
{
    //TODO доделать сервис по eTicket и примерам с UOW
    //https://github.com/etrupja/complete-ecommerce-aspnet-mvc-application
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddNewMovieAsync(NewMovieVM model)
        {
            var movie = _mapper.Map<Movie>(model);
            var movieRepository = _unitOfWork.GetRepository<Movie>();
            try
            {
               await movieRepository.AddAsync(movie);

            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
            }
            await _unitOfWork.CommitAsync();
        }

        public Task DeleteMovieById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NewMovieDropdownsVM> GetNewMovieDropdownsValues()
        {
            throw new NotImplementedException();
        }

        public Task UpdateMovieAsync(NewMovieVM model)
        {
            throw new NotImplementedException();
        }
    }
}
