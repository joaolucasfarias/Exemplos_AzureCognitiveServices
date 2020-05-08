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

        private readonly Treinamento _treinamento;

        public Predicao()
        {
            _servicoCognitivoDeVisaoPersonalizadaPredicao = new CustomVisionPredictionClient()
            {
                ApiKey = _chaveDePredicao,
                Endpoint = _endpoint
            };

            _treinamento = new Treinamento();
        }

        public IList<string> ClassificarPorUrl(string idDoProjeto, string url)
        {
            var resultadoDaClassificacao = _servicoCognitivoDeVisaoPersonalizadaPredicao
                .ClassifyImageUrl(
                new Guid(idDoProjeto),
                UltimaIteracaoRealizada(idDoProjeto),
                new ImageUrl(url));

            return AnalisarResultadoDaClassificacao(resultadoDaClassificacao);
        }

        public IList<string> ClassificarPorArquivo(string idDoProjeto, string localDoArquivo)
        {
            var arquivo = new FileStream(localDoArquivo, FileMode.Open);

            var resultadoDaClassificacao = _servicoCognitivoDeVisaoPersonalizadaPredicao
                .ClassifyImage(
                new Guid(idDoProjeto),
                UltimaIteracaoRealizada(idDoProjeto),
                arquivo);

            return AnalisarResultadoDaClassificacao(resultadoDaClassificacao);
        }

        private string UltimaIteracaoRealizada(string idDoProjeto) =>
            _treinamento.RetornarUltimaIteracaoRealizada(idDoProjeto);

        private static IList<string> AnalisarResultadoDaClassificacao(ImagePrediction resultadoDaClassificacao) =>
            resultadoDaClassificacao.Predictions
                .Select(resultado => $"Pode ser \"{resultado.TagName}\" com {resultado.Probability:P2} de probabilidade").ToList();
    }
}
