using EJPro.Web.Data;
using EJPro.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EJPro.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var statusEmAndamento = new[]
            {
                StatusOrdemServico.Aberta,
                StatusOrdemServico.EmDiagnostico,
                StatusOrdemServico.AguardandoAprovacao,
                StatusOrdemServico.EmExecucao
            };

            var ordensPorStatus = await _context.OrdensServico
    .AsNoTracking()
    .GroupBy(ordem => ordem.Status)
    .Select(grupo => new
    {
        Status = grupo.Key,
        Quantidade = grupo.Count()
    })
    .ToDictionaryAsync(
        item => item.Status.ToString(),
        item => item.Quantidade
    );

            var agora = DateTime.UtcNow;

            var inicioPeriodo = new DateTime(
                agora.Year,
                agora.Month,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc
            ).AddMonths(-5);

            var ordensFaturadas = await _context.OrdensServico
                .AsNoTracking()
                .Where(ordem =>
                    ordem.DataConclusao.HasValue &&
                    ordem.DataConclusao.Value >= inicioPeriodo &&
                    (ordem.Status == StatusOrdemServico.Finalizada ||
                     ordem.Status == StatusOrdemServico.Entregue))
                .Select(ordem => new
                {
                    Data = ordem.DataConclusao!.Value,
                    Valor = ordem.ValorMaoDeObra +
                            ordem.ValorPecas -
                            ordem.Desconto
                })
                .ToListAsync();

            var mesesFaturamento = new List<string>();
            var valoresFaturamento = new List<decimal>();

            for (var indice = 0; indice < 6; indice++)
            {
                var inicioMes = inicioPeriodo.AddMonths(indice);
                var fimMes = inicioMes.AddMonths(1);

                mesesFaturamento.Add(
                    inicioMes.ToString(
                        "MMM/yy",
                        new System.Globalization.CultureInfo("pt-BR")
                    )
                );

                valoresFaturamento.Add(
                    ordensFaturadas
                        .Where(ordem =>
                            ordem.Data >= inicioMes &&
                            ordem.Data < fimMes)
                        .Sum(ordem => ordem.Valor)
                );
            }

            var dashboard = new DashboardViewModel
            {
                TotalClientesAtivos = await _context.Clientes
                    .CountAsync(cliente => cliente.Ativo),

                TotalVeiculosAtivos = await _context.Veiculos
                    .CountAsync(veiculo => veiculo.Ativo),

                TotalOrdensEmAndamento = await _context.OrdensServico
                    .CountAsync(ordem =>
                        statusEmAndamento.Contains(ordem.Status)),

                ValorOrdensEmAndamento = await _context.OrdensServico
                    .Where(ordem =>
                        statusEmAndamento.Contains(ordem.Status))
                    .SumAsync(ordem =>
                        ordem.ValorMaoDeObra +
                        ordem.ValorPecas -
                        ordem.Desconto),

                OrdensPorStatus = ordensPorStatus,

                MesesFaturamento = mesesFaturamento,

                ValoresFaturamento = valoresFaturamento,

                UltimasOrdens = await _context.OrdensServico
                    .AsNoTracking()
                    .Include(ordem => ordem.Veiculo)
                    .ThenInclude(veiculo => veiculo.Cliente)
                    .OrderByDescending(ordem => ordem.DataAbertura)
                    .Take(5)
                    .ToListAsync()
            };

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ??
                            HttpContext.TraceIdentifier
            });
        }
    }
}