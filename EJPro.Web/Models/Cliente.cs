using System.ComponentModel.DataAnnotations;

namespace EJPro.Web.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        //valor '?' significa que é OPCIONAL
        [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
        [StringLength(150, ErrorMessage = "O nome deve possuir no máximo 150 caracteres.")]
        [Display(Name = "Nome completo")]
        public string NomeCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o CPF ou CNPJ.")]
        [Display(Name = "CPF/CNPJ")]
        [RegularExpression(
            @"^(\d{11}|\d{14})$",
            ErrorMessage = "Informe um CPF com 11 números ou CNPJ com 14 números.")]
        public string Cpf { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [EmailAddress(ErrorMessage = "Informe um endereço de e-mail válido.")]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(250)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [StringLength(500)]
        [Display(Name = "Observações")]
        public string? Observacoes { get; set; }

        [Display(Name = "Data de cadastro")]
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

        public bool Ativo { get; set; } = true;

        public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();
    }
}