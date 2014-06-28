using UnityEngine;
using System.Collections;

[AddComponentMenu("Reaktion/Source/Reaktor Source")]
public class ReaktorSource : MonoBehaviour
{
    public bool mute = true;

    const float zeroOffset = 1.5849e-13f;
    const float refLevel = 0.70710678118f; // 1/sqrt(2)
    const float minDB = -60.0f;

    float squareSum;
    int sampleCount;

    float dbLevel;

    public float DbLevel
    {
        get { return dbLevel; }
    }

    void Update()
    {
        if (sampleCount < 1) return;

        var rms = Mathf.Min(1.0f, Mathf.Sqrt(squareSum / sampleCount));
        dbLevel = 20.0f * Mathf.Log10(rms / refLevel + zeroOffset);

        squareSum = 0;
        sampleCount = 0;
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (var i = 0; i < data.Length; i += channels)
        {
            var level = data[i];
            squareSum += level * level;
        }

        sampleCount += data.Length / channels;

        if (mute)
            for (var i = 0; i < data.Length; i++)
                data[i] = 0;
    }
}
