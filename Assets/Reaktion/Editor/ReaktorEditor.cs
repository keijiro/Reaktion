using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

[CustomEditor(typeof(Reaktor)), CanEditMultipleObjects]
public class ReaktorEditor : Editor
{
    // Properties.
    SerializedProperty propBandIndex;
    SerializedProperty propDynamicRange;
    SerializedProperty propHeadroom;
    SerializedProperty propFalldown;
    SerializedProperty propLowerBound;
    SerializedProperty propPowerFactor;
    SerializedProperty propSensibility;
    SerializedProperty propShowOptions;

    // Texutres for drawing bars.
    Texture2D[] barTextures;

    // Shows the inspaector.
    public override void OnInspectorGUI ()
    {
        serializedObject.Update ();
        
        // Show the editable properties.
        propBandIndex.intValue = EditorGUILayout.IntField ("Band Index", propBandIndex.intValue);
        EditorGUILayout.Slider (propPowerFactor, 0.1f, 20.0f);
        EditorGUILayout.Slider (propSensibility, 0.1f, 40.0f);
        propShowOptions.boolValue = EditorGUILayout.Foldout (propShowOptions.boolValue, "Audio Input Options");
        if (propShowOptions.boolValue)
        {
            EditorGUILayout.Slider (propHeadroom, 0.0f, 20.0f, "Headroom [dB]");
            EditorGUILayout.Slider (propDynamicRange, 1.0f, 60.0f, "Dynamic Range [dB]");
            EditorGUILayout.Slider (propLowerBound, -100.0f, -10.0f, "Lower Bound [dB]");
            EditorGUILayout.Slider (propFalldown, 0.0f, 10.0f, "Falldown [dB/Sec]");
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
        
        // Draw the level bar on play mode.
        if (EditorApplication.isPlaying && !serializedObject.isEditingMultipleObjects)
        {
            DrawLevelBar (target as Reaktor);
            // Make it dirty to update the view.
            EditorUtility.SetDirty (target);
        }
    }

    // On Enable (initialization)
    void OnEnable ()
    {
        // Get references to the properties.
        propBandIndex = serializedObject.FindProperty ("bandIndex");
        propDynamicRange = serializedObject.FindProperty ("dynamicRange");
        propHeadroom = serializedObject.FindProperty ("headroom");
        propFalldown = serializedObject.FindProperty ("falldown");
        propLowerBound = serializedObject.FindProperty ("lowerBound");
        propPowerFactor = serializedObject.FindProperty ("powerFactor");
        propSensibility = serializedObject.FindProperty ("sensibility");
        propShowOptions = serializedObject.FindProperty ("showOptions");
    }

    // On Disable (cleanup)
    void OnDisable ()
    {
        if (barTextures != null)
        {
            // Destroy the bar textures.
            foreach (var texture in barTextures)
                DestroyImmediate (texture);
            barTextures = null;
        }
    }

    // Make a texture which contains only one pixel.
    Texture2D NewBarTexture (Color color)
    {
        var texture = new Texture2D (1, 1);
        texture.SetPixel (0, 0, color);
        texture.Apply ();
        return texture;
    }

    // Draw the input level bar.
    void DrawLevelBar (Reaktor reaktor)
    {
        if (barTextures == null)
        {
            // Make textures for drawing level bars.
            barTextures = new Texture2D[] {
                NewBarTexture (Color.red),
                NewBarTexture (Color.green),
                NewBarTexture (Color.blue),
                NewBarTexture (Color.gray)
            };
        }
        
        // Peak level label.
        EditorGUILayout.LabelField ("Peak Level", reaktor.Peak.ToString ("0.0") + " dB");
        
        // Get a rectangle as a text field.
        var rect = GUILayoutUtility.GetRect (18, 10, "TextField");
        var width = rect.width;
        
        // Fill the rectangle with gray.
        GUI.DrawTexture (rect, barTextures [3]);
        
        // Draw the range bar with red.
        rect.x += width * (reaktor.Peak - reaktor.lowerBound - reaktor.dynamicRange - reaktor.headroom) / (3 - reaktor.lowerBound);
        rect.width = width * (reaktor.dynamicRange + reaktor.headroom) / (3 - reaktor.lowerBound);
        GUI.DrawTexture (rect, barTextures [0]);
        
        // Draw the effective range bar with green.
        rect.width = width * (reaktor.dynamicRange) / (3 - reaktor.lowerBound);
        GUI.DrawTexture (rect, barTextures [1]);
        
        // Draw the output level bar with blue.
        rect.width = width * (reaktor.dynamicRange + reaktor.headroom) * reaktor.Output / (3 - reaktor.lowerBound);
        rect.y += rect.height / 2;
        rect.height /= 2;
        GUI.DrawTexture (rect, barTextures [2]);
    }
}
