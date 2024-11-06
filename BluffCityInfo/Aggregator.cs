using System.Collections.Generic;
using System.Text;

namespace BluffCityInfo;

public class Aggregator
{
    public string CombineMessages(IEnumerable<string> messages)
    {
        var combinedMessage = new StringBuilder();
        combinedMessage.Append("<CombinedMessages>");

        foreach (var message in messages)
        {
            combinedMessage.Append(message);
        }

        combinedMessage.Append("</CombinedMessages>");
        return combinedMessage.ToString();
    }
}