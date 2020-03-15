using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var imagem = string.Empty;

            if (Arquivo is null)
            {
                imagem = Request.Form["url"];
                _analisarPorUrl = true;
            }
            else
            {
                var pasta = Path.Combine(_environment.ContentRootPath, "imagens");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var caminhoDoArquivo = Path.Combine(_environment.ContentRootPath, "imagens", Arquivo.FileName);

                using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Create))
                    Arquivo.CopyTo(fileStream);

                _analisarPorUrl = false;

                imagem = caminhoDoArquivo;
            }

            if (!_analisarPorUrl)
                Directory.Delete(imagem, true);
        }
    }
}