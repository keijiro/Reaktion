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

[CustomEditor(typeof(ReaktorToParticleSystem)), CanEditMultipleObjects]
public class ReaktorToParticleSystemEditor : Editor
{
    SerializedProperty propEnableBurst;
    SerializedProperty propBurstThreshold;
    SerializedProperty propBurstInterval;
    SerializedProperty propBurstEmissionNumber;

    SerializedProperty propEnableEmissionRate;
    SerializedProperty propEmissionRateCurve;

    GUIContent labelBurst;
    GUIContent labelParticles;
    GUIContent labelEmissionRate;
    GUIContent labelCurve;

    void OnEnable()
    {
        propEnableBurst         = serializedObject.FindProperty("enableBurst");
        propBurstThreshold      = serializedObject.FindProperty("burstThreshold");
        propBurstInterval       = serializedObject.FindProperty("burstInterval");
        propBurstEmissionNumber = serializedObject.FindProperty("burstEmissionNumber");

        propEnableEmissionRate = serializedObject.FindProperty("enableEmissionRate");
        propEmissionRateCurve  = serializedObject.FindProperty("emissionRateCurve");

        labelBurst        = new GUIContent("Burst");
        labelParticles    = new GUIContent("Particles");
        labelEmissionRate = new GUIContent("Emission Rate");
        labelCurve        = new GUIContent("Curve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propEnableBurst, labelBurst);
        if (propEnableBurst.boolValue || propEnableBurst.hasMultipleDifferentValues)
        {
            EditorGUILayout.Slider(propBurstThreshold, 0.0f, 1.0f, "Threshold");
            EditorGUILayout.Slider(propBurstInterval, 0.0f, 1.0f, "Interval");
            EditorGUILayout.PropertyField(propBurstEmissionNumber, labelParticles);
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(propEnableEmissionRate, labelEmissionRate);
        if (propEnableEmissionRate.boolValue || propEnableEmissionRate.hasMultipleDifferentValues)
            EditorGUILayout.PropertyField(propEmissionRateCurve, labelCurve);

        serializedObject.ApplyModifiedProperties();
    }
}
