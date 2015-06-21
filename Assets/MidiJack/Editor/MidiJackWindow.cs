//
// MidiJack - MIDI Input Plugin for Unity
//
// Copyright (C) 2013-2015 Keijiro Takahashi
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
using UnityEditor;
using System.Runtime.InteropServices;

namespace MidiJack
{
    class MidiJackWindow : EditorWindow
    {
        #region Custom Editor Window Code

        [MenuItem("Window/MIDI Jack")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<MidiJackWindow>("MIDI Jack");
        }

        void OnGUI()
        {
            var endpointCount = CountEndpoints();

            // Endpoints
            var temp = "Detected MIDI devices:";
            for (var i = 0; i < endpointCount; i++)
            {
                var id = GetEndpointIdAtIndex(i);
                var name = GetEndpointName(id);
                temp += "\n" + id.ToString("X8") + ": " + name;
            }
            EditorGUILayout.HelpBox(temp, MessageType.None);

            // Message history
            temp = "Recent MIDI messages:";
            foreach (var message in MidiDriver.Instance.History)
                temp += "\n" + message.ToString();
            EditorGUILayout.HelpBox(temp, MessageType.None);
        }

        #endregion

        #region Update And Repaint

        const int _updateInterval = 15;
        int _countToUpdate;
        int _lastMessageCount;

        void Update()
        {
            if (--_countToUpdate > 0) return;

            var mcount = MidiDriver.Instance.TotalMessageCount;
            if (mcount != _lastMessageCount) {
                Repaint();
                _lastMessageCount = mcount;
            }

            _countToUpdate = _updateInterval;
        }

        #endregion

        #region Native Plugin Interface

        [DllImport("MidiJackPlugin", EntryPoint="MidiJackCountEndpoints")]
        static extern int CountEndpoints();

        [DllImport("MidiJackPlugin", EntryPoint="MidiJackGetEndpointIDAtIndex")]
        static extern uint GetEndpointIdAtIndex(int index);

        [DllImport("MidiJackPlugin")]
        static extern System.IntPtr MidiJackGetEndpointName(uint id);

        static string GetEndpointName(uint id) {
            return Marshal.PtrToStringAnsi(MidiJackGetEndpointName(id));
        }

        #endregion
    }
}
