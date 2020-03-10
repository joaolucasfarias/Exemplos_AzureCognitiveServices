using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerVision
{
    public class AnaliseDeImagem
    {
        private const string _chave = "<sua chave>";
        private const string _endpoint = "<seu endpoint>";

        private IComputerVisionClient _servicoCognitivoDeVisaoComputacional;

        public AnaliseDeImagem() =>
            _servicoCognitivoDeVisaoComputacional = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_chave))
            { Endpoint = _endpoint };

        public async Task<ImageAnalysis> AnalisarPorUrl(string url) =>
            await _servicoCognitivoDeVisaoComputacional.AnalyzeImageAsync(
                url,
                new List<VisualFeatureTypes>()
                    {
                      VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                      VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                      VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                      VisualFeatureTypes.Color, VisualFeatureTypes.Brands
                    }, language: "pt");

        public async Task<ImageAnalysis> AnalisarPorArquivo(string localDoArquivo)
        {
            var arquivo = new FileStream(localDoArquivo, FileMode.Open);

            return await _servicoCognitivoDeVisaoComputacional.AnalyzeImageInStreamAsync(
                arquivo,
                new List<VisualFeatureTypes>()
                    {
                      VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
                      VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
                      VisualFeatureTypes.Tags, VisualFeatureTypes.Adult,
                      VisualFeatureTypes.Color, VisualFeatureTypes.Brands
                    }, language: "pt");
        }

        public ICollection<string> RetornarLegenda(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Description.Captions.Count == 0)
                return new List<string> { "Não foi possível definir o que há na imagem." };

            var legendas = new List<string>(analiseRealizada.Description.Captions.Count);

            legendas
                .AddRange(analiseRealizada
                    .Description
                    .Captions
                    .Select(c => $"{c.Text} ({c.Confidence * 100:n2}% de certeza)"));

            return legendas;
        }

        public ICollection<string> RetornarCategorias(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Categories.Count == 0)
                return new List<string> { "Não foi possível categorizar a imagem." };

            var categorias = new List<string>(analiseRealizada.Categories.Count);

            categorias
                .AddRange(analiseRealizada
                    .Categories
                    .Select(c => $"{c.Name} ({c.Score * 100:n2}% de certeza)"));

            return categorias;
        }

        public string RetornarTags(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Tags.Count == 0)
                return "Não foi possível tagear a imagem.";

            return analiseRealizada
                .Tags
                .Aggregate(string.Empty, (atual, tag) => $"{atual}{tag.Name} ({tag.Confidence * 100:n2}% de certeza) - ");
        }

        public ICollection<string> RetornarLogos(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Brands.Count == 0)
                return new List<string> { "Não foi possível identificar logos na imagem." };

            var logos = new List<string>(analiseRealizada.Brands.Count);

            logos
                .AddRange(analiseRealizada
                    .Brands
                    .Select(l => $"{l.Name} ({l.Confidence * 100:n2}% de certeza)"));

            return logos;
        }

        public ICollection<string> RetornarRostos(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Faces.Count == 0)
                return new List<string> { "Não foi possível identificar rostos na imagem." };

            var rostos = new List<string>(analiseRealizada.Faces.Count);

            rostos
                .AddRange(analiseRealizada
                    .Faces
                    .Select(f => $"Possivelmente {("female".Equals(f.Gender.ToString().ToLower()) ? "mulher" : "homem")} com {f.Age} anos."));

            return rostos;
        }

        public string RetornarConteudoAdultoOuSensivel(ImageAnalysis analiseRealizada) =>
            $"{(analiseRealizada.Adult.IsAdultContent ? "Possui" : "Não possui")} conteúdo adulto ({analiseRealizada.Adult.AdultScore * 100:n2}% de certeza). {(analiseRealizada.Adult.IsRacyContent ? "Possui" : "Não possui")} conteúdo sensível ({analiseRealizada.Adult.RacyScore * 100:n2}% de certeza).";

        public string RetornarEsquemaDeCores(ImageAnalysis analiseRealizada) =>
            $"A imagem {(analiseRealizada.Color.IsBWImg ? "é" : "não é")} em preto e branco e as cores dominantes são: {string.Join(", ", analiseRealizada.Color.DominantColors)}";

        public ICollection<string> RetornarCelebridades(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Categories.Count == 0)
                return new List<string> { "Não foi possível encontrar celebridades na imagem." };

            var celebridades = new List<string>();

            foreach (var categoria in analiseRealizada.Categories)
            {
                if (categoria.Detail?.Celebrities is null || categoria.Detail?.Celebrities.Count == 0)
                    continue;

                celebridades.AddRange(
                    categoria
                        .Detail
                        .Celebrities
                        .Select(c => $"{c.Name} ({c.Confidence * 100:n2}% de certeza)"));
            }

            return celebridades.Count == 0
                ? new List<string> { "Não foi possível encontrar celebridades na imagem." }
                : celebridades;
        }

        public ICollection<string> RetornarPontosDeReferencia(ImageAnalysis analiseRealizada)
        {
            if (analiseRealizada.Categories.Count == 0)
                return new List<string> { "Não foi possível encontrar pontos de referência na imagem." };

            var pontosDeReferencia = new List<string>();

            foreach (var categoria in analiseRealizada.Categories)
            {
                if (categoria.Detail?.Landmarks is null || categoria.Detail?.Landmarks.Count == 0)
                    continue;

                pontosDeReferencia.AddRange(
                    categoria
                        .Detail
                        .Landmarks
                        .Select(l => $"{l.Name} ({l.Confidence * 100:n2}% de certeza)"));
            }

            return pontosDeReferencia.Count == 0
                ? new List<string> { "Não foi possível encontrar pontos de referência na imagem." }
                : pontosDeReferencia;
        }
    }
}
