using System.Xml.Linq;
using System.Xml;

namespace XMLBeautifier;
public static class XmlUtilities
{
    public static void SaveIndentedXml(XDocument doc, string filePath)
    {
        XmlWriterSettings settings = new()
        {
            Indent = true,
            IndentChars = "\t", // Use a tab character for indentation
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            Encoding = new System.Text.UTF8Encoding(false) // Do not add BOM
        };

        var formattedFile = Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, Path.GetFileNameWithoutExtension(filePath) + "_formatted.xml");

        using var writer = XmlWriter.Create(formattedFile, settings);
        doc.Save(writer);

        Console.WriteLine($"Formatted file saved: {formattedFile}");
    }

    public static string BeautifyXmlText(string xmlContent)
    {
        var doc = XDocument.Parse(xmlContent);
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "\t",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace
        };

        using var sw = new StringWriter();
        using (var xw = XmlWriter.Create(sw, settings))
        {
            doc.Save(xw);
        }

        return sw.ToString();
    }
}