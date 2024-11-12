using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(IComponentData), true)]
public class ComponentDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Type t = GetFieldType();

        string typeName = property.managedReferenceValue?.GetType().Name ?? "Not set";

        Rect dropdownRect = position;
        dropdownRect.x += EditorGUIUtility.labelWidth + 2;
        dropdownRect.width -= EditorGUIUtility.labelWidth + 2;
        dropdownRect.height = EditorGUIUtility.singleLineHeight;
        if (EditorGUI.DropdownButton(dropdownRect, new(typeName), UnityEngine.FocusType.Keyboard))
        {
            GenericMenu menu = new GenericMenu();

            // null
            menu.AddItem(new GUIContent("None"), property.managedReferenceValue == null, () =>
            {
                property.managedReferenceValue = null;
                property.serializedObject.ApplyModifiedProperties();
            });

            // inherited types
            foreach (Type type in FindDerivedTypes(t))
            {
                menu.AddItem(new GUIContent(type.Name), typeName == type.Name, () =>
                {
                    property.managedReferenceValue = type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.ShowAsContext();
        }

        EditorGUI.PropertyField(position, property, label, true);
    }

    private Type GetFieldType()
    {
        Type type = fieldInfo.FieldType;
        if (type.IsArray) type = type.GetElementType();
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            type = type.GetGenericArguments()[0];
        return type;
    }

    private List<Type> FindDerivedTypes(Type baseType)
    {
        return Assembly
            .GetAssembly(baseType)
            .GetTypes()
            .Where(t =>
                baseType.IsAssignableFrom(t) &&
                t.IsClass &&
                !t.IsAbstract &&
                !typeof(UnityEngine.Object).IsAssignableFrom(t))
            .ToList();
    }
}