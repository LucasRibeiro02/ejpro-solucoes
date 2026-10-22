using System.ComponentModel.DataAnnotations;

namespace EJPro.Web.Models
{
    public class ConfiguracaoOficina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome da oficina.")]
        [StringLength(150)]
        [Display(Name = "Nome da oficina")]
        public string NomeOficina { get; set; } = "E&J PRO Soluções";

        [StringLength(18)]
        [Display(Name = "CPF/CNPJ")]
        public string? Documento { get; set; }

        [StringLength(20)]
        [Display(Name = "Telefone")]
        public string? Telefone { get; set; }

        [StringLength(20)]
        [Display(Name = "WhatsApp")]
        public string? WhatsApp { get; set; }

        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [StringLength(150)]
        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [StringLength(250)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [StringLength(100)]
        [Display(Name = "Cidade")]
        public string? Cidade { get; set; }

        [StringLength(2)]
        [Display(Name = "Estado")]
        public string? Estado { get; set; }

        [StringLength(10)]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [StringLength(30)]
        [Display(Name = "Tipo da chave PIX")]
        public string? TipoChavePix { get; set; }

        [StringLength(150)]
        [Display(Name = "Chave PIX")]
        public string? ChavePix { get; set; }

        [StringLength(150)]
        [Display(Name = "Beneficiário do PIX")]
        public string? BeneficiarioPix { get; set; }

        [StringLength(100)]
        [Display(Name = "Cidade do beneficiário")]
        public string? CidadePix { get; set; }

        [StringLength(300)]
        [Display(Name = "Caminho do logotipo")]
        public string? LogoUrl { get; set; } = "/images/logo.png";
    }
}