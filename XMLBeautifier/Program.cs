using System.Xml.Linq;
using System.Xml;
using XMLBeautifier;


Console.WriteLine("====================================");
Console.WriteLine(" Welcome to XML Beautifier");
Console.WriteLine("====================================\n");

string currentDirectory = Directory.GetCurrentDirectory();
Console.WriteLine($"The program is running in: {currentDirectory}");
Console.WriteLine("If you want to specify a custom folder enter the full path now. \nPress Enter to use the default location.");
string inputPath = Console.ReadLine();
string xmlPath = string.IsNullOrWhiteSpace(inputPath) ? currentDirectory : inputPath;

var xmlFiles = Directory.GetFiles(xmlPath, "*.xml");
Console.WriteLine($"{xmlFiles.Length} files found");
for(int i = 0; i<=xmlFiles.Length; i++)
{ 
    Console.WriteLine($"Processing file {xmlFiles[i]} ({i+1}/{xmlFiles.Length}");
    try
    {
        // Try to load the XML file to check its validity
        var doc = XDocument.Load(xmlFiles[i]);

        // If it's valid, indent it normally
        XmlUtilities.SaveIndentedXml(doc, xmlFiles[i]);
    }
    catch (XmlException)
    {
        // If the file is not valid, try to format its content as text
        Console.WriteLine($"The file {xmlFiles[i]} is not a valid XML. Attempting to format the text.");
        try
        {
            var xmlContent = File.ReadAllText(xmlFiles[i]);
            var beautifiedContent = XmlUtilities.BeautifyXmlText(xmlContent);

            var formattedFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetFileNameWithoutExtension(xmlFiles[i]) + "_formatted.xml");
            File.WriteAllText(formattedFile, beautifiedContent);

            Console.WriteLine($"Formatted file saved: {formattedFile}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while formatting the file {xmlFiles[i]}: {ex.Message}");
        }
    }
}