using EJPro.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace EJPro.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<OrdemServico> OrdensServico { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Veiculo>()
                .HasOne(veiculo => veiculo.Cliente)
                .WithMany(cliente => cliente.Veiculos)
                .HasForeignKey(veiculo => veiculo.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Veiculo>()
                .HasIndex(veiculo => veiculo.Placa)
                .IsUnique();

            modelBuilder.Entity<OrdemServico>()
                .HasOne(ordem => ordem.Veiculo)
                .WithMany(veiculo => veiculo.OrdensServico)
                .HasForeignKey(ordem => ordem.VeiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrdemServico>()
                 .Property(ordem => ordem.Status)
                 .HasConversion<string>()
                 .HasMaxLength(30);

            modelBuilder.Entity<OrdemServico>()
                 .HasIndex(ordem => ordem.DataAbertura);

            modelBuilder.Entity<OrdemServico>()
                 .HasIndex(ordem => ordem.Status);
        }
    }
}