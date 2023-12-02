using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using HtmlAgilityPack;

namespace AdobeDocsParser;

public class Parser
{
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

public record class HeaderNode(string Header);

public static class AgilityPackExtensions
{
    public static IEnumerable<HtmlNode> GetDirectDescendents(this HtmlNode node)
        => node.SelectNodes("*");
}