//
// MidiJack - MIDI Input Plug-in for Unity
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
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MidiJack))]
class MidiJackEditor : Editor
{
    public override void OnInspectorGUI ()
    {
        // Only shows the details on Play Mode.
        if (EditorApplication.isPlaying)
        {
            var endpointCount = MidiJack.CountEndpoints ();

            // Endpoints.
            var temp = "Detected MIDI endpoints:";
            for (var i = 0; i < endpointCount; i++)
            {
                var id = MidiJack.GetEndpointIdAtIndex (i);
                var name = MidiJack.GetEndpointName (id);
                temp += "\n" + id.ToString ("X8") + ": " + name;
            }
            EditorGUILayout.HelpBox (temp, MessageType.None);

            // Incomming messages.
            temp = "Incoming MIDI messages:";
            foreach (var message in (target as MidiJack).History)
            {
                temp += "\n" + message.ToString ();
            }
            EditorGUILayout.HelpBox (temp, MessageType.None);

            // Make itself dirty to update on every time.
            EditorUtility.SetDirty (target);
        }
        else
        {
            EditorGUILayout.HelpBox ("You can view the sutatus on Play Mode.", MessageType.Info);
        }
    }
}
