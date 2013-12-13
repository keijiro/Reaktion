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

[CustomEditor(typeof(ReaktorToTransform)), CanEditMultipleObjects]
public class ReaktorToTransformEditor : Editor
{
    #region References to the properties

    // Position control.
    SerializedProperty propEnablePosition;
    SerializedProperty propPosition;

    // Rotation control.
    SerializedProperty propEnableRotation;
    SerializedProperty propRotationAxis;
    SerializedProperty propMinAngle;
    SerializedProperty propMaxAngle;

    // Scale control.
    SerializedProperty propEnableScale;
    SerializedProperty propScale;

    #endregion

    #region Editor functions

    void OnEnable ()
    {
        // Position control.
        propEnablePosition = serializedObject.FindProperty ("enablePosition");
        propPosition = serializedObject.FindProperty ("position");

        // Rotation control.
        propEnableRotation = serializedObject.FindProperty ("enableRotation");
        propRotationAxis = serializedObject.FindProperty ("rotationAxis");
        propMinAngle = serializedObject.FindProperty ("minAngle");
        propMaxAngle = serializedObject.FindProperty ("maxAngle");

        // Scale control.
        propEnableScale = serializedObject.FindProperty ("enableScale");
        propScale = serializedObject.FindProperty ("scale");
    }

    public override void OnInspectorGUI ()
    {
        // Update the references.
        serializedObject.Update ();
        
        // Position control.
        propEnablePosition.boolValue = EditorGUILayout.Toggle ("Translation", propEnablePosition.boolValue);
        if (propEnablePosition.boolValue)
        {
            propPosition.vector3Value = EditorGUILayout.Vector3Field("Vector", propPosition.vector3Value);
            EditorGUILayout.Space ();
        }

        // Rotation control.
        propEnableRotation.boolValue = EditorGUILayout.Toggle ("Rotation", propEnableRotation.boolValue);
        if (propEnableRotation.boolValue)
        {
            propRotationAxis.vector3Value = EditorGUILayout.Vector3Field("Axis", propRotationAxis.vector3Value);
            propMinAngle.floatValue = EditorGUILayout.FloatField("Min Angle", propMinAngle.floatValue);
            propMaxAngle.floatValue = EditorGUILayout.FloatField("Max Angle", propMaxAngle.floatValue);
            EditorGUILayout.Space ();
        }

        // Scale control.
        propEnableScale.boolValue = EditorGUILayout.Toggle ("Scaling", propEnableScale.boolValue);
        if (propEnableScale.boolValue)
        {
            propScale.vector3Value = EditorGUILayout.Vector3Field("Vector", propScale.vector3Value);
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    #endregion
}
