using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig;
using System.Linq;

namespace AdobeDocsParser;

public class Parser(string _pdfFilePath)
{
    public void Test()
    {
        using var document = PdfDocument.Open(_pdfFilePath);

        foreach (Page page in document.GetPages())
        {
            var letters = page.Letters;
            var example = string.Join(string.Empty, letters.Select(x => x.Value));

            var words = page.GetWords();

            var images = page.GetImages();
        }
    }
}
