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
    public class SalvarProjetoModel : PageModel
    {
        private readonly Treinamento _treinamento;

        public Project Projeto { get; set; }

        public SalvarProjetoModel()
        {
            _treinamento = new Treinamento();
        }

        public void OnGet(string idDoProjeto)
        {

        }

        public void OnPost()
        {
            var nome = Request.Form["nome"];
            var descricao = Request.Form["descricao"];
            _treinamento.CriarProjeto(nome, descricao);
            RedirectToPage("/Vision/CustomVision/Projetos");
        }

        private void CarregarProjeto(string idDoProjeto)
        {
            Projeto = _treinamento.CarregarProjeto(idDoProjeto);
        }
    }
}