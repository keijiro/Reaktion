//
// Reaktion - Audio Reactive Animation Toolkit
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
using OscJack;

namespace Reaktion
{
    [AddComponentMenu("Reaktion/Injector/OSC Injector")]
    public class OscInjector : InjectorBase
    {
        public enum ScaleMode { Linear01, Decibel };
        public ScaleMode scaleMode;
        public string address = "/reaktion";
        public int dataIndex = 0;

        void Update()
        {
            System.Object[] data = OscMaster.GetData(address);
            if (data == null) return;

            var level = (float)data[dataIndex];

            if (scaleMode == ScaleMode.Linear01)
            {
                const float refLevel = 0.70710678118f; // 1/sqrt(2)
                const float zeroOffs = 1.5849e-13f;
                level = Mathf.Clamp01(level);
                dbLevel = Mathf.Log(level / refLevel + zeroOffs, 10) * 20;
            }
            else
            {
                dbLevel = Mathf.Min(level, 0.0f);
            }
        }
    }
}
