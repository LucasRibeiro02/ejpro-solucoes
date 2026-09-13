using System.ComponentModel.DataAnnotations;

namespace EJPro.Web.Models
{
    public enum StatusOrdemServico
    {
        [Display(Name = "Aberta")]
        Aberta = 1,

        [Display(Name = "Em diagnóstico")]
        EmDiagnostico = 2,

        [Display(Name = "Aguardando aprovação")]
        AguardandoAprovacao = 3,

        [Display(Name = "Em execução")]
        EmExecucao = 4,

        [Display(Name = "Finalizada")]
        Finalizada = 5,

        [Display(Name = "Entregue")]
        Entregue = 6,

        [Display(Name = "Cancelada")]
        Cancelada = 7
    }
}