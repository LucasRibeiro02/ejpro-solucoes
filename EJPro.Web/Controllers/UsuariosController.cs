using EJPro.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EJPro.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsuariosController(
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(
            CriarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usuarioExistente =
                await _userManager.FindByEmailAsync(model.Email);

            if (usuarioExistente != null)
            {
                ModelState.AddModelError(
                    nameof(model.Email),
                    "Já existe um usuário cadastrado com esse e-mail.");

                return View(model);
            }

            var usuario = new IdentityUser
            {
                UserName = model.Email.Trim(),
                Email = model.Email.Trim(),
                EmailConfirmed = true
            };

            var resultado = await _userManager.CreateAsync(
                usuario,
                model.Senha);

            if (!resultado.Succeeded)
            {
                foreach (var erro in resultado.Errors)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        erro.Description);
                }

                return View(model);
            }

            var resultadoPerfil = await _userManager.AddToRoleAsync(
                usuario,
                "Usuario");

            if (!resultadoPerfil.Succeeded)
            {
                await _userManager.DeleteAsync(usuario);

                ModelState.AddModelError(
                    string.Empty,
                    "Não foi possível atribuir o perfil ao usuário.");

                return View(model);
            }

            TempData["MensagemSucesso"] =
                "Usuário criado com sucesso.";

            return RedirectToAction(nameof(Criar));
        }
    }
}