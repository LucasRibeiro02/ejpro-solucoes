using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EJPro.Web.Models
{
    public class Veiculo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o proprietário do veículo.")]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "A placa é obrigatória.")]
        [StringLength(8)]
        [Display(Name = "Placa")]
        public string Placa { get; set; } = string.Empty;

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [StringLength(50)]
        [Display(Name = "Marca")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "O modelo é obrigatório.")]
        [StringLength(80)]
        [Display(Name = "Modelo")]
        public string Modelo { get; set; } = string.Empty;

        [Range(1900, 2100, ErrorMessage = "Informe um ano válido.")]
        [Display(Name = "Ano")]
        public int? Ano { get; set; }

        [StringLength(30)]
        [Display(Name = "Cor")]
        public string? Cor { get; set; }

        [Range(0, 9999999, ErrorMessage = "Informe uma quilometragem válida.")]
        [Display(Name = "Quilometragem")]
        public int? Quilometragem { get; set; }

        [StringLength(17)]
        [Display(Name = "Chassi")]
        public string? Chassi { get; set; }

        [StringLength(500)]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public bool Ativo { get; set; } = true;

        [ValidateNever]
        public Cliente Cliente { get; set; } = null!;

        public ICollection<OrdemServico> OrdensServico { get; set; }
            = new List<OrdemServico>();
    }
}