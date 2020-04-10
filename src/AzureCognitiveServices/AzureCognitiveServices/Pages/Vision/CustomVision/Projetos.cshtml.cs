using CustomVision;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices
{
    public class ProjetosModel : PageModel
    {
        private readonly Treinamento _treinamento;

        public IEnumerable<Project> Projetos { get; private set; }

        public bool PodeAdicionarNovosProjetos { get; private set; }

        public ProjetosModel()
        {
            _treinamento = new Treinamento();
            ListarProjetos();
        }

        private void ListarProjetos()
        {
            Projetos = _treinamento.ListarProjetos();
            PodeAdicionarNovosProjetos = Projetos.Count() < 2;
        }

        public IEnumerable<Tag> ListarTags(Project projeto) =>
            _treinamento.ListarTags(projeto);
    }
}