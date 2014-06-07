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
using System.Collections;
using System.Runtime.InteropServices;

public class AudioJack : MonoBehaviour
{
    // Octave band type definition
    public enum OctaveBandType
    {
        FourBand,
        FourBandVisual,
        EightBand,
        TenBand,
        TwentySixBand,
        ThirtyOneBand
    }
    
    // Channel selection.
    public enum ChannelSelect
    {
        Mono,
        MixStereo,
        MixAll
    }

    #region Public variables

    // Octave band type.
    public OctaveBandType octaveBandType = OctaveBandType.TenBand;
    
    // Channel selection.
    public int channelToAnalyze = 0;
    public ChannelSelect channelSelect = ChannelSelect.MixStereo;
    
    // Timekeeping.
    public float minimumInterval = 0.0f;
    
    // Internal audio mode.
    public bool useAudioListener = false;

    // Editor properties.
    public bool showLevels = false;
    public bool showSpectrum = false;

    // Reference to the last created instance.
    static public AudioJack instance;
    
    #endregion

    #region Public properties

    #endregion

    // Octave band spectrum (readonly).
    public float[] OctaveBandSpectrum {
        get {
            if (!octaveBandSpectrumIsUpdated && analyzer != System.IntPtr.Zero)
            {
                // Retrieve the spectrum from the analyzer.
                var count = AudioJackGetOctaveBandSpectrum (analyzer, tempArrayForOctaveBandSpectrum);

                // Reallocate the array if mismatch.
                if (octaveBandSpectrum == null || octaveBandSpectrum.Length != count)
                    octaveBandSpectrum = new float[count];

                // Update the spectrum.
                System.Array.Copy (tempArrayForOctaveBandSpectrum, 0, octaveBandSpectrum, 0, count);
                octaveBandSpectrumIsUpdated = true;
            }
            return octaveBandSpectrum;
        }
    }
    
    // Raw Fourier spectrum (readonly).
    public float[] RawSpectrum {
        get {
            if (!rawSpectrumIsUpdated && analyzer != System.IntPtr.Zero)
            {
                // Reallocate the array if mismatch.
                if (rawSpectrum == null || rawSpectrum.Length != dftPointNumber)
                    rawSpectrum = new float[dftPointNumber];

                // Update the spectrum.
                AudioJackGetRawSpectrum (analyzer, rawSpectrum);
                rawSpectrumIsUpdated = true;
            }
            return rawSpectrum;
        }
    }
    
    // Channel RMS level array (readonly).
    public float[] ChannelRmsLevels {
        get {
            if (!channelRmsLevelsAreUpdated)
            {
                if (useAudioListener)
                {
                    // Reallocate the array if mismatch.
                    if (channelRmsLevels == null || channelRmsLevels.Length != 2)
                        channelRmsLevels = new float[2];

                    // Special case: Update the loopback buffer if not yet.
                    if (loopbackBuffer1 == null) UpdateLoopbackBuffer();

                    // Update the array.
                    channelRmsLevels[0] = AudioJackCalculateRmsWaveform (loopbackBuffer1, 512U);
                    channelRmsLevels[1] = AudioJackCalculateRmsWaveform (loopbackBuffer2, 512U);
                }
                else
                {
                    // Reallocate the array if mismatch.
                    var count = AudioJackCountInputChannels ();
                    if (channelRmsLevels == null || channelRmsLevels.Length != count)
                        channelRmsLevels = new float[count];
                    
                    // Update the array.
                    float duration = 1.0f / 60;
                    for (var i = 0U; i < count; i++)
                        channelRmsLevels [i] = AudioJackCalculateRmsAudioInput (i, duration);
                }

                channelRmsLevelsAreUpdated = true;
            }
            return channelRmsLevels;
        }
    }

    #region Private variables

    // Octave band spectrum.
    float[] octaveBandSpectrum;
    float[] tempArrayForOctaveBandSpectrum;
    bool octaveBandSpectrumIsUpdated;

    // Raw DFT spectrum.
    float[] rawSpectrum;
    uint dftPointNumber;
    bool rawSpectrumIsUpdated;

