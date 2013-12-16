//
// AudioJack - External Audio Analyzer for Unity
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

[CustomEditor(typeof(AudioJack)), CanEditMultipleObjects]
public class AudioJackEditor : Editor
{
    #region Reference to properties

    SerializedProperty propBandType;
    SerializedProperty propMinimumInterval;
    SerializedProperty propInternalMode;
    SerializedProperty propChannelToAnalyze;
    SerializedProperty propChannelSelect;
    SerializedProperty propShowLevels;
    SerializedProperty propShowSpectrum;

    #endregion

    #region UI constants and variables

    static string[] bandTypeLabels = {
        "4 Band", "8 Band", "10 Band (Standard)", "26 Band"
    };
    static int[] BandTypeValues = {
        0, 1, 2, 3
    };
    static string[] mixModeLabels = {
        "Discrete", "Mix L + R", "Mix All"
    };
    static int[] mixModeValues = {
        0, 1, 2
    };
    string[] channelLabels;
    int[] channelOptions;

    #endregion

    #region Editor functions

    void OnEnable ()
    {
        propBandType = serializedObject.FindProperty ("bandType");
        propMinimumInterval = serializedObject.FindProperty ("minimumInterval");
        propInternalMode = serializedObject.FindProperty ("internalMode");
        propChannelToAnalyze = serializedObject.FindProperty ("channelToAnalyze");
        propChannelSelect = serializedObject.FindProperty ("channelSelect");
        propShowLevels = serializedObject.FindProperty ("showLevels");
        propShowSpectrum = serializedObject.FindProperty ("showSpectrum");
    }

    public override void OnInspectorGUI ()
    {
        // Update the properties.
        serializedObject.Update ();

        // Basic settings.
        propBandType.intValue = EditorGUILayout.IntPopup ("Band Type", propBandType.intValue, bandTypeLabels, BandTypeValues);
        EditorGUILayout.Slider (propMinimumInterval, 0.0f, 1.0f, "Interval");

        // Input settings.
        propInternalMode.boolValue = !EditorGUILayout.Toggle ("External Audio", !propInternalMode.boolValue);
        if (propInternalMode.boolValue)
        {
            EditorGUILayout.HelpBox ("The External Audio option is disabled. Captures audio data from the Audio Listener.", MessageType.None);
        }
        else
        {
            propChannelSelect.intValue = EditorGUILayout.IntPopup ("Mix Mode", propChannelSelect.intValue, mixModeLabels, mixModeValues);
            if (propChannelSelect.intValue == 2)
            {
                EditorGUILayout.HelpBox ("Mix All Mode. Mixes all input channels.", MessageType.None);
            }
            else
            {
                var channel = UpdateAndShowChannelOptions ();
                if (propChannelSelect.intValue == 0)
                    EditorGUILayout.HelpBox ("Discrete Mode. Handles audio signals from channel #" + channel + ".", MessageType.None);
                else
                    EditorGUILayout.HelpBox ("Mix L + R Mode. Mixes audio signals from channel #" + channel + " and #" + (channel + 1) + ".", MessageType.None);
            }
        }

        // Level meters.
        if (!serializedObject.isEditingMultipleObjects)
        {
            var audioJack = target as AudioJack;
            
            // Channel level bars.
            EditorGUILayout.Space ();
            propShowLevels.boolValue = EditorGUILayout.Foldout (propShowLevels.boolValue, "Channel Levels");
            if (propShowLevels.boolValue)
                DisplayLevelBars (audioJack.ChannelLevels);
            
            // Band level bars.
            EditorGUILayout.Space ();
            propShowSpectrum.boolValue = EditorGUILayout.Foldout (propShowSpectrum.boolValue, "Spectrum");
            if (propShowSpectrum.boolValue)
                DisplayLevelBars (audioJack.BandLevels);
            
            // Make itself dirty to update on every time.
            if ((propShowLevels.boolValue || propShowSpectrum.boolValue) && EditorApplication.isPlaying)
            {
                EditorUtility.SetDirty (target);
            }
        }

        // Apply modifications.
        serializedObject.ApplyModifiedProperties ();
    }

    #endregion

    #region Private functions

    int UpdateAndShowChannelOptions ()
    {
        var count = AudioJack.AudioJackCountChannels ();

        if (propChannelSelect.intValue == (int)AudioJack.ChannelSelect.MixStereo)
            count--;

        if (count == 1)
        {
            // There is only one option.
            channelLabels = null;
            channelOptions = null;
            propChannelToAnalyze.intValue = 0;
            return 0;
        }

        if (channelLabels == null || channelLabels.Length != count)
        {
            // Make arrays.
            channelLabels = new string[count];
            channelOptions = new int[count];
            for (var i = 0; i < count; i++)
            {
                channelLabels [i] = i.ToString ();
                channelOptions [i] = i;
            }
        }

        // Show the pop-up.
        var choice = EditorGUILayout.IntPopup ("Channel", propChannelToAnalyze.intValue, channelLabels, channelOptions);
        propChannelToAnalyze.intValue = choice;

        return choice;
    }

    void DisplayLevelBars (float[] levels)
    {
        if (EditorApplication.isPlaying)
        {
            for (var i = 0; i < levels.Length; i++)
            {
                var db = levels[i];
                var width = Mathf.Clamp (1.0f + db / 40, 0.01f, 1.0f);
                var text = db.ToString ("0.0") + " dB";
                // Show the channel number.
                var rect = GUILayoutUtility.GetRect (18, 18, "TextField");
                EditorGUI.LabelField (rect, "#" + i);
                // Show the level bar.
                rect.x += rect.width * 0.1f;
                rect.width *= 0.9f;
                EditorGUI.ProgressBar (rect, width, text);
            }
        }
        else
        {
            EditorGUILayout.HelpBox ("You can view the sutatus only on Play Mode.", MessageType.None);
        }
    }

    #endregion
}
