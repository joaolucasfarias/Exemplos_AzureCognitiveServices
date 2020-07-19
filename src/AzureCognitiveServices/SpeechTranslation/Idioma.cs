using System.Collections.Generic;

namespace SpeechTranslation
{
    public class Idioma
    {
        private Idioma(string nome, string codigo)
        {
            Nome = nome;
            Codigo = codigo;
        }

        public string Nome { get; private set; }

        public string Codigo { get; private set; }

        public static IEnumerable<Idioma> IdiomasDisponiveis() =>
            new List<Idioma>(10)
            {
                new Idioma("Português (Brasil)", "pt-BR"),
                new Idioma("Inglês (EUA)", "en-US"),
                new Idioma("Espanhol", "es-ES"),
                new Idioma("Finlandês", "fi-FI"),
                new Idioma("Francês", "fr-FR"),
                new Idioma("Italiano", "IT-IT"),
                new Idioma("Japonês", "ja-JP"),
                new Idioma("Holandês", "nl-NL"),
                new Idioma("Russo", "ru-RU"),
                new Idioma("Chinês", "zh-CN"),
            };
    }
}
