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

[CustomEditor(typeof(Reaktor)), CanEditMultipleObjects]
public class ReaktorEditor : Editor
{
    // Audio input settings.
    SerializedProperty propAutoBind;
    SerializedProperty propSource;
    SerializedProperty propAudioCurve;

    // Gain control.
    SerializedProperty propGainEnabled;
    SerializedProperty propGainKnobIndex;
    SerializedProperty propGainKnobChannel;
    SerializedProperty propGainInputAxis;
    SerializedProperty propGainCurve;

    // Offset control.
    SerializedProperty propOffsetEnabled;
    SerializedProperty propOffsetKnobIndex;
    SerializedProperty propOffsetKnobChannel;
    SerializedProperty propOffsetInputAxis;
    SerializedProperty propOffsetCurve;

    // General option.
    SerializedProperty propSensitivity;
    SerializedProperty propDecaySpeed;

    // Audio input options.
    SerializedProperty propShowAudioOptions;
    SerializedProperty propHeadroom;
    SerializedProperty propDynamicRange;
    SerializedProperty propLowerBound;
    SerializedProperty propFalldown;

    // String labels.
    GUIContent labelGainControl;
    GUIContent labelOffsetControl;
    GUIContent labelCurve;
    GUIContent labelMidiCC;
    GUIContent labelMidiChannel;
    GUIContent labelInputAxis;

    // Texutres for drawing level bars.
    Texture2D[] barTextures;

    // UI contents.
    static GUIContent[] midiChannelLabels = {
        new GUIContent("Channel 1"),
        new GUIContent("Channel 2"),
        new GUIContent("Channel 3"),
        new GUIContent("Channel 4"),
        new GUIContent("Channel 5"),
        new GUIContent("Channel 6"),
        new GUIContent("Channel 7"),
        new GUIContent("Channel 8"),
        new GUIContent("Channel 9"),
        new GUIContent("Channel 10"),
        new GUIContent("Channel 11"),
        new GUIContent("Channel 12"),
        new GUIContent("Channel 13"),
        new GUIContent("Channel 14"),
        new GUIContent("Channel 15"),
        new GUIContent("Channel 16"),
        new GUIContent("All Channels")
    };
    static int[] midiChannelOptions = {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16
    };

    // On Enable (initialization)
    void OnEnable()
    {
        // Audio input settings.
        propAutoBind          = serializedObject.FindProperty("autoBind");
        propSource            = serializedObject.FindProperty("source");
        propAudioCurve        = serializedObject.FindProperty("audioCurve");

        // Gain control.
        propGainEnabled       = serializedObject.FindProperty("gainEnabled");
        propGainKnobIndex     = serializedObject.FindProperty("gainKnobIndex");
        propGainKnobChannel   = serializedObject.FindProperty("gainKnobChannel");
        propGainInputAxis     = serializedObject.FindProperty("gainInputAxis");
        propGainCurve         = serializedObject.FindProperty("gainCurve");

        // Offset control.
        propOffsetEnabled     = serializedObject.FindProperty("offsetEnabled");
        propOffsetKnobIndex   = serializedObject.FindProperty("offsetKnobIndex");
        propOffsetKnobChannel = serializedObject.FindProperty("offsetKnobChannel");
        propOffsetInputAxis   = serializedObject.FindProperty("offsetInputAxis");
        propOffsetCurve       = serializedObject.FindProperty("offsetCurve");

        // General option.
        propSensitivity       = serializedObject.FindProperty("sensitivity");
        propDecaySpeed        = serializedObject.FindProperty("decaySpeed");
        
        // Audio input options.
        propShowAudioOptions  = serializedObject.FindProperty("showAudioOptions");
        propHeadroom          = serializedObject.FindProperty("headroom");
        propDynamicRange      = serializedObject.FindProperty("dynamicRange");
        propLowerBound        = serializedObject.FindProperty("lowerBound");
        propFalldown          = serializedObject.FindProperty("falldown");

        // String labels.
        labelGainControl   = new GUIContent("Gain Control");
        labelOffsetControl = new GUIContent("Offset Control");
        labelCurve         = new GUIContent("Curve");
        labelMidiCC        = new GUIContent("MIDI CC#");
        labelMidiChannel   = new GUIContent("MIDI Channel");
        labelInputAxis     = new GUIContent("Input Axis");
    }
    
