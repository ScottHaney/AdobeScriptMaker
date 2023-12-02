using AdobeDocsParser;
using HtmlAgilityPack;
using System.Runtime.InteropServices;

namespace AdobeDocsRunner.Tests
{
    public class ParserTests
    {
        [Test]
        public void Parses_Sections()
        {
            var doc = new HtmlDocument();
            AddSections(doc.DocumentNode,
                new("Description", "Description Content"),
                new("Type", "Type Content"));

            var parser = CreateParser();
            var results = parser.GetSections(doc.DocumentNode).ToList();

            Assert.That(results, Is.EquivalentTo(new Section[] { new("Description", "Description Content"), new("Type", "Type Content") }));
        }

        [Test]
        public void Parses_Header_Node()
        {
            var headerNode = CreateHeaderNode("Test");
            var parser = CreateParser();

            var succeeded = parser.TryParseHeader(headerNode, out var result);

            Assert.That(succeeded, Is.True);
            Assert.That(result.Header, Is.EqualTo("Test"));
        }

        private void AddSections(HtmlNode node, params TestSectionData[] sections)
        {
            foreach (var section in sections)
                AddSectionNodes(node, section);
        }

        private void AddSectionNodes(HtmlNode node, TestSectionData testSection)
        {
            var headerNode = CreateHeaderNode(testSection.Header);
            var contentNode = CreateContentNode(testSection.Content);

            node.AppendChild(headerNode);
            node.AppendChild(contentNode);
        }

        private record TestSectionData(string Header, string Content);

        private HtmlNode CreateContentNode(string content)
        {
            return HtmlNode.CreateNode($"<p>{content}</p>");
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