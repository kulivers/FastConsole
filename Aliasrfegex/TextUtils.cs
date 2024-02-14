using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Aliasrfegex
{
    public static partial class TextUtils
    {
        private static readonly Dictionary<char, string> ReplaceTable = new Dictionary<char, string>
        {
            { 'А', "A" }, { 'а', "a" }, { 'Б', "B" }, { 'б', "b" }, { 'В', "V" }, { 'в', "v" }, { 'Г', "G" }, { 'г', "g" },
            { 'Д', "D" }, { 'д', "d" }, { 'Е', "E" }, { 'е', "e" }, { 'Ё', "Yo" }, { 'ё', "yo" }, { 'Ж', "Zh" }, { 'ж', "zh" },
            { 'З', "Z" }, { 'з', "z" }, { 'И', "I" }, { 'и', "i" }, { 'Й', "Y" }, { 'й', "y" }, { 'К', "K" }, { 'к', "k" },
            { 'Л', "L" }, { 'л', "l" }, { 'М', "M" }, { 'м', "m" }, { 'Н', "N" }, { 'н', "n" }, { 'О', "O" }, { 'о', "o" },
            { 'П', "P" }, { 'п', "p" }, { 'Р', "R" }, { 'р', "r" }, { 'С', "S" }, { 'с', "s" }, { 'Т', "T" }, { 'т', "t" },
            { 'У', "U" }, { 'у', "u" }, { 'Ф', "F" }, { 'ф', "f" }, { 'Х', "Kh" }, { 'х', "kh" }, { 'Ц', "Ts" }, { 'ц', "ts" },
            { 'Ч', "Ch" }, { 'ч', "ch" }, { 'Ш', "Sh" }, { 'ш', "sh" }, { 'Щ', "Sch" }, { 'щ', "sch" }, { 'Ъ', "\'" }, { 'ъ', "\'" },
            { 'Ы', "Y" }, { 'ы', "y" }, { 'Ь', "\'" }, { 'ь', "\'" }, { 'Э', "E" }, { 'э', "e" }, { 'Ю', "Yu" }, { 'ю', "yu" },
            { 'Я', "Ya" }, { 'я', "ya" }, { ' ', "_" }, { 'Ä', "A" }, { 'ä', "a" }, { 'É', "E" }, { 'é', "e" }, { 'Ö', "O" },
            { 'ö', "o" }, { 'Ü', "U" }, { 'ü', "u" }, { 'ß', "s" }
        };

        public static string Translit(string inputString)
        {
            var sb = new StringBuilder();
            foreach (var ch in inputString)
            {
                string str;
                if (ReplaceTable.TryGetValue(ch, out str))
                    sb.Append(str);
                else
                    sb.Append(ch);
            }

            return sb.ToString();
        }

        public static string Sanitize(string inputString, string sanitizeReplacement = "_")
        {
            return Regex.Replace(Translit(inputString), "[^a-zA-Z0-9\\.\\-]", sanitizeReplacement);
        }
    }
}