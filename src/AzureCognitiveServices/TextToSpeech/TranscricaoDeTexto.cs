using Microsoft.CognitiveServices.Speech;
using System;
using System.Threading.Tasks;

namespace TextToSpeech
{
    public class TranscricaoDeTexto
    {
        private const string _chave = "<sua chave>";
        private const string _endpoint = "<seu endpoint>";

        private readonly SpeechConfig _configuracaoDaTranscricao;

        public TranscricaoDeTexto() =>
            _configuracaoDaTranscricao = SpeechConfig.FromEndpoint(new Uri(_endpoint), _chave);

        public async Task<string> Falar(string textoParaFalar, Narrador narrador)
        {
            _configuracaoDaTranscricao.SpeechSynthesisVoiceName = narrador.Codigo;

            using var sintetizador = new SpeechSynthesizer(_configuracaoDaTranscricao);
            using var resultado = await sintetizador.SpeakTextAsync(textoParaFalar);
            if (resultado.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                return $"O texto \"{textoParaFalar}\" será falado na saída de som padrão por {narrador.Nome} ({narrador.Pais})";
            }
            else if (resultado.Reason == ResultReason.Canceled)
            {
                var cancelamento = SpeechSynthesisCancellationDetails.FromResult(resultado);
                Console.WriteLine($"CANCELED: Reason={cancelamento.Reason}");

                if (cancelamento.Reason == CancellationReason.Error)
                    return $"Erro: {cancelamento.ErrorDetails}";
            }

            return string.Empty;
        }
    }
}
