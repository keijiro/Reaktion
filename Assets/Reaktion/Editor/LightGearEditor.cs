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

[CustomEditor(typeof(LightGear)), CanEditMultipleObjects]
public class LightGearEditor : Editor
{
    SerializedProperty propAutoBind;
    SerializedProperty propReaktor;
    SerializedProperty propEnableIntensity;
    SerializedProperty propIntensityCurve;
    SerializedProperty propEnableColor;
    SerializedProperty propColorGradient;

    GUIContent labelIntensity;
    GUIContent labelCurve;
    GUIContent labelColor;
    GUIContent labelGradient;

    void OnEnable()
    {
        propAutoBind        = serializedObject.FindProperty("autoBind");
        propReaktor         = serializedObject.FindProperty("reaktor");
        propEnableIntensity = serializedObject.FindProperty("enableIntensity");
        propIntensityCurve  = serializedObject.FindProperty("intensityCurve");
        propEnableColor     = serializedObject.FindProperty("enableColor");
        propColorGradient   = serializedObject.FindProperty("colorGradient");

        labelIntensity = new GUIContent("Intensity");
        labelCurve     = new GUIContent("Curve");
        labelColor     = new GUIContent("Color");
        labelGradient  = new GUIContent("Gradient");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propAutoBind);
        if (propAutoBind.hasMultipleDifferentValues || !propAutoBind.boolValue)
            EditorGUILayout.PropertyField(propReaktor);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propEnableIntensity, labelIntensity);
        if (propEnableIntensity.hasMultipleDifferentValues || propEnableIntensity.boolValue)
            EditorGUILayout.PropertyField(propIntensityCurve, labelCurve);

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propEnableColor, labelColor);
        if (propEnableColor.hasMultipleDifferentValues || propEnableColor.boolValue)
            EditorGUILayout.PropertyField(propColorGradient, labelGradient);

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
