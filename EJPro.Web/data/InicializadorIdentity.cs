using Microsoft.AspNetCore.Identity;

namespace EJPro.Web.Data
{
    public static class InicializadorIdentity
    {
        public static async Task InicializarAsync(
            IServiceProvider services)
        {
            using var escopo = services.CreateScope();

            var roleManager = escopo.ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = escopo.ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            var configuration = escopo.ServiceProvider
                .GetRequiredService<IConfiguration>();

            string[] perfis =
            {
                "Administrador",
                "Usuario"
            };

            foreach (var perfil in perfis)
            {
                if (!await roleManager.RoleExistsAsync(perfil))
                {
                    await roleManager.CreateAsync(
                        new IdentityRole(perfil));
                }
            }

            var email = configuration["AdminInicial:Email"];
            var senha = configuration["AdminInicial:Senha"];

            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(senha))
            {
                throw new InvalidOperationException(
                    "Os dados do administrador inicial não foram configurados.");
            }

            var administrador =
                await userManager.FindByEmailAsync(email);

            if (administrador == null)
            {
                administrador = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var resultado = await userManager.CreateAsync(
                    administrador,
                    senha);

                if (!resultado.Succeeded)
                {
                    var erros = string.Join(
                        "; ",
                        resultado.Errors.Select(
                            erro => erro.Description));

                    throw new InvalidOperationException(
                        $"Não foi possível criar o administrador: {erros}");
                }
            }

            if (!await userManager.IsInRoleAsync(
                    administrador,
                    "Administrador"))
            {
                await userManager.AddToRoleAsync(
                    administrador,
                    "Administrador");
            }
        }
    }
}