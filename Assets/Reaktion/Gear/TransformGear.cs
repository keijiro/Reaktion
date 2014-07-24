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

[AddComponentMenu("Reaktion/Gear/Transform Gear")]
public class TransformGear : MonoBehaviour
{
    public bool autoBind = true;
    public Reaktor reaktor;

    public Modifier position;
    public Vector3 positionVector = Vector3.up;

    public Modifier rotation = Modifier.Linear(-90, 90);
    public Vector3 rotationAxis = Vector3.up;

    public Modifier scale;
    public Vector3 scaleVector = Vector3.one;

    public bool addInitialValue = true;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 initialScale;

    void Awake()
    {
        if (autoBind || reaktor == null)
            reaktor = Reaktor.SearchAvailableFrom(gameObject);
    }

    void OnEnable()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (position.enabled)
        {
            transform.localPosition = positionVector * position.Evaluate(reaktor.Output);
            if (addInitialValue) transform.localPosition += initialPosition;
        }

        if (rotation.enabled)
        {
            transform.localRotation = Quaternion.AngleAxis(rotation.Evaluate(reaktor.Output), rotationAxis);
            if (addInitialValue) transform.localRotation *= initialRotation;
        }

        if (scale.enabled)
        {
            transform.localScale = scaleVector * scale.Evaluate(reaktor.Output);
            if (addInitialValue) transform.localScale += initialScale;
        }
    }
}

} // namespace Reaktion
