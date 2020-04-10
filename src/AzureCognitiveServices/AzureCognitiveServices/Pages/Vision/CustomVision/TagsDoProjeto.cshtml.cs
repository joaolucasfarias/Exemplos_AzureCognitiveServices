using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomVision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace AzureCognitiveServices
{
    public class TagsDoProjetoModel : PageModel
    {
        private readonly Treinamento _treinamento;

        public Project Projeto { get; set; }

        public IEnumerable<Tag> Tags { get; set; }

        public void OnGet()
        {

        }
    }
}