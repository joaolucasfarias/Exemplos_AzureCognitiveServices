using CustomVision;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AzureCognitiveServices
{
    public class TreinamentoModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Treinamento _treinamento;

        public Project Projeto { get; private set; }

        public IEnumerable<Tag> TagsDoProjeto { get; private set; }

        public string Mensagem { get; private set; }

        public bool Erro { get; private set; }

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        [BindProperty]
        public IList<string> Tags { get; set; }

        public TreinamentoModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _treinamento = new Treinamento();
        }

        public void OnGet(string idDoProjeto) =>
            CarregarProjeto(idDoProjeto);

        private void CarregarProjeto(string idDoProjeto)
        {
            Projeto = _treinamento.ListarProjetos().FirstOrDefault(p => idDoProjeto.Equals(p.Id.ToString()));
            TagsDoProjeto = _treinamento.ListarTags(Projeto);
        }

        public void OnPost(string idDoProjeto)
        {
            CarregarProjeto(idDoProjeto);

            if (ErroAoPreencherCampos()) return;

            var tagsEscolhidas = TagsDoProjeto
                .Where(tagDoProjeto => Tags.Any(t => tagDoProjeto.Id.ToString().Equals(t)));

            if (Arquivo is null)
            {
                var url = Request.Form["url"];
                _treinamento.AdicionarImagemPorUrl(Projeto, url, tagsEscolhidas);
            }
            else
            {
                var pasta = Path.Combine(_environment.ContentRootPath, "imagens");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var caminhoDoArquivo = Path.Combine(_environment.ContentRootPath, "imagens", Arquivo.FileName);

                using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Create))
                    Arquivo.CopyTo(fileStream);

                _treinamento.AdicionarImagemPorArquivo(Projeto, caminhoDoArquivo, tagsEscolhidas);

                Directory.Delete(pasta, true);
            }

            Mensagem = "Imagem adicionada e projeto treinado com sucesso!";
        }

        private bool ErroAoPreencherCampos()
        {
            var url = Request.Form["url"];

            if (string.IsNullOrWhiteSpace(url) && Arquivo is null)
            {
                Mensagem = "É necessário enviar uma imagem por URL OU por arquivo.";
                return Erro = true;
            }

            if (!string.IsNullOrWhiteSpace(url) && !(Arquivo is null))
            {
                Mensagem = "É necessário escolher apenas UM método de envio de imagem.";
                return Erro = true;
            }

            if (!Tags.Any())
            {
                Mensagem = "É necessário escolher pelo menos uma tag.";
                return Erro = true;
            }

            return Erro = false;
        }
    }
}