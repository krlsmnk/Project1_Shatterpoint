using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "GlobalEventDefinitions", menuName = "Event System/Global Event Definitions")]
public class GlobalEventDefinitions : ScriptableObject
{
    [System.Serializable]
    public class EventDefinition
    {
        public string eventName;
        // You can add more properties here if needed, like description, category, etc.
    }

    public List<EventDefinition> eventDefinitions = new List<EventDefinition>();

    // Helper method to get all event names
    public string[] GetEventNames()
    {
        return eventDefinitions.Select(ed => ed.eventName).ToArray();
    }
}