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
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OscJack
{
    // OSC over UDP server class
    public class OscServer
    {
        Thread _thread;
        UdpClient _udpClient;
        IPEndPoint _endPoint;
        OscParser _osc;

        public bool IsRunning {
            get { return _thread != null && _thread.IsAlive; }
        }

        public int MessageCount {
            get {
                lock (_osc) return _osc.MessageCount;
            }
        }

        public OscMessage PopMessage()
        {
            lock (_osc) return _osc.PopMessage();
        }

        public OscServer(int listenPort)
        {
            _endPoint = new IPEndPoint(IPAddress.Any, listenPort);
            _udpClient = new UdpClient(_endPoint);
            _osc = new OscParser();
        }

        public void Start()
        {
            if (_thread == null) {
                _thread = new Thread(ServerLoop);
                _thread.Start();
            }
        }

        public void Close()
        {
            _udpClient.Close();
        }

        void ServerLoop()
        {
            try {
                while (true) {
                    var data = _udpClient.Receive(ref _endPoint);
                    lock (_osc) _osc.FeedData(data);
                }
            }
            catch (SocketException)
            {
                // Shutdown: nothing to do here.
            }
        }
    }
}
