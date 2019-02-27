using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Gets the txt that lies between these two strings
        /// </summary>
        public static string GetTxtBtwn(this String input, string start, string end, int startIndex)
        {
            return GetTxtBtwn(input, start, end, startIndex, false);
        }

        /// <summary>
        ///     Gets the txt that lies between these two strings
        /// </summary>
        public static string GetLastTxtBtwn(this String input, string start, string end, int startIndex)
        {
            return GetTxtBtwn(input, start, end, startIndex, true);
        }

        /// <summary>
        ///     Gets the txt that lies between these two strings
        /// </summary>
        private static string GetTxtBtwn(this String input, string start, string end, int startIndex, bool UseLastIndexOf)
        {
            int index1 = UseLastIndexOf
                ? input.LastIndexOf(start, startIndex)
                : input.IndexOf(start, startIndex);
            if (index1 == -1) return "";
            index1 += start.Length;
            int index2 = input.IndexOf(end, index1);
            if (index2 == -1) return input.Substring(index1);
            return input.Substring(index1, index2 - index1);
        }
        public static String ConvertWhitespaceToSpaces(this String value)
        {
            char[] arr = value.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (Char.IsWhiteSpace(arr[i]))
                    arr[i] = ' ';
            }
            return new string(arr);
        }

        public static String ToTitleCase(this String input)
        {
            return System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(input.ToLower());
        }

        public static String[] SplitAtFirst(this String input, char splitter)
        {
            int idx = input.IndexOf(splitter);
            if (idx < 0)
                return new String[] { input, null };
            else
                return new String[] {
                input.Substring(0, idx),
                    input.Substring(idx + 1)
            };
        }

        public static Char GetRandomChar(this String input)
        {
            return GetRandomItem<Char>(input.ToCharArray());
        }

        public static T GetRandomItem<T>(this T[] input)
        {
            int idx = Rando.RandomInt(0, input.Length);
            return input[idx];
        }

        public static String GetBase64Hash(this String input)
        {
            return GetBase64Hash(input, "MD5");
        }

        public static String GetBase64Hash(this String input, String hashalgorithm)
        {
            HashAlgorithm hasher = HashAlgorithm.Create(hashalgorithm);
            return Convert.ToBase64String(hasher.ComputeHash(Encoding.Default.GetBytes(input)));
        }

        public static String TrimBOM(this String input)
        {
            String tmp = input.TrimStart(Encoding.Unicode.GetString(Encoding.Unicode.GetPreamble()).ToCharArray());
            tmp = input.TrimStart(Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()).ToCharArray());
            return tmp;
        }

        public static String MakeSafeFileName(this String input, Char replacement)
        {
            String ans = input;
            Char[] badchars = "\\/:*?\"<>|".ToCharArray();
            for (int i = 0; i < badchars.Length; i++)
                ans = ans.Replace(badchars[i], replacement);
            return ans;
        }

        public static String MakeSafeFileName(this String input)
        {
            if (!String.IsNullOrWhiteSpace(input))
            {
                String ans = input;
                Char[] badchars = "\\/:*?\"<>|".ToCharArray();
                for (int i = 0; i < badchars.Length; i++)
                    ans = ans.Replace(Convert.ToString(badchars[i]), String.Empty);
                return ans;
            }
            else
                return input;
        }

    }

    /// <summary>
    /// Extensions for Statistics
    /// </summary>
    public static class StatisticsExtensions
    {
        public static IEnumerable<T> Coalesce<T>(this IEnumerable<T?> source) where T : struct
        {
            System.Diagnostics.Debug.Assert(source != null);
            return source.Where(x => x.HasValue).Select(x => (T)x);
        }
        public static double Variance(this IEnumerable<double> source)
        {
            double avg = source.Average();
            double d = source.Aggregate(0.0,
                         (total, next) => total += Math.Pow(next - avg, 2));
            return d / (source.Count() - 1);
        }
        public static double VarianceP(this IEnumerable<double> source)
        {
            double avg = source.Average();
            double d = source.Aggregate(0.0, (total, next) => total += Math.Pow(next - avg, 2));
            return d / source.Count();
        }
        public static double StandardDeviation(this IEnumerable<double> source)
        {
            return Math.Sqrt(source.Variance());
        }
        public static double StandardDeviationP(this IEnumerable<double> source)
        {
            return Math.Sqrt(source.VarianceP());
        }
        public static double Median(this IEnumerable<double> source)
        {
            var sortedList = from number in source
                             orderby number
                             select number;

            int count = sortedList.Count();
            int itemIndex = count / 2;
            if (count % 2 == 0) // Even number of items. 
                return (sortedList.ElementAt(itemIndex) +
                        sortedList.ElementAt(itemIndex - 1)) / 2;

            // Odd number of items. 
            return sortedList.ElementAt(itemIndex);
        }
        public static T? Mode<T>(this IEnumerable<T> source) where T : struct
        {
            var sortedList = from number in source
                             orderby number
                             select number;

            int count = 0;
            int max = 0;
            T current = default(T);
            T? mode = new T?();

            foreach (T next in sortedList)
            {
                if (current.Equals(next) == false)
                {
                    current = next;
                    count = 1;
                }
                else
                {
                    count++;
                }

                if (count > max)
                {
                    max = count;
                    mode = current;
                }
            }

            if (max > 1)
                return mode;

            return null;
        }
        public static double Range(this IEnumerable<double> source)
        {
            return source.Max() - source.Min();
        }
        public static double Covariance(this IEnumerable<double> source, IEnumerable<double> other)
        {
            int len = source.Count();

            double avgSource = source.Average();
            double avgOther = other.Average();
            double covariance = 0;

            for (int i = 0; i < len; i++)
                covariance += (source.ElementAt(i) - avgSource) * (other.ElementAt(i) - avgOther);

            return covariance / len;
        }
        public static double Pearson(this IEnumerable<double> source, IEnumerable<double> other)
        {
            return source.Covariance(other) / (source.StandardDeviationP() *
                                     other.StandardDeviationP());
        }


        public static double Variance(this IEnumerable<int> source)
        {
            double avg = source.Average();
            double d = source.Aggregate(0.0,
                         (total, next) => total += Math.Pow(next - avg, 2));
            return d / (source.Count() - 1);
        }
        public static double VarianceP(this IEnumerable<int> source)
        {
            double avg = source.Average();
            double d = source.Aggregate(0.0, (total, next) => total += Math.Pow(next - avg, 2));
            return d / source.Count();
        }
        public static double StandardDeviation(this IEnumerable<int> source)
        {
            return Math.Sqrt(source.Variance());
        }
        public static double StandardDeviationP(this IEnumerable<int> source)
        {
            return Math.Sqrt(source.VarianceP());
        }
        public static double Median(this IEnumerable<int> source)
        {
            var sortedList = from number in source
                             orderby number
                             select number;

            int count = sortedList.Count();
            int itemIndex = count / 2;
            if (count % 2 == 0) // Even number of items. 
                return (sortedList.ElementAt(itemIndex) +
                        sortedList.ElementAt(itemIndex - 1)) / 2;

            // Odd number of items. 
            return sortedList.ElementAt(itemIndex);
        }
        public static double Range(this IEnumerable<int> source)
        {
            return source.Max() - source.Min();
        }
        public static double Covariance(this IEnumerable<int> source, IEnumerable<int> other)
        {
            int len = source.Count();

            double avgSource = source.Average();
            double avgOther = other.Average();
            double covariance = 0;

            for (int i = 0; i < len; i++)
                covariance += (source.ElementAt(i) - avgSource) * (other.ElementAt(i) - avgOther);

            return covariance / len;
        }
        public static double Pearson(this IEnumerable<int> source, IEnumerable<int> other)
        {
            return source.Covariance(other) / (source.StandardDeviationP() *
                                     other.StandardDeviationP());
        }

        public static double PearsonChiSquared(this IEnumerable<int> source, int iterations, int range)
        {
            double ans = 0.0;
            double totalCount = source.Count();
            double expected = (iterations / range) / totalCount;
            for (int i = 0; i < source.Count(); i++)
            {
                double t = (source.ElementAt(i) / totalCount) - expected;
                ans += (t * t) / expected;
            }
            return ans;
        }
    }
}
