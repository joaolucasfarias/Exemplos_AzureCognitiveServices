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

        public string MensagemFotos { get; private set; }

        public bool ErroFotos { get; private set; }

        public string MensagemTreinamento { get; private set; }

        public bool ErroTreinamento { get; private set; }

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
            if (ErroAoPreencherCampos())
            {
                CarregarProjeto(idDoProjeto);
                return;
            }

            if (Arquivo is null)
            {
                var url = Request.Form["url"];
                _treinamento.AdicionarImagemPorUrl(idDoProjeto, url, Tags);
            }
            else
            {
                var pasta = Path.Combine(_environment.ContentRootPath, "imagens");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var caminhoDoArquivo = Path.Combine(_environment.ContentRootPath, "imagens", Arquivo.FileName);

                using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Create))
                    Arquivo.CopyTo(fileStream);

                _treinamento.AdicionarImagemPorArquivo(idDoProjeto, caminhoDoArquivo, Tags);

                Directory.Delete(pasta, true);
            }

            CarregarProjeto(idDoProjeto);

            MensagemFotos = "Imagem adicionada com sucesso!";

            if (!_treinamento.Treinar(idDoProjeto, TagsDoProjeto))
            {
                ErroTreinamento = true;
                MensagemTreinamento = "Porém, não é possível TREINAR o modelo sem que TODAS as tags tenham, ao menos, 5 fotos cada.";
                return;
            }

            MensagemTreinamento = "Treinamento realizado com sucesso!";
        }

        private bool ErroAoPreencherCampos()
        {
            var url = Request.Form["url"];

            if (string.IsNullOrWhiteSpace(url) && Arquivo is null)
            {
                MensagemFotos = "É necessário enviar uma imagem por URL OU por arquivo.";
                return ErroFotos = true;
            }

            if (!string.IsNullOrWhiteSpace(url) && !(Arquivo is null))
            {
                MensagemFotos = "É necessário escolher apenas UM método de envio de imagem.";
                return ErroFotos = true;
            }

            if (!Tags.Any())
            {
                MensagemFotos = "É necessário escolher pelo menos uma tag.";
                return ErroFotos = true;
            }

            return ErroFotos = false;
        }
    }
}