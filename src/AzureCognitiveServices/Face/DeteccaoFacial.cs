using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Face
{
    public class DeteccaoFacial
    {
        private const string _chave = "<sua chave>";
        private const string _endpoint = "<seu endpoint>";

        private IFaceClient _servicoCongnitivoDeFace;

        public DeteccaoFacial() =>
            _servicoCongnitivoDeFace = new FaceClient(new ApiKeyServiceClientCredentials(_chave)) { Endpoint = _endpoint };

        public async Task<ICollection<string>> DetectarRostosPorUrl(string url)
        {
            var rostosDetectados = await _servicoCongnitivoDeFace.Face.DetectWithUrlAsync(url,
                    returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Age, FaceAttributeType.Emotion,
                FaceAttributeType.Gender, FaceAttributeType.Smile });

            return RetornarInformacoesDosRostosDetectados(rostosDetectados);
        }

        public async Task<ICollection<string>> DetectarRostosPorArquivo(string localDoArquivo)
        {
            var arquivo = new FileStream(localDoArquivo, FileMode.Open);

            var rostosDetectados = await _servicoCongnitivoDeFace.Face.DetectWithStreamAsync(arquivo,
                returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Age, FaceAttributeType.Emotion,
                FaceAttributeType.Gender, FaceAttributeType.Smile });

            return RetornarInformacoesDosRostosDetectados(rostosDetectados);
        }

        private static ICollection<string> RetornarInformacoesDosRostosDetectados(IList<DetectedFace> rostosDetectados)
        {
            if (rostosDetectados.Count == 0)
                return new List<string> { "Nenhum rosto encontrado na imagem." };

            var informacoesDeRetorno = new List<string>(rostosDetectados.Count)
            {$"Foram encontrados {rostosDetectados.Count} rosto(s) na imagem:" };

            var posicaoDoRosto = 0;

            foreach (var rosto in rostosDetectados)
            {
                var informacoesDoRosto = $"{++posicaoDoRosto}º - Aparenta ter {rosto.FaceAttributes.Age} anos, ";

                var tipoDeEmocao = string.Empty;
                var porcentagemDaEmocao = 0.0;
                var emocao = rosto.FaceAttributes.Emotion;
                if (emocao.Anger > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Anger; tipoDeEmocao = "com raiva"; }
                if (emocao.Contempt > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Contempt; tipoDeEmocao = "com desprezo"; }
                if (emocao.Disgust > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Disgust; tipoDeEmocao = "com nojo"; }
                if (emocao.Fear > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Fear; tipoDeEmocao = "com medo"; }
                if (emocao.Happiness > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Happiness; tipoDeEmocao = "feliz"; }
                if (emocao.Neutral > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Neutral; tipoDeEmocao = "neutro"; }
                if (emocao.Sadness > porcentagemDaEmocao) { porcentagemDaEmocao = emocao.Sadness; tipoDeEmocao = "triste"; }
                if (emocao.Surprise > porcentagemDaEmocao) { tipoDeEmocao = "surpreso"; }

                var possivelGenero = "female".Equals(rosto.FaceAttributes.Gender.ToString().ToLower())
                    ? "uma mulher"
                    : "um homem";

                informacoesDoRosto += $"deve estar {tipoDeEmocao}, baseado na aparência, há a possibilidade de ser {possivelGenero}, porcentagem de sorriso é de: {rosto.FaceAttributes.Smile * 100}%";

                informacoesDeRetorno.Add(informacoesDoRosto);
            }

            return informacoesDeRetorno;
        }
    }
}
