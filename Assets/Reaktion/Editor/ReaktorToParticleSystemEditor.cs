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

[CustomEditor(typeof(ReaktorToParticleSystem))]
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
    SerializedProperty propBurstInterval;
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
        propBurstInterval = serializedObject.FindProperty ("burstInterval");
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
            EditorGUILayout.Slider (propBurstInterval, 0.0f, 1.0f, "Interval");
            propBurstEmissionNumber.intValue = EditorGUILayout.IntField ("Particles", propBurstEmissionNumber.intValue);
            EditorGUILayout.Space ();
        }

        // Emission rate options.
        propEnableEmissionRate.boolValue = EditorGUILayout.Toggle ("Emission Rate", propEnableEmissionRate.boolValue);
        if (propEnableEmissionRate.boolValue)
        {
            propMaxEmissionRate.floatValue = EditorGUILayout.FloatField ("Max Rate", propMaxEmissionRate.floatValue);
            propEmissionRateCurve.animationCurveValue = EditorGUILayout.CurveField ("Rate Curve", propEmissionRateCurve.animationCurveValue);
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    #endregion
}
