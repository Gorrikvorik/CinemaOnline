using System.ComponentModel.DataAnnotations;

namespace CinemaOnline.Data.Base
{
    public interface IEntityBase
    {
        [Key]
        int Id { get; set; }
    }
}
