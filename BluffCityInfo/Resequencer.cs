using System.Collections.Generic;
using System.Linq;

namespace BluffCityInfo;

public class Resequencer
{
    private readonly List<string> _messages = new List<string>();

    public void AddMessage(string message)
    {
        _messages.Add(message);
    }

    public IEnumerable<string> GetOrderedMessages()
    {
        // Sort messages if needed, here we assume they are already in order
        return _messages.OrderBy(m => m); // Adjust sorting logic as needed
    }
}