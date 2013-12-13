using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(ReaktorToTransform)), CanEditMultipleObjects]
public class ReaktorToTransformEditor : Editor
{
    // Properties.
    SerializedProperty propEnablePosition;
    SerializedProperty propEnableRotation;
    SerializedProperty propEnableScale;
    SerializedProperty propPosition;
    SerializedProperty propRotationAxis;
    SerializedProperty propMinAngle;
    SerializedProperty propMaxAngle;
    SerializedProperty propScale;

    // Shows the inspaector.
    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();
        
        // Show the editable properties.
        propEnablePosition.boolValue = EditorGUILayout.Toggle ("Translation", propEnablePosition.boolValue);
        if (propEnablePosition.boolValue)
        {
            propPosition.vector3Value = EditorGUILayout.Vector3Field("Vector", propPosition.vector3Value);

            EditorGUILayout.Space ();
        }

        propEnableRotation.boolValue = EditorGUILayout.Toggle ("Rotation", propEnableRotation.boolValue);
        if (propEnableRotation.boolValue)
        {
            propRotationAxis.vector3Value = EditorGUILayout.Vector3Field("Axis", propRotationAxis.vector3Value);
            var min = propMinAngle.floatValue;
            var max = propMaxAngle.floatValue;
            min = EditorGUILayout.FloatField("Minimum Angle", min);
            max = EditorGUILayout.FloatField("Maximum Angle", max);

            EditorGUILayout.MinMaxSlider(ref min, ref max, -360.0f, 360.0f);
            propMinAngle.floatValue = min;
            propMaxAngle.floatValue = max;

            EditorGUILayout.Space ();
        }

        propEnableScale.boolValue = EditorGUILayout.Toggle ("Scaling", propEnableScale.boolValue);
        if (propEnableScale.boolValue)
        {
            propScale.vector3Value = EditorGUILayout.Vector3Field("Vector", propScale.vector3Value);
        }
        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    // On Enable (initialization)
    void OnEnable ()
    {
        // Get references to the properties.
        propEnablePosition = serializedObject.FindProperty ("enablePosition");
        propEnableRotation = serializedObject.FindProperty ("enableRotation");
        propEnableScale = serializedObject.FindProperty ("enableScale");
        propPosition = serializedObject.FindProperty ("position");
        propRotationAxis = serializedObject.FindProperty ("rotationAxis");
        propMinAngle = serializedObject.FindProperty ("minAngle");
        propMaxAngle = serializedObject.FindProperty ("maxAngle");
        propScale = serializedObject.FindProperty ("scale");
    }
}
