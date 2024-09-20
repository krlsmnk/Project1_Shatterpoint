using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

[Serializable]
public class GameEvent
{
    public string eventName;
    public float delay = 0f;
    public AudioClip audioClip;
    public float duration = 1f; // Default duration is now 1 second
    public bool playWithPrevious; // Allow playing with previous event

    public float Duration
    {
        get { return audioClip != null ? audioClip.length : duration; }
    }
}

[Serializable]
public class EventCollection
{
    public string collectionName;
    public List<GameEvent> events = new List<GameEvent>();
    public bool loop = false; // Loop the event collection
    [Tooltip("Number of times to loop the collection. Set to 0 for an infinite loop.")]
    public int loopCount = 1;  // Number of times to loop (0 for infinite)
    public float delayBeforeStart = 0f; // Optional delay before starting

    public IEnumerator RunEventCollection()
    {
        if (delayBeforeStart > 0f)
        {
            yield return new WaitForSeconds(delayBeforeStart);
        }

        // Infinite loop if loopCount is 0
        int loopsRemaining = loop ? (loopCount > 0 ? loopCount : -1) : 1;

        while (loopsRemaining != 0)
        {
            foreach (var gameEvent in events)
            {
                if (!gameEvent.playWithPrevious)
                {
                    yield return new WaitForSeconds(gameEvent.delay);
                }

                Debug.Log($"Triggering event in collection '{collectionName}': {gameEvent.eventName}");

                EventManager.Instance.TriggerEvent(gameEvent.eventName, gameEvent.audioClip, gameEvent.Duration);

                if (!gameEvent.playWithPrevious)
                {
                    yield return new WaitForSeconds(gameEvent.Duration);
                }
            }

            if (loopsRemaining > 0)
            {
                loopsRemaining--;
            }
        }
    }
}

public class EventSequence : MonoBehaviour
{
    public GlobalEventDefinitions globalEventDefinitions;
    public List<EventCollection> eventCollections = new List<EventCollection>(); // Removed the main event list

    public void StartEventSequence()
    {
        StartCoroutine(RunEventCollections());
    }

    public void Start()
    {
        StartEventSequence();
    }

    private IEnumerator RunEventCollections()
    {
        List<Coroutine> runningCollections = new List<Coroutine>();

        foreach (var collection in eventCollections)
        {
            runningCollections.Add(StartCoroutine(collection.RunEventCollection()));
        }

        // Wait for all collections to finish
        foreach (var coroutine in runningCollections)
        {
            yield return coroutine;
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(GameEvent))]
public class GameEventDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var eventNameProp = property.FindPropertyRelative("eventName");
        var delayProp = property.FindPropertyRelative("delay");
        var audioClipProp = property.FindPropertyRelative("audioClip");
        var durationProp = property.FindPropertyRelative("duration");
        var playWithPreviousProp = property.FindPropertyRelative("playWithPrevious");

        // Draw Event Name as Dropdown
        var eventSequence = property.serializedObject.targetObject as EventSequence;
        if (eventSequence != null && eventSequence.globalEventDefinitions != null)
        {
            string[] eventNames = eventSequence.globalEventDefinitions.GetEventNames();
            int currentIndex = Array.IndexOf(eventNames, eventNameProp.stringValue);

            position.height = EditorGUIUtility.singleLineHeight;
            int selectedIndex = EditorGUI.Popup(position, "Event Name", currentIndex, eventNames);
            if (selectedIndex >= 0 && selectedIndex < eventNames.Length)
            {
                eventNameProp.stringValue = eventNames[selectedIndex];
            }
        }

        // Draw Delay field
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, delayProp, new GUIContent("Delay"));

        // Draw Play With Previous field
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, playWithPreviousProp, new GUIContent("Play With Previous"));

        // Draw Audio Clip field
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, audioClipProp, new GUIContent("Audio Clip"));

        // Draw Duration field, editable only when no audio clip is assigned
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        GUI.enabled = audioClipProp.objectReferenceValue == null;
        EditorGUI.PropertyField(position, durationProp, new GUIContent("Duration (default 1 second)"));
        GUI.enabled = true; // Re-enable for other properties

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Adjust the height to accommodate all fields (event name dropdown, delay, play with previous, audio clip, and duration)
        return EditorGUIUtility.singleLineHeight * 5 + EditorGUIUtility.standardVerticalSpacing * 4;
    }
}

[CustomEditor(typeof(EventSequence))]
public class EventSequenceEditor : Editor
{
    private ReorderableList collectionList;

    private void OnEnable()
    {
        collectionList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("eventCollections"),
            true, true, true, true);

        collectionList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Event Collections");
        };

        collectionList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = collectionList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element, true);
        };

        collectionList.elementHeightCallback = (int index) =>
        {
            var element = collectionList.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("globalEventDefinitions"));

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "This Event Sequencer allows you to create event collections that trigger events with optional delays. " +
            "Collections can be looped and started with a delay. Set the loop count to 0 for infinite looping.",
            MessageType.Info);

        EditorGUILayout.Space();
        collectionList.DoLayoutList();

        if (GUILayout.Button("Start Event Sequence"))
        {
            ((EventSequence)target).StartEventSequence();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
