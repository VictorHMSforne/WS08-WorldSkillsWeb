using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS.Models
{
    [Table("Receitas")]
    public class Receita
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Por Favor Digite o Nome do Remédio!")]
        public string Remedio { get; set; }
        [Required(ErrorMessage = "Por Favor Digite a Hora!")]
        public string Hora { get; set; }

        public int PacienteId { get; set; }
        public Paciente Paciente { get; set; } //Propriedade De Navegação
    }
}
