using System.ComponentModel.DataAnnotations;

namespace CinemaOnline.Models.CinemaModels
{
    public class Producer
    {
        public int Id { get; set; }

        [Display(Name = "Фото профиля")]
        public string ProfilePictureURL { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Biography")]
        public string Bio { get; set; }

        //Связи
        public List<Movie> Movies { get; set; }
    }
}
