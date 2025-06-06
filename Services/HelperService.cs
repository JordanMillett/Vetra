using Vetra;
using System.Security.Cryptography;
using System.Text;
using Microsoft.JSInterop;

namespace Vetra
{
    public class HelperService
    {
        IJSRuntime Runtime;
        SettingsService Settings;

        public HelperService(IJSRuntime JS, SettingsService S)
        {
            Runtime = JS;
            Settings = S;
        }

        public async Task<string> Hash(string input)
        {
            return await Runtime.InvokeAsync<string>("md5Hash", input);
        }

        Dictionary<char, string> Map = new Dictionary<char, string>
        {
            {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"}, {'е', "e"},
            {'ё', "yo"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"}, {'й', "yi"}, {'к', "k"},
            {'л', "l"}, {'м', "m"}, {'н', "n"}, {'о', "o"}, {'п', "p"}, {'р', "r"},
            {'с', "s"}, {'т', "t"}, {'у', "oo"}, {'ф', "f"}, {'х', "h"}, {'ц', "ts"},
            {'ч', "ch"}, {'ш', "sh"}, {'щ', "sh"}, {'ъ', ""}, {'ы', "e"}, {'ь', ""},
            {'э', "e"}, {'ю', "yu"}, {'я', "ya"}
        };

        public string DefinitionFormat(VocabHeader Term, string transliterationOverride = "")
        {
            if (Settings.Data.ShowTransliteration)
            {
                if(string.IsNullOrEmpty(transliterationOverride))
                    return "(" + Term.EN + " / " + Transliterate(Term.RU) + ")";
                else
                    return "(" + Term.EN + " / " + transliterationOverride + ")";
            }
            else
            {
                return "(" + Term.EN + ")";
            }
        }

        public string Transliterate(string RU)
        {
            string output = "";
            foreach (char Letter in RU.ToLower())
            {
                output += Map.TryGetValue(Letter, out string? Found) ? Found : Letter.ToString();
            }

            return CapitalizedCase(output);
        }

        public string CapitalizedCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            bool isNumber = int.TryParse(input, out int number);
            string output = input;

            if (isNumber)
            {
                if (RussianNumbers.ContainsKey(number))
                    output = RussianNumbers[number];
            }

            output = output.ToLower();
            output = output.Replace("ё", "е");
            output = char.ToUpper(output[0]) + output.Substring(1).ToLower();
            return output;
        }

        public string ConvertFromURL(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return string.Join(' ', input.Replace('+', ' ').Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }

        Dictionary<int, string> RussianNumbers = new Dictionary<int, string>
        {
            { 0, "ноль" },
            { 1, "один" },
            { 2, "два" },
            { 3, "три" },
            { 4, "четыре" },
            { 5, "пять" },
            { 6, "шесть" },
            { 7, "семь" },
            { 8, "восемь" },
            { 9, "девять" },
            { 10, "десять" }
        };
    }
}