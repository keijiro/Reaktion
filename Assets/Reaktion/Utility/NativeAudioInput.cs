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
using System.Runtime.InteropServices;

#if UNITY_STANDALONE_OSX && UNITY_PRO_LICENSE

public static class Lasp
{
    [DllImport("Lasp", EntryPoint="LaspInitialize")]
    public static extern void Initialize();

    [DllImport("Lasp", EntryPoint="LaspCountInputChannels")]
    public static extern uint CountInputChannels();

    [DllImport("Lasp", EntryPoint="LaspRetrieveWaveform")]
    public static extern void RetrieveWaveform(uint sourceChannel, float[] buffer, uint bufferLength, uint channelCount);
}

#endif

namespace Reaktion {

[AddComponentMenu("Reaktion/Utility/Native Audio Input")]
public class NativeAudioInput : MonoBehaviour
{
    public int channel = 0;
    public int bufferSize = 320;

    public float estimatedLatency { get; protected set; }

    AudioSource audioSource;

    void Awake()
    {
        var sampleRate = AudioSettings.outputSampleRate;

        // Create an audio source.
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;

#if UNITY_STANDALONE_OSX && UNITY_PRO_LICENSE
        // Initialize the Lasp module.
        Lasp.Initialize();

        // Shrink the DSP buffer to reduce latency.
        AudioSettings.SetDSPBufferSize(bufferSize, 4);

        // Create a null clip and kick it.
        audioSource.clip = AudioClip.Create("Lasp", 1024, 1, sampleRate, false, false);
        audioSource.Play();

        // Estimate the latency.
        estimatedLatency = (float)bufferSize / sampleRate;
#else
        Debug.LogWarning("NativeAudioInput is not supported in the current configuration.");
#endif
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
#if UNITY_STANDALONE_OSX && UNITY_PRO_LICENSE
        Lasp.RetrieveWaveform((uint)channel, data, (uint)data.Length, (uint)channels);
#endif
    }
}

} // namespace Reaktion
