using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboDave.Random
{

    public class StringToken
    {
        public String String { get; set; }
        public StringTokenType Type { get; set; }
    }

    public enum StringTokenType
    {
        Whitespace,
        Word,
        Punctuation,
        Number,
        Symbol,
    }

    public class StringTokenizer : IEnumerator<StringToken>, IEnumerable<StringToken>
    {
        private Int32 idx = 0;
        private Char[] array;
        private StringToken current = null;

        public StringTokenizer(String input)
        {
            this.array = input.ToCharArray();
            this.idx = 0;
        }

        public StringToken Current
        {
            get { return this.current; }
        }

        public void Dispose()
        {
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.current; }
        }

        public StringToken Peek()
        {
            int oldIdx = this.idx;
            if (this.idx >= this.array.Length)
                return null;

            Char c = this.array[this.idx];
            StringToken token = null;
            if (Char.IsPunctuation(c))
            {
                token = new StringToken();
                token.String = c.ToString();
                token.Type = StringTokenType.Punctuation;
                this.idx++;
            }
            else if (Char.IsWhiteSpace(c))
                Consume(Char.IsWhiteSpace, StringTokenType.Whitespace, out token);
            else if (Char.IsLetter(c))
                Consume(Char.IsLetter, StringTokenType.Word, out token);
            else if (Char.IsDigit(c))
                Consume(Char.IsDigit, StringTokenType.Number, out token);
            else if (Char.IsSymbol(c))
                Consume(Char.IsSymbol, StringTokenType.Symbol, out token);

            this.idx = oldIdx;

            return token;
        }

        public bool MoveNext()
        {
            StringToken token = Peek();

            if (token == null)
                return false;

            this.current = token;
            this.idx += this.current.String.Length;
            return this.idx <= this.array.Length;
        }

        public void Reset()
        {
            this.idx = 0;
            this.current = null;
        }

        private void Consume(Func<Char, Boolean> isCheck, StringTokenType type, out StringToken token)
        {
            Char c = this.array[this.idx];
            int start = this.idx;
            while (isCheck(c))
            {
                this.idx++;
                if (this.idx >= this.array.Length)
                    break;
                c = this.array[this.idx];
            }
            token = new StringToken();
            token.String = new String(this.array, start, this.idx - start);
            token.Type = type;
        }


        public IEnumerator<StringToken> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }
    }
}
