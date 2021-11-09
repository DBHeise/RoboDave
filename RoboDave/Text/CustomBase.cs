using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Text
{
    public class CustomBase
    {

        public static String ConvertToBaseAlpha(double number, String baseStr = "0123456789ABCDEFGHIKJLMNOPQRSTUVWXYZ")
        {            
            String ans = "";
            int power = baseStr.Length;

            while ((int)number != 0)
            {
                var m = (int)(number % power);
                number /= power;
                ans = baseStr.Substring(m, 1) + ans;
            }
            return ans;
        }
        public static ulong ConvertFromBaseAlpha(string alpha, String baseStr = "0123456789ABCDEFGHIKJLMNOPQRSTUVWXYZ")
        {
            ulong val = 0;
            ulong power = 1;
            foreach(Char c in Enumerable.Reverse(alpha.ToCharArray()))
            {
                ulong posValue = (ulong) baseStr.IndexOf(c);
                val += posValue * power;
                power *= (ulong)baseStr.Length;
            }
            return val;
        }
    }
}
