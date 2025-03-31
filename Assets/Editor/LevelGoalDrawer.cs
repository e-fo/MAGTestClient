using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LevelGoal))]
public class LevelGoalDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.FindPropertyRelative(LevelGoal.GoalTypePropertyName));

        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
            property.FindPropertyRelative(LevelGoal.CountPropertyName));

        LevelGoal.Type goalType = (LevelGoal.Type)property.FindPropertyRelative(LevelGoal.GoalTypePropertyName).enumValueIndex;
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        switch (goalType)
        {
            case LevelGoal.Type.TypeLess:
                break;

            case LevelGoal.Type.BaseType:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                    property.FindPropertyRelative(LevelGoal.TileBaseTypePropertyName));
                break;

            case LevelGoal.Type.SpecificType:
                EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                    property.FindPropertyRelative(LevelGoal.TileConfigPropertyName));
                break;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        LevelGoal.Type goalType = (LevelGoal.Type)property.FindPropertyRelative(LevelGoal.GoalTypePropertyName).enumValueIndex;

        if (goalType == LevelGoal.Type.TypeLess)
            return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
        else
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 2;
    }
}