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

[CustomEditor(typeof(ReaktorToParticleSystem)), CanEditMultipleObjects]
public class ReaktorToParticleSystemEditor : Editor
{
    #region References to the properties

    // Emission rate options.
    SerializedProperty propEnableEmissionRate;
    SerializedProperty propMaxEmissionRate;
    SerializedProperty propEmissionRateCurve;

    // Burst options.
    SerializedProperty propEnableBurst;
    SerializedProperty propBurstThreshold;
    SerializedProperty propBurstEmissionNumber;

    #endregion

    #region Editor functions

    void OnEnable ()
    {
        // Emission rate options.
        propEnableEmissionRate = serializedObject.FindProperty ("enableEmissionRate");
        propMaxEmissionRate = serializedObject.FindProperty ("maxEmissionRate");
        propEmissionRateCurve = serializedObject.FindProperty ("emissionRateCurve");

        // Burst options.
        propEnableBurst = serializedObject.FindProperty ("enableBurst");
        propBurstThreshold = serializedObject.FindProperty ("burstThreshold");
        propBurstEmissionNumber = serializedObject.FindProperty ("burstEmissionNumber");
    }

    public override void OnInspectorGUI ()
    {
        // Update the references.
        serializedObject.Update ();

        // Burst options.
        propEnableBurst.boolValue = EditorGUILayout.Toggle ("Burst", propEnableBurst.boolValue);
        if (propEnableBurst.boolValue)
        {
            EditorGUILayout.Slider (propBurstThreshold, 0.0f, 1.0f, "Threshold");
            EditorGUILayout.PropertyField (propBurstEmissionNumber, new GUIContent ("Particles"));
        }

        // Emission rate options.
        propEnableEmissionRate.boolValue = EditorGUILayout.Toggle ("Emission Rate", propEnableEmissionRate.boolValue);
        if (propEnableEmissionRate.boolValue)
        {
            EditorGUILayout.PropertyField (propMaxEmissionRate, new GUIContent ("Max Rate"));
            EditorGUILayout.PropertyField (propEmissionRateCurve, new GUIContent ("Rate Curve"));
            EditorGUILayout.Space ();
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    #endregion
}
