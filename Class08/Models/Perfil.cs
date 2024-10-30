using System.ComponentModel.DataAnnotations;

namespace Class08.Models
{
    public class Perfil
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        // we can put other atributes
        public string? UserName { get; set; } //username of the User in AspNetUsers
    }
}
