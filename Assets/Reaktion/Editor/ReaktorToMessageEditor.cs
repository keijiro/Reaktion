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

[CustomEditor(typeof(ReaktorToMessage)), CanEditMultipleObjects]
public class ReaktorToMessageEditor : Editor
{
    SerializedProperty propTarget;
    SerializedProperty propBroadcast;

    SerializedProperty propEnableTrigger;
    SerializedProperty propTriggerThreshold;
    SerializedProperty propTriggerInterval;
    SerializedProperty propTriggerMessage;
    
    SerializedProperty propEnableInput;
    SerializedProperty propInputCurve;
    SerializedProperty propMaxInput;
    SerializedProperty propInputMessage;

    GUIContent labelSendTo;
    GUIContent labelTriggerMessage;
    GUIContent labelInputMessage;
    GUIContent labelMessage;
    GUIContent labelCurve;

    void OnEnable()
    {
        propTarget    = serializedObject.FindProperty("target");
        propBroadcast = serializedObject.FindProperty("broadcast");

        propEnableTrigger    = serializedObject.FindProperty("enableTrigger");
        propTriggerThreshold = serializedObject.FindProperty("triggerThreshold");
        propTriggerInterval  = serializedObject.FindProperty("triggerInterval");
        propTriggerMessage   = serializedObject.FindProperty("triggerMessage");

        propEnableInput  = serializedObject.FindProperty("enableInput");
        propInputCurve   = serializedObject.FindProperty("inputCurve");
        propInputMessage = serializedObject.FindProperty("inputMessage");

        labelSendTo         = new GUIContent("Send To");
        labelTriggerMessage = new GUIContent("Trigger Message");
        labelInputMessage   = new GUIContent("Input Message");
        labelMessage        = new GUIContent("Message");
        labelCurve          = new GUIContent("Curve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propTarget, labelSendTo);
        EditorGUILayout.PropertyField(propBroadcast);
        if (propTarget.objectReferenceValue == null)
            EditorGUILayout.HelpBox("Leave None to send messages to itself (loopback)", MessageType.None);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propEnableTrigger, labelTriggerMessage);
        if (propEnableTrigger.hasMultipleDifferentValues || propEnableTrigger.boolValue)
        {
            EditorGUILayout.Slider(propTriggerThreshold, 0.0f, 1.0f, "Threahold");
            EditorGUILayout.Slider(propTriggerInterval, 0.0f, 1.0f, "Interval");
            EditorGUILayout.PropertyField(propTriggerMessage, labelMessage);
            EditorGUILayout.Space();
        }

        EditorGUILayout.PropertyField(propEnableInput, labelInputMessage);
        if (propEnableInput.hasMultipleDifferentValues || propEnableInput.boolValue)
        {
            EditorGUILayout.PropertyField(propInputCurve, labelCurve);
            EditorGUILayout.PropertyField(propInputMessage, labelMessage);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
