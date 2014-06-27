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
    public bool autoBind = true;
    public Reaktor reaktor;

    public bool enableTranslation;
    public Vector3 translationVector = Vector3.up;
    public AnimationCurve translationCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public bool enableRotation;
    public Vector3 rotationAxis = Vector3.up;
    public AnimationCurve rotationCurve = AnimationCurve.Linear(0, -90, 1, 90);

    public bool enableScaling;
    public Vector3 scalingVector = Vector3.one;
    public AnimationCurve scalingCurve = AnimationCurve.Linear(0, 0, 1, 1);

    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 initialScale;

    void Awake()
    {
        if (autoBind || reaktor == null)
            reaktor = Reaktor.SearchAvailableFrom(gameObject);
    }

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (enableTranslation)
            transform.localPosition = initialPosition + translationVector * translationCurve.Evaluate(reaktor.Output);

        if (enableRotation)
            transform.localRotation = Quaternion.AngleAxis(rotationCurve.Evaluate(reaktor.Output), rotationAxis) * initialRotation;

        if (enableScaling)
            transform.localScale = initialScale + scalingVector * scalingCurve.Evaluate(reaktor.Output);
    }
}
