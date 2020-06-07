using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TextToSpeech;

namespace AzureCognitiveServices
{
    public class TextToSpeechModel : PageModel
    {
        public string Mensagem { get; private set; }

        [BindProperty]
        public Narrador Narrador { get; set; }

        public readonly IEnumerable<Narrador> Narradores;

        public TextToSpeechModel() =>
            Narradores = Narrador.NarradoresDisponiveis();

        public void OnPost()
        {
            var textoParaFalar = Request.Form["textoParaFalar"];
            Mensagem = new TranscricaoDeTexto().Falar(textoParaFalar, Narrador).Result;
        }
    }
}