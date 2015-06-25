//
// OSC Jack - OSC Input Plugin for Unity
//
// Copyright (C) 2015 Keijiro Takahashi
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
using System;
using System.Collections.Generic;

namespace OscJack
{
    // OSC message storage struct
    public struct OscMessage
    {
        public string address;
        public object[] data;

        public OscMessage(string address, object[] data)
        {
            this.address = address;
            this.data = data;
        }

        public override string ToString ()
        {
            var temp = address + ":";
            for (var i = 0; i < data.Length - 1; i++)
                temp += data[i] + ",";
            return temp + data[data.Length];
        }
    }

    // OSC packet parser
    public class OscParser
    {
        #region Public Methods And Properties

        public int MessageCount {
            get { return _messageQueue.Count; }
        }

        public OscParser()
        {
            _messageQueue = new Queue<OscMessage>();
        }

        public OscMessage PopMessage()
        {
            return _messageQueue.Dequeue();
        }

        public void FeedData(Byte[] data)
        {
            _readBuffer = data;
            _readPoint = 0;

            ReadMessage();

            _readBuffer = null;
        }

        #endregion

        #region Private Implementation

        Queue<OscMessage> _messageQueue;
        Byte[] _readBuffer;
        int _readPoint;

        void ReadMessage()
        {
            var address = ReadString();

            if (address == "#bundle")
            {
                ReadInt64();

                while (true)
                {
                    if (_readPoint >= _readBuffer.Length) return;

                    var peek = _readBuffer[_readPoint];
                    if (peek == '/' || peek == '#') {
                        ReadMessage();
                        return;
                    }

                    var bundleEnd = _readPoint + ReadInt32();
                    while (_readPoint < bundleEnd)
                        ReadMessage();
                }
            }

            var types = ReadString();
            var temp = new OscMessage(address, new object[types.Length - 1]);

            for (var i = 0; i < types.Length - 1; i++)
            {
                switch (types[i + 1])
                {
                case 'f':
                    temp.data[i] = ReadFloat32();
                    break;
                case 'i':
                    temp.data[i] = ReadInt32();
                    break;
                case 's':
                    temp.data[i] = ReadString();
                    break;
                case 'b':
                    temp.data[i] = ReadBlob();
                    break;
                }
            }

            _messageQueue.Enqueue(temp);
        }

        float ReadFloat32()
        {
            Byte[] temp = {
                _readBuffer[_readPoint + 3],
                _readBuffer[_readPoint + 2],
                _readBuffer[_readPoint + 1],
                _readBuffer[_readPoint]
            };
            _readPoint += 4;
            return BitConverter.ToSingle(temp, 0);
        }

        int ReadInt32 ()
        {
            int temp =
                (_readBuffer[_readPoint + 0] << 24) +
                (_readBuffer[_readPoint + 1] << 16) +
                (_readBuffer[_readPoint + 2] << 8) +
                (_readBuffer[_readPoint + 3]);
            _readPoint += 4;
            return temp;
        }

        long ReadInt64 ()
        {
            long temp =
                ((long)_readBuffer[_readPoint + 0] << 56) +
                ((long)_readBuffer[_readPoint + 1] << 48) +
                ((long)_readBuffer[_readPoint + 2] << 40) +
                ((long)_readBuffer[_readPoint + 3] << 32) +
                ((long)_readBuffer[_readPoint + 4] << 24) +
                ((long)_readBuffer[_readPoint + 5] << 16) +
                ((long)_readBuffer[_readPoint + 6] << 8) +
                ((long)_readBuffer[_readPoint + 7]);
            _readPoint += 8;
            return temp;
        }

        string ReadString()
        {
            var offset = 0;
            while (_readBuffer[_readPoint + offset] != 0) offset++;
            var s = System.Text.Encoding.UTF8.GetString(_readBuffer, _readPoint, offset);
            _readPoint += (offset + 4) & ~3;
            return s;
        }

        Byte[] ReadBlob()
        {
            var length = ReadInt32();
            var temp = new Byte[length];
            Array.Copy(_readBuffer, _readPoint, temp, 0, length);
            _readPoint += (length + 3) & ~3;
            return temp;
        }

        #endregion
    }
}
