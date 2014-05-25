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

public class ReaktorToAnimator : MonoBehaviour
{
    #region Public properties

    // Speed control.
    public bool enableSpeed;
    public float maxSpeed = 1.0f;
    public AnimationCurve speedCurve = AnimationCurve.Linear (0, 0, 1, 1);

    // Trigger control.
    public bool enableTrigger;
    public float triggerThreshold = 0.9f;
    public string triggerName;

    #endregion

    #region Private variables

    Reaktor reaktor;
    Animator animator;
    float previousOutput;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
        animator = GetComponent<Animator> ();
    }

    void Update ()
    {
        if (enableTrigger)
        {
            if (previousOutput < triggerThreshold && reaktor.Output >= triggerThreshold)
            {
                animator.SetTrigger (triggerName);
            }
        }

        if (enableSpeed)
        {
            animator.speed = maxSpeed * speedCurve.Evaluate (reaktor.Output);
        }

        previousOutput = reaktor.Output;
    }

    #endregion
}
