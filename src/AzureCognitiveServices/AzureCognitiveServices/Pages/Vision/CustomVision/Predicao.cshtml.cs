using CustomVision;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace AzureCognitiveServices
{
    public class PredicaoModel : PageModel
    {
        private readonly IWebHostEnvironment _environment;
        private readonly Treinamento _treinamento;
        private readonly Predicao _predicao;

        public Project Projeto { get; private set; }

        public bool PodePredizer { get; private set; }

        public string Mensagem { get; private set; }

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        public PredicaoModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _treinamento = new Treinamento();
            _predicao = new Predicao();
        }

        public void OnGet(string idDoProjeto)
        {
            CarregarProjeto(idDoProjeto);

            if (!_treinamento.PodePredizer(idDoProjeto))
            {
                Mensagem = "Não é possível predizer, pois ainda não há nenhum treinamento realizado.";
                return;
            }

            PodePredizer = true;
        }

        private void CarregarProjeto(string idDoProjeto) =>
            Projeto = _treinamento.CarregarProjeto(idDoProjeto);
    }
}