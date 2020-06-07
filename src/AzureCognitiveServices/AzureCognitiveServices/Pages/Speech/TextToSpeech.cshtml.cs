using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using TextToSpeech;

namespace AzureCognitiveServices
{
    public class TextToSpeechModel : PageModel
    {
        public string Mensagem { get; private set; } = string.Empty;

        public readonly IEnumerable<Narrador> Narradores;

        public TextToSpeechModel() =>
            Narradores = Narrador.NarradoresDisponiveis();

        public void OnPost()
        {
            var textoParaFalar = Request.Form["textoParaFalar"];
            var narrador = Narradores.FirstOrDefault(n => Request.Form["Narrador"].Equals(n.Codigo));
            Mensagem = new TranscricaoDeTexto().Falar(textoParaFalar, narrador).Result;
        }s
    }
}