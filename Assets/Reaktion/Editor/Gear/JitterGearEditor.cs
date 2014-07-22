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

namespace Reaktion {

[CustomEditor(typeof(JitterGear)), CanEditMultipleObjects]
public class JitterGearEditor : Editor
{
    SerializedProperty propAutoBind;
    SerializedProperty propReaktor;

    SerializedProperty propChangePositionFrequency;
    SerializedProperty propPositionFrequencyMin;
    SerializedProperty propPositionFrequencyMax;
    SerializedProperty propPositionFrequencyCurve;

    SerializedProperty propChangeRotationFrequency;
    SerializedProperty propRotationFrequencyMin;
    SerializedProperty propRotationFrequencyMax;
    SerializedProperty propRotationFrequencyCurve;

    SerializedProperty propChangePositionAmount;
    SerializedProperty propPositionAmountMin;
    SerializedProperty propPositionAmountMax;
    SerializedProperty propPositionAmountCurve;

    SerializedProperty propChangeRotationAmount;
    SerializedProperty propRotationAmountMin;
    SerializedProperty propRotationAmountMax;
    SerializedProperty propRotationAmountCurve;

    GUIContent labelChangePositionFrequency;
    GUIContent labelChangeRotationFrequency;
    GUIContent labelChangePositionAmount;
    GUIContent labelChangeRotationAmount;
    GUIContent labelMin;
    GUIContent labelMax;
    GUIContent labelCurve;

    bool showPositionFrequency = true;
    bool showRotationFrequency = true;
    bool showPositionAmount = true;
    bool showRotationAmount = true;

    void OnEnable()
    {
        propAutoBind = serializedObject.FindProperty("autoBind");
        propReaktor  = serializedObject.FindProperty("reaktor");

        propChangePositionFrequency = serializedObject.FindProperty("changePositionFrequency");
        propPositionFrequencyMin    = serializedObject.FindProperty("positionFrequencyMin");
        propPositionFrequencyMax    = serializedObject.FindProperty("positionFrequencyMax");
        propPositionFrequencyCurve  = serializedObject.FindProperty("positionFrequencyCurve");

        propChangeRotationFrequency = serializedObject.FindProperty("changeRotationFrequency");
        propRotationFrequencyMin    = serializedObject.FindProperty("rotationFrequencyMin");
        propRotationFrequencyMax    = serializedObject.FindProperty("rotationFrequencyMax");
        propRotationFrequencyCurve  = serializedObject.FindProperty("rotationFrequencyCurve");

        propChangePositionAmount = serializedObject.FindProperty("changePositionAmount");
        propPositionAmountMin    = serializedObject.FindProperty("positionAmountMin");
        propPositionAmountMax    = serializedObject.FindProperty("positionAmountMax");
        propPositionAmountCurve  = serializedObject.FindProperty("positionAmountCurve");

        propChangeRotationAmount = serializedObject.FindProperty("changeRotationAmount");
        propRotationAmountMin   = serializedObject.FindProperty("rotationAmountMin");
        propRotationAmountMax   = serializedObject.FindProperty("rotationAmountMax");
        propRotationAmountCurve = serializedObject.FindProperty("rotationAmountCurve");

        labelChangePositionFrequency = new GUIContent("Position Frequency");
        labelChangeRotationFrequency = new GUIContent("Rotation Frequency");
        labelChangePositionAmount = new GUIContent("Position Amount");
        labelChangeRotationAmount = new GUIContent("Rotation Amount");
        labelMin = new GUIContent("Minimum Value");
        labelMax = new GUIContent("Maximum Value");
        labelCurve = new GUIContent("Curve");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propAutoBind);
        if (propAutoBind.hasMultipleDifferentValues || !propAutoBind.boolValue)
            EditorGUILayout.PropertyField(propReaktor);

        EditorGUILayout.PropertyField(propChangePositionFrequency, labelChangePositionFrequency);
        if (propChangePositionFrequency.hasMultipleDifferentValues || propChangePositionFrequency.boolValue)
        {
            EditorGUILayout.PropertyField(propPositionFrequencyMin, labelMin);
            EditorGUILayout.PropertyField(propPositionFrequencyMax, labelMax);
            EditorGUILayout.PropertyField(propPositionFrequencyCurve, labelCurve);
        }

        EditorGUILayout.PropertyField(propChangeRotationFrequency, labelChangeRotationFrequency);
        if (propChangeRotationFrequency.hasMultipleDifferentValues || propChangeRotationFrequency.boolValue)
        {
            EditorGUILayout.PropertyField(propRotationFrequencyMin, labelMin);
            EditorGUILayout.PropertyField(propRotationFrequencyMax, labelMax);
            EditorGUILayout.PropertyField(propRotationFrequencyCurve, labelCurve);
        }

        EditorGUILayout.PropertyField(propChangePositionAmount, labelChangePositionAmount);
        if (propChangePositionAmount.hasMultipleDifferentValues || propChangePositionAmount.boolValue)
        {
            EditorGUILayout.PropertyField(propPositionAmountMin, labelMin);
            EditorGUILayout.PropertyField(propPositionAmountMax, labelMax);
            EditorGUILayout.PropertyField(propPositionAmountCurve, labelCurve);
        }

        EditorGUILayout.PropertyField(propChangeRotationAmount, labelChangeRotationAmount);
        if (propChangeRotationAmount.hasMultipleDifferentValues || propChangeRotationAmount.boolValue)
        {
            EditorGUILayout.PropertyField(propRotationAmountMin, labelMin);
            EditorGUILayout.PropertyField(propRotationAmountMax, labelMax);
            EditorGUILayout.PropertyField(propRotationAmountCurve, labelCurve);
        }

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
