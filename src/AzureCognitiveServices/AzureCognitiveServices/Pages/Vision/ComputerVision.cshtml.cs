using ComputerVision;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.IO;

namespace AzureCognitiveServices
{
    public class ComputerVisionModel : PageModel
    {
        public ICollection<string> Mensagens { get; private set; }

        public string AnaliseEscolhida { get; private set; }

        private IWebHostEnvironment _environment;
        private bool _analisarPorUrl;

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        public ComputerVisionModel(IWebHostEnvironment environment) =>
            _environment = environment;

        public void OnPost()
        {
            string imagem;
            var pasta = string.Empty;

            if (Arquivo is null)
            {
                imagem = Request.Form["url"];
                _analisarPorUrl = true;
            }
            else
            {
                pasta = Path.Combine(_environment.ContentRootPath, "imagens");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var caminhoDoArquivo = Path.Combine(_environment.ContentRootPath, "imagens", Arquivo.FileName);

                using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Create))
                    Arquivo.CopyTo(fileStream);

                _analisarPorUrl = false;

                imagem = caminhoDoArquivo;
            }

            AnalisarImagem(imagem);

            if (!_analisarPorUrl)
                Directory.Delete(pasta, true);
        }

        private void AnalisarImagem(string imagem)
        {
            var analiseDeImagem = new AnaliseDeImagem();

            var analiseRealizada = _analisarPorUrl
                ? analiseDeImagem.AnalisarPorUrl(imagem).Result
                : analiseDeImagem.AnalisarPorArquivo(imagem).Result;

            Mensagens = new List<string>();

            AnalisarLegenda(analiseDeImagem, analiseRealizada);
            AnalisarCategorias(analiseDeImagem, analiseRealizada);
            AnalisarTags(analiseDeImagem, analiseRealizada);
            AnalisarLogos(analiseDeImagem, analiseRealizada);
            AnalisarRostos(analiseDeImagem, analiseRealizada);
            AnalisarConteudoAdultoOuSensivel(analiseDeImagem, analiseRealizada);
            AnalisarEsquemaDeCores(analiseDeImagem, analiseRealizada);
            AnalisarCelebridades(analiseDeImagem, analiseRealizada);
            AnalisarPontosDeReferencia(analiseDeImagem, analiseRealizada);
        }

        private void AnalisarLegenda(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("legenda", "Descrição"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarLegenda(analiseRealizada));
        }

        private void AnalisarCategorias(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("categoria", "Categorias"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarCategorias(analiseRealizada));
        }

        private void AnalisarTags(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("tags", "Tags"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarTags(analiseRealizada));
        }

        private void AnalisarLogos(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("logos", "Logos"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarLogos(analiseRealizada));
        }

        private void AnalisarRostos(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("rostos", "Rostos"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarRostos(analiseRealizada));
        }

        private void AnalisarConteudoAdultoOuSensivel(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("adulto", "Conteúdo adulto ou sensível"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarConteudoAdultoOuSensivel(analiseRealizada));
        }

        private void AnalisarEsquemaDeCores(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("cores", "Esquema de cores"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarEsquemaDeCores(analiseRealizada));
        }

        private void AnalisarCelebridades(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("celebridades", "Celebridades"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarCelebridades(analiseRealizada));
        }

        private void AnalisarPontosDeReferencia(AnaliseDeImagem analiseDeImagem, ImageAnalysis analiseRealizada)
        {
            if (!AnaliseEscolhidaFoi("landmark", "Pontos de referência"))
                return;

            Mensagens.Add(analiseDeImagem.RetornarPontosDeReferencia(analiseRealizada));
        }

        private bool AnaliseEscolhidaFoi(string analiseEscolhida, string tituloDaAnaliseEscolhida)
        {
            if (!analiseEscolhida.Equals(Request.Form["analiseEscolhida"])
                && !"tudo".Equals(Request.Form["analiseEscolhida"]))
                return false;

            AnaliseEscolhida = "tudo".Equals(Request.Form["analiseEscolhida"])
                ? "Analise da imagem por completo"
                : tituloDaAnaliseEscolhida;

            return true;
        }
    }
}