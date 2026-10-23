using QRCoder;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EJPro.Web.Services
{
    public class PixResultado
    {
        public string CodigoCopiaECola { get; set; } = string.Empty;

        public string QrCodeBase64 { get; set; } = string.Empty;
    }

    public static class PixService
    {
        public static PixResultado Gerar(
            string chavePix,
            string beneficiario,
            string cidade,
            decimal valor,
            string identificador)
        {
            if (string.IsNullOrWhiteSpace(chavePix))
            {
                throw new ArgumentException(
                    "A chave PIX não foi informada.");
            }

            if (valor <= 0)
            {
                throw new ArgumentException(
                    "O valor do PIX deve ser maior que zero.");
            }

            chavePix = chavePix.Trim();

            beneficiario = NormalizarTexto(
                beneficiario,
                25,
                "EJ PRO SOLUCOES");

            cidade = NormalizarTexto(
                cidade,
                15,
                "CATANDUVA");

            identificador = NormalizarTexto(
                identificador,
                25,
                "***");

            var dadosContaPix =
                CriarCampo("00", "br.gov.bcb.pix") +
                CriarCampo("01", chavePix);

            var dadosAdicionais =
                CriarCampo("05", identificador);

            var payload =
                CriarCampo("00", "01") +
                CriarCampo("26", dadosContaPix) +
                CriarCampo("52", "0000") +
                CriarCampo("53", "986") +
                CriarCampo(
                    "54",
                    valor.ToString(
                        "0.00",
                        CultureInfo.InvariantCulture)) +
                CriarCampo("58", "BR") +
                CriarCampo("59", beneficiario) +
                CriarCampo("60", cidade) +
                CriarCampo("62", dadosAdicionais) +
                "6304";

            payload += CalcularCrc16(payload);

            using var geradorQrCode = new QRCodeGenerator();

            using var dadosQrCode = geradorQrCode.CreateQrCode(
                payload,
                QRCodeGenerator.ECCLevel.Q);

            using var qrCode = new PngByteQRCode(dadosQrCode);

            var imagemQrCode = qrCode.GetGraphic(10);

            return new PixResultado
            {
                CodigoCopiaECola = payload,

                QrCodeBase64 =
                    "data:image/png;base64," +
                    Convert.ToBase64String(imagemQrCode)
            };
        }

        private static string CriarCampo(
            string identificador,
            string valor)
        {
            var tamanhoEmBytes =
                Encoding.UTF8.GetByteCount(valor);

            if (tamanhoEmBytes > 99)
            {
                throw new ArgumentException(
                    $"O campo PIX {identificador} excedeu o tamanho permitido.");
            }

            return identificador +
                   tamanhoEmBytes.ToString("D2") +
                   valor;
        }

        private static string NormalizarTexto(
            string? texto,
            int tamanhoMaximo,
            string valorPadrao)
        {
            if (string.IsNullOrWhiteSpace(texto))
            {
                texto = valorPadrao;
            }

            texto = texto.Trim().ToUpperInvariant();

            var textoDecomposto =
                texto.Normalize(NormalizationForm.FormD);

            var resultado = new StringBuilder();

            foreach (var caractere in textoDecomposto)
            {
                var categoria =
                    CharUnicodeInfo.GetUnicodeCategory(caractere);

                if (categoria != UnicodeCategory.NonSpacingMark)
                {
                    resultado.Append(caractere);
                }
            }

            texto = resultado
                .ToString()
                .Normalize(NormalizationForm.FormC);

            texto = Regex.Replace(
                texto,
                @"[^A-Z0-9 $%*+\-./:]",
                " ");

            texto = Regex.Replace(
                texto,
                @"\s+",
                " ");

            texto = texto.Trim();

            if (texto.Length > tamanhoMaximo)
            {
                texto = texto[..tamanhoMaximo];
            }

            return texto;
        }

        private static string CalcularCrc16(string texto)
        {
            const ushort polinomio = 0x1021;
            ushort resultado = 0xFFFF;

            var bytes = Encoding.UTF8.GetBytes(texto);

            foreach (var byteAtual in bytes)
            {
                resultado ^= (ushort)(byteAtual << 8);

                for (var i = 0; i < 8; i++)
                {
                    resultado = (resultado & 0x8000) != 0
                        ? (ushort)((resultado << 1) ^ polinomio)
                        : (ushort)(resultado << 1);
                }
            }

            return resultado.ToString("X4");
        }
    }
}