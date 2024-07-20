using Assets.TValle.Tools.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityObject = UnityEngine.Object;

namespace Assets.TValle.Tools.CustomDrawers.Abstracts
{
    public abstract class SerializableDictionaryDrawer<TK, TV> : PropertyDrawer
    {
        //tvalle
        Dictionary<TK, int> m_indexOfKey;

        private SerializableDictionary<TK, TV> _Dictionary;
        private bool _Foldout;
        private const float kButtonWidth = 18f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CheckInitialize(property, label);
            if(_Foldout)
                return (_Dictionary.Count + 1) * 17f;
            return 17f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            CheckInitialize(property, label);

            position.height = 17f;

            var foldoutRect = position;
            foldoutRect.width -= 2 * kButtonWidth;
            EditorGUI.BeginChangeCheck();
            _Foldout = EditorGUI.Foldout(foldoutRect, _Foldout, label, true);
            if(EditorGUI.EndChangeCheck())
                EditorPrefs.SetBool(label.text, _Foldout);

            var buttonRect = position;
            buttonRect.x = position.width - kButtonWidth + position.x;
            buttonRect.width = kButtonWidth + 2;

            var pathKeys = property.propertyPath + ".keys";
            var pathValues = property.propertyPath + ".values";

            var keysProperty = property.serializedObject.FindProperty(pathKeys);
            var valuesProperty = property.serializedObject.FindProperty(pathValues);

            if(GUI.Button(buttonRect, new GUIContent("+", "Add item"), EditorStyles.miniButton))
            {
                AddNewItem(keysProperty, valuesProperty);
            }

            buttonRect.x -= kButtonWidth;

            if(GUI.Button(buttonRect, new GUIContent("X", "Clear dictionary"), EditorStyles.miniButtonRight))
            {
                ClearDictionary();
            }

            if(!_Foldout)
                return;






            foreach(var item in _Dictionary)
            {
                var key = item.Key;
                var value = item.Value;
                var index = m_indexOfKey[key];

                SerializedProperty keyProperty;
                SerializedProperty valueProperty;

                if(index >= keysProperty.arraySize)
                    keyProperty = null;
                else
                    keyProperty = keysProperty.GetArrayElementAtIndex(index);

                if(index >= valuesProperty.arraySize)
                    valueProperty = null;
                else
                    valueProperty = valuesProperty.GetArrayElementAtIndex(index);



                position.y += 17f;

                var keyRect = position;
                keyRect.width /= 2;
                keyRect.width -= 4;
                EditorGUI.BeginChangeCheck();
                var newKey = DoField(keyRect, typeof(TK), key, keyProperty);
                if(EditorGUI.EndChangeCheck())
                {
                    try
                    {
                        _Dictionary.Remove(key);
                        _Dictionary.Add(newKey, value);

                        m_indexOfKey.Remove(key);
                        m_indexOfKey.Add(newKey, index);

                        EditorUtility.SetDirty(property.serializedObject.targetObject);
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e.Message);
                    }
                    break;
                }

                var valueRect = position;
                valueRect.x = position.width / 2 + 15;
                valueRect.width = keyRect.width - kButtonWidth;
                EditorGUI.BeginChangeCheck();
                value = DoField(valueRect, typeof(TV), value, valueProperty);
                if(EditorGUI.EndChangeCheck())
                {
                    _Dictionary[key] = value;

                    EditorUtility.SetDirty(property.serializedObject.targetObject);

                    break;
                }

