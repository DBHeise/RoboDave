
namespace RoboDave.Random
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    /// <summary>
    /// Types of Strings
    /// </summary>
    public enum StringType
    {
        Random,
        AaZz,
        AlphaNumeric,
        ASCII,
        ANSI,
        Hex,
        Unicode,
        Digits,
        Name,
        Word,
        Sentence,
        Email,
        EmailSimple,
        Domain,
        TLD,
        LowerCase,
        UpperCase,
        Uri,
        IPAddress,
        IPv4,
        IPv6
    }

    /// <summary>
    /// Generates a String
    /// </summary>
    public static class StringGenerator
    {
        public static String GetString(StringType type)
        {
            ulong length = UInt64.Parse(Rando.Generate<Byte>().ToString());
            return GetString(type, length);
        }
        public static String GetString(StringType type, uint length)
        {
            return GetString(type, UInt64.Parse(length.ToString()));
        }
        public static String GetString(StringType type, ulong length)
        {
            String ans = null;
            String charset = null;
            Boolean useMadLib = false;
            switch (type)
            {
                case StringType.AaZz:
                case StringType.Digits:
                case StringType.AlphaNumeric:
                case StringType.ANSI:
                case StringType.ASCII:
                case StringType.Hex:
                case StringType.UpperCase:
                case StringType.LowerCase:
                    charset = GetCharSet(type);
                    break;
                case StringType.Unicode:
                case StringType.Random:
                    ans = GetUnicodeString(length);
                    break;
                case StringType.EmailSimple:
                    ans = GetString(StringType.Name, length).Replace(' ', '.') + "@" + GetString(StringType.Domain, length);
                    ans = ans.ToLowerInvariant();
                    break;
                case StringType.Email:
                    ans = GetString(StringType.ASCII, length) + "@" + GetString(StringType.Domain, length);
                    break;
                case StringType.Domain:
                    String tld = "." + GetString(StringType.TLD);
                    ulong l = (Int64)length < (Int64)tld.Length ? 3 : length - (ulong)tld.Length;
                    if (l < 3) l = 3;
                    ans = GetString(StringType.AaZz, l) + tld;
                    ans = ans.ConvertWhitespaceToSpaces().Replace(" ", "");
                    break;
                case StringType.TLD:
                    ans = GetTLD();
                    break;
                case StringType.Name:
                    useMadLib = true;
                    charset = Rando.RandomBoolean() ? "[boyname]" : "[girlname]";
                    charset += " [lastname]";
                    break;
                case StringType.Word:
                    useMadLib = true;
                    charset = "[top5000]";
                    break;
                case StringType.Sentence:
                    useMadLib = true;
                    charset = GetSimpleSentenceStructure();
                    break;
                case StringType.Uri:
                    ans = GenerateUri();
                    break;
                case StringType.IPAddress:
                    if (Rando.RandomBoolean())
                    {
                        ans = GetString(StringType.IPv4);
                    }
                    else
                    {
                        ans = GetString(StringType.IPv6);
                    }
                    break;
                case StringType.IPv4:
                    ans = (new System.Net.IPAddress((long)Rando.RandomInt(0, int.MaxValue))).ToString();
                    break;
                case StringType.IPv6:
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < 8; i++)
                        {
                            if (i != 0)
                                sb.Append(":");
                            
                            if (!Rando.RandomBoolean(10))
                                sb.Append(GetString(StringType.Hex, 4));
                        }

                        ans = sb.ToString();
                    }
                    break;
            }
            if (useMadLib)
            {
                var m = new MadLib();
                ans = m.Generate(charset);
            }
            else if (!String.IsNullOrEmpty(charset))
            {
                StringBuilder sb = new StringBuilder();
                for (ulong i = 0; i < length; i++)
                    sb.Append(Rando.RandomPick(charset));
                ans = sb.ToString();
            }

            return ans;
        }

        private static String GetTLD()
        {
            #region TLDS
            String[] availableTlds = new String[] { "AC", "AD", "AE", "AERO", "AF", "AG", "AI", "AL", "AM", "AN", "AO", "AQ", "AR", "ARPA", "AS", "ASIA", "AT", "AU", "AW", "AX", "AZ", "BA", "BB", "BD", "BE", "BF", "BG", "BH", "BI", "BIZ", "BJ", "BM", "BN", "BO", "BR", "BS", "BT", "BV", "BW", "BY", "BZ", "CA", "CAT", "CC", "CD", "CF", "CG", "CH", "CI", "CK", "CL", "CM", "CN", "CO", "COM", "COOP", "CR", "CU", "CV", "CW", "CX", "CY", "CZ", "DE", "DJ", "DK", "DM", "DO", "DZ", "EC", "EDU", "EE", "EG", "ER", "ES", "ET", "EU", "FI", "FJ", "FK", "FM", "FO", "FR", "GA", "GB", "GD", "GE", "GF", "GG", "GH", "GI", "GL", "GM", "GN", "GOV", "GP", "GQ", "GR", "GS", "GT", "GU", "GW", "GY", "HK", "HM", "HN", "HR", "HT", "HU", "ID", "IE", "IL", "IM", "IN", "INFO", "INT", "IO", "IQ", "IR", "IS", "IT", "JE", "JM", "JO", "JOBS", "JP", "KE", "KG", "KH", "KI", "KM", "KN", "KP", "KR", "KW", "KY", "KZ", "LA", "LB", "LC", "LI", "LK", "LR", "LS", "LT", "LU", "LV", "LY", "MA", "MC", "MD", "ME", "MG", "MH", "MIL", "MK", "ML", "MM", "MN", "MO", "MOBI", "MP", "MQ", "MR", "MS", "MT", "MU", "MUSEUM", "MV", "MW", "MX", "MY", "MZ", "NA", "NAME", "NC", "NE", "NET", "NF", "NG", "NI", "NL", "NO", "NP", "NR", "NU", "NZ", "OM", "ORG", "PA", "PE", "PF", "PG", "PH", "PK", "PL", "PM", "PN", "POST", "PR", "PRO", "PS", "PT", "PW", "PY", "QA", "RE", "RO", "RS", "RU", "RW", "SA", "SB", "SC", "SD", "SE", "SG", "SH", "SI", "SJ", "SK", "SL", "SM", "SN", "SO", "SR", "ST", "SU", "SV", "SX", "SY", "SZ", "TC", "TD", "TEL", "TF", "TG", "TH", "TJ", "TK", "TL", "TM", "TN", "TO", "TP", "TR", "TRAVEL", "TT", "TV", "TW", "TZ", "UA", "UG", "UK", "US", "UY", "UZ", "VA", "VC", "VE", "VG", "VI", "VN", "VU", "WF", "WS", "XN--0ZWM56D", "XN--11B5BS3A9AJ6G", "XN--3E0B707E", "XN--45BRJ9C", "XN--80AKHBYKNJ4F", "XN--80AO21A", "XN--90A3AC", "XN--9T4B11YI5A", "XN--CLCHC0EA0B2G2A9GCD", "XN--DEBA0AD", "XN--FIQS8S", "XN--FIQZ9S", "XN--FPCRJ9C3D", "XN--FZC2C9E2C", "XN--G6W251D", "XN--GECRJ9C", "XN--H2BRJ9C", "XN--HGBK6AJ7F53BBA", "XN--HLCJ6AYA9ESC7A", "XN--J1AMH", "XN--J6W193G", "XN--JXALPDLP", "XN--KGBECHTV", "XN--KPRW13D", "XN--KPRY57D", "XN--LGBBAT1AD8J", "XN--MGB9AWBF", "XN--MGBAAM7A8H", "XN--MGBAYH7GPA", "XN--MGBBH1A71E", "XN--MGBC0A9AZCG", "XN--MGBERP4A5D4AR", "XN--MGBX4CD0AB", "XN--O3CW4H", "XN--OGBPF8FL", "XN--P1AI", "XN--PGBS0DH", "XN--S9BRJ9C", "XN--WGBH1C", "XN--WGBL6A", "XN--XKC2AL3HYE2A", "XN--XKC2DL3A5EE0H", "XN--YFRO4I67O", "XN--YGBI2AMMX", "XN--ZCKZAH", "XXX", "YE", "YT", "ZA", "ZM", "ZW" };
            #endregion

            return Rando.RandomPick(availableTlds);
        }

        private static String GetSimpleSentenceStructure()
        {
            String[] simpleSentences = new String[] {
                "The [ADJECTIVE] [NOUN] [VERB_PRESENTTENSE] [ADVERB].",
                "The [ADJECTIVE-1] [OCCUPATION-2] [VERB_PASTTENSE-3] a [OBJECT-4] [ADVERB-5] the [OBJECT-6] [PLACE-7] for the [OCCUPATION-8].",
                "The [COLOR-1] [ANIMAL-2] went to the [ADJECTIVE-3] [PLACE-4].",
                "﻿The [PERSON] [VERB_PRESENTTENSE] [ADVERB].",
                "[RELATION-1] [VERB_PASTTENSE-2] [ADVERB-3].",
                "Of course, no man is [ADVERB-1] [VERB-2] his right [OBJECT-3] at any time.",
                "[ADJECTIVE-1] to [VERB_PRESENTTENSE-2] and [ADJECTIVE-1] to [VERB_PRESENTTENSE-5] makes a [PERSON-6] [ADJECTIVE-3] and [ADJECTIVE-4] and [ADJECTIVE-6].",
                "I'd rather [VERB_PRESENTTENSE-2] a [NOUN-3] than a [NOUN-4].",
                "[VERB_PRESENTTENSE-1] nothing.",
                "[VERB_PRESENTTENSE-2] frugally [ADVERB-3] surprise.",
                "I was [VERB_PROGRESSIVE-1] my powder-[COLOR-2] [VERB_PRESENTTENSE-3], with dark [COLOR-2] [THING-4], [THING-5] and display [THING-6], [COLOR-3] [THING-7], [COLOR-3] [ADJECTIVE-8] [THING-9] with dark [COLOR-2] [THING] [ADVERB-3] them.",
                "They [VERB_PASTTENSE-1] the six cabinet ministers at [ADJECTIVE-2]-past six [ADVERB-3] the morning against the [PLACE-4] of a [PLACE-5].",
                "[ADVERB-1] were pools of [FOOD-2] [ADVERB-3] the courtyard.",
                "[ADVERB-1] were [ADJECTIVE-2] dead leaves [ADVERB-3] the paving of the courtyard.",
                "It [VERB_PASTTENSE-2] [ADVERB-1].",
                "All the [THING-4] of the [PLACE-3] were [VERB_PASTTENSE-2] [VERB_PRESENTTENSE-1].",
                "Two [OCCUPATION-4] [VERB_PASTTENSE-1] him [ADVERB-2] and [ADVERB-3] into the rain.",
            };
            return Rando.RandomPick(simpleSentences);
        }

        private static String GetCharSet(StringType type)
        {
            StringBuilder charset = new StringBuilder();
            switch (type)
            {
                case StringType.LowerCase:
                    charset.Append("abcdefghijklmnopqrstuvwxyz");
                    break;
                case StringType.UpperCase:
                    charset.Append("﻿ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    break;
                case StringType.Digits:
                    charset.Append("0123456789");
                    break;
                case StringType.AaZz:
                    charset.Append(GetCharSet(StringType.LowerCase));
                    charset.Append(GetCharSet(StringType.UpperCase));
                    break;
                case StringType.AlphaNumeric:
                    charset.Append(GetCharSet(StringType.AaZz));
                    charset.Append(GetCharSet(StringType.Digits));
                    break;
                case StringType.Hex:
                    charset.Append(GetCharSet(StringType.Digits));
                    charset.Append("ABCDEF");
                    break;
                case StringType.ASCII:
                    for (byte i = 0; i < 128; i++)
                        charset.Append((char)i);
                    break;
                case StringType.ANSI:
                    for (byte i = 0; i < byte.MaxValue; i++)
                        charset.Append((char)i);
                    break;
                case StringType.Unicode:
                case StringType.Random:
                case StringType.Name:
                case StringType.Word:
                case StringType.Sentence:
                default:
                    break;
            }
            return charset.ToString();
        }

        private static String GetUnicodeString(ulong length)
        {
            StringBuilder str = new StringBuilder();
            while (((ulong)str.Length) < length)
            {
                str.Append(BitConverter.ToChar(Rando.GetBytes(2), 0));
            }
            return str.ToString();
        }

        private static String GenerateValidUri()
        {
            Uri u = null;
            while (!Uri.TryCreate(GenerateUri(), UriKind.RelativeOrAbsolute, out u)) { }
            return u.ToString();
        }

        private static String GenerateUri()
        {
            //<scheme name> : <hierarchical part> [ ? <query> ] [ # <fragment> ]
            StringBuilder sb = new StringBuilder();
            #region Schemes
            //Taken from perm list at http://www.iana.org/assignments/uri-schemes/uri-schemes.xhtml on 2014-09-17 (last updated: 2014-09-16)
            String[] schemes = new String[] { "aaa", "aaas", "about", "acap", "acct", "cap", "cid", "coap", "coaps", "crid", "data", "dav", "dict", "dns", "file", "ftp", "geo", "go", "gopher", "h323", "http", "https", "iax", "icap", "im", "imap", "info", "ipp", "iris", "iris.beep", "iris.xpc", "iris.xpcs", "iris.lwz", "jabber", "ldap", "mailto", "mid", "msrp", "msrps", "mtqp", "mupdate", "news", "nfs", "ni", "nih", "nntp", "opaquelocktoken", "pop", "pres", "reload", "rtsp", "rtsps", "rtspu", "service", "session", "shttp", "sieve", "sip", "sips", "sms", "snmp", "soap.beep", "soap.beeps", "stun", "stuns", "tag", "tel", "telnet", "tftp", "thismessage", "tn3270", "tip", "turn", "turns", "tv", "urn", "vemmi", "ws", "wss", "xcon", "xcon-userid", "xmlrpc.beep", "xmlrpc.beeps", "xmpp", "z39.50r", "z39.50s" };
            #endregion
            sb.Append(Rando.RandomPick(schemes));
            sb.Append(@"://");

            if (Rando.RandomInt(0, 20) % 20 == 0) //1 in 20 chance
            { // UserInfo
                sb.Append(GetString(StringType.Word));
                sb.Append(":");
                sb.Append(GetString(StringType.Word));
            }

            if (Rando.RandomInt(0, 5) % 5 == 0) //1 in 5 chance
            { //Subdomain
                int num = Rando.RandomInt(1, 5);
                for (int i = 0; i < num; i++)
                {
                    sb.Append(GetString(StringType.Word));
                    sb.Append(".");
                }
            }

            //Domain
            sb.Append(StringGenerator.GetString(StringType.Word));
            sb.Append(".");
            sb.Append(StringGenerator.GetString(StringType.TLD));


            if (Rando.RandomBoolean())
            { // Path
                int num = Rando.RandomInt(1, 5);
                for (int i = 0; i < num; i++)
                {
                    sb.Append(@"/");
                    sb.Append(GetString(StringType.Word));
                }

            }

            if (Rando.RandomBoolean())
            { // File
                sb.Append(GetString(StringType.Word));
                sb.Append(".");
                sb.Append(GetString(StringType.AlphaNumeric, (ulong)Rando.RandomInt(3, 5)));
            }

            if (Rando.RandomInt(0, 3) % 3 == 0) //1 in 3 chance
            { // Query
                int num = Rando.RandomInt(1, 10);
                sb.Append("?");
                for (int i = 0; i < num; i++)
                {
                    if (i > 0)
                        sb.Append("&");

                    sb.Append(GetString(StringType.Word));
                    sb.Append("=");
                    sb.Append(GetString(StringType.Word));
                }
            }

            if (Rando.RandomInt(0, 5) % 5 == 0) //1 in 5 chance
            { // Fragment
                sb.Append("#");
                sb.Append(GetString(StringType.Word));
            }


            return sb.ToString();
        }
    }
}
