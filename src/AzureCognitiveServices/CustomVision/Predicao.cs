using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImagePrediction = Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models.ImagePrediction;
using ImageUrl = Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models.ImageUrl;

namespace CustomVision
{
    public class Predicao
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

        public IEnumerable<string> ClassificarPorUrl(string idDoProjeto, string url)
        {
            var resultadoDaClassificacao = _servicoCognitivoDeVisaoPersonalizadaPredicao
                .ClassifyImageUrl(new Guid(idDoProjeto), "treeClassModel1", new ImageUrl(url));

            return AnalisarResultadoDaClassificacao(resultadoDaClassificacao);
        }

        public IEnumerable<string> ClassificarPorArquivo(string idDoProjeto, string localDoArquivo)
        {
            var arquivo = new FileStream(localDoArquivo, FileMode.Open);

            var resultadoDaClassificacao = _servicoCognitivoDeVisaoPersonalizadaPredicao
                .ClassifyImage(new Guid(idDoProjeto), "treeClassModel", arquivo);

            return AnalisarResultadoDaClassificacao(resultadoDaClassificacao);
        }

        private static IEnumerable<string> AnalisarResultadoDaClassificacao(ImagePrediction resultadoDaClassificacao) =>
            resultadoDaClassificacao.Predictions
                .Select(resultado => $"Pode ser \"{resultado.TagName}\" com {resultado.Probability:P2} de probabilidade").ToList();
    }
}
