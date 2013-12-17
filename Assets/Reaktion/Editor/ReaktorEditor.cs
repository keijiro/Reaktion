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

    // Audio input settings.
    SerializedProperty propAudioMode;
    SerializedProperty propAudioIndex;
    SerializedProperty propAudioCurve;

    // Gain control.
    SerializedProperty propGainEnabled;
    SerializedProperty propGainKnobIndex;
    SerializedProperty propGainCurve;

	// Offset control.
	SerializedProperty propOffsetEnabled;
	SerializedProperty propOffsetKnobIndex;
	SerializedProperty propOffsetCurve;

	// General option.
    SerializedProperty propSensibility;

    // Audio input options.
    SerializedProperty propShowAudioOptions;
    SerializedProperty propHeadroom;
    SerializedProperty propDynamicRange;
    SerializedProperty propLowerBound;
    SerializedProperty propFalldown;

    #endregion

    #region Private variables

    // Texutres for drawing level bars.
    Texture2D[] barTextures;

    #endregion

    #region UI constants

    static string[] sourceLabels = {
        "No Input",
        "RMS Level (mono)",
        "RMS Level (L+R)",
        "Frequency Band"
    };
    static int[] sourceOptions = {
        0, 1, 2, 3
    };
    
    #endregion

    #region Editor functions

    // On Enable (initialization)
    void OnEnable ()
    {
        // Audio input settings.
        propAudioMode = serializedObject.FindProperty ("audioMode");
        propAudioIndex = serializedObject.FindProperty ("audioIndex");
        propAudioCurve = serializedObject.FindProperty ("audioCurve");

        // Gain control.
        propGainEnabled = serializedObject.FindProperty ("gainEnabled");
        propGainKnobIndex = serializedObject.FindProperty ("gainKnobIndex");
        propGainCurve = serializedObject.FindProperty ("gainCurve");

		// Offset control.
		propOffsetEnabled = serializedObject.FindProperty ("offsetEnabled");
		propOffsetKnobIndex = serializedObject.FindProperty ("offsetKnobIndex");
		propOffsetCurve = serializedObject.FindProperty ("offsetCurve");

		// General option.
		propSensibility = serializedObject.FindProperty ("sensibility");
        
        // Audio input options.
        propShowAudioOptions = serializedObject.FindProperty ("showAudioOptions");
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

		// Audio input settings.
		propAudioMode.intValue = EditorGUILayout.IntPopup ("Audio Source", propAudioMode.intValue, sourceLabels, sourceOptions);
		if (propAudioMode.intValue > 0)
        {
            var label = (propAudioMode.intValue == (int)Reaktor.AudioMode.SpecturmBand) ? "Band" : "Channel";
            propAudioIndex.intValue = EditorGUILayout.IntField (label, propAudioIndex.intValue);
            propAudioCurve.animationCurveValue = EditorGUILayout.CurveField ("Curve", propAudioCurve.animationCurveValue);
        }

        // Gain control.
        if (propAudioMode.intValue > 0)
        {
            if (propGainEnabled.boolValue || propOffsetEnabled.boolValue)
                EditorGUILayout.Space ();

            if (propGainEnabled.boolValue = EditorGUILayout.Toggle ("Gain Control", propGainEnabled.boolValue))
            {
                propGainKnobIndex.intValue = EditorGUILayout.IntField ("MIDI CC #", propGainKnobIndex.intValue);
                propGainCurve.animationCurveValue = EditorGUILayout.CurveField ("Curve", propGainCurve.animationCurveValue);
            }
        }

        // Offset control.
        if (propGainEnabled.boolValue || propOffsetEnabled.boolValue)
            EditorGUILayout.Space ();

        if (propOffsetEnabled.boolValue = EditorGUILayout.Toggle ("Offset Control", propOffsetEnabled.boolValue))
        {
            propOffsetKnobIndex.intValue = EditorGUILayout.IntField ("MIDI CC #", propOffsetKnobIndex.intValue);
            propOffsetCurve.animationCurveValue = EditorGUILayout.CurveField ("Curve", propOffsetCurve.animationCurveValue);
        }
		
        if (propGainEnabled.boolValue || propOffsetEnabled.boolValue)
            EditorGUILayout.Space ();

        // General option.
        EditorGUILayout.Slider (propSensibility, 0.1f, 40.0f);

        // Audio input options.
        if (propAudioMode.intValue > 0)
        {
            if (propShowAudioOptions.boolValue = EditorGUILayout.Foldout (propShowAudioOptions.boolValue, "Audio Input Options"))
            {
                EditorGUILayout.Slider (propHeadroom, 0.0f, 20.0f, "Headroom [dB]");
                EditorGUILayout.Slider (propDynamicRange, 1.0f, 60.0f, "Dynamic Range [dB]");
                EditorGUILayout.Slider (propLowerBound, -100.0f, -10.0f, "Lower Bound [dB]");
                EditorGUILayout.Slider (propFalldown, 0.0f, 10.0f, "Falldown [dB/Sec]");
            }
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
        
        // Draw the level bar on play mode.
        if (EditorApplication.isPlaying && !serializedObject.isEditingMultipleObjects)
        {
            EditorGUILayout.Space ();
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
                NewBarTexture (new Color(55.0f / 255, 53.0f / 255, 45.0f / 255)),
                NewBarTexture (new Color(250.0f / 255, 249.0f / 255, 248.0f / 255)),
                NewBarTexture (new Color(110.0f / 255, 192.0f / 255, 91.0f / 255, 0.8f)),
                NewBarTexture (new Color(226.0f / 255, 0, 7.0f / 255, 0.8f)),
                NewBarTexture (new Color(249.0f / 255, 185.0f / 255, 22.0f / 255))
            };
        }
        
        // Get a rectangle as a text field and fill it.
        var rect = GUILayoutUtility.GetRect (18, 16, "TextField");
        GUI.DrawTexture (rect, barTextures [0]);

        // Draw the raw input bar.
        var temp = rect;
        temp.width *= Mathf.Clamp01((reaktor.RawInput - reaktor.lowerBound) / (3 - reaktor.lowerBound));
        GUI.DrawTexture (temp, barTextures [1]);

        // Draw the dynamic range.
        temp.x += rect.width * (reaktor.Peak - reaktor.lowerBound - reaktor.dynamicRange - reaktor.headroom) / (3 - reaktor.lowerBound);
        temp.width = rect.width * reaktor.dynamicRange / (3 - reaktor.lowerBound);
        GUI.DrawTexture (temp, barTextures [2]);

        // Draw the headroom.
        temp.x += temp.width;
        temp.width = rect.width * reaktor.headroom / (3 - reaktor.lowerBound);
        GUI.DrawTexture (temp, barTextures [3]);

        // Display the peak level value.
        EditorGUI.LabelField(rect, "Peak: " + reaktor.Peak.ToString ("0.0") + " dB");
        
        // Get a rectangle as a text field and fill it.
        rect = GUILayoutUtility.GetRect (18, 16, "TextField");
        GUI.DrawTexture (rect, barTextures [0]);

        // Draw the output level.
        temp = rect;
        temp.width *= reaktor.Output;
        GUI.DrawTexture (temp, barTextures [4]);

        // Display the output value.
        EditorGUI.LabelField (rect, "Out: " + (reaktor.Output * 100).ToString ("0.0") + " %");
    }

    #endregion
}
