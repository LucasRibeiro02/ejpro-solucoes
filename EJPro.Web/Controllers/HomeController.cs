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