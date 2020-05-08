using CustomVision;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.IO;

namespace AzureCognitiveServices
{
    public class PredicaoModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Treinamento _treinamento;

        public Project Projeto { get; private set; }

        public bool PodePredizer { get; private set; }

        public IList<string> Mensagens { get; private set; }

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        public PredicaoModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _treinamento = new Treinamento();
        }

        public void OnGet(string idDoProjeto)
        {
            CarregarProjeto(idDoProjeto);

            if (!_treinamento.PodePredizer(idDoProjeto))
            {
                Mensagens = new List<string> { "Não é possível predizer neste projeto, pois ainda não há nenhum treinamento realizado." };
                return;
            }

            PodePredizer = true;
        }

        private void CarregarProjeto(string idDoProjeto) =>
            Projeto = _treinamento.CarregarProjeto(idDoProjeto);

        public void OnPost(string idDoProjeto)
        {
            if (Arquivo is null)
            {
                var url = Request.Form["url"];
                Mensagens = new Predicao().ClassificarPorUrl(idDoProjeto, url);
            }
            else
            {
                var pasta = Path.Combine(_environment.ContentRootPath, "imagens");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var caminhoDoArquivo = Path.Combine(_environment.ContentRootPath, "imagens", Arquivo.FileName);

                using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Create))
                    Arquivo.CopyTo(fileStream);

                Mensagens = new Predicao().ClassificarPorArquivo(idDoProjeto, caminhoDoArquivo);

                Directory.Delete(pasta, true);
            }

            CarregarProjeto(idDoProjeto);
        }
    }
}