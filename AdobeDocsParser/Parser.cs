using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using HtmlAgilityPack;

namespace AdobeDocsParser;

public class Parser
{
    public IEnumerable<Section> GetSections(HtmlNode root)
    {
        HeaderNode? currentHeader = null;
        List<HtmlNode> contentNodes = [];
        foreach (var node in root.GetDirectDescendents())
        {
            if (TryParseHeader(node, out var header))
            {
                if (currentHeader != null)
                {
                    yield return CreateSection(currentHeader, contentNodes);
                    contentNodes = [];
                }

                currentHeader = header;
            }
            else if (currentHeader != null)
                contentNodes.Add(node);
        }

        if (currentHeader != null)
            yield return CreateSection(currentHeader, contentNodes);
    }

    private Section CreateSection(HeaderNode headerNode, IEnumerable<HtmlNode> contentNodes)
    {
        return new(headerNode.Header, string.Join("", contentNodes.Select(x => x.InnerText)));
    }

    public bool TryParseHeader(HtmlNode node, out HeaderNode header)
    {
        if (node.Name == "p")
        {
            if (node.FirstChild?.Name == "strong")
            {
                header = new(node.FirstChild.InnerText);
                return true;
            }    
        }

        header = new(string.Empty);
        return false;
    }
}

public record Section(string Header, string Content);



public record class HeaderNode(string Header);

public static class AgilityPackExtensions
{
    public static IEnumerable<HtmlNode> GetDirectDescendents(this HtmlNode node)
        => node.SelectNodes("*");
}