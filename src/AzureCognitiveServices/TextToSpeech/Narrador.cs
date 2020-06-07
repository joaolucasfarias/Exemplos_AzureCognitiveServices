using System.Collections.Generic;

namespace TextToSpeech
{
    public class Narrador
    {
        private Narrador(string nome, string codigo, string pais)
        {
            Nome = nome;
            Codigo = codigo;
            Pais = pais;
        }

        public string Nome { get; private set; }

        public string Codigo { get; private set; }

        public string Pais { get; private set; }

        public static IEnumerable<Narrador> NarradoresDisponiveis() =>
            new List<Narrador>(20)
            {
                new Narrador("Heloísa", "pt-BR-HeloisaRUS", "Brasil"),
                new Narrador("Daniel", "pt-BR-Daniel-Apollo", "Brasil"),
                new Narrador("Zira", "en-US-ZiraRUS", "EUA"),
                new Narrador("Aria", "en-US-AriaRUS", "EUA"),
                new Narrador("Benjamin", "en-US-BenjaminRUS", "EUA"),
                new Narrador("Laura", "es-ES-Laura-Apollo", "Espanha"),
                new Narrador("Helena", "es-ES-HelenaRUS", "Espanha"),
                new Narrador("Heidi", "fi-FI-HeidiRUS", "Finlândia"),
                new Narrador("Julie", "fr-FR-Julie-Apollo", "França"),
                new Narrador("Hortense", "fr-FR-HortenseRUS", "França"),
                new Narrador("Paul", "fr-FR-Paul-Apollo", "França"),
                new Narrador("Cosimo", "IT-IT-Cosimo-Apollo", "Itália"),
                new Narrador("Lucia", "IT-IT-LuciaRUS", "Itália"),
                new Narrador("Ayumi", "ja-JP-Ayumi-Apollo", "Japão"),
                new Narrador("Ichiro", "ja-JP-Ichiro-Apollo", "Japão"),
                new Narrador("Hanna", "nl-NL-HannaRUS", "Holanda"),
                new Narrador("Irina", "ru-RU-Irina-Apollo", "Rússia"),
                new Narrador("Pavel", "ru-RU-Pavel-Apollo", "Rússia"),
                new Narrador("Huihui", "zh-CN-HuihuiRUS", "China"),
                new Narrador("Kangkang", "zh-CN-Kangkang-Apollo", "China")
            };
    }
}
