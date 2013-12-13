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
    #region Basic settings

    public enum InputMode
    {
        MonoLevel,
        StereoLevel,
        SpecturmBand
    }

    public InputMode inputMode = InputMode.SpecturmBand;
    public int inputIndex = 1;
    public float sensibility = 18.0f;
    public AnimationCurve curve = AnimationCurve.Linear (0, 0, 1, 1);

    #endregion

    #region Audio input options

    public bool showOptions;
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

    #endregion

    #region Private variables
    
    float output;
    float peak;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        // Begins with the lowest level.
        peak = lowerBound + dynamicRange + headroom;
    }

    void Update ()
    {
        // Audio input.
        float input;
        if (inputMode == InputMode.MonoLevel)
        {
            input = AudioJack.instance.ChannelLevels [inputIndex];
        }
        else if (inputMode == InputMode.StereoLevel)
        {
            input = 0.5f * (AudioJack.instance.ChannelLevels [inputIndex] + AudioJack.instance.ChannelLevels [inputIndex + 1]);
        }
        else
        {
            input = AudioJack.instance.BandLevels [inputIndex];
        }

        // Check the peak level.
        peak -= Time.deltaTime * falldown;
        peak = Mathf.Max (peak, Mathf.Max (input, lowerBound + dynamicRange + headroom));

        // Normalize the input level.
        input = (input - peak + headroom + dynamicRange) / dynamicRange;
        input = curve.Evaluate (Mathf.Clamp01 (input));

        // Interpolation.
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