                var removeRect = valueRect;
                removeRect.x = valueRect.xMax + 2;
                removeRect.width = kButtonWidth;
                if(GUI.Button(removeRect, new GUIContent("x", "Remove item"), EditorStyles.miniButtonRight))
                {
                    RemoveItem(key);
                    break;
                }
            }

        }

        private void RemoveItem(TK key)
        {
            _Dictionary.Remove(key);
            m_indexOfKey.Remove(key);
        }

        private void CheckInitialize(SerializedProperty property, GUIContent label)
        {
            if(_Dictionary == null)
            {
                // var target = property.serializedObject.targetObject;
                var clas = FindValue(property, true);
                var value = fieldInfo.GetValue(clas);
                _Dictionary = value as SerializableDictionary<TK, TV>;
                if(_Dictionary == null)
                {
                    _Dictionary = new SerializableDictionary<TK, TV>();
                    try
                    {
                        fieldInfo.SetValue(clas, _Dictionary);
                    }
                    catch(Exception e)
                    {
                        throw e;
                    }

                }
                //tvalle
                try
                {
                    m_indexOfKey = new Dictionary<TK, int>(_Dictionary.Count);
                    for(int i = 0; i < _Dictionary.serializedKeys.Count; i++)
                        m_indexOfKey.Add(_Dictionary.serializedKeys[i], i);
                }
                catch(Exception e)
                {
                    throw e;
                }

                _Foldout = EditorPrefs.GetBool(label.text);
            }
        }
        public static object FindValue(SerializedProperty prop, bool parent)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');

            IEnumerable<string> elementsToLoop;
            if(parent)
                elementsToLoop = elements.Take(elements.Length - 1);
            else
                elementsToLoop = elements;

            foreach(var element in elementsToLoop)
            {
                if(element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }
        public static object GetValue(object source, string name)
        {
            if(source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var baseType = type.BaseType;
            while(f == null && baseType != null)
            {
                f = baseType.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                baseType = baseType.BaseType;
            }
            if(f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                baseType = type.BaseType;
                while(p == null && baseType != null)
                {
                    p = baseType.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    baseType = baseType.BaseType;
                }
                if(p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }
        public static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while(index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }

        private static readonly Dictionary<Type, Func<Rect, object, object>> _Fields =
            new Dictionary<Type, Func<Rect, object, object>>()
        {
            { typeof(int), (rect, value) => EditorGUI.IntField(rect, (int)value) },
            { typeof(long), (rect, value) => EditorGUI.LongField(rect, (long)value) },
            { typeof(float), (rect, value) => EditorGUI.FloatField(rect, (float)value) },
            { typeof(string), (rect, value) => EditorGUI.TextField(rect, (string)value) },
            { typeof(bool), (rect, value) => EditorGUI.Toggle(rect, (bool)value) },
            { typeof(Vector2), (rect, value) => EditorGUI.Vector2Field(rect, GUIContent.none, (Vector2)value) },
            { typeof(Vector3), (rect, value) => EditorGUI.Vector3Field(rect, GUIContent.none, (Vector3)value) },
            { typeof(Bounds), (rect, value) => EditorGUI.BoundsField(rect, (Bounds)value) },
            { typeof(Rect), (rect, value) => EditorGUI.RectField(rect, (Rect)value) },
        };

        private static T DoField<T>(Rect rect, Type type, T value, SerializedProperty actualProperty)
        {
            Func<Rect, object, object> field;
            if(_Fields.TryGetValue(type, out field))
                return (T)field(rect, value);

            if(type.IsEnum)
                return (T)(object)EditorGUI.EnumPopup(rect, (Enum)(object)value);

            if(typeof(UnityObject).IsAssignableFrom(type))
                return (T)(object)EditorGUI.ObjectField(rect, (UnityObject)(object)value, type, true);

            if(actualProperty != null)
            {
                var temp = EditorGUIUtility.labelWidth;
                EditorGUIUtility.labelWidth = 1;
                //var r = rect;
                //r.x -= 100;
                EditorGUI.PropertyField(rect, actualProperty, GUIContent.none);
                EditorGUIUtility.labelWidth = temp;
                return (T)FindValue(actualProperty, false);
            }

            Debug.Log("Type is not supported: " + type);
            return value;
        }

        private void ClearDictionary()
        {
            _Dictionary.Clear();
            m_indexOfKey.Clear();
        }

        private void AddNewItem(SerializedProperty keysProperty, SerializedProperty valuesProperty)
        {
            TK key;
            if(typeof(TK) == typeof(string))
                key = (TK)(object)"";
            else key = default(TK);

            var value = default(TV);
            try
            {
                if(_Dictionary.TryAddInmediate(key, value))
                {
                    m_indexOfKey.Add(key, _Dictionary.Count - 1);
                    //keysProperty.InsertArrayElementAtIndex(keysProperty.arraySize);
                    //valuesProperty.InsertArrayElementAtIndex(valuesProperty.arraySize);
                }
            }
            catch(Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }


    [CustomPropertyDrawer(typeof(AssetReferenceDictionary))]
    public class AssetReferenceDictionaryDrawer : SerializableDictionaryDrawer<string, AssetReference> { }
}
