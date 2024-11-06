namespace BluffCityInfo;

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

public static class MessageSplitter
{
    public static List<XElement> SplitMessages(string filePath)
    {
        var messages = new List<XElement>();

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        var document = XDocument.Load(filePath);
        foreach (var element in document.Root.Elements())
        {
            messages.Add(element);
        }

        return messages;
    }
}