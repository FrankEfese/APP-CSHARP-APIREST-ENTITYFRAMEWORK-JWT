using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiRest_EntityFramework.Models
{
    public class Jugador
    {

        // ATRIBUTOS

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }

        [Required]
        public string nombre { get; set; }

        [Required]
        public int edad {  get; set; }

        [Required]
        public string nacionalidad { get; set; }

        [Required]
        public int altura { get; set; }

        [Required]
        public int peso { get; set; }


        // RELACION 1 A MUCHOS CON EQUIPO

        [ForeignKey("equipo")]
        public int? idEquipo { get; set; }
        
        public Equipo? equipo { get; set; }


    }
}
