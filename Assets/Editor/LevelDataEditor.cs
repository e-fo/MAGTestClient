using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelConfig))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelConfig levelData = (LevelConfig)target;

        if (GUILayout.Button("Open Level Editor Window"))
        {
            LevelDataEditorWindow.ShowWindow();
        }

        DrawDefaultInspector();
    }
}
