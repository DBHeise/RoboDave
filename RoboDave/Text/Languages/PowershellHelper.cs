using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Text.Languages
{
    class PowershellHelper : ILanguageHelper
    {
        public String Unescape(String input)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                Char c = input[i];
                if (c == '`')
                {
                    //?
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public String Escape(String input, String charsToEscape)
        {
            Char[] chars = charsToEscape.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                Char c = input[i];
                if (chars.Contains(c))
                {
                    sb.Append("`");
                }
                sb.Append(c);

            }
            return sb.ToString();

        }

        public String EncodeToStringReorder(String[] parts, int[] order)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append('"');
            for (int i = 0; i < order.Length; i++)
            {
                sb.Append("{" + order[i] + "}");
            }
            sb.Append("\"-f");
            for (int i = 0; i < parts.Length; i++)
            {
                sb.Append("'" + Escape(parts[i], "'") + "',");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        public void ParseStringReorder(string input, out string[] parts, out int[] order)
        {
            //Assuming a format {list of order}[-f|-format]{array of parts}
            int formatIndex = input.IndexOf("-f");
            order = ParseOrder(input.Substring(0, formatIndex - 1));
            parts = ParseStringList(input.Substring(formatIndex));
        }

        private int[] ParseOrder(String input)
        {
            List<int> order = new List<int>();
            String digit = "";
            bool isInBracket = false;
            for (int i = 0; i < input.Length; i++)
            {
                Char c = input[i];
                if (c == '{')
                {
                    isInBracket = true;
                }
                else if (c == '}')
                {
                    isInBracket = false;
                    if (!String.IsNullOrWhiteSpace(digit))
                    {
                        order.Add(int.Parse(digit));
                        digit = "";
                    }
                }
                else if (Char.IsDigit(c))
                {
                    if (isInBracket)
                    {
                        digit += c;
                    }
                }
            }
            return order.ToArray();
        }

        private String[] ParseStringList(String input)
        {
            List<String> list = new List<String>();
            int index = input.IndexOfAny("\"'".ToCharArray());
            for (; index < input.Length; index++)
            {
                Char c = input[index];
                if (c == '\'' || c == '"')
                {
                    index++;
                    StringBuilder sb = new StringBuilder();
                    while (index < input.Length)
                    {
                        if (input[index] == c && input[index - 1] != '`')
                        {
                            break;
                        }
                        sb.Append(input[index]);
                        index++;
                    }
                    list.Add(Unescape(sb.ToString()));
                }
            }
            return list.ToArray();
        }
    }
}
