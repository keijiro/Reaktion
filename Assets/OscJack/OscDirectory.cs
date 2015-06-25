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
using System.Collections;
using System.Collections.Generic;

namespace OscJack
{
    // OSC data directory class
    // Provides last received data for each address.
    public class OscDirectory : IEnumerable<KeyValuePair<string, Object[]>>
    {
        #region Public Methods

        public OscDirectory(int port) : this(new int[]{port})
        {
        }

        public OscDirectory(int[] portList)
        {
            _dataStore = new Dictionary<string, Object[]>();
            _servers = new OscServer[portList.Length];
            for (var i = 0; i < portList.Length; i++) {
                _servers[i] = new OscServer(portList[i]);
                _servers[i].Start();
            }
        }

        public int TotalMessageCount {
            get {
                UpdateState();
                return _totalMessageCount;
            }
        }

        public bool HasData(string address)
        {
            return _dataStore.ContainsKey(address);
        }

        public Object[] GetData(string address)
        {
            UpdateState();
            Object[] data;
            _dataStore.TryGetValue(address, out data);
            return data;
        }

        #endregion

        #region Enumerable Interface

        public IEnumerator<KeyValuePair<string, Object[]>> GetEnumerator()
        {
            return _dataStore.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dataStore.GetEnumerator();
        }

        #endregion

        #region Private Objects And Functions

        Dictionary<string, Object[]> _dataStore;
        OscServer[] _servers;
        int _totalMessageCount;

        void UpdateState()
        {
            foreach (var server in _servers) {
                while (server.MessageCount > 0) {
                    var message = server.PopMessage();
                    _dataStore[message.address] = message.data;
                    _totalMessageCount++;
                }
            }
        }

        #endregion
    }
}
