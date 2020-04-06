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
            ListarProjetos();

        public void OnPost()
        {
            var tipo = Request.Form["tipo"];
            var nome = Request.Form["nome"];
            var descricao = Request.Form["descricao"];

            if ("projeto".Equals(tipo))
                CriarProjeto(nome, descricao);
            else
                CriarTag(nome, descricao);

            ListarProjetos();
        }

        private void CriarProjeto(string nome, string descricao) =>
            _treinamento.CriarProjeto(nome, descricao);

        private void CriarTag(string nome, string descricao)
        {
            var idDoProjeto = Request.Form["idDoProjeto"];

            _treinamento.CriarTag(
                Projetos.FirstOrDefault(p => idDoProjeto.Equals(p.Id)),
                nome,
                descricao);
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