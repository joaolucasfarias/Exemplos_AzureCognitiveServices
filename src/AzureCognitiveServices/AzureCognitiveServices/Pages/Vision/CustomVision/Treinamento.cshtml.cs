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

        public IEnumerable<Tag> TagsDoProjeto { get; private set; }

        public string Mensagem { get; private set; }

        public bool Erro { get; private set; }

        [BindProperty]
        public IFormFile Arquivo { get; set; }

        [BindProperty]
        public IList<string> Tags { get; set; }

        public TreinamentoModel(IWebHostEnvironment environment)
        {
            _environment = environment;
            _treinamento = new Treinamento();
        }

        public void OnGet(string idDoProjeto) =>
            CarregarProjeto(idDoProjeto);

        private void CarregarProjeto(string idDoProjeto)
        {
            Projeto = _treinamento.ListarProjetos().FirstOrDefault(p => idDoProjeto.Equals(p.Id.ToString()));
            TagsDoProjeto = _treinamento.ListarTags(Projeto);
        }

        public void OnPost(string idDoProjeto)
        {
            CarregarProjeto(idDoProjeto);

            var url = Request.Form["url"];

            if (!CamposPreenchidosCorretamente(url))
            {
                Erro = true;
                return;
            }
        }

        private bool CamposPreenchidosCorretamente(string url)
        {
            if (string.IsNullOrWhiteSpace(url) && Arquivo is null)
            {
                Mensagem = "É necessário enviar uma imagem por URL OU por arquivo.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(url) && !(Arquivo is null))
            {
                Mensagem = "É necessário escolher apenas UM método de envio de imagem.";
                return false;
            }

            if (!Tags.Any())
            {
                Mensagem = "É necessário escolher pelo menos uma tag.";
                return false;
            }

            return true;
        }
    }
}