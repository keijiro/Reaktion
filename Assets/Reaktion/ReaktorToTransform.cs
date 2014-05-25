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

public class ReaktorToTransform : MonoBehaviour
{
    #region Public properties

    // Position control.
    public bool enablePosition;
    public Vector3 position = Vector3.up;

    // Rotation control.
    public bool enableRotation;
    public Vector3 rotationAxis = Vector3.up;
    public float minAngle = -90.0f;
    public float maxAngle = 90.0f;

    // Scale control.
    public bool enableScale;
    public Vector3 scale = Vector3.one;

    #endregion

    #region Private variables

    Reaktor reaktor;
    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 initialScale;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    void Update ()
    {
        if (enablePosition)
        {
            transform.localPosition = initialPosition + position * reaktor.Output;
        }

        if (enableRotation)
        {
            var angle = Mathf.Lerp (minAngle, maxAngle, reaktor.Output);
            transform.localRotation = Quaternion.AngleAxis (angle, rotationAxis) * initialRotation;
        }

        if (enableScale)
        {
            transform.localScale = initialScale + scale * reaktor.Output;
        }
    }

    #endregion
}
