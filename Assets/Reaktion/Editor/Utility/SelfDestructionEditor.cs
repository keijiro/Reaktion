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

[CustomEditor(typeof(SelfDestruction)), CanEditMultipleObjects]
public class SelfDestructionEditor : Editor
{
    SerializedProperty propConditionType;
    SerializedProperty propReferenceType;

    SerializedProperty propDistance;
    SerializedProperty propBounds;

    SerializedProperty propReferencePoint;
    SerializedProperty propReferenceObject;
    SerializedProperty propReferenceName;

    void OnEnable()
    {
        propConditionType = serializedObject.FindProperty("conditionType");
        propReferenceType = serializedObject.FindProperty("referenceType");

        propDistance = serializedObject.FindProperty("distance");
        propBounds   = serializedObject.FindProperty("bounds");

        propReferencePoint  = serializedObject.FindProperty("referencePoint");
        propReferenceObject = serializedObject.FindProperty("referenceObject");
        propReferenceName   = serializedObject.FindProperty("referenceName");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(propConditionType);

        if (propConditionType.hasMultipleDifferentValues ||
            propConditionType.enumValueIndex == (int)SelfDestruction.ConditionType.Distance)
            EditorGUILayout.PropertyField(propDistance);

        if (propConditionType.hasMultipleDifferentValues ||
            propConditionType.enumValueIndex == (int)SelfDestruction.ConditionType.Bounds)
            EditorGUILayout.PropertyField(propBounds);

        EditorGUILayout.PropertyField(propReferenceType);

        if (propReferenceType.hasMultipleDifferentValues ||
            propReferenceType.enumValueIndex == (int)SelfDestruction.ReferenceType.Point)
            EditorGUILayout.PropertyField(propReferencePoint);

        if (propReferenceType.hasMultipleDifferentValues ||
            propReferenceType.enumValueIndex == (int)SelfDestruction.ReferenceType.GameObject)
            EditorGUILayout.PropertyField(propReferenceObject);

        if (propReferenceType.hasMultipleDifferentValues ||
            propReferenceType.enumValueIndex == (int)SelfDestruction.ReferenceType.GameObjectName)
            EditorGUILayout.PropertyField(propReferenceName);

        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
