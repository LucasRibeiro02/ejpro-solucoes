namespace EJPro.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalClientesAtivos { get; set; }

        public int TotalVeiculosAtivos { get; set; }

        public int TotalOrdensEmAndamento { get; set; }

        public decimal ValorOrdensEmAndamento { get; set; }

        public Dictionary<string, int> OrdensPorStatus { get; set; }
            = new Dictionary<string, int>();

        public List<string> MesesFaturamento { get; set; }
            = new List<string>();

        public List<decimal> ValoresFaturamento { get; set; }
            = new List<decimal>();

        public List<OrdemServico> UltimasOrdens { get; set; }
            = new List<OrdemServico>();
    }
}