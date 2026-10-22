using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EJPro.Web.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarConfiguracaoOficina : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConfiguracoesOficina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NomeOficina = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Documento = table.Column<string>(type: "character varying(18)", maxLength: 18, nullable: true),
                    Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    WhatsApp = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Endereco = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Cidade = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    Cep = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    TipoChavePix = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ChavePix = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    BeneficiarioPix = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    CidadePix = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesOficina", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesOficina");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Clientes",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
