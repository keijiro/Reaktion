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
using System.Collections;

namespace Reaktion {

[AddComponentMenu("Reaktion/Reaktor And Source/Reaktor")]
public class Reaktor : MonoBehaviour
{
    #region Audio input settings

    public bool autoBind = true;
    public ReaktorSourceBase source;
    public AnimationCurve audioCurve = AnimationCurve.Linear(0, 0, 1, 1);

    #endregion

    #region Gain control settings

    public bool gainEnabled = false;
    public int gainKnobIndex = 2;
    public MidiChannel gainKnobChannel = MidiChannel.All;
    public string gainInputAxis;
    public AnimationCurve gainCurve = AnimationCurve.Linear(0, 0, 1, 1);

    #endregion

    #region Offset control settings

    public bool offsetEnabled = false;
    public int offsetKnobIndex = 3;
    public MidiChannel offsetKnobChannel = MidiChannel.All;
    public string offsetInputAxis;
    public AnimationCurve offsetCurve = AnimationCurve.Linear(0, 0, 1, 1);

    #endregion

    #region General option

    public float sensitivity = 15.1f;
    public float decaySpeed = 5.5f;

    #endregion

    #region Audio input options

    public bool showAudioOptions = false;
    public float headroom = 2.0f;
    public float dynamicRange = 16.0f;
    public float lowerBound = -60.0f;
    public float falldown = 0.5f;

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

    public float Override {
        get { return Mathf.Clamp01(fakeInput); }
        set { fakeInput = value; }
    }

    public bool IsOverridden {
        get { return fakeInput >= 0.0f; }
    }

    public bool Bang {
        get { return fakeInput > 1.0f; }
        set { fakeInput = value ? 10.0f : -1.0f; }
    }

    public static int ActiveInstanceCount {
        get { return activeInstanceCount; }
    }

    #endregion

    #region Private variables and functions

    float output;
    float peak;
    float rawInput;
    float gain;
    float offset;
    float fakeInput = -1.0f;

    static int activeInstanceCount;

    #endregion

    #region MonoBehaviour functions

    void Start()
    {
        // Search for a Reaktor source.
        if (autoBind)
        {
            source = GetComponentInChildren<ReaktorSourceBase>();
            if (source == null)
            {
                source = GetComponentInParent<ReaktorSourceBase>();
                if (source == null)
                    source= FindObjectOfType<ReaktorSourceBase>();
            }
        }

        // Begins with the lowest level.
        peak = lowerBound + dynamicRange + headroom;
        rawInput = -1e12f;
        gain = 1.0f;
    }

    void Update()
    {
        float input = 0.0f;

        // Audio input.
        rawInput = source ? source.DbLevel : -1e12f;

        // Check the peak level.
        peak -= Time.deltaTime * falldown;
        peak = Mathf.Max(peak, Mathf.Max(rawInput, lowerBound + dynamicRange + headroom));
        
        // Normalize the input level.
        input = (rawInput - peak + headroom + dynamicRange) / dynamicRange;
        input = audioCurve.Evaluate(Mathf.Clamp01(input));

        // MIDI CC input.
        if (gainEnabled)
        {
            gain = MidiJack.GetKnob(gainKnobChannel, gainKnobIndex, 1.0f);
            if (!string.IsNullOrEmpty(gainInputAxis))
                gain = Mathf.Clamp01(gain + Input.GetAxis(gainInputAxis));
            gain = gainCurve.Evaluate(gain);
            input *= gain;
        }
        if (offsetEnabled)
        {
            offset = MidiJack.GetKnob(offsetKnobChannel, offsetKnobIndex);
            if (!string.IsNullOrEmpty(offsetInputAxis))
                offset = Mathf.Clamp01(offset + Input.GetAxis(offsetInputAxis));
            offset = offsetCurve.Evaluate(offset);
            input += offset;
        }

        // Make output.
        input = Mathf.Clamp01(fakeInput < 0.0f ? input : fakeInput);

        if (sensitivity > 0.0f)
        {
            input -= (input - output) * Mathf.Exp(-sensitivity * Time.deltaTime);
        }

        output = Mathf.Max(input, output - Time.deltaTime * decaySpeed);
    }

    void OnEnable()
    {
        activeInstanceCount++;
    }

    void OnDisable()
    {
        activeInstanceCount--;
    }

    #endregion

    #region Public functions

    // Stop overriding.
    public void StopOverride()
    {
        fakeInput = -1.0f;
    }

    // Search an available Reaktor placed close to the game object.
    public static Reaktor SearchAvailableFrom(GameObject go)
    {
        var r = go.GetComponent<Reaktor>();
        if (r) return r;

        r = go.GetComponentInParent<Reaktor>();
        if (r) return r;

        r = go.GetComponentInChildren<Reaktor>();
        return r;
    }

    #endregion
}

} // namespace Reaktion
