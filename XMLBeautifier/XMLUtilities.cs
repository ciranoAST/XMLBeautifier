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
        // Use a simple formatting method that inserts new lines and tabs for each tag
        var beautifiedXml = new System.Text.StringBuilder();
        var indent = 0;
        var insideTag = false;

        foreach (var c in xmlContent)
        {
            switch (c)
            {
                case '<':
                {
                    if (!insideTag)
                    {
                        if (beautifiedXml.Length > 0)
                            beautifiedXml.Append("\n" + new string('\t', indent));
                        insideTag = true;
                    }

                    beautifiedXml.Append(c);
                    break;
                }
                case '>':
                {
                    beautifiedXml.Append(c);
                    insideTag = false;

                    if (beautifiedXml.Length <= 1 || beautifiedXml[^2] == '/') 
                        continue;

                    if (beautifiedXml[^3] == '/')// If it's a closing tag, decrease indentation
                        indent--;

                    else if (beautifiedXml[^3] != '?' && beautifiedXml[^3] != '!') // Otherwise increase indentation
                        indent++;
                    break;
                }
                default:
                    beautifiedXml.Append(c);
                    break;
            }
        }
        return beautifiedXml.ToString();
    }
}