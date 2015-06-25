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
namespace MidiJack
{
    public static class MidiMaster
    {
        // Returns the key state (on: velocity, off: zero).
        public static float GetKey(MidiChannel channel, int noteNumber)
        {
            return MidiDriver.Instance.GetKey(channel, noteNumber);
        }

        public static float GetKey(int noteNumber)
        {
            return MidiDriver.Instance.GetKey(MidiChannel.All, noteNumber);
        }

        // Returns true if the key was pressed down in the current frame.
        public static bool GetKeyDown(MidiChannel channel, int noteNumber)
        {
            return MidiDriver.Instance.GetKeyDown(channel, noteNumber);
        }

        public static bool GetKeyDown(int noteNumber)
        {
            return MidiDriver.Instance.GetKeyDown(MidiChannel.All, noteNumber);
        }

        // Returns true if the key was released in the current frame.
        public static bool GetKeyUp(MidiChannel channel, int noteNumber)
        {
            return MidiDriver.Instance.GetKeyUp(channel, noteNumber);
        }

        public static bool GetKeyUp(int noteNumber)
        {
            return MidiDriver.Instance.GetKeyUp(MidiChannel.All, noteNumber);
        }

        // Provides the CC (knob) list.
        public static int[] GetKnobNumbers(MidiChannel channel)
        {
            return MidiDriver.Instance.GetKnobNumbers(channel);
        }

        public static int[] GetKnobNumbers()
        {
            return MidiDriver.Instance.GetKnobNumbers(MidiChannel.All);
        }

        // Returns the CC (knob) value.
        public static float GetKnob(MidiChannel channel, int knobNumber, float defaultValue = 0)
        {
            return MidiDriver.Instance.GetKnob(channel, knobNumber, defaultValue);
        }

        public static float GetKnob(int knobNumber, float defaultValue = 0)
        {
            return MidiDriver.Instance.GetKnob(MidiChannel.All, knobNumber, defaultValue);
        }
    }
}
