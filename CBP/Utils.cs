using System.IO;

namespace CBP;

class Utils
{
    public static string ReadXmlFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}