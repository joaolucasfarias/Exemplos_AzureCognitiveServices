using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            foreach (var caption in analiseRealizada.Description.Captions)
            {
                Console.WriteLine($"{caption.Text} with confidence {caption.Confidence}");
            }
            return new List<string>(0);
        }

        public ICollection<string> RetornarCategorias(ImageAnalysis analiseRealizada)
        {
            foreach (var category in analiseRealizada.Categories)
            {
                Console.WriteLine($"{category.Name} with confidence {category.Score}");
            }
            return new List<string>(0);
        }

        public ICollection<string> RetornarTags(ImageAnalysis analiseRealizada)
        {
            foreach (var tag in analiseRealizada.Tags)
            {
                Console.WriteLine($"{tag.Name} {tag.Confidence}");
            }
            return new List<string>(0);
        }

        public ICollection<string> RetornarLogos(ImageAnalysis analiseRealizada)
        {
            foreach (var brand in analiseRealizada.Brands)
            {
                Console.WriteLine($"Logo of {brand.Name} with confidence {brand.Confidence} at location {brand.Rectangle.X}, " +
                  $"{brand.Rectangle.X + brand.Rectangle.W}, {brand.Rectangle.Y}, {brand.Rectangle.Y + brand.Rectangle.H}");
            }
            return new List<string>(0);
        }

        public ICollection<string> RetornarRostos(ImageAnalysis analiseRealizada)
        {
            foreach (var face in analiseRealizada.Faces)
            {
                Console.WriteLine($"A {face.Gender} of age {face.Age} at location {face.FaceRectangle.Left}, " +
                  $"{face.FaceRectangle.Left}, {face.FaceRectangle.Top + face.FaceRectangle.Width}, " +
                  $"{face.FaceRectangle.Top + face.FaceRectangle.Height}");
            }
            return new List<string>(0);
        }

        public ICollection<string> RetornarConteudoAdultoOuSensivel(ImageAnalysis analiseRealizada)
        {
            Console.WriteLine($"Has adult content: {analiseRealizada.Adult.IsAdultContent} with confidence {analiseRealizada.Adult.AdultScore}");
            Console.WriteLine($"Has racy content: {analiseRealizada.Adult.IsRacyContent} with confidence {analiseRealizada.Adult.RacyScore}");
            return new List<string>(0);
        }

        public ICollection<string> RetornarEsquemaDeCores(ImageAnalysis analiseRealizada)
        {
            Console.WriteLine("Color Scheme:");
            Console.WriteLine("Is black and white?: " + analiseRealizada.Color.IsBWImg);
            Console.WriteLine("Accent color: " + analiseRealizada.Color.AccentColor);
            Console.WriteLine("Dominant background color: " + analiseRealizada.Color.DominantColorBackground);
            Console.WriteLine("Dominant foreground color: " + analiseRealizada.Color.DominantColorForeground);
            Console.WriteLine("Dominant colors: " + string.Join(",", analiseRealizada.Color.DominantColors));
            return new List<string>(0);
        }

        public ICollection<string> RetornarCelebridades(ImageAnalysis analiseRealizada)
        {
            foreach (var category in analiseRealizada.Categories)
            {
                if (category.Detail?.Celebrities != null)
                {
                    foreach (var celeb in category.Detail.Celebrities)
                    {
                        Console.WriteLine($"{celeb.Name} with confidence {celeb.Confidence} at location {celeb.FaceRectangle.Left}, " +
                          $"{celeb.FaceRectangle.Top}, {celeb.FaceRectangle.Height}, {celeb.FaceRectangle.Width}");
                    }
                }
            }
            return new List<string>(0);
        }

        public ICollection<string> RetornarPontosDeReferencia(ImageAnalysis analiseRealizada)
        {
            foreach (var category in analiseRealizada.Categories)
            {
                if (category.Detail?.Landmarks != null)
                {
                    foreach (var landmark in category.Detail.Landmarks)
                    {
                        Console.WriteLine($"{landmark.Name} with confidence {landmark.Confidence}");
                    }
                }
            }
            return new List<string>(0);
        }
    }
}
