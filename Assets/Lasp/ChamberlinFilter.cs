// Corrected Chamberlin Bandpass Filter
// http://courses.cs.washington.edu/courses/cse490s/11au/Readings/Digital_Sound_Generation_2.pdf

using UnityEngine;
using System.Collections;

public class ChamberlinFilter : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float cutoff = 0.5f;
    
    [Range(1.0f, 10.0f)]
    public float q = 1.0f;

    // DSP variables
    float vF;
    float vD;
    float vZ1;
    float vZ2;
    float vZ3;

    // Cutoff frequency in Hz
    public float CutoffFrequency {
        get { return Mathf.Pow(2, 10 * cutoff - 10) * 15000; }
    }

    void Awake()
    {
        Update();
    }
    
    void Update()
    {
        var f = 2 / 1.85f * Mathf.Sin(Mathf.PI * CutoffFrequency / AudioSettings.outputSampleRate);
        vD = 1 / q;
        vF = (1.85f - 0.75f * vD * f) * f;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (var i = 0; i < data.Length; i += channels)
        {
            var si = data[i];
            
            var _vZ1 = 0.5f * si;
            var _vZ3 = vZ2 * vF + vZ3;
            var _vZ2 = (_vZ1 + vZ1 - _vZ3 - vZ2 * vD) * vF + vZ2;
            
            for (var c = 0; c < channels; c++)
                data[i + c] = _vZ2;

            vZ1 = _vZ1;
            vZ2 = _vZ2;
            vZ3 = _vZ3;
        }
    }
}
