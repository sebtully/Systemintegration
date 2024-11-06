namespace WeatherApp;

public interface IMessageTranslator
{
    string Translate(object data);
}

public class StringTranslator : IMessageTranslator
{
    public string Translate(object data)
    {
        return data.ToString();
    }
}

public class ClassTranslator : IMessageTranslator
{
    public string Translate(object data)
    {
        // Simulér konvertering til en klasse som string output
        return $"Class object: {data}";
    }
}

public class XmlTranslator : IMessageTranslator
{
    public string Translate(object data)
    {
        // Simulér konvertering til XML
        return "<xml>" + data.ToString() + "</xml>";
    }
}
