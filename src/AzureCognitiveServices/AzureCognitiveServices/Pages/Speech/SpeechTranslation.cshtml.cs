using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpeechTranslation;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices
{
    public class SpeechTranslationModel : PageModel
    {
        public ICollection<string> Mensagens { get; private set; } = new List<string>(0);

        public readonly IEnumerable<Idioma> Idiomas;

        [BindProperty]
        public IList<string> TraduzirPara { get; set; }

        public SpeechTranslationModel() =>
            Idiomas = Idioma.IdiomasDisponiveis();

        public void OnPost()
        {
            if (!TraduzirPara.Any())
            {
                Mensagens.Add("Não foi possível traduzir. Você precisa selecionar pelo menos 1 idioma alvo.");
                return;
            }

            var traduzirDe = Idiomas.FirstOrDefault(n => Request.Form["traduzirDe"].Equals(n.Codigo));

            var traduzirPara = Idiomas.Where(i => TraduzirPara.Any(alvo => alvo.Equals(i.Codigo)));

            Mensagens = new TraducaoDeFala().Ouvir(traduzirDe, traduzirPara).Result;
        }
    }
}