    // On Disable (cleanup)
    void OnDisable()
    {
        if (barTextures != null)
        {
            // Destroy the bar textures.
            foreach (var texture in barTextures)
                DestroyImmediate(texture);
            barTextures = null;
        }
    }

    // Shows the inspector.
    public override void OnInspectorGUI()
    {
        // Update the references.
        serializedObject.Update();

        // Audio input settings.
        EditorGUILayout.PropertyField(propAutoBind);

        if (propAutoBind.hasMultipleDifferentValues || !propAutoBind.boolValue)
            EditorGUILayout.PropertyField(propSource);

        EditorGUILayout.PropertyField(propAudioCurve, labelCurve);

        var shouldInsertSpace = propGainEnabled.hasMultipleDifferentValues || propGainEnabled.boolValue ||
                                propOffsetEnabled.hasMultipleDifferentValues || propOffsetEnabled.boolValue;

        // Gain control.
        if (shouldInsertSpace) EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propGainEnabled, labelGainControl);
        if (propGainEnabled.hasMultipleDifferentValues || propGainEnabled.boolValue)
        {
            EditorGUILayout.PropertyField(propGainKnobIndex, labelMidiCC);
            EditorGUILayout.IntPopup(propGainKnobChannel, midiChannelLabels, midiChannelOptions, labelMidiChannel);
            EditorGUILayout.PropertyField(propGainInputAxis, labelInputAxis);
            EditorGUILayout.PropertyField(propGainCurve, labelCurve);
        }

        // Offset control.
        if (shouldInsertSpace) EditorGUILayout.Space();

        EditorGUILayout.PropertyField(propOffsetEnabled, labelOffsetControl);
        if (propOffsetEnabled.hasMultipleDifferentValues || propOffsetEnabled.boolValue)
        {
            EditorGUILayout.PropertyField(propOffsetKnobIndex, labelMidiCC);
            EditorGUILayout.IntPopup(propOffsetKnobChannel, midiChannelLabels, midiChannelOptions, labelMidiChannel);
            EditorGUILayout.PropertyField(propOffsetInputAxis, labelInputAxis);
            EditorGUILayout.PropertyField(propOffsetCurve, labelCurve);
        }
        
        if (shouldInsertSpace) EditorGUILayout.Space();

        // General option.
        if (!propSensitivity.hasMultipleDifferentValues)
        {
            var value = propSensitivity.floatValue;
            if (value > 0.0f)
                value = EditorGUILayout.Slider("Sensitivity", (value - 0.1f) / 30, 0.0f, 1.0f);
            else
                value = EditorGUILayout.Slider("(Filter Off)", 1.0f, 0.0f, 1.0f);
            propSensitivity.floatValue = (value == 1.0f) ? 0.0f : value * 30 + 0.1f;
        }
        else
        {
            EditorGUILayout.PropertyField(propSensitivity);
        }

        if (!propDecaySpeed.hasMultipleDifferentValues)
        {
            var value = propDecaySpeed.floatValue;
            if (value < 10.5f)
                value = EditorGUILayout.Slider("Decay Speed", (value - 0.5f) / 10, 0.0f, 1.0f);
            else
                value = EditorGUILayout.Slider("(Infinite)", 1.0f, 0.0f, 1.0f);
            propDecaySpeed.floatValue = (value == 1.0f) ? 100.0f : value * 10f + 0.5f;
        }
        else
        {
            EditorGUILayout.PropertyField(propDecaySpeed);
        }

        // Audio input options.
        if (!propShowAudioOptions.hasMultipleDifferentValues)
        {
            propShowAudioOptions.boolValue = EditorGUILayout.Foldout(propShowAudioOptions.boolValue, "Audio Input Options");
        }

