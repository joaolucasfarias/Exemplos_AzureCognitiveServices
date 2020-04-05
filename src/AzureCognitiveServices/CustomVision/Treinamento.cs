using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CustomVision
{
    public class Treinamento
    {
        private const string _chaveDeTreinamento = "<sua chave de treinamento>";
        private const string _endpoint = "<seu endpoint>";
        private const string _idDoRecursoDePredicao = "<id do recurso de predição>";

        private ICustomVisionTrainingClient _servicoCognitivoDeVisaoPersonalizadaTreinamento;

        public Treinamento() =>
            _servicoCognitivoDeVisaoPersonalizadaTreinamento = new CustomVisionTrainingClient()
            {
                ApiKey = _chaveDeTreinamento,
                Endpoint = _endpoint
            };

        public IEnumerable<Project> ListarProjetos() =>
            _servicoCognitivoDeVisaoPersonalizadaTreinamento.GetProjects();

        public void CriarProjeto(string nome, string descricao = "") =>
            _servicoCognitivoDeVisaoPersonalizadaTreinamento.CreateProject(nome, descricao);

        public IEnumerable<Tag> ListarTags(Project projeto) =>
            _servicoCognitivoDeVisaoPersonalizadaTreinamento.GetTags(projeto.Id);

        public void CriarTag(Project projeto, string nome, string descricao = "") =>
            _servicoCognitivoDeVisaoPersonalizadaTreinamento.CreateTag(projeto.Id, nome, descricao);

        public void AdicionarImagemPorUrl(Project projeto, string url, IEnumerable<Tag> tags)
        {
            var urls = new List<ImageUrlCreateEntry>(1)
            { new ImageUrlCreateEntry(url) };

            _servicoCognitivoDeVisaoPersonalizadaTreinamento.CreateImagesFromUrls(
                projeto.Id,
                new ImageUrlCreateBatch(urls, tags.Select(t => t.Id).ToList()));

            Treinar(projeto);
        }

        public void AdicionarImagemPorArquivo(Project projeto, string localDoArquivo, IEnumerable<Tag> tags)
        {
            var arquivo = new FileStream(localDoArquivo, FileMode.Open);

            _servicoCognitivoDeVisaoPersonalizadaTreinamento.CreateImagesFromData(
                projeto.Id,
                arquivo,
                tags.Select(t => t.Id).ToList());

            Treinar(projeto);
        }

        private void Treinar(Project projeto)
        {
            var treinamento = _servicoCognitivoDeVisaoPersonalizadaTreinamento.TrainProject(projeto.Id);

            while ("Training".Equals(treinamento.Status))
            {
                Thread.Sleep(1000);

                treinamento = _servicoCognitivoDeVisaoPersonalizadaTreinamento.GetIteration(projeto.Id, treinamento.Id);
            }

            EnviarResultadosParaPedicao(projeto, treinamento);
        }

        private void EnviarResultadosParaPedicao(Project projeto, Iteration treinamento) =>
            _servicoCognitivoDeVisaoPersonalizadaTreinamento.PublishIteration(projeto.Id, treinamento.Id, "treeClassModel", _idDoRecursoDePredicao);
    }
}
