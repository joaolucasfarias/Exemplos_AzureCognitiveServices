using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;
using TextToSpeech;

namespace SpeechToText
{
    public class TranscricaoDeFala
    {
        private const string _chave = "<sua chave>";
        private const string _endpoint = "<seu endpoint>";

        private readonly SpeechConfig _configuracaoDaTranscricao;

        public TranscricaoDeFala() =>
            _configuracaoDaTranscricao = SpeechConfig.FromEndpoint(new Uri(_endpoint), _chave);

        public async Task<string> Ouvir(Idioma idioma)
        {
            _configuracaoDaTranscricao.SpeechRecognitionLanguage = idioma.Codigo;

            using var reconhecer = new SpeechRecognizer(_configuracaoDaTranscricao);
            var resultado = await reconhecer.RecognizeOnceAsync();

            if (resultado.Reason == ResultReason.RecognizedSpeech)
                return $"Reconhecido: \"{resultado.Text}\" (em {idioma.Nome})";

            var semReconhecimento = "Não foi possível reconhecer a fala.";

            if (resultado.Reason == ResultReason.Canceled)
            {
                var cancelamento = CancellationDetails.FromResult(resultado);

                semReconhecimento += $"Cancelado por: \"{cancelamento.Reason}\".";

                if (cancelamento.Reason == CancellationReason.Error)
                    semReconhecimento += $"Erro: \"{cancelamento.ErrorDetails}\".";
            }

            return semReconhecimento;
        }
    }
}