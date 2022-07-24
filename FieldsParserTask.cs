using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        [TestCase("a\"b c d e\"f", new[] { "a", "b c d e", "f" })]
        [TestCase("text", new[] { "text" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("text", new[] { "text" })]
        [TestCase("hello     world", new[] { "hello", "world" })]
        [TestCase("\'te'xt", new[] { "te", "xt" })]
        [TestCase("\'te ' xt", new[] { "te ", "xt" })]
        [TestCase("\"te' xt\"", new[] { "te' xt" })]
        [TestCase("\'te\" xt\'", new[] { "te\" xt" })]
        [TestCase("''", new[] { "" })]
        [TestCase("a'bc'c", new[] { "a", "bc", "c" })]
        [TestCase("a'bc", new[] { "a", "bc" })]
        [TestCase(@"'a\'bc'", new[] { "a'bc" })]
        [TestCase(@"""a\""bc""", new[] { @"a""bc" })]
        [TestCase(@"""a\\bc""", new[] { @"a\bc" })]
        [TestCase(@"""a\\""", new[] { @"a\" })]
        [TestCase(@"     text   ", new[] { @"text" })]
        [TestCase(@"'text  ", new[] { @"text  " })]
        [TestCase("", new string[0])]
        public static void RunTests(string input, string[] expectedOutput)
        {
            Test(input, expectedOutput);
        }

        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var tokenList = new List<Token>();
            for(int i = 0; i < line.Length; i++)
            {
                if(line[i].Equals('\'') || line[i].Equals('"'))
                {
                    var token = ReadQuotedField(line, i);
                    tokenList.Add(token);
                    i = token.GetIndexNextToToken()-1;
                }
                else if(!line[i].Equals(' '))
                {
                    var token = ReadField(line, i);
                    tokenList.Add(token);
                    i = token.GetIndexNextToToken()-1;
                }
            }
            return tokenList;
        }
        
        private static Token ReadField(string line, int startIndex)
        {
            int numberChars = startIndex;
            while (numberChars < line.Length && !line[numberChars].Equals(' ') && !line[numberChars].Equals('"') && !(line[numberChars].Equals('\'')))
            {
                numberChars++;
            }
            return new Token(line.Substring(startIndex, numberChars - startIndex).Trim(), startIndex, numberChars - startIndex);
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }
    }
}