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
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .Where(cliente => cliente.Ativo)
                .OrderBy(cliente => cliente.NomeCompleto)
                .ToListAsync();

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("NomeCompleto,Cpf,Telefone,Email,Endereco,Observacoes")]
            Cliente cliente)
                {
                    if (ModelState.IsValid)
                    {
                        cliente.DataCadastro = DateTime.UtcNow;
                        cliente.Ativo = true;

                        _context.Add(cliente);
                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(Index));
                    }

                    return View(cliente);
                }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,NomeCompleto,Cpf,Telefone,Email,Endereco,Observacoes")]
            Cliente cliente)
                {
                    if (id != cliente.Id)
                    {
                        return NotFound();
                    }

                    if (!ModelState.IsValid)
                    {
                        return View(cliente);
                    }

                    var clienteExistente = await _context.Clientes.FindAsync(id);

                    if (clienteExistente == null)
                    {
                        return NotFound();
                    }

                    clienteExistente.NomeCompleto = cliente.NomeCompleto;
                    clienteExistente.Cpf = cliente.Cpf;
                    clienteExistente.Telefone = cliente.Telefone;
                    clienteExistente.Email = cliente.Email;
                    clienteExistente.Endereco = cliente.Endereco;
                    clienteExistente.Observacoes = cliente.Observacoes;

            await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.


        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente != null)
            {
                cliente.Ativo = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
