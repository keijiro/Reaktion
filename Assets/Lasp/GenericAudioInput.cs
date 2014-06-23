using UnityEngine;
using System.Collections;

public class GenericAudioInput : MonoBehaviour
{
    const int bufferSizeInLaspMode = 320;

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
        AudioSettings.SetDSPBufferSize(bufferSizeInLaspMode, 4);

        // Create a null clip and kick it.
        audioSource.clip = AudioClip.Create("(null)", 1024, 1, sampleRate, false, false);
        audioSource.Play();

        // Display some info.
        var latency = 1000.0f * bufferSizeInLaspMode / sampleRate;
        Debug.Log("Low-latency mode (latency=" + latency + "msec)");
#else
        // Create a clip which is assigned to the default microphone.
        int minFreq, maxFreq;
        Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
        audio.clip = Microphone.Start(null, true, 1, minFreq > 0 ? minFreq : 44100);

        if (audio.clip != null)
        {
            // Wait until the microphone gets initialized.
            int delay = 0;
            while (delay <= 0) delay = Microphone.GetPosition(null);

            // Start playing.
            audio.Play();

            // Display some Info.
            var latency = 1000.0f * delay / sampleRate;
            Debug.Log("Microphone mode (latency=" + latency + "msec)");
        }
        else
        {
            Debug.LogWarning("AudioInput: Initialization failed.");
        }
#endif
    }

#if UNITY_STANDALONE_OSX && UNITY_PRO_LICENSE

    void OnAudioFilterRead(float[] data, int channels)
    {
        Lasp.RetrieveWaveform(0, data, (uint)data.Length, (uint)channels);
    }

#endif
}
