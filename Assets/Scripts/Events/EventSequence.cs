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
    public float duration; // Duration can now be set manually if no audio

    public float Duration
    {
        get { return audioClip != null ? audioClip.length : duration; }
    }
}


public class EventSequence : MonoBehaviour
{
    public GlobalEventDefinitions globalEventDefinitions;
    public List<GameEvent> events = new List<GameEvent>();

    public void StartEventSequence()
    {
        StartCoroutine(RunEventSequence());
    }

    public void Start()
    {
        StartEventSequence();
    }

    private IEnumerator RunEventSequence()
    {
        foreach (var gameEvent in events)
        {
            yield return new WaitForSeconds(gameEvent.delay);

            Debug.Log($"Triggering event: {gameEvent.eventName}");

            // Pass both the event name and the audio clip
            EventManager.Instance.TriggerEvent(gameEvent.eventName, gameEvent.audioClip, gameEvent.Duration);

            yield return new WaitForSeconds(gameEvent.Duration);
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

        // Draw Audio Clip field
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(position, audioClipProp, new GUIContent("Audio Clip"));

        // Draw Duration field, editable only when no audio clip is assigned
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        GUI.enabled = audioClipProp.objectReferenceValue == null;
        EditorGUI.PropertyField(position, durationProp, new GUIContent("Duration"));
        GUI.enabled = true; // Re-enable for other properties

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Adjust the height to accommodate all fields (event name dropdown, delay, audio clip, and duration)
        return EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing * 3;
    }
}



[CustomEditor(typeof(EventSequence))]
public class EventSequenceEditor : Editor
{
    private ReorderableList reorderableList;

    private void OnEnable()
    {
        reorderableList = new ReorderableList(serializedObject,
            serializedObject.FindProperty("events"),
            true, true, true, true);

        reorderableList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Event Sequence");
        };

        reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, element, true);
        };

        reorderableList.elementHeightCallback = (int index) =>
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("globalEventDefinitions"));

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "This Event Sequencer allows you to create a series of timed events. " +
            "Each event can have a name, a delay (in seconds), and an optional audio clip. " +
            "Events will trigger in the order listed, with the specified delay between each event. " +
            "Use the Global Event Definitions to manage the available event types.",
            MessageType.Info);

        EditorGUILayout.Space();
        reorderableList.DoLayoutList();

        if (GUILayout.Button("Start Event Sequence"))
        {
            ((EventSequence)target).StartEventSequence();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif