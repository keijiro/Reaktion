//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(ReaktorToMaterial)), CanEditMultipleObjects]
public class ReaktorToMaterialEditor : Editor
{
    SerializedProperty propColorName;
    SerializedProperty propColorFrom;
    SerializedProperty propColorTo;

    SerializedProperty propFloatName;
    SerializedProperty propFloatFrom;
    SerializedProperty propFloatTo;

    SerializedProperty propVectorName;
    SerializedProperty propVectorFrom;
    SerializedProperty propVectorTo;

    GUIContent labelFrom;
    GUIContent labelTo;

    void OnEnable()
    {
        propColorName = serializedObject.FindProperty("colorName");
        propColorFrom = serializedObject.FindProperty("colorFrom");
        propColorTo   = serializedObject.FindProperty("colorTo");

        propFloatName = serializedObject.FindProperty("floatName");
        propFloatFrom = serializedObject.FindProperty("floatFrom");
        propFloatTo   = serializedObject.FindProperty("floatTo");

        propVectorName = serializedObject.FindProperty("vectorName");
        propVectorFrom = serializedObject.FindProperty("vectorFrom");
        propVectorTo   = serializedObject.FindProperty("vectorTo");

        labelFrom = new GUIContent("From");
        labelTo   = new GUIContent("To");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update ();

        EditorGUILayout.PropertyField(propColorName);
        if (propColorName.hasMultipleDifferentValues || propColorName.stringValue.Length > 0)
        {
            EditorGUILayout.PropertyField(propColorFrom, labelFrom);
            EditorGUILayout.PropertyField(propColorTo, labelTo);
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(propFloatName);
        if (propFloatName.hasMultipleDifferentValues || propFloatName.stringValue.Length > 0)
        {
            EditorGUILayout.PropertyField(propFloatFrom, labelFrom);
            EditorGUILayout.PropertyField(propFloatTo, labelTo);
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(propVectorName);
        if (propVectorName.hasMultipleDifferentValues || propVectorName.stringValue.Length > 0)
        {
            EditorGUILayout.PropertyField(propVectorFrom, labelFrom, true);
            EditorGUILayout.PropertyField(propVectorTo, labelTo, true);
        }

        serializedObject.ApplyModifiedProperties ();
    }
}
