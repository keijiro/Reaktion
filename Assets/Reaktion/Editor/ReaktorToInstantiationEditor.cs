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

[CustomEditor(typeof(ReaktorToInstantiation)), CanEditMultipleObjects]
public class ReaktorToInstantiationEditor : Editor
{
    SerializedProperty propPrefabs;
    SerializedProperty propRandomRotation;
    SerializedProperty propParent;

    SerializedProperty propEnableBurst;
    SerializedProperty propBurstThreshold;
    SerializedProperty propBurstInterval;
    SerializedProperty propBurstNumber;

    SerializedProperty propEnableTimer;
    SerializedProperty propMaxRate;
    SerializedProperty propRateCurve;

    SerializedProperty propRangeType;
    SerializedProperty propRangeRadius;
    SerializedProperty propRangeVector;

    GUIContent labelEnableBurst;
    GUIContent labelBurstThreshold;
    GUIContent labelBurstInterval;
    GUIContent labelBurstNumber;

    GUIContent labelEnableTimer;
    GUIContent labelMaxRate;

    GUIContent labelRangeRadius;
    GUIContent labelRangeVector;

    void OnEnable ()
    {
        propPrefabs        = serializedObject.FindProperty("prefabs");
        propRandomRotation = serializedObject.FindProperty("randomRotation");
        propParent         = serializedObject.FindProperty("parent");

        propEnableBurst    = serializedObject.FindProperty("enableBurst");
        propBurstThreshold = serializedObject.FindProperty("burstThreshold");
        propBurstInterval  = serializedObject.FindProperty("burstInterval");
        propBurstNumber    = serializedObject.FindProperty("burstNumber");

        propEnableTimer    = serializedObject.FindProperty("enableTimer");
        propMaxRate        = serializedObject.FindProperty("maxRate");
        propRateCurve      = serializedObject.FindProperty("rateCurve");

        propRangeType      = serializedObject.FindProperty("rangeType");
        propRangeRadius    = serializedObject.FindProperty("rangeRadius");
        propRangeVector    = serializedObject.FindProperty("rangeVector");

        labelEnableBurst    = new GUIContent("Burst");
        labelBurstThreshold = new GUIContent("Threshold");
        labelBurstInterval  = new GUIContent("Minimum Interval");
        labelBurstNumber    = new GUIContent("Instance Count");

        labelEnableTimer    = new GUIContent("Time-Based");
        labelMaxRate        = new GUIContent("Maximum Rate");

        labelRangeRadius    = new GUIContent("Radius");
        labelRangeVector    = new GUIContent("Extent");
    }

    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();

        EditorGUILayout.PropertyField(propPrefabs, true);
        EditorGUILayout.Space();

        // Burst instantiation
        EditorGUILayout.PropertyField(propEnableBurst, labelEnableBurst);
        if (propEnableBurst.boolValue || propEnableBurst.hasMultipleDifferentValues)
        {
            EditorGUILayout.Slider(propBurstThreshold, 0.0f, 1.0f, labelBurstThreshold);
            EditorGUILayout.PropertyField(propBurstInterval, labelBurstInterval);
            EditorGUILayout.PropertyField(propBurstNumber, labelBurstNumber);
            EditorGUILayout.Space();
        }

        // Time-based instantiation
        EditorGUILayout.PropertyField(propEnableTimer, labelEnableTimer);
        if (propEnableTimer.boolValue || propEnableTimer.hasMultipleDifferentValues)
        {
            EditorGUILayout.PropertyField(propMaxRate, labelMaxRate);
            EditorGUILayout.PropertyField(propRateCurve);
        }

        EditorGUILayout.Space();

        // Range information
        EditorGUILayout.PropertyField(propRangeType);
        {
            var isSphere = (propRangeType.enumValueIndex == (int)ReaktorToInstantiation.RangeType.Sphere);
            var showBoth = propRangeType.hasMultipleDifferentValues;
            if ( isSphere || showBoth) EditorGUILayout.PropertyField(propRangeRadius, labelRangeRadius);
            if (!isSphere || showBoth) EditorGUILayout.PropertyField(propRangeVector, labelRangeVector);
        }
        EditorGUILayout.PropertyField(propRandomRotation);
        EditorGUILayout.PropertyField(propParent);
        serializedObject.ApplyModifiedProperties ();
    }
}
