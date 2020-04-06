using CustomVision;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices
{
    public class ProjetosModel : PageModel
    {
        private Treinamento _treinamento;

        public IEnumerable<Project> Projetos { get; private set; }

        public bool PodeAdicionarNovosProjetos { get; private set; }

        public ProjetosModel() =>
            _treinamento = new Treinamento();

        public void OnGet() =>
            RecuperarProjetos();

        public IEnumerable<Tag> ListarTags(Project projeto) =>
            _treinamento.ListarTags(projeto);

        public void OnPost()
        {
            var tipo = Request.Form["tipo"];
            var nome = Request.Form["nome"];
            var descricao = Request.Form["descricao"];

            _treinamento.CriarProjeto(nome, descricao);

            RecuperarProjetos();
        }

        private void RecuperarProjetos()
        {
            Projetos = _treinamento.ListarProjetos();
            PodeAdicionarNovosProjetos = Projetos.Count() < 2;
        }
    }
}