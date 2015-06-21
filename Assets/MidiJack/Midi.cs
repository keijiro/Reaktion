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
    // MIDI channel names
    public enum MidiChannel
    {
        Ch1,    // 0
        Ch2,    // 1
        Ch3,
        Ch4,
        Ch5,
        Ch6,
        Ch7,
        Ch8,
        Ch9,
        Ch10,
        Ch11,
        Ch12,
        Ch13,
        Ch14,
        Ch15,
        Ch16,
        All     // 16
    }

    // MIDI message structure
    public struct MidiMessage
    {
        public uint source; // MIDI source (endpoint) ID
        public byte status; // MIDI status byte
        public byte data1;  // MIDI data bytes
        public byte data2;

        public MidiMessage(ulong data)
        {
            source = (uint)(data & 0xffffffffUL);
            status = (byte)((data >> 32) & 0xff);
            data1 = (byte)((data >> 40) & 0xff);
            data2 = (byte)((data >> 48) & 0xff);
        }

        public override string ToString()
        {
            const string fmt = "s({0:X2}) d({1:X2},{2:X2}) from {3:X8}";
            return string.Format(fmt, status, data1, data2, source);
        }
    }
}
