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

[CustomEditor(typeof(ReaktorToAnimator)), CanEditMultipleObjects]
public class ReaktorToAnimatorEditor : Editor
{
    #region References to the properties

    // Speed control.
    SerializedProperty propEnableSpeed;
    SerializedProperty propMaxSpeed;
    SerializedProperty propSpeedCurve;

    // Trigger control.
    SerializedProperty propEnableTrigger;
    SerializedProperty propTriggerThreshold;
    SerializedProperty propTriggerName;

    #endregion

    #region Editor functions

    void OnEnable ()
    {
        // Speed control.
        propEnableSpeed = serializedObject.FindProperty ("enableSpeed");
        propMaxSpeed = serializedObject.FindProperty ("maxSpeed");
        propSpeedCurve = serializedObject.FindProperty ("speedCurve");

        // Trigger control.
        propEnableTrigger = serializedObject.FindProperty ("enableTrigger");
        propTriggerThreshold = serializedObject.FindProperty ("triggerThreshold");
        propTriggerName = serializedObject.FindProperty ("triggerName");
    }

    public override void OnInspectorGUI ()
    {
        // Update the properties.
        serializedObject.Update ();
        
        // Speed control.
        propEnableSpeed.boolValue = EditorGUILayout.Toggle ("Speed Control", propEnableSpeed.boolValue);
        if (propEnableSpeed.boolValue)
        {
            propMaxSpeed.floatValue = EditorGUILayout.FloatField ("Max Speed", propMaxSpeed.floatValue);
            propSpeedCurve.animationCurveValue = EditorGUILayout.CurveField ("Speed Curve", propSpeedCurve.animationCurveValue);
            EditorGUILayout.Space ();
        }

        // Trigger control.
        propEnableTrigger.boolValue = EditorGUILayout.Toggle ("Trigger", propEnableTrigger.boolValue);
        if (propEnableTrigger.boolValue)
        {
            EditorGUILayout.Slider (propTriggerThreshold, 0.0f, 1.0f, "Threshold");
            propTriggerName.stringValue = EditorGUILayout.TextField ("Trigger Name", propTriggerName.stringValue);
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    #endregion
}
