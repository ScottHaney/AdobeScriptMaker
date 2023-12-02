using AdobeDocsParser;
using HtmlAgilityPack;

namespace AdobeDocsRunner.Tests
{
    public class ParserTests
    {
        [Test]
        public void Parses_Header_Node()
        {
            var headerNode = CreateHeaderNode("Test");
            var parser = CreateParser();

            var succeeded = parser.TryParseHeader(headerNode, out var result);

            Assert.That(succeeded, Is.True);
            Assert.That(result.Header, Is.EqualTo("Test"));
        }

        private HtmlNode CreateHeaderNode(string header)
        {
            var htmlText = $"<p><strong>{header}</strong></p>";
            return HtmlNode.CreateNode(htmlText);
        }

        private Parser CreateParser()
            => new Parser();
    }
}