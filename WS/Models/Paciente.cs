using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WS.Models
{
    [Table("Pacientes")]
    public class Paciente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Por Favor Digite o Nome do Paciente!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por Favor Insira o Quarto!")]
        [Range(1, int.MaxValue, ErrorMessage = "O número do Quarto não pode ser menor que 1")]
        public int Quarto { get; set; }

        public ICollection<Receita> Receitas { get; set; }
    }
}
