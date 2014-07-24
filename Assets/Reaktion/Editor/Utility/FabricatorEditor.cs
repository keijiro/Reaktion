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

[CustomEditor(typeof(Fabricator)), CanEditMultipleObjects]
public class FabricatorEditor : Editor
{
    SerializedProperty propPrefabs;
    SerializedProperty propInstantiationRate;
    SerializedProperty propRandomRotation;
    SerializedProperty propParent;

    SerializedProperty propRangeType;
    SerializedProperty propRangeRadius;
    SerializedProperty propRangeVector;

    GUIContent labelRangeRadius;
    GUIContent labelParent;

    void OnEnable()
    {
        propPrefabs           = serializedObject.FindProperty("prefabs");
        propInstantiationRate = serializedObject.FindProperty("instantiationRate");
        propRandomRotation    = serializedObject.FindProperty("randomRotation");
        propParent            = serializedObject.FindProperty("parent");

        propRangeType   = serializedObject.FindProperty("rangeType");
        propRangeRadius = serializedObject.FindProperty("rangeRadius");
        propRangeVector = serializedObject.FindProperty("rangeVector");

        labelRangeRadius = new GUIContent("Radius");
        labelParent      = new GUIContent("Set Parent");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propPrefabs, true);
        EditorGUILayout.PropertyField(propInstantiationRate);
        EditorGUILayout.PropertyField(propRangeType);

        EditorGUI.indentLevel++;
        var isSphere = (propRangeType.enumValueIndex == (int)Fabricator.RangeType.Sphere);
        var showBoth = propRangeType.hasMultipleDifferentValues;
        if ( isSphere || showBoth) EditorGUILayout.PropertyField(propRangeRadius, labelRangeRadius);
        if (!isSphere || showBoth) EditorGUILayout.PropertyField(propRangeVector, GUIContent.none);
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(propRandomRotation);
        EditorGUILayout.PropertyField(propParent, labelParent);

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
