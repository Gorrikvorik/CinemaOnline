using CinemaOnline.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace CinemaOnline.Models.CinemaModels
{
    public class Cinema : IEntityBase
    {
         
        public int Id { get; set; }

        [Display(Name = "Лого кинотаетра")]
        [Required(ErrorMessage = "Лого кинотаетра обязателен")]
        public string Logo { get; set; } = null!;

        [Display(Name = "Название кинотаетра")]
        [Required(ErrorMessage = "Название кинотаетра обязательно")]
        public string Name { get; set; } = null!;

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Описание кинотаетра обязательно")]
        public string Description { get; set; } = null!;

        //Связи
        public List<Movie> Movies { get; set; }
    }
}
