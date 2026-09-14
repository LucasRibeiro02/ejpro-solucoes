using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EJPro.Web.Data;
using EJPro.Web.Models;

namespace EJPro.Web.Controllers
{
    public class OrdemServicoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdemServicoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrdemServicoes
        public async Task<IActionResult> Index()
        {
            var ordens = await _context.OrdensServico
                .AsNoTracking()
                .Include(ordem => ordem.Veiculo)
                .ThenInclude(veiculo => veiculo.Cliente)
                .OrderByDescending(ordem => ordem.DataAbertura)
                .ToListAsync();

            return View(ordens);
        }

        // GET: OrdemServicoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordemServico = await _context.OrdensServico
                .AsNoTracking()
                .Include(ordem => ordem.Veiculo)
                .ThenInclude(veiculo => veiculo.Cliente)
                .FirstOrDefaultAsync(ordem => ordem.Id == id);

            if (ordemServico == null)
            {
                return NotFound();
            }

            return View(ordemServico);
        }

        // GET: OrdemServicoes/Imprimir/5
        public async Task<IActionResult> Imprimir(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordemServico = await _context.OrdensServico
                .AsNoTracking()
                .Include(ordem => ordem.Veiculo)
                .ThenInclude(veiculo => veiculo.Cliente)
                .FirstOrDefaultAsync(ordem => ordem.Id == id);

            if (ordemServico == null)
            {
                return NotFound();
            }

            return View(ordemServico);
        }

        // GET: OrdemServicoes/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.VeiculoId = await MontarListaVeiculosAsync();

            return View();
        }

        // POST: OrdemServicoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("VeiculoId,DataPrevisaoEntrega,QuilometragemEntrada,ProblemaRelatado,Diagnostico,ServicoExecutado,ValorMaoDeObra,ValorPecas,Desconto,Observacoes")]
            OrdemServico ordemServico)
        {
            var veiculoExiste = await _context.Veiculos
                .AnyAsync(veiculo =>
                    veiculo.Id == ordemServico.VeiculoId &&
                    veiculo.Ativo);

            if (!veiculoExiste)
            {
                ModelState.AddModelError(
                    nameof(OrdemServico.VeiculoId),
                    "Selecione um veículo ativo.");
            }

            if (ModelState.IsValid)
            {
                if (ordemServico.DataPrevisaoEntrega.HasValue)
                {
                    var dataInformada = DateTime.SpecifyKind(
                        ordemServico.DataPrevisaoEntrega.Value,
                        DateTimeKind.Unspecified);

                    var fusoHorarioBrasil = TimeZoneInfo.FindSystemTimeZoneById(
                        "America/Sao_Paulo");

                    ordemServico.DataPrevisaoEntrega =
                        TimeZoneInfo.ConvertTimeToUtc(
                            dataInformada,
                            fusoHorarioBrasil);
                }

                ordemServico.DataAbertura = DateTime.UtcNow;
                ordemServico.DataConclusao = null;
                ordemServico.Status = StatusOrdemServico.Aberta;

                _context.Add(ordemServico);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.VeiculoId = await MontarListaVeiculosAsync(
                ordemServico.VeiculoId);

            return View(ordemServico);
        }

        // GET: OrdemServicoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordemServico = await _context.OrdensServico.FindAsync(id);

            if (ordemServico == null)
            {
                return NotFound();
            }

            ViewBag.VeiculoId = await MontarListaVeiculosAsync(
                ordemServico.VeiculoId);

            return View(ordemServico);
        }

        // POST: OrdemServicoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,VeiculoId,DataPrevisaoEntrega,QuilometragemEntrada,ProblemaRelatado,Diagnostico,ServicoExecutado,ValorMaoDeObra,ValorPecas,Desconto,Status,Observacoes")]
            OrdemServico ordemServico)
        {
            if (id != ordemServico.Id)
            {
                return NotFound();
            }

            var veiculoValido = await _context.Veiculos
                .AnyAsync(veiculo =>
                    veiculo.Id == ordemServico.VeiculoId &&
                    (veiculo.Ativo ||
                     _context.OrdensServico.Any(ordem =>
                         ordem.Id == id &&
                         ordem.VeiculoId == veiculo.Id)));

            if (!veiculoValido)
            {
                ModelState.AddModelError(
                    nameof(OrdemServico.VeiculoId),
                    "Selecione um veículo válido.");
            }

            if (ModelState.IsValid)
            {
                var ordemExistente = await _context.OrdensServico.FindAsync(id);

                if (ordemExistente == null)
                {
                    return NotFound();
                }

                DateTime? previsaoEmUtc = null;

                if (ordemServico.DataPrevisaoEntrega.HasValue)
                {
                    var previsaoLocal = DateTime.SpecifyKind(
                        ordemServico.DataPrevisaoEntrega.Value,
                        DateTimeKind.Unspecified);

                    var fusoBrasil = TimeZoneInfo.FindSystemTimeZoneById(
                        "America/Sao_Paulo");

                    previsaoEmUtc = TimeZoneInfo.ConvertTimeToUtc(
                        previsaoLocal,
                        fusoBrasil);
                }

                ordemExistente.VeiculoId = ordemServico.VeiculoId;
                ordemExistente.DataPrevisaoEntrega = previsaoEmUtc;
                ordemExistente.QuilometragemEntrada =
                    ordemServico.QuilometragemEntrada;
                ordemExistente.ProblemaRelatado =
                    ordemServico.ProblemaRelatado;
                ordemExistente.Diagnostico = ordemServico.Diagnostico;
                ordemExistente.ServicoExecutado =
                    ordemServico.ServicoExecutado;
                ordemExistente.ValorMaoDeObra =
                    ordemServico.ValorMaoDeObra;
                ordemExistente.ValorPecas = ordemServico.ValorPecas;
                ordemExistente.Desconto = ordemServico.Desconto;
                ordemExistente.Status = ordemServico.Status;
                ordemExistente.Observacoes = ordemServico.Observacoes;

                if (ordemServico.Status == StatusOrdemServico.Finalizada ||
                    ordemServico.Status == StatusOrdemServico.Entregue)
                {
                    ordemExistente.DataConclusao ??= DateTime.UtcNow;
                }
                else
                {
                    ordemExistente.DataConclusao = null;
                }

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.VeiculoId = await MontarListaVeiculosAsync(
                ordemServico.VeiculoId);

            return View(ordemServico);
        }

        // GET: OrdemServicoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ordemServico = await _context.OrdensServico
                .Include(o => o.Veiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ordemServico == null)
            {
                return NotFound();
            }

            return View(ordemServico);
        }

        // POST: OrdemServicoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordemServico = await _context.OrdensServico.FindAsync(id);

            if (ordemServico == null)
            {
                return NotFound();
            }

            ordemServico.Status = StatusOrdemServico.Cancelada;
            ordemServico.DataConclusao = null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<SelectListItem>> MontarListaVeiculosAsync(
            int? veiculoSelecionado = null)
        {
            var veiculos = await _context.Veiculos
                .AsNoTracking()
                .Include(veiculo => veiculo.Cliente)
                .Where(veiculo =>
                    veiculo.Ativo ||
                    veiculo.Id == veiculoSelecionado)
                .OrderBy(veiculo => veiculo.Placa)
                .ToListAsync();

            return veiculos
                .Select(veiculo => new SelectListItem
                {
                    Value = veiculo.Id.ToString(),
                    Text = $"{veiculo.Placa} — {veiculo.Marca} " +
                           $"{veiculo.Modelo} — {veiculo.Cliente.NomeCompleto}",
                    Selected = veiculo.Id == veiculoSelecionado
                })
                .ToList();
        }

        private bool OrdemServicoExists(int id)
        {
            return _context.OrdensServico.Any(e => e.Id == id);
        }
    }
}
