using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImagePrediction = Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models.ImagePrediction;
using ImageUrl = Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models.ImageUrl;

namespace CustomVision
{
    class Predicao
    {
        private const string _chaveDePredicao = "<sua chave de predição>";
        private const string _endpoint = "<seu endpoin>";

        private readonly ICustomVisionPredictionClient _servicoCognitivoDeVisaoPersonalizadaPredicao;

        public Predicao() =>
            _servicoCognitivoDeVisaoPersonalizadaPredicao = new CustomVisionPredictionClient()
            {
                ApiKey = _chaveDePredicao,
                Endpoint = _endpoint
            };

        public IEnumerable<string> ClassificarPorUrl(Project projeto, string url)
        {
            var resultadoDaClassificacao = _servicoCognitivoDeVisaoPersonalizadaPredicao
                .ClassifyImageUrl(projeto.Id, "treeClassModel", new ImageUrl(url));

            return AnalisarResultadoDaClassificacao(resultadoDaClassificacao);
        }

        public IEnumerable<string> ClassificarPorArquivo(Project projeto, string localDoArquivo)
        {
            var arquivo = new FileStream(localDoArquivo, FileMode.Open);

            var resultadoDaClassificacao = _servicoCognitivoDeVisaoPersonalizadaPredicao
                .ClassifyImage(projeto.Id, "treeClassModel", arquivo);

            return AnalisarResultadoDaClassificacao(resultadoDaClassificacao);
        }

        private static IEnumerable<string> AnalisarResultadoDaClassificacao(ImagePrediction resultadoDaClassificacao) =>
            resultadoDaClassificacao.Predictions
                .Select(resultado => $"Pode ser \"{resultado.TagName}\" com {resultado.Probability:P2} de probabilidade");
    }
}
