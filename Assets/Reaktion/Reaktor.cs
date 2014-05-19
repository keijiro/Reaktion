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
using System.Collections;

public class Reaktor : MonoBehaviour
{
    #region Audio input settings

    public enum AudioMode
    {
        NoInput,
        MonoLevel,
        StereoLevel,
        SpecturmBand
    }

    public AudioMode audioMode = AudioMode.SpecturmBand;
    public int audioIndex = 1;
    public AnimationCurve audioCurve = AnimationCurve.Linear (0, 0, 1, 1);

    #endregion

    #region Gain control settings

    public bool gainEnabled = false;
    public int gainKnobIndex = 2;
    public MidiChannel gainKnobChannel = MidiChannel.All;
    public AnimationCurve gainCurve = AnimationCurve.Linear (0, 0, 1, 1);

    #endregion

    #region Offset control settings

    public bool offsetEnabled = false;
    public int offsetKnobIndex = 3;
    public MidiChannel offsetKnobChannel = MidiChannel.All;
    public AnimationCurve offsetCurve = AnimationCurve.Linear (0, 0, 1, 1);

    #endregion

    #region General option

    public float sensitivity = 15.1f;
    public float decaySpeed = 5.5f;

    #endregion

    #region Audio input options

    public bool showAudioOptions = false;
    public float headroom = 6.0f;
    public float dynamicRange = 20.0f;
    public float lowerBound = -60.0f;
    public float falldown = 0.4f;

    #endregion

    #region Output properties

    public float Output {
        get { return output; }
    }
    
    public float Peak {
        get { return peak; }
    }

    public float RawInput {
        get { return rawInput; }
    }

    public float Gain {
        get { return gain; }
    }

    public float Offset {
        get { return offset; }
    }

    public bool Overridden {
        get { return overridden; }
    }

    public static int ActiveInstanceCount {
        get { return activeInstanceCount; }
    }

    #endregion

    #region Private variables

    float output;
    float peak;
    float rawInput;
    float gain;
    float offset;
    bool overridden;

    static int activeInstanceCount;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        // Begins with the lowest level.
        peak = lowerBound + dynamicRange + headroom;
        rawInput = -1e12f;
        gain = 1.0f;
    }

    void Update ()
    {
        float input = 0.0f;

        // Audio input.
        if (audioMode != AudioMode.NoInput)
        {
            if (audioMode == AudioMode.MonoLevel)
            {
                rawInput = AudioJack.instance.ChannelRmsLevels [audioIndex];
            }
            else if (audioMode == AudioMode.StereoLevel)
            {
                rawInput = 0.5f * (AudioJack.instance.ChannelRmsLevels [audioIndex] + AudioJack.instance.ChannelRmsLevels [audioIndex + 1]);
            }
            else
            {
                rawInput = AudioJack.instance.OctaveBandSpectrum [audioIndex];
            }

            // Check the peak level.
            peak -= Time.deltaTime * falldown;
            peak = Mathf.Max (peak, Mathf.Max (rawInput, lowerBound + dynamicRange + headroom));
            
            // Normalize the input level.
            input = (rawInput - peak + headroom + dynamicRange) / dynamicRange;
            input = audioCurve.Evaluate (Mathf.Clamp01 (input));
        }

        // MIDI CC input.
        if (gainEnabled)
        {
            gain = gainCurve.Evaluate (MidiJack.GetKnob (gainKnobChannel, gainKnobIndex, 1.0f));
            input *= gain;
        }
        if (offsetEnabled)
        {
            offset = offsetCurve.Evaluate (MidiJack.GetKnob (offsetKnobChannel, offsetKnobIndex));
            input += offset;
        }

        // Make output.
        input = Mathf.Clamp01 (input);

        if (sensitivity > 0.0f)
        {
            input -= (input - output) * Mathf.Exp (-sensitivity * Time.deltaTime);
        }

        if (!overridden) 
        {
            output = Mathf.Max (input, output - Time.deltaTime * decaySpeed);
        }
    }

    void OnEnable ()
    {
        activeInstanceCount++;
    }

    void OnDisable ()
    {
        activeInstanceCount--;
    }

    #endregion

    #region Public functions

    // Override the output.
    public void OverrideOutput (float value)
    {
        overridden = true;
        output = value;
    }

    // Stop overriding.
    public void StopOverriding ()
    {
        overridden = false;
    }

    // Search an available Reaktor placed close to the game object.
    public static Reaktor SearchAvailableFrom (GameObject go)
    {
        var r = go.GetComponent<Reaktor> ();
        if (r) return r;

        // Look for a Reaktor component within the upper hierarchy.
        for (var t = go.transform.parent; t != null; t = t.parent)
        {
            r = t.GetComponent<Reaktor> ();
            if (r) return r;
        }

        // Search the lower hierarchy.
        r = go.GetComponentInChildren<Reaktor> ();
        return r;
    }

    #endregion
}
