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

[AddComponentMenu("Reaktion/Gear/Jitter Gear")]
public class JitterGear : MonoBehaviour
{
    public bool autoBind = true;
    public Reaktor reaktor;

    public bool changePositionFrequency;
    public float positionFrequencyMin = 0;
    public float positionFrequencyMax = 0.2f;
    public AnimationCurve positionFrequencyCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public bool changeRotationFrequency;
    public float rotationFrequencyMin = 0;
    public float rotationFrequencyMax = 0.2f;
    public AnimationCurve rotationFrequencyCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public bool changePositionAmount;
    public float positionAmountMin = 0;
    public float positionAmountMax = 1;
    public AnimationCurve positionAmountCurve = AnimationCurve.Linear(0, 0, 1, 1);

    public bool changeRotationAmount;
    public float rotationAmountMin = 0;
    public float rotationAmountMax = 30;
    public AnimationCurve rotationAmountCurve = AnimationCurve.Linear(0, 0, 1, 1);

    TransformJitter jitter;

    void Awake()
    {
        if (autoBind || reaktor == null)
            reaktor = Reaktor.SearchAvailableFrom(gameObject);

        jitter = GetComponent<TransformJitter>();
    }

    void Update()
    {
        var o = reaktor.Output;

        if (changePositionFrequency)
            jitter.positionFrequency = Mathf.Lerp(positionFrequencyMin, positionFrequencyMax, positionFrequencyCurve.Evaluate(o));

        if (changeRotationFrequency)
            jitter.rotationFrequency = Mathf.Lerp(rotationFrequencyMin, rotationFrequencyMax, rotationFrequencyCurve.Evaluate(o));

        if (changePositionAmount)
            jitter.positionAmount = Mathf.Lerp(positionAmountMin, positionAmountMax, positionAmountCurve.Evaluate(o));

        if (changeRotationAmount)
            jitter.rotationAmount = Mathf.Lerp(rotationAmountMin, rotationAmountMax, rotationAmountCurve.Evaluate(o));
    }
}

} // namespace Reaktion
