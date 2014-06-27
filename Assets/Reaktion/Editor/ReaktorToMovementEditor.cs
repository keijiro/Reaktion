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

[CustomEditor(typeof(ReaktorToMovement)), CanEditMultipleObjects]
public class ReaktorToMovementEditor : Editor
{
    SerializedProperty propAutoBind;
    SerializedProperty propReaktor;

    SerializedProperty propEnableTranslation;
    SerializedProperty propTranslationVector;
    SerializedProperty propVelocityCurve;

    SerializedProperty propEnableRotation;
    SerializedProperty propRotationAxis;
    SerializedProperty propAngularVelocityCurve;

    SerializedProperty propUseLocal;

    GUIContent labelTranslation;
    GUIContent labelRotation;
    GUIContent labelVector;
    GUIContent labelAxis;
    GUIContent labelVelocityCurve;
    GUIContent labelUseLocal;

    void OnEnable()
    {
        propAutoBind = serializedObject.FindProperty("autoBind");
        propReaktor  = serializedObject.FindProperty("reaktor");

        propEnableTranslation = serializedObject.FindProperty("enableTranslation");
        propTranslationVector = serializedObject.FindProperty("translationVector");
        propVelocityCurve     = serializedObject.FindProperty("velocityCurve");

        propEnableRotation       = serializedObject.FindProperty("enableRotation");
        propRotationAxis         = serializedObject.FindProperty("rotationAxis");
        propAngularVelocityCurve = serializedObject.FindProperty("angularVelocityCurve");

        propUseLocal = serializedObject.FindProperty("useLocal");

        labelTranslation   = new GUIContent("Translation");
        labelRotation      = new GUIContent("Rotation");
        labelVector        = new GUIContent("Vector");
        labelAxis          = new GUIContent("Axis");
        labelVelocityCurve = new GUIContent("Velocity Curve");
        labelUseLocal      = new GUIContent("Local Coordinate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propAutoBind);

        if (propAutoBind.hasMultipleDifferentValues || !propAutoBind.boolValue)
            EditorGUILayout.PropertyField(propReaktor);

        EditorGUILayout.PropertyField(propUseLocal, labelUseLocal);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propEnableTranslation, labelTranslation);
        if (propEnableTranslation.hasMultipleDifferentValues || propEnableTranslation.boolValue)
        {
            EditorGUILayout.PropertyField(propTranslationVector, labelVector);
            EditorGUILayout.PropertyField(propVelocityCurve, labelVelocityCurve);
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propEnableRotation, labelRotation);
        if (propEnableRotation.hasMultipleDifferentValues || propEnableRotation.boolValue)
        {
            EditorGUILayout.PropertyField(propRotationAxis, labelAxis);
            EditorGUILayout.PropertyField(propAngularVelocityCurve, labelVelocityCurve);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
