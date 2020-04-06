using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CustomVision;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace AzureCognitiveServices
{
    public class TreinamentoModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Treinamento _treinamento;

        public Project Projeto { get; private set; }

        public IEnumerable<Tag> Tags { get; private set; }

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        public TreinamentoModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _treinamento = new Treinamento();
        }

        public void OnGet(string idDoProjeto)
        {
            Projeto = _treinamento.ListarProjetos().FirstOrDefault(p => idDoProjeto.Equals(p.Id.ToString()));
            Tags = _treinamento.ListarTags(Projeto);
        }
    }
}