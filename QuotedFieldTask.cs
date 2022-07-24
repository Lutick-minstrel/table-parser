using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("\"abc\"", 0, "abc", 5)]
        [TestCase("b \"a'\"", 2, "a'", 4)]
        [TestCase(@"'a\' b'", 0, "a' b", 7)]
        [TestCase("\"", 0, "", 1)]
        [TestCase("'", 0, "", 1)]
        [TestCase("'a'a", 0, "a", 3)]
        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual(new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static Token ReadQuotedField(string line, int startIndex)
        {
            StringBuilder lineBuilder = new StringBuilder();
            int numberChars = startIndex + 1; 
            while (numberChars < line.Length && !(line[numberChars]).Equals(line[startIndex]))
            {
                if (line[numberChars].Equals('\\')) numberChars++;
                lineBuilder.Append(line[numberChars]);
                numberChars++;
            }
            if (numberChars < line.Length) numberChars++;
            return new Token(lineBuilder.ToString(),startIndex, numberChars - startIndex);
        }
    }
}
