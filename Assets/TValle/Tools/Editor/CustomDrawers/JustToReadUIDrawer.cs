using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(JustToReadUIAttribute))]
public class JustToReadUIDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        bool wasDisabled = !GUI.enabled;
        if(!wasDisabled)
            GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        if(!wasDisabled)
            GUI.enabled = true;
    }
}
