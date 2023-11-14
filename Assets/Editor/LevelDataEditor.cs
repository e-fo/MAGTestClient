using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;

        if (GUILayout.Button("Open Level Editor Window"))
        {
            LevelDataEditorWindow.ShowWindow();
        }

        DrawDefaultInspector();
    }
}
