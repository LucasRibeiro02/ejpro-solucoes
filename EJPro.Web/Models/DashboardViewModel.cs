namespace EJPro.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClientesAtivos { get; set; }

        public int TotalVeiculosAtivos { get; set; }

        public int TotalOrdensEmAndamento { get; set; }

        public decimal ValorOrdensEmAndamento { get; set; }

        public List<OrdemServico> UltimasOrdens { get; set; }
            = new List<OrdemServico>();
    }
}