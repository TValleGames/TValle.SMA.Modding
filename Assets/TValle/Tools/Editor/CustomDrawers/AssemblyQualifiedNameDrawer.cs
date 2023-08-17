using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(AssemblyQualifiedNameAttribute))]
public class AssemblyQualifiedNameDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type currentType = null;
        var currentValue = property.stringValue;
        if(!string.IsNullOrWhiteSpace(currentValue))
            currentType = Type.GetType(currentValue);


        if(currentType == null)
        {
            Rect first = position;
            first.height = EditorGUIUtility.singleLineHeight;
            var script = EditorGUI.ObjectField(first, "Select or Drag a Script", null, typeof(MonoScript), true) as MonoScript;
            currentType = script?.GetClass();
            if(currentType != null)
                property.stringValue = currentType.AssemblyQualifiedName;
        }
        else
        {
            Rect first = position;
            first.height = EditorGUIUtility.singleLineHeight;
            first.width = 100;
            if(GUI.Button(first,"Remove Script"))
            {
                currentType = null;
                property.stringValue = string.Empty;
            }
        }

        Rect sec = position;
        sec.y += EditorGUIUtility.singleLineHeight;
        sec.height -= EditorGUIUtility.singleLineHeight;
        if(currentType==null)
            EditorGUI.LabelField(sec, "-no script selected-");
        else
        EditorGUI.LabelField(sec, property.stringValue);
    }
}

