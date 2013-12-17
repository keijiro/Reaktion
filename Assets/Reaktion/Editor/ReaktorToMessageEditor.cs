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

[CustomEditor(typeof(ReaktorToMessage)), CanEditMultipleObjects]
public class ReaktorToMessageEditor : Editor
{
    #region References to the properties

    // Target settings.
    SerializedProperty propTarget;
    SerializedProperty propBroadcast;

    // Trigger message.
    SerializedProperty propEnableTrigger;
    SerializedProperty propTriggerThreshold;
    SerializedProperty propTriggerInterval;
    SerializedProperty propTriggerMessage;
    
    // Input message.
    SerializedProperty propEnableInput;
    SerializedProperty propInputCurve;
    SerializedProperty propMaxInput;
    SerializedProperty propInputMessage;

    #endregion

    #region Editor functions

    void OnEnable ()
    {
        // Target settings.
        propTarget = serializedObject.FindProperty ("target");
        propBroadcast = serializedObject.FindProperty ("broadcast");

        // Trigger message.
        propEnableTrigger = serializedObject.FindProperty ("enableTrigger");
        propTriggerThreshold = serializedObject.FindProperty ("triggerThreshold");
        propTriggerInterval = serializedObject.FindProperty ("triggerInterval");
        propTriggerMessage = serializedObject.FindProperty ("triggerMessage");

        // Input message.
        propEnableInput = serializedObject.FindProperty ("enableInput");
        propInputCurve = serializedObject.FindProperty ("inputCurve");
        propInputMessage = serializedObject.FindProperty ("inputMessage");
    }

    public override void OnInspectorGUI ()
    {
        // Update the references.
        serializedObject.Update ();

        // Target settings.
        propTarget.objectReferenceValue = EditorGUILayout.ObjectField ("Send To", propTarget.objectReferenceValue, typeof(GameObject), true);
        if (propTarget.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox ("Leave None to send messages to itself (loopback)", MessageType.None);
        }
        else
        {
            propBroadcast.boolValue = EditorGUILayout.Toggle ("Broadcast", propBroadcast.boolValue);
        }
        EditorGUILayout.Space ();

        // Trigger message.
        propEnableTrigger.boolValue = EditorGUILayout.Toggle ("Trigger Message", propEnableTrigger.boolValue);
        if (propEnableTrigger.boolValue)
        {
            EditorGUILayout.Slider (propTriggerThreshold, 0.0f, 1.0f, "Threahold");
            EditorGUILayout.Slider (propTriggerInterval, 0.0f, 1.0f, "Interval");
            propTriggerMessage.stringValue = EditorGUILayout.TextField ("Message", propTriggerMessage.stringValue);
            EditorGUILayout.Space ();
        }
        
        // Input message.
        propEnableInput.boolValue = EditorGUILayout.Toggle ("Input Message", propEnableInput.boolValue);
        if (propEnableInput.boolValue)
        {
            propInputCurve.animationCurveValue = EditorGUILayout.CurveField ("Input Curve", propInputCurve.animationCurveValue);
            propInputMessage.stringValue = EditorGUILayout.TextField ("Message", propInputMessage.stringValue);
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    #endregion
}
