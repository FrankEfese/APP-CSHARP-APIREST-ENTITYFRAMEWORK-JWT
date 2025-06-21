using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRest_EntityFramework.Models
{
    public class Equipo
    {

        // ATRIBUTOS

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public string liga {  get; set; }


        // RELACION 1 A MUCHOS CON JUGADOR
        public List<Jugador>? jugadores { get; set; }

    }
}
