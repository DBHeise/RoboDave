using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RoboDave.Random
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MadLib
    {
        private static Dictionary<String, String[]> replacements;
        private static Dictionary<String, String> map;


        static MadLib()
        {
            MadLib.Initialize();
        }

        static String GetRandomItem(String key)
        {
            String ans = null;
            String modifer = null;
            if (key.Contains(":"))
            {
                String[] parts = key.SplitAtFirst(':');
                key = parts[0];
                modifer = parts[1];
            }

            if (replacements.ContainsKey(key))
            {
                int idx = Rando.RandomInt(0, replacements[key].Length);
                ans = replacements[key][idx];
                if (ans.StartsWith("[") && ans.EndsWith("]"))
                    ans = GetRandomItem(ans.Substring(1, ans.Length - 2).ToUpperInvariant());
            }
            if (!String.IsNullOrWhiteSpace(modifer))
            {
                switch(modifer.ToUpperInvariant())
                {
                    case "TITLECASE":
                        ans = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ans);
                        break;
                    case "UPPERCASE":
                        ans = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToUpper(ans);
                        break;
                    case "LOWERCASE":
                        ans = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToLower(ans);
                        break;
                }
            }
            return ans;
        }

        private static void Initialize()
        {
            MadLib.replacements = new Dictionary<String, String[]>();
            MadLib.map = new Dictionary<String, String>();

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RoboDave.Random.MadLib.xml"))
            using (XmlReader reader = XmlReader.Create(stream))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Replacement")
                    {
                        String name = reader.GetAttribute("Name").ToUpperInvariant();
                        String include = reader.GetAttribute("IncludeInMap");

                        String[] items = reader.ReadElementContentAsString().Split('|');

                        MadLib.replacements.Add(name, items);

                        if (String.IsNullOrEmpty(include) || String.Compare(include, "false", 0) != 0)
                        {
                            foreach (String item in items)
                            {
                                String i = item.ToLowerInvariant();
                                if (!MadLib.map.ContainsKey(i))
                                    MadLib.map.Add(i, name);
                            }
                        }
                    }
                }
            }
        }

        public MadLib()
        {
        }

        public String GenerateMadLib(FileInfo file)
        {
            return GenerateMadLib(File.ReadAllText(file.FullName));
        }
        public String GenerateMadLib(String content)
        {
            StringTokenizer tokenizer = new StringTokenizer(content);
            StringBuilder output = new StringBuilder();
            Dictionary<String, uint> internalMap = new Dictionary<string, uint>();
            uint idx = 0;
            foreach (StringToken token in tokenizer)
            {
                if (token.Type == StringTokenType.Word)
                {
                    String t = token.String.ToLowerInvariant();
                    if (MadLib.map.ContainsKey(t))
                    {
                        if (internalMap.ContainsKey(t))
                        {
                            idx = internalMap[t];
                        }
                        else
                        {
                            idx++;
                            internalMap.Add(t, idx);
                        }

                        output.Append("[" + MadLib.map[t] + "-" + idx + "]");
                    }
                    else
                        output.Append(token.String);
                }
                else
                    output.Append(token.String);
            }
            return output.ToString();
        }

        public String Generate(FileInfo file)
        {
            return Generate(File.ReadAllText(file.FullName));
        }
        public String Generate(String content)
        {
            StringTokenizer tokenizer = new StringTokenizer(content);
            StringBuilder output = new StringBuilder();
            Dictionary<String, String> localReplacements = new Dictionary<String, String>();

            Boolean inReplacement = false;
            Boolean inIndex = false;
            String replacer = null;
            String index = null;
            foreach (StringToken token in tokenizer)
            {
                if (token.Type == StringTokenType.Punctuation && token.String == "[")
                    inReplacement = true;
                else if (token.Type == StringTokenType.Punctuation && token.String == "]")
                {
                    if (String.IsNullOrEmpty(index))
                        output.Append(MadLib.GetRandomItem(replacer));
                    else
                    {
                        String key = replacer + "-" + index;
                        if (!localReplacements.ContainsKey(key))
                            localReplacements.Add(key, MadLib.GetRandomItem(replacer));
                        output.Append(localReplacements[key]);
                    }

                    replacer = null;
                    inReplacement = false;
                    inIndex = false;
                    index = null;
                }
                else if (inReplacement)
                {
                    if (inIndex)
                    {
                        index += token.String.ToUpperInvariant();
                    }
                    else
                    {
                        if (token.Type != StringTokenType.Punctuation || token.String == "_" || token.String == ":")
                            replacer += token.String.ToUpperInvariant();
                        else if (token.String == "-")
                            inIndex = true;                        
                    }
                }
                else
                    output.Append(token.String);
            }

            return output.ToString();
        }

    }
}
