using CinemaOnline.Data.Base;
using CinemaOnline.Models.OwnedModels;
using System.ComponentModel.DataAnnotations;

namespace CinemaOnline.Models.CinemaModels
{
    public class Actor : IEntityBase
    {

        
        public int Id { get; set; }

        [Display(Name = "Фотография профиля")]
        [Required(ErrorMessage = "Фото профиля обязательно")]
        public string ProfilePictureURL { get; set; } = null!;

        [Display(Name = "ФИО")]
        [Required(ErrorMessage = "ФИО обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "ФИО должно быть не меньше 3  и не больше 100 символов")]
        public string FullName { get; set; } = null!;

        [Display(Name = "Биография")]
        [Required(ErrorMessage = "Биография обязательна")]
        public string Bio { get; set; } = null!;

        public Rating Rating { get; set; }

        //Связи
        public List<ActorMovies>? ActorsMovies { get; set; }
    }
}
