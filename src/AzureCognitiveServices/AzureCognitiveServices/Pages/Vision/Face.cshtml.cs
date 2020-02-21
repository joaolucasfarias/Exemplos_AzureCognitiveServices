using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;

namespace AzureCognitiveServices
{
    public class FaceModel : PageModel
    {
        public ICollection<string> Mensagens { get; private set; }


        private IWebHostEnvironment _environment;
        public FaceModel(IWebHostEnvironment environment) =>
            _environment = environment;

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        public void OnPost()
        {
            if (Arquivo is null)
            {
                var url = Request.Form["url"];
                Mensagens = new Face.DeteccaoFacial().DetectarRostosPorUrl(url).Result;
            }
            else
            {
                var pasta = Path.Combine(_environment.ContentRootPath, "imagens");
                if (!Directory.Exists(pasta))
                    Directory.CreateDirectory(pasta);

                var caminhoDoArquivo = Path.Combine(_environment.ContentRootPath, "imagens", Arquivo.FileName);

                using (var fileStream = new FileStream(caminhoDoArquivo, FileMode.Create))
                    Arquivo.CopyTo(fileStream);

                Mensagens = new Face.DeteccaoFacial().DetectarRostosPorArquivo(caminhoDoArquivo).Result;

                Directory.Delete(pasta, true);
            }
        }
    }
}