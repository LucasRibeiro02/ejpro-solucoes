using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EJPro.Web.Models
{
    public class OrdemServico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o veículo.")]
        [Display(Name = "Veículo")]
        public int VeiculoId { get; set; }

        [Display(Name = "Data de abertura")]
        public DateTime DataAbertura { get; set; } = DateTime.UtcNow;

        [Display(Name = "Previsão de entrega")]
        public DateTime? DataPrevisaoEntrega { get; set; }

        [Display(Name = "Data de conclusão")]
        public DateTime? DataConclusao { get; set; }

        [Range(0, 9999999, ErrorMessage = "Informe uma quilometragem válida.")]
        [Display(Name = "Quilometragem de entrada")]
        public int? QuilometragemEntrada { get; set; }

        [Required(ErrorMessage = "Informe o problema relatado pelo cliente.")]
        [StringLength(1000)]
        [Display(Name = "Problema relatado")]
        public string ProblemaRelatado { get; set; } = string.Empty;

        [StringLength(1000)]
        [Display(Name = "Diagnóstico")]
        public string? Diagnostico { get; set; }

        [StringLength(2000)]
        [Display(Name = "Serviço executado")]
        public string? ServicoExecutado { get; set; }

        [Range(
            typeof(decimal),
            "0",
            "99999999.99",
            ErrorMessage = "Informe um valor válido.",
            ParseLimitsInInvariantCulture = true)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Valor da mão de obra")]
        public decimal ValorMaoDeObra { get; set; }

        [Range(
            typeof(decimal),
            "0",
            "99999999.99",
            ErrorMessage = "Informe um valor válido.",
            ParseLimitsInInvariantCulture = true)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Valor das peças")]
        public decimal ValorPecas { get; set; }

        [Range(
            typeof(decimal),
            "0",
            "99999999.99",
            ErrorMessage = "Informe um desconto válido.",
            ParseLimitsInInvariantCulture = true)]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Desconto")]
        public decimal Desconto { get; set; }

        [Display(Name = "Situação")]
        public StatusOrdemServico Status { get; set; }
            = StatusOrdemServico.Aberta;

        [StringLength(1000)]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [NotMapped]
        [Display(Name = "Valor total")]
        public decimal ValorTotal =>
            Math.Max(0, ValorMaoDeObra + ValorPecas - Desconto);

        [ValidateNever]
        public Veiculo Veiculo { get; set; } = null!;
    }
}