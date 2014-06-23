using System.Runtime.InteropServices;

public static class Lasp
{
#if UNITY_STANDALONE_OSX && UNITY_PRO_LICENSE
    [DllImport("Lasp", EntryPoint="LaspInitialize")]
    public static extern void Initialize();

    [DllImport("Lasp", EntryPoint="LaspCountInputChannels")]
    public static extern uint CountInputChannels();

    [DllImport("Lasp", EntryPoint="LaspRetrieveWaveform")]
    public static extern void RetrieveWaveform(uint sourceChannel, float[] buffer, uint bufferLength, uint channelCount);
#endif
}
