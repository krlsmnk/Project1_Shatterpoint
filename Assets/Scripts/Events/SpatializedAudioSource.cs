using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpatializedAudioSource : MonoBehaviour
{
    [SerializeField] private string eventName;
    public string EventName => eventName;

    [Range(0f, 1f)]
    public float spatialBlend = 1f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialize = true;
        UpdateSpatialBlend();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener(eventName, PlayAudio);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener(eventName, PlayAudio);
    }

    private void PlayAudio(AudioClip clip, float duration)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void UpdateSpatialBlend()
    {
        if (audioSource != null)
        {
            audioSource.spatialBlend = spatialBlend;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateSpatialBlend();
    }
#endif
}

#if UNITY_EDITOR

[CustomEditor(typeof(SpatializedAudioSource))]
public class SpatializedAudioSourceEditor : Editor
{
    private string[] eventNames;
    private int selectedEventIndex = -1;

    private void OnEnable()
    {
        RefreshEventNames();
    }

    private void RefreshEventNames()
    {
        EventSequence eventSequence = FindObjectOfType<EventSequence>();
        if (eventSequence != null && eventSequence.globalEventDefinitions != null)
        {
            eventNames = eventSequence.globalEventDefinitions.GetEventNames();
            SpatializedAudioSource source = (SpatializedAudioSource)target;
            selectedEventIndex = System.Array.IndexOf(eventNames, source.EventName);
        }
        else
        {
            eventNames = new string[0];
            selectedEventIndex = -1;
        }
    }

    public override void OnInspectorGUI()
    {
        SpatializedAudioSource source = (SpatializedAudioSource)target;

        EditorGUI.BeginChangeCheck();

        if (eventNames != null && eventNames.Length > 0)
        {
            selectedEventIndex = EditorGUILayout.Popup("Event Name", selectedEventIndex, eventNames);
            if (selectedEventIndex != -1 && selectedEventIndex < eventNames.Length)
            {
                SerializedProperty eventNameProp = serializedObject.FindProperty("eventName");
                eventNameProp.stringValue = eventNames[selectedEventIndex];
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No EventSequence found in the scene or no events defined.", MessageType.Warning);
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("spatialBlend"));

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Refresh Event List"))
        {
            RefreshEventNames();
        }
    }
}
#endif