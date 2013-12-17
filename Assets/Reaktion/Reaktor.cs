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
    public AnimationCurve gainCurve = AnimationCurve.Linear (0, 0, 1, 1);

    #endregion

    #region Offset control settings

    public bool offsetEnabled = false;
    public int offsetKnobIndex = 3;
    public AnimationCurve offsetCurve = AnimationCurve.Linear (0, 0, 1, 1);

    #endregion

    #region General option

    public float sensibility = 18.0f;

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

    #endregion

    #region Private variables

    float output;
    float peak;
    float rawInput;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        // Begins with the lowest level.
        peak = lowerBound + dynamicRange + headroom;
        rawInput = -1e12f;
    }

    void Update ()
    {
        float input = 0.0f;

        // Audio input.
        if (audioMode != AudioMode.NoInput)
        {
            if (audioMode == AudioMode.MonoLevel)
            {
                rawInput = AudioJack.instance.ChannelLevels [audioIndex];
            }
            else if (audioMode == AudioMode.StereoLevel)
            {
                rawInput = 0.5f * (AudioJack.instance.ChannelLevels [audioIndex] + AudioJack.instance.ChannelLevels [audioIndex + 1]);
            }
            else
            {
                rawInput = AudioJack.instance.BandLevels [audioIndex];
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
            input *= gainCurve.Evaluate (MidiJack.GetKnob (gainKnobIndex, 1.0f));
        }
        if (offsetEnabled)
        {
            input += offsetCurve.Evaluate (MidiJack.GetKnob (offsetKnobIndex));
        }

        // Interpolation.
        input = Mathf.Clamp01 (input);
        output = input - (input - output) * Mathf.Exp (-sensibility * Time.deltaTime);
    }

    // Search an available Reaktor placed close to the game object.
    public static Reaktor SearchAvailableFrom (GameObject go)
    {
        // 1. Look for a component from the parent.
        // 2. From the grandparent.
        // 3. From the childlen.
        // 4. Give up.
        var r = go.GetComponent<Reaktor> ();
        if (r == null)
        {
            r = go.transform.parent.GetComponent<Reaktor> ();
            if (r == null)
            {
                r = go.transform.parent.GetComponent<Reaktor> ();
                if (r == null)
                {
                    r = go.GetComponentInChildren<Reaktor> ();
                }
            }
        }
        return r;
    }

    #endregion
}
