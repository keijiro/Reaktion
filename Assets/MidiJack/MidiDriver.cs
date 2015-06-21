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
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MidiJack
{
    public class MidiDriver
    {
        #region Internal Data

        class ChannelState
        {
            // Note state array
            // X<0    : Released on this frame
            // X=0    : Off
            // 0<X<=1 : On (X represents velocity)
            // 1<X<=2 : Triggered on this frame
            //          (X-1 represents velocity)
            public float[] _noteArray;

            // Knob number to knob value mapping
            public Dictionary<int, float> _knobMap;

            public ChannelState()
            {
                _noteArray = new float[128];
                _knobMap = new Dictionary<int, float>();
            }
        }

        // Channel state array
        ChannelState[] _channelArray;

        // Last update frame number
        int _lastFrame;

        #endregion

        #region Accessor Methods

        public float GetKey(MidiChannel channel, int noteNumber)
        {
            UpdateIfNeeded();
            var v = _channelArray[(int)channel]._noteArray[noteNumber];
            if (v > 1) return v - 1;
            if (v > 0) return v;
            return 0.0f;
        }

        public bool GetKeyDown(MidiChannel channel, int noteNumber)
        {
            UpdateIfNeeded();
            return _channelArray[(int)channel]._noteArray[noteNumber] > 1;
        }

        public bool GetKeyUp(MidiChannel channel, int noteNumber)
        {
            UpdateIfNeeded();
            return _channelArray[(int)channel]._noteArray[noteNumber] < 0;
        }

        public int[] GetKnobNumbers(MidiChannel channel)
        {
            UpdateIfNeeded();
            var cs = _channelArray[(int)channel];
            var numbers = new int[cs._knobMap.Count];
            cs._knobMap.Keys.CopyTo(numbers, 0);
            return numbers;
        }

        public float GetKnob(MidiChannel channel, int knobNumber, float defaultValue)
        {
            UpdateIfNeeded();
            var cs = _channelArray[(int)channel];
            if (cs._knobMap.ContainsKey(knobNumber)) return cs._knobMap[knobNumber];
            return defaultValue;
        }

        #endregion

        #region Editor Support

        #if UNITY_EDITOR

        // Update timer
        const float _updateInterval = 1.0f / 30;
        float _lastUpdateTime;

        bool CheckUpdateInterval()
        {
            var current = Time.realtimeSinceStartup;
            if (current - _lastUpdateTime > _updateInterval || current < _lastUpdateTime) {
                _lastUpdateTime = current;
                return true;
            }
            return false;
        }

        // Total message count
        int _totalMessageCount;

        public int TotalMessageCount {
            get {
                UpdateIfNeeded();
                return _totalMessageCount;
            }
        }

        // Message history
        Queue<MidiMessage> _messageHistory;

        public Queue<MidiMessage> History {
            get { return _messageHistory; }
        }

        #endif

        #endregion

        #region Public Methods

        MidiDriver()
        {
            _channelArray = new ChannelState[17];
            for (var i = 0; i < 17; i++)
                _channelArray[i] = new ChannelState();

            #if UNITY_EDITOR
            _messageHistory = new Queue<MidiMessage>();
            #endif
        }

        #endregion

        #region Private Methods

        void UpdateIfNeeded()
        {
            if (Application.isPlaying)
            {
                var frame = Time.frameCount;
                if (frame != _lastFrame) {
                    Update();
                    _lastFrame = frame;
                }
            }
            else
            {
                #if UNITY_EDITOR
                if (CheckUpdateInterval()) Update();
                #endif
            }
        }

        void Update()
        {
            // Update the note state array.
            foreach (var cs in _channelArray)
            {
                for (var i = 0; i < 128; i++)
                {
                    var x = cs._noteArray[i];
                    if (x > 1)
                        cs._noteArray[i] = x - 1; // Key down -> Hold.
                    else if (x < 0)
                        cs._noteArray[i] = 0; // Key up -> Off.
                }
            }

            // Process the message queue.
            while (true)
            {
                // Pop from the queue.
                var data = DequeueIncomingData();
                if (data == 0) break;

                // Parse the message.
                var message = new MidiMessage(data);

                // Split the first byte.
                var statusCode = message.status >> 4;
                var channelNumber = message.status & 0xf;

                // Note on message?
                if (statusCode == 9)
                {
                    var velocity = 1.0f / 127 * message.data2 + 1;
                    _channelArray[channelNumber]._noteArray[message.data1] = velocity;
                    _channelArray[(int)MidiChannel.All]._noteArray[message.data1] = velocity;
                }

                // Note off message?
                if (statusCode == 8 || (statusCode == 9 && message.data2 == 0))
                {
                    _channelArray[channelNumber]._noteArray[message.data1] = -1;
                    _channelArray[(int)MidiChannel.All]._noteArray[message.data1] = -1;
                }

                // CC message?
                if (statusCode == 0xb)
                {
                    // Normalize the value.
                    var level = 1.0f / 127 * message.data2;
                    // Update the channel if it already exists, or add a new channel.
                    _channelArray[channelNumber]._knobMap[message.data1] = level;
                    // Do again for All-ch.
                    _channelArray[(int)MidiChannel.All]._knobMap[message.data1] = level;
                }

                #if UNITY_EDITOR
                // Record the message.
                _totalMessageCount++;
                _messageHistory.Enqueue(message);
                #endif
            }

            #if UNITY_EDITOR
            // Truncate the history.
            while (_messageHistory.Count > 8)
                _messageHistory.Dequeue();
            #endif
        }

        #endregion

        #region Native Plugin Interface

        [DllImport("MidiJackPlugin", EntryPoint="MidiJackDequeueIncomingData")]
        public static extern ulong DequeueIncomingData();

        #endregion

        #region Singleton Class Instance

        static MidiDriver _instance;

        public static MidiDriver Instance {
            get {
                if (_instance == null)
                    _instance = new MidiDriver();
                return _instance;
            }
        }

        #endregion
    }
}
