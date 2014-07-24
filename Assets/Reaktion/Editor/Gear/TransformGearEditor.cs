//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
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

namespace Reaktion {

[CustomEditor(typeof(TransformGear)), CanEditMultipleObjects]
public class TransformGearEditor : Editor
{
    SerializedProperty propAutoBind;
    SerializedProperty propReaktor;

    SerializedProperty propPosition;
    SerializedProperty propPositionVector;

    SerializedProperty propRotation;
    SerializedProperty propRotationAxis;

    SerializedProperty propScale;
    SerializedProperty propScaleVector;

    SerializedProperty propAddInitialValue;

    void OnEnable()
    {
        propAutoBind = serializedObject.FindProperty("autoBind");
        propReaktor  = serializedObject.FindProperty("reaktor");

        propPosition       = serializedObject.FindProperty("position");
        propPositionVector = serializedObject.FindProperty("positionVector");

        propRotation     = serializedObject.FindProperty("rotation");
        propRotationAxis = serializedObject.FindProperty("rotationAxis");

        propScale       = serializedObject.FindProperty("scale");
        propScaleVector = serializedObject.FindProperty("scaleVector");

        propAddInitialValue = serializedObject.FindProperty("addInitialValue");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propAutoBind);
        if (propAutoBind.hasMultipleDifferentValues || !propAutoBind.boolValue)
            EditorGUILayout.PropertyField(propReaktor);

        EditorGUILayout.PropertyField(propPosition);
        if (propPosition.hasMultipleDifferentValues ||
            propPosition.FindPropertyRelative("enabled").boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(propPositionVector, GUIContent.none);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(propRotation);
        if (propRotation.hasMultipleDifferentValues ||
            propRotation.FindPropertyRelative("enabled").boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(propRotationAxis, GUIContent.none);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(propScale);
        if (propScale.hasMultipleDifferentValues ||
            propScale.FindPropertyRelative("enabled").boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(propScaleVector, GUIContent.none);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.PropertyField(propAddInitialValue);

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
