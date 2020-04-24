using CustomVision;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices
{
    public class TagsDoProjetoModel : PageModel
    {
        private readonly Treinamento _treinamento;

        public Project Projeto { get; private set; }

        public IList<Tag> Tags { get; private set; }

        public TagsDoProjetoModel() =>
            _treinamento = new Treinamento();

        public void OnGet(string idDoProjeto) =>
            CarregarInformacoes(idDoProjeto);

        private void CarregarInformacoes(string idDoProjeto)
        {
            CarregarProjeto(idDoProjeto);
            ListarTags(Projeto);
        }

        private void CarregarProjeto(string idDoProjeto) =>
            Projeto = _treinamento.CarregarProjeto(idDoProjeto);

        private void ListarTags(Project projeto) =>
            Tags = _treinamento.ListarTags(Projeto).ToList();

        public void OnPost(string idDoProjeto)
        {
            CarregarProjeto(idDoProjeto);
            _treinamento.ExcluirTag(Projeto, Request.Form["idDaTag"]);
            ListarTags(Projeto);
        }
    }
}