        if (propShowAudioOptions.hasMultipleDifferentValues || propShowAudioOptions.boolValue)
        {
            EditorGUILayout.Slider(propHeadroom, 0.0f, 20.0f, "Headroom [dB]");
            EditorGUILayout.Slider(propDynamicRange, 1.0f, 60.0f, "Dynamic Range [dB]");
            EditorGUILayout.Slider(propLowerBound, -100.0f, -10.0f, "Lower Bound [dB]");
            EditorGUILayout.Slider(propFalldown, 0.0f, 10.0f, "Falldown [dB/Sec]");
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties();
        
        // Draw the level bar on play mode.
        if (EditorApplication.isPlaying && !serializedObject.isEditingMultipleObjects)
        {
            EditorGUILayout.Space();
            DrawInputLevelBars(target as Reaktor);
            EditorUtility.SetDirty(target); // Make it dirty to update the view.
        }
    }

    // Make a texture which contains only one pixel.
    Texture2D NewBarTexture(Color color)
    {
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    // Draw the input level bar.
    void DrawInputLevelBars(Reaktor reaktor)
    {
        if (barTextures == null)
        {
            // Make textures for drawing level bars.
            barTextures = new Texture2D[] {
                NewBarTexture(new Color(55.0f / 255, 53.0f / 255, 45.0f / 255)),
                NewBarTexture(new Color(250.0f / 255, 249.0f / 255, 248.0f / 255)),
                NewBarTexture(new Color(110.0f / 255, 192.0f / 255, 91.0f / 255, 0.8f)),
                NewBarTexture(new Color(226.0f / 255, 0, 7.0f / 255, 0.8f)),
                NewBarTexture(new Color(249.0f / 255, 185.0f / 255, 22.0f / 255))
            };
        }
        
        // Get a rectangle as a text field and fill it.
        var rect = GUILayoutUtility.GetRect(18, 16, "TextField");
        GUI.DrawTexture(rect, barTextures[0]);

        // Draw the raw input bar.
        var temp = rect;
        temp.width *= Mathf.Clamp01((reaktor.RawInput - reaktor.lowerBound) / (3 - reaktor.lowerBound));
        GUI.DrawTexture(temp, barTextures[1]);

        // Draw the dynamic range.
        temp.x += rect.width * (reaktor.Peak - reaktor.lowerBound - reaktor.dynamicRange - reaktor.headroom) / (3 - reaktor.lowerBound);
        temp.width = rect.width * reaktor.dynamicRange / (3 - reaktor.lowerBound);
        GUI.DrawTexture(temp, barTextures[2]);

        // Draw the headroom.
        temp.x += temp.width;
        temp.width = rect.width * reaktor.headroom / (3 - reaktor.lowerBound);
        GUI.DrawTexture(temp, barTextures[3]);

        // Display the peak level value.
        EditorGUI.LabelField(rect, "Peak: " + reaktor.Peak.ToString ("0.0") + " dB");

        // Draw the gain level.
        if (reaktor.gainEnabled)
            DrawLevelBar("Gain", reaktor.Gain, barTextures[0], barTextures[1]);

        // Draw the offset level.
        if (reaktor.offsetEnabled)
            DrawLevelBar("Offset", reaktor.Offset, barTextures[0], barTextures[1]);

        // Draw the output level.
        DrawLevelBar("Out", reaktor.Output, barTextures[0], barTextures[4]);
    }

    void DrawLevelBar(string label, float value, Texture bg, Texture fg)
    {
        // Get a rectangle as a text field and fill it.
        var rect = GUILayoutUtility.GetRect(18, 16, "TextField");
        GUI.DrawTexture(rect, bg);
        
        // Draw a level bar.
        var temp = rect;
        temp.width *= value;
        GUI.DrawTexture(temp, fg);
        
        // Display the level value in percentage.
        EditorGUI.LabelField(rect, label + ": " + (value * 100).ToString("0.0") + " %");
    }
}
