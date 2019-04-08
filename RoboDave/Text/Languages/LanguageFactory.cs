using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Text.Languages
{
    internal interface ILanguageHelper 
    {
        String EncodeToStringReorder(String[] parts, int[] order);
        void ParseStringReorder(String input, out String[] parts, out int[] order);
    }

    internal static class LanguageFactory
    {
        private static Dictionary<Language, ILanguageHelper> book;

        static LanguageFactory()
        {
            book = new Dictionary<Language, ILanguageHelper>();
            book.Add(Language.Powershell, new PowershellHelper());
        }

        public static string EncodeToStringReorder(Language language, String[] parts, int[] order)
        {
            return book[language].EncodeToStringReorder(parts, order);
        }
        public static void ParseStringReorder(Language language, String input, out String[] parts, out int[] order)
        {
            book[language].ParseStringReorder(input, out parts, out order);
        }
    }
}
