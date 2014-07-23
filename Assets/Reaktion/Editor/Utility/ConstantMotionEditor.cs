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

[CustomPropertyDrawer(typeof(ConstantMotion.MotionElement))]
class ConstantMotionElementDrawer : PropertyDrawer
{
    static bool CheckExpandVectorOption(SerializedProperty property)
    {
        // Expand if the vector option is "Arbitral".
        var option = property.FindPropertyRelative("vectorOption");
        return option.hasMultipleDifferentValues ||
               option.enumValueIndex == (int)ConstantMotion.VectorOption.Arbitral;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var expand = CheckExpandVectorOption(property);
        return EditorGUIUtility.singleLineHeight * (expand ? 5 : 4) +
               EditorGUIUtility.standardVerticalSpacing * (expand ? 3 : 2);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var expand = CheckExpandVectorOption(property);
        position.height = EditorGUIUtility.singleLineHeight;

        // Simply put the label.
        EditorGUI.LabelField(position, label);
        position.y += EditorGUIUtility.singleLineHeight;

        // Make an indent.
        position.x += 16;
        position.width -= 16;
        EditorGUIUtility.labelWidth -= 16;

        // Vector option drop-down.
        EditorGUI.PropertyField(position, property.FindPropertyRelative("vectorOption"), new GUIContent("Direction/Axis"));
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if (expand)
        {
            // Vector box.
            EditorGUI.PropertyField(position, property.FindPropertyRelative("arbitralVector"), GUIContent.none);
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        // Velocity box.
        EditorGUI.PropertyField(position, property.FindPropertyRelative("velocity"), new GUIContent("Velocity"));
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Randomness slider.
        EditorGUI.Slider(position, property.FindPropertyRelative("velocityRandomness"), 0, 1, new GUIContent("Randomness"));

        EditorGUI.EndProperty();
    }
}

[CustomEditor(typeof(ConstantMotion)), CanEditMultipleObjects]
public class ConstantMotionEditor : Editor
{
    SerializedProperty propPosition;
    SerializedProperty propRotation;
    SerializedProperty propUseLocalCoordinate;
    GUIContent labelLocalCoordinate;

    void OnEnable()
    {
        propPosition = serializedObject.FindProperty("position");
        propRotation = serializedObject.FindProperty("rotation");
        propUseLocalCoordinate = serializedObject.FindProperty("useLocalCoordinate");
        labelLocalCoordinate = new GUIContent("Local Coordinate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(propPosition);
        EditorGUILayout.PropertyField(propRotation);
        EditorGUILayout.PropertyField(propUseLocalCoordinate, labelLocalCoordinate);
        serializedObject.ApplyModifiedProperties();
    }
}

} // namespace Reaktion
