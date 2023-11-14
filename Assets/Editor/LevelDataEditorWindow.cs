using UnityEditor;

public class LevelDataEditorWindow : EditorWindow
{
    private LevelData levelData;

    [MenuItem("Window/LevelData Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataEditorWindow>("LevelData Editor");
    }

    private void OnEnable()
    {
        if (Selection.activeObject is LevelData)
        {
            levelData = (LevelData)Selection.activeObject;
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.Space();

        if (levelData == null)
        {
            EditorGUILayout.HelpBox("Select a LevelData scriptable object in the project window.", MessageType.Info);
            return;
        }

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < levelData.Rows; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < levelData.Cols; ++j)
            {
                TileConfig tile = EditorGUILayout.ObjectField(levelData[j, i], typeof(TileConfig), false) as TileConfig;
                levelData[j, i] = tile;
            }
            EditorGUILayout.EndHorizontal();

        }

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(levelData);
        }
    }
}
