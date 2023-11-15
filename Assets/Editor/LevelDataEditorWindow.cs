using UnityEditor;
using UnityEngine;

public class LevelDataEditorWindow : EditorWindow
{
    private LevelConfig levelData;
    private int numRows = 5;
    private int numCols = 5;

    public static void ShowWindow()
    {
        GetWindow<LevelDataEditorWindow>("LevelData Editor");
    }

    private void OnEnable()
    {
        if (Selection.activeObject is LevelConfig)
        {
            levelData = (LevelConfig)Selection.activeObject;
            if(levelData != null)
            {
                numRows = levelData.RowsCount;
                numCols = levelData.ColsCount;
            }
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("LevelData Editor", EditorStyles.boldLabel);
        
        if (levelData == null)
        {
            EditorGUILayout.HelpBox("Select a LevelData scriptable object in the project window.", MessageType.Info);
            return;
        }

        numRows = EditorGUILayout.IntField("Rows", numRows);
        numCols = EditorGUILayout.IntField("Columns", numCols);

        if (numRows <= 0 || numCols <= 0)
        {
            EditorGUILayout.HelpBox("Rows and Columns must be greater than 0.", MessageType.Error);
            return;
        }

        if (GUILayout.Button("Apply Size"))
        {
            levelData.SetSize(numRows, numCols);

            EditorUtility.SetDirty(levelData);
            return;
        }

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();

        for (int i = 0; i < levelData.ColsCount; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            for (int j = 0; j < levelData.RowsCount; ++j)
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
