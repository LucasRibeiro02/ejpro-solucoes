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
    public class VeiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VeiculosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            var veiculos = await _context.Veiculos
                .AsNoTracking()
                .Include(veiculo => veiculo.Cliente)
                .Where(veiculo => veiculo.Ativo)
                .OrderBy(veiculo => veiculo.Placa)
                .ToListAsync();

            return View(veiculos);
        }

        // GET: Veiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // GET: Veiculos/Create
        public IActionResult Create()
        {
            var clientesAtivos = _context.Clientes
                .Where(cliente => cliente.Ativo)
                .OrderBy(cliente => cliente.NomeCompleto);

            ViewData["ClienteId"] = new SelectList(
                clientesAtivos,
                "Id",
                "NomeCompleto");

            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("ClienteId,Placa,Marca,Modelo,Ano,Cor,Quilometragem,Chassi,Observacoes")]
            Veiculo veiculo)
                {
                    veiculo.Placa = (veiculo.Placa ?? string.Empty)
                        .Trim()
                        .ToUpperInvariant();

                    var placaJaCadastrada = await _context.Veiculos
                        .AnyAsync(item => item.Placa == veiculo.Placa);

            if (placaJaCadastrada)
                    {
                        ModelState.AddModelError(
                            nameof(Veiculo.Placa),
                            "Já existe um veículo ativo cadastrado com esta placa.");
                    }

                    if (ModelState.IsValid)
                    {
                        veiculo.DataCadastro = DateTime.UtcNow;
                        veiculo.Ativo = true;

                        _context.Add(veiculo);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }

                    var clientesAtivos = _context.Clientes
                        .Where(cliente => cliente.Ativo)
                        .OrderBy(cliente => cliente.NomeCompleto);

                    ViewData["ClienteId"] = new SelectList(
                        clientesAtivos,
                        "Id",
                        "NomeCompleto",
                        veiculo.ClienteId);

                    return View(veiculo);
                }

        // GET: Veiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo == null)
            {
                return NotFound();
            }

            var clientesDisponiveis = _context.Clientes
                .Where(cliente =>
                    cliente.Ativo ||
                    cliente.Id == veiculo.ClienteId)
                .OrderBy(cliente => cliente.NomeCompleto);

            ViewData["ClienteId"] = new SelectList(
                clientesDisponiveis,
                "Id",
                "NomeCompleto",
                veiculo.ClienteId);

            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,ClienteId,Placa,Marca,Modelo,Ano,Cor,Quilometragem,Chassi,Observacoes")]
            Veiculo veiculo)
                {
                    if (id != veiculo.Id)
                    {
                        return NotFound();
                    }

                    veiculo.Placa = (veiculo.Placa ?? string.Empty)
                        .Trim()
                        .ToUpperInvariant();

                    var placaJaCadastrada = await _context.Veiculos
                        .AnyAsync(item =>
                            item.Placa == veiculo.Placa &&
                            item.Id != veiculo.Id);

                    if (placaJaCadastrada)
                    {
                        ModelState.AddModelError(
                            nameof(Veiculo.Placa),
                            "Já existe outro veículo cadastrado com esta placa.");
                    }

                    if (ModelState.IsValid)
                    {
                        var veiculoExistente = await _context.Veiculos.FindAsync(id);

                        if (veiculoExistente == null)
                        {
                            return NotFound();
                        }

                        veiculoExistente.ClienteId = veiculo.ClienteId;
                        veiculoExistente.Placa = veiculo.Placa;
                        veiculoExistente.Marca = veiculo.Marca;
                        veiculoExistente.Modelo = veiculo.Modelo;
                        veiculoExistente.Ano = veiculo.Ano;
                        veiculoExistente.Cor = veiculo.Cor;
                        veiculoExistente.Quilometragem = veiculo.Quilometragem;
                        veiculoExistente.Chassi = veiculo.Chassi;
                        veiculoExistente.Observacoes = veiculo.Observacoes;

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }

                    var clientesDisponiveis = _context.Clientes
                        .Where(cliente =>
                            cliente.Ativo ||
                            cliente.Id == veiculo.ClienteId)
                        .OrderBy(cliente => cliente.NomeCompleto);

                    ViewData["ClienteId"] = new SelectList(
                        clientesDisponiveis,
                        "Id",
                        "NomeCompleto",
                        veiculo.ClienteId);

                    return View(veiculo);
                }

        // GET: Veiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = await _context.Veiculos
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var veiculo = await _context.Veiculos.FindAsync(id);

            if (veiculo != null)
            {
                veiculo.Ativo = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculos.Any(e => e.Id == id);
        }
    }
}
