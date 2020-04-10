using CustomVision;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace AzureCognitiveServices
{
    public class SalvarTagModel : PageModel
    {
        private readonly Treinamento _treinamento;

        public Project Projeto { get; set; }

        public Tag Tag { get; set; }

        public SalvarTagModel() =>
            _treinamento = new Treinamento();

        public void OnGet(string idDoProjeto, string idDaTag)
        {
            CarregarProjeto(idDoProjeto);
            Tag = _treinamento.CarregarTag(Projeto, idDaTag);
        }

        private void CarregarProjeto(string idDoProjeto) =>
            Projeto = _treinamento.CarregarProjeto(idDoProjeto);

        public IActionResult OnPost(string idDoProjeto, string idDaTag)
        {
            var nome = Request.Form["nome"];
            var descricao = Request.Form["descricao"];

            CarregarProjeto(idDoProjeto);

            if (string.IsNullOrWhiteSpace(idDaTag))
                _treinamento.CriarTag(Projeto, nome, descricao);
            else
                _treinamento.EditarTag(Projeto,
                    idDaTag,
                    new Tag
                    {
                        Name = nome,
                        Description = descricao
                    });

            return RedirectToPage("/Vision/CustomVision/TagsDoProjeto", new { idDoProjeto });
        }
    }
}