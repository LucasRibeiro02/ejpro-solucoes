using System.ComponentModel.DataAnnotations;

namespace EJPro.Web.Models
{
    public class CriarUsuarioViewModel
    {
        [Required(ErrorMessage = "Informe o e-mail.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha.")]
        [StringLength(
            100,
            MinimumLength = 8,
            ErrorMessage = "A senha deve possuir pelo menos 8 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirme a senha.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar senha")]
        [Compare(
            nameof(Senha),
            ErrorMessage = "As senhas informadas não são iguais.")]
        public string ConfirmacaoSenha { get; set; } = string.Empty;
    }
}