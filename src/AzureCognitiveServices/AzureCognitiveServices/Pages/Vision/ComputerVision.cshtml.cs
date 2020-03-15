using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace AzureCognitiveServices
{
    public class ComputerVisionModel : PageModel
    {
        public ICollection<string> Mensagens { get; private set; }

        public string AnaliseEscolhida { get; private set; }

        private IWebHostEnvironment _environment;

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        public ComputerVisionModel(IWebHostEnvironment environment) =>
            _environment = environment;
    }
}