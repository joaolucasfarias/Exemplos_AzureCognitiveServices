using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechTranslation
{
    public class TraducaoDeFala
    {
        private const string _chave = "<sua chave>";
        private const string _endpoint = "<seu endpoint>";

        private readonly SpeechTranslationConfig _configuracaoDaTraducao;

        public TraducaoDeFala() =>
            _configuracaoDaTraducao = SpeechTranslationConfig.FromEndpoint(new Uri(_endpoint), _chave);

        public async Task<IList<string>> Ouvir(Idioma traduzirDe, IEnumerable<Idioma> traduzirPara)
        {
            _configuracaoDaTraducao.SpeechRecognitionLanguage = traduzirDe.Codigo;

            foreach (var idioma in traduzirPara)
                _configuracaoDaTraducao.AddTargetLanguage(idioma.Codigo);

            using var reconhecer = new TranslationRecognizer(_configuracaoDaTraducao);
            var resultado = await reconhecer.RecognizeOnceAsync();

            if (resultado.Reason == ResultReason.TranslatedSpeech)
            {
                var traducoes = new List<string>(resultado.Translations.Count + 1)
                    { $"Traduzindo de \"{traduzirDe.Nome}\"" };

                traducoes.AddRange(
                    resultado
                    .Translations
                    .Select(r => $"Traduzido para \"{NomearIdioma(r.Key)}\": \"{r.Value}\""));

                return traducoes;
            }

            var semTraducao = new List<string> { $"Não foi possível traduzir de \"{traduzirDe.Nome}\"." };

            if (resultado.Reason == ResultReason.Canceled)
            {
                var cancelamento = CancellationDetails.FromResult(resultado);

                if (cancelamento.Reason == CancellationReason.Error)
                    semTraducao.Add($" Erro: \"{cancelamento.ErrorDetails}\".");
            }

            return semTraducao;
        }

        private static string NomearIdioma(string codigo) =>
            Idioma.IdiomasDisponiveis()
                .Any(i => i.Codigo.ToLower().Contains(codigo.ToLower()))
                    ? Idioma.IdiomasDisponiveis()
                        .First(i => i.Codigo.ToLower().Contains(codigo.ToLower())).Nome
                    : codigo;
    }
}
