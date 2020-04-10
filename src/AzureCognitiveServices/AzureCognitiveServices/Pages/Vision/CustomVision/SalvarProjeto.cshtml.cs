using CustomVision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace AzureCognitiveServices
{
    public class SalvarProjetoModel : PageModel
    {
        private readonly Treinamento _treinamento;

        public Project Projeto { get; private set; }

        public SalvarProjetoModel() =>
            _treinamento = new Treinamento();

        public void OnGet(string idDoProjeto) =>
            Projeto = _treinamento.CarregarProjeto(idDoProjeto);

        public IActionResult OnPost(string idDoProjeto)
        {
            var nome = Request.Form["nome"];
            var descricao = Request.Form["descricao"];

            if (string.IsNullOrWhiteSpace(idDoProjeto))
                _treinamento.CriarProjeto(nome, descricao);
            else
                _treinamento.EditarProjeto(idDoProjeto,
                    new Project
                    {
                        Name = nome,
                        Description = descricao
                    });

            return RedirectToPage("/Vision/CustomVision/Projetos");
        }
    }
}