using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlToOpenXml;

namespace Swagger2Docx.Helpers
{
    /// <summary> html轉word helper </summary>
    public class DocHelper
    {
        public void HtmlToWord(string html, MemoryStream memoryStream)
        {
            using WordprocessingDocument package =
                           WordprocessingDocument.Create(memoryStream,
                           WordprocessingDocumentType.Document);
            MainDocumentPart mainPart;
            if (package.MainDocumentPart != null)
            {
                mainPart = package.MainDocumentPart;
            }
            else
            {
                mainPart = package.AddMainDocumentPart();
                new Document(new Body()).Save(mainPart);
            }
            HtmlConverter converter = new HtmlConverter(mainPart);
            converter.ParseHtml(html);
            mainPart.Document.Save();
        }
    }
}
