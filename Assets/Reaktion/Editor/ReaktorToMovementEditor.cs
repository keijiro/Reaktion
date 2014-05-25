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

[CustomEditor(typeof(ReaktorToMovement)), CanEditMultipleObjects]
public class ReaktorToMovementEditor : Editor
{
    SerializedProperty propEnableTranslation;
    SerializedProperty propMinVelocity;
    SerializedProperty propMaxVelocity;

    SerializedProperty propEnableRotation;
    SerializedProperty propRotationAxis;
    SerializedProperty propMinAngularVelocity;
    SerializedProperty propMaxAngularVelocity;

    SerializedProperty propUseLocal;

    GUIContent labelTranslation;
    GUIContent labelRotation;
    GUIContent labelMinVelocity;
    GUIContent labelMaxVelocity;
    GUIContent labelAxis;
    GUIContent labelUseLocal;

    void OnEnable()
    {
        propEnableTranslation  = serializedObject.FindProperty("enableTranslation");
        propMinVelocity        = serializedObject.FindProperty("minVelocity");
        propMaxVelocity        = serializedObject.FindProperty("maxVelocity");

        propEnableRotation     = serializedObject.FindProperty("enableRotation");
        propRotationAxis       = serializedObject.FindProperty("rotationAxis");
        propMinAngularVelocity = serializedObject.FindProperty("minAngularVelocity");
        propMaxAngularVelocity = serializedObject.FindProperty("maxAngularVelocity");

        propUseLocal           = serializedObject.FindProperty("useLocal");

        labelTranslation  = new GUIContent("Translation");
        labelRotation     = new GUIContent("Rotation");
        labelMinVelocity  = new GUIContent("Minimum Velocity");
        labelMaxVelocity  = new GUIContent("Maximum Velocity");
        labelAxis         = new GUIContent("Axis");
        labelUseLocal     = new GUIContent("Local Coordinate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        bool shouldSpace = 
            propEnableTranslation.hasMultipleDifferentValues ||
            propEnableTranslation.boolValue || propEnableRotation.boolValue;

        EditorGUILayout.PropertyField(propEnableTranslation, labelTranslation);
        if (propEnableTranslation.hasMultipleDifferentValues || propEnableTranslation.boolValue)
        {
            EditorGUILayout.PropertyField(propMinVelocity, labelMinVelocity);
            EditorGUILayout.PropertyField(propMaxVelocity, labelMaxVelocity);
        }

        if (shouldSpace) EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propEnableRotation, labelRotation);
        if (propEnableRotation.hasMultipleDifferentValues || propEnableRotation.boolValue)
        {
            EditorGUILayout.PropertyField(propRotationAxis, labelAxis);
            EditorGUILayout.PropertyField(propMinAngularVelocity, labelMinVelocity);
            EditorGUILayout.PropertyField(propMaxAngularVelocity, labelMaxVelocity);
        }

        if (shouldSpace) EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propUseLocal, labelUseLocal);

        serializedObject.ApplyModifiedProperties ();
    }
}
