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
        var defaultLabel = new GUIContent(label);
        Type currentType = null;
        var currentValue = property.stringValue;
        if(!string.IsNullOrWhiteSpace(currentValue))
            currentType = Type.GetType(currentValue);

        var att = attribute as AssemblyQualifiedNameAttribute;
        if(currentType != null)
        {
            if(!att.IsImplementing(currentType))
            {
                Debug.LogError(att.GetError(currentType), property?.serializedObject?.targetObject);
                currentType = null;
                property.stringValue = string.Empty;
                //EditorUtility.DisplayDialog("Please fix the selected script.", "The selected script does not implement the type " + att.implementing.Name, "OK");
            }
        }


        if(currentType == null)
        {
            Rect first = position;
            first.height = EditorGUIUtility.singleLineHeight;
            var script = EditorGUI.ObjectField(first, defaultLabel, null, typeof(MonoScript), true) as MonoScript;
            currentType = script?.GetClass();
            if(currentType != null)
                property.stringValue = currentType.AssemblyQualifiedName;
        }
        else
        {
            Rect first = position;
            first.height = EditorGUIUtility.singleLineHeight;
            first.width = 100;
            first.x += EditorGUIUtility.labelWidth;
            if(GUI.Button(first, "Remove Script"))
            {
                currentType = null;
                property.stringValue = string.Empty;
            }
        }

        Rect sec = position;
        sec.y += EditorGUIUtility.singleLineHeight;
        sec.height -= EditorGUIUtility.singleLineHeight;
        if(currentType == null)
        {
            GUIStyle s = new GUIStyle("miniLabel");
            EditorGUI.LabelField(sec, new GUIContent(" "), new GUIContent("Select or Drag a Script", defaultLabel.tooltip)/*"-no script selected-"*/, s);
        }
        else
        {
            EditorGUI.LabelField(sec, defaultLabel, new GUIContent(property.stringValue, defaultLabel.tooltip));
        }
    }
}

