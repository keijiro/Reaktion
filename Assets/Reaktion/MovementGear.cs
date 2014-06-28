//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
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
using System.Collections;

namespace Reaktion {

[AddComponentMenu("Reaktion/Gear/Movement Gear")]
public class MovementGear : MonoBehaviour
{
    public bool autoBind = true;
    public Reaktor reaktor;

    public bool enableTranslation;
    public Vector3 translationVector = Vector3.up;
    public AnimationCurve velocityCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public bool enableRotation;
    public Vector3 rotationAxis = Vector3.up;
    public AnimationCurve angularVelocityCurve = AnimationCurve.Linear(0, 0, 1, 360);

    public bool useLocal = true;

    void Awake()
    {
        if (autoBind || reaktor == null)
            reaktor = Reaktor.SearchAvailableFrom(gameObject);
    }

    void Update()
    {
        if (enableTranslation)
        {
            var delta = translationVector * velocityCurve.Evaluate(reaktor.Output) * Time.deltaTime;
            if (useLocal)
                transform.localPosition += delta;
            else
                transform.position += delta;
        }

        if (enableRotation)
        {
            var omega = angularVelocityCurve.Evaluate(reaktor.Output);
            var delta = Quaternion.AngleAxis(omega * Time.deltaTime, rotationAxis);
            if (useLocal)
                transform.localRotation = delta * transform.localRotation;
            else
                transform.rotation = delta * transform.rotation;
        }
    }
}

} // namespace Reaktion