    // Channel RMS level array.
    float[] channelRmsLevels;
    bool channelRmsLevelsAreUpdated;

    // Loopback buffer.
    float[] loopbackBuffer1;
    float[] loopbackBuffer2;

    // Timekeeping.
    float timer;

    // Reference to the analyzer object.
    System.IntPtr analyzer;

    #endregion

    #region Private functions

    uint GetRecommendedDftPointNumber ()
    {
        if (octaveBandType <= OctaveBandType.FourBandVisual)
            return 512U;
        if (octaveBandType <= OctaveBandType.TenBand)
            return 1024U;
        return (octaveBandType == OctaveBandType.TwentySixBand) ? 2048U : 4096U;
    }

    void UpdateLoopbackBuffer ()
    {
        if (loopbackBuffer1 == null || loopbackBuffer1.Length != dftPointNumber)
        {
            loopbackBuffer1 = new float[dftPointNumber];
            loopbackBuffer2 = new float[dftPointNumber];
        }
        AudioListener.GetOutputData (loopbackBuffer1, 0);
        AudioListener.GetOutputData (loopbackBuffer2, 1);
    }

    #endregion

    #region MonoBehaviour functions

    void Awake ()
    {
        AudioJackInitialize ();
        tempArrayForOctaveBandSpectrum = new float[32];
        instance = this;
    }

    void Update ()
    {
		if (minimumInterval > 0.0f)
        {
            if (timer > 0.0f)
            {
                timer -= Time.deltaTime;
                return;
            }
            while (timer <= 0.0f)
                timer += minimumInterval;
		}

        dftPointNumber = GetRecommendedDftPointNumber ();
        AudioJackSetDftPointNumber (analyzer, dftPointNumber);
        AudioJackSetOctaveBandType (analyzer, (uint)octaveBandType);
        if (useAudioListener)
        {
            UpdateLoopbackBuffer ();
            AudioJackAnalyzerWaveform (analyzer, loopbackBuffer1, loopbackBuffer2, AudioSettings.outputSampleRate); 
        }
        else
        {
            loopbackBuffer1 = loopbackBuffer2 = null;
            AudioJackAnalyzeAudioInput (analyzer, (uint)channelToAnalyze, (uint)channelSelect);
        }
        octaveBandSpectrumIsUpdated = rawSpectrumIsUpdated = channelRmsLevelsAreUpdated = false;
    }

    void OnEnable ()
    {
        if (analyzer == System.IntPtr.Zero)
        {
            analyzer = AudioJackCreateAnalyzer ();
            Update ();
        }
    }

    void OnDisable ()
    {
        if (analyzer != System.IntPtr.Zero)
        {
            AudioJackReleaseAnalyzer (analyzer);
            analyzer = System.IntPtr.Zero;
        }
    }

    #endregion

    #region Native plugin import
    
    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackInitialize ();

    [DllImport ("AudioJackPlugin")]
    static extern uint AudioJackCountInputChannels ();

    [DllImport ("AudioJackPlugin")]
    static extern System.IntPtr AudioJackCreateAnalyzer ();

    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackReleaseAnalyzer (System.IntPtr analyzer);

    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackSetDftPointNumber (System.IntPtr analyzer, uint number);

    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackSetOctaveBandType (System.IntPtr analyzer, uint type);

    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackAnalyzeAudioInput (System.IntPtr analyzer, uint channel, uint mode);

    [DllImport ("AudioJackPlugin")]
    static extern void AudioJackAnalyzerWaveform (System.IntPtr analyzer, float[] waveform1, float[] waveform2, float sampleRate);

    [DllImport ("AudioJackPlugin")]
    static extern uint AudioJackGetRawSpectrum (System.IntPtr analyzer, float[] destination);

    [DllImport ("AudioJackPlugin")]
    static extern uint AudioJackGetOctaveBandSpectrum (System.IntPtr analyzer, float[] destination);

    [DllImport ("AudioJackPlugin")]
    static extern float AudioJackCalculateRmsAudioInput (uint channel, float duration);

    [DllImport ("AudioJackPlugin")]
    static extern float AudioJackCalculateRmsWaveform (float[] waveform, uint length);
    
    #endregion
}
