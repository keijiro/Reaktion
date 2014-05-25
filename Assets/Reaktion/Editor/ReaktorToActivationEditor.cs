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

[CustomEditor(typeof(ReaktorToActivation)), CanEditMultipleObjects]
public class ReaktorToActivationEditor : Editor
{
    SerializedProperty propTargetType;
    SerializedProperty propTargetGameObjects;
    SerializedProperty propTargetComponents;
    SerializedProperty propThreshold;
    SerializedProperty propInterval;
    SerializedProperty propInvert;

    GUIContent labelTargetType;
    GUIContent labelTargetList;
    GUIContent labelInterval;
    GUIContent labelInvert;

    void OnEnable()
    {
        propTargetType        = serializedObject.FindProperty("targetType");
        propTargetGameObjects = serializedObject.FindProperty("targetGameObjects");
        propTargetComponents  = serializedObject.FindProperty("targetComponents");
        propThreshold         = serializedObject.FindProperty("threshold");
        propInterval          = serializedObject.FindProperty("interval");
        propInvert            = serializedObject.FindProperty("invert");

        labelTargetType = new GUIContent("Target Type");
        labelTargetList = new GUIContent("Target List");
        labelInterval   = new GUIContent("Minimum Interval");
        labelInvert     = new GUIContent("Invert (low true)");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update ();

        EditorGUILayout.PropertyField(propTargetType, labelTargetType);

        if (propTargetType.hasMultipleDifferentValues)
        {
            EditorGUILayout.PropertyField(propTargetGameObjects, true);
            EditorGUILayout.PropertyField(propTargetComponents, true);
        }
        else if (propTargetType.enumValueIndex == (int)ReaktorToActivation.TargetType.GameObject)
        {
            EditorGUILayout.PropertyField(propTargetGameObjects, labelTargetList, true);
        }
        else
        {
            EditorGUILayout.PropertyField(propTargetComponents, labelTargetList, true);
        }

        EditorGUILayout.Space();

        EditorGUILayout.Slider(propThreshold, 0.01f, 0.99f, "Threshold");
        EditorGUILayout.PropertyField(propInterval, labelInterval);
        EditorGUILayout.PropertyField(propInvert, labelInvert);

        serializedObject.ApplyModifiedProperties ();
    }
}
