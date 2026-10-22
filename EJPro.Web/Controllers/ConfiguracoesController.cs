using EJPro.Web.Data;
using EJPro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EJPro.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var configuracao = await _context.ConfiguracoesOficina
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return View(configuracao ?? new ConfiguracaoOficina());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Salvar(ConfiguracaoOficina model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var configuracao = await _context.ConfiguracoesOficina
                .FirstOrDefaultAsync();

            if (configuracao == null)
            {
                _context.ConfiguracoesOficina.Add(model);
            }
            else
            {
                configuracao.NomeOficina = model.NomeOficina;
                configuracao.Documento = model.Documento;
                configuracao.Telefone = model.Telefone;
                configuracao.WhatsApp = model.WhatsApp;
                configuracao.Email = model.Email;
                configuracao.Endereco = model.Endereco;
                configuracao.Cidade = model.Cidade;
                configuracao.Estado = model.Estado;
                configuracao.Cep = model.Cep;
                configuracao.TipoChavePix = model.TipoChavePix;
                configuracao.ChavePix = model.ChavePix;
                configuracao.BeneficiarioPix = model.BeneficiarioPix;
                configuracao.CidadePix = model.CidadePix;
                configuracao.LogoUrl = model.LogoUrl;
            }

            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Configurações da oficina salvas com sucesso.";

            return RedirectToAction(nameof(Index));
        }
    }
}