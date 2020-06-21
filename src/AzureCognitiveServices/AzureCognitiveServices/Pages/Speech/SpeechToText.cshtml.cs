using Microsoft.AspNetCore.Mvc.RazorPages;
using SpeechToText;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices
{
    public class SpeechToTextModel : PageModel
    {
        public string Mensagem { get; private set; } = string.Empty;

        public readonly IEnumerable<Idioma> Idiomas;

        public SpeechToTextModel() =>
            Idiomas = Idioma.IdiomasDisponiveis();

        public void OnPost()
        {
            var idioma = Idiomas.FirstOrDefault(n => Request.Form["idioma"].Equals(n.Codigo));
            Mensagem = new TranscricaoDeFala().Ouvir(idioma).Result;
        }
    }
}
