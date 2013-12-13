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

[CustomEditor(typeof(Reaktor)), CanEditMultipleObjects]
public class ReaktorEditor : Editor
{
    #region References to the properties

    // Basic settings.
    SerializedProperty propInputMode;
	SerializedProperty propInputIndex;
	SerializedProperty propSensibility;
    SerializedProperty propCurve;

    // Audio input options.
    SerializedProperty propShowOptions;
    SerializedProperty propHeadroom;
    SerializedProperty propDynamicRange;
    SerializedProperty propLowerBound;
    SerializedProperty propFalldown;

    #endregion

    #region Private variables

    // Texutres for drawing level bars.
    Texture2D[] barTextures;

    #endregion

    #region Editor functions

    // On Enable (initialization)
    void OnEnable ()
    {
        // Basic settings.
        propInputMode = serializedObject.FindProperty ("inputMode");
		propInputIndex = serializedObject.FindProperty ("inputIndex");
		propSensibility = serializedObject.FindProperty ("sensibility");
        propCurve = serializedObject.FindProperty ("curve");
        
        // Audio input options.
        propShowOptions = serializedObject.FindProperty ("showOptions");
        propHeadroom = serializedObject.FindProperty ("headroom");
        propDynamicRange = serializedObject.FindProperty ("dynamicRange");
        propLowerBound = serializedObject.FindProperty ("lowerBound");
        propFalldown = serializedObject.FindProperty ("falldown");
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

    // Shows the inspector.
    public override void OnInspectorGUI ()
    {
        // Update the references.
        serializedObject.Update ();

        // Basic settings.
        propInputMode.intValue = (int)(Reaktor.InputMode)EditorGUILayout.EnumPopup ("Input Mode", (Reaktor.InputMode)propInputMode.intValue);
        if (propInputMode.intValue == (int)Reaktor.InputMode.SpecturmBand)
        {
            propInputIndex.intValue = EditorGUILayout.IntField ("Band Index", propInputIndex.intValue);
        }
        else
        {
            propInputIndex.intValue = EditorGUILayout.IntField ("Channel", propInputIndex.intValue);
        }
		EditorGUILayout.Slider (propSensibility, 0.1f, 40.0f);
        propCurve.animationCurveValue = EditorGUILayout.CurveField ("Output Curve", propCurve.animationCurveValue);

        // Audio input options.
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
            EditorUtility.SetDirty (target); // Make it dirty to update the view.
        }
    }

    #endregion
    
    #region Private functions

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
        rect.width = width * reaktor.dynamicRange * reaktor.Output / (3 - reaktor.lowerBound);
        rect.y += rect.height / 2;
        rect.height /= 2;
        GUI.DrawTexture (rect, barTextures [2]);
    }

    #endregion
}
