using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SpeechTranslation;
using System.Collections.Generic;
using System.Linq;

namespace AzureCognitiveServices
{
    public class SpeechTranslationModel : PageModel
    {
        public ICollection<string> Mensagens { get; private set; }

        public readonly IEnumerable<Idioma> Idiomas;

        [BindProperty]
        public IList<string> IdiomasAlvoDeTraducao { get; set; }

        public SpeechTranslationModel() =>
            Idiomas = Idioma.IdiomasDisponiveis();

        public void OnPost()
        {
            var traduzirDe = Idiomas.FirstOrDefault(n => Request.Form["traduzirDe"].Equals(n.Codigo));

            var traduzirPara = Idiomas.Where(i => IdiomasAlvoDeTraducao.Any(alvo => alvo.Equals(i)));

            Mensagens = new TraducaoDeFala().Ouvir(traduzirDe, traduzirPara).Result;
        }
    }
}