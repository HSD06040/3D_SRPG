#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class EventBusMonitorWindow : EditorWindow
{
    [MenuItem("Tools/Event Bus Monitor")]
    public static void Open()
    {
        var window = GetWindow<EventBusMonitorWindow>("Event Bus Monitor");
        window.minSize = new Vector2(500, 350);
    }

    private Vector2 scrollPosBindings;
    private Vector2 scrollPosLogs;

    private GUIStyle headerStyle;
    private GUIStyle boxStyle;
    private GUIStyle logStyle;

    private void OnEnable()
    {
        headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 15,
            normal = { textColor = Color.white }
        };

        boxStyle = new GUIStyle("box")
        {
            padding = new RectOffset(12, 12, 8, 8),
            margin = new RectOffset(0, 0, 5, 5)
        };

        logStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 12,
            richText = true,
            wordWrap = false
        };
    }

    private void OnGUI()
    {
        GUILayout.Space(5);
        GUILayout.Label("Event Bus Monitor", headerStyle);
        GUILayout.Space(10);

        DrawBindingSection();
        GUILayout.Space(15);
        DrawLogSection();
    }

    private void DrawBindingSection()
    {
        GUILayout.Label("Active Event Bindings", EditorStyles.boldLabel);
        GUILayout.BeginVertical(boxStyle);

        scrollPosBindings = GUILayout.BeginScrollView(scrollPosBindings, GUILayout.Height(150));

        foreach (var pair in EventBusMonitor.BusBindingMethods)
        {
            GUILayout.Label($"<b>{pair.Key.Name}</b>  ({pair.Value.Count} listeners)", logStyle);

            foreach (var method in pair.Value)
            {
                GUILayout.Label($"{method}", logStyle);
            }

            GUILayout.Space(5);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    private void DrawLogSection()
    {
        GUILayout.Label("Recent Event Logs", EditorStyles.boldLabel);

        GUILayout.BeginVertical(boxStyle);

        scrollPosLogs = GUILayout.BeginScrollView(scrollPosLogs, GUILayout.Height(150));

        foreach (var log in EventBusMonitor.EventLogs)
        {
            GUILayout.Label(log, logStyle);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }
}
#endif