//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013 Keijiro Takahashi
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

public class ReaktorToMessage : MonoBehaviour
{
    #region Public properties

    const float MinValue = 1e-6f;

    #endregion

    #region Public properties

    // Target settings.
    public GameObject target;
    public bool broadcast;

    // Trigger message.
    public bool enableTrigger;
    public float triggerThreshold = 0.9f;
    public float triggerInterval = 0.1f;
    public string triggerMessage = "OnReaktorTrigger";
    
    // Input message.
    public bool enableInput;
	public AnimationCurve inputCurve = AnimationCurve.Linear (0, 0, 1, 1);
    public string inputMessage = "OnReaktorInput";

    #endregion

    #region Private variables

    Reaktor reaktor;
    float previousOutput = MinValue;
    float triggerTimer;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
    }

    void Update ()
    {
        var sendTo = (target == null) ? gameObject : target;
        var level = Mathf.Max (reaktor.Output, MinValue);

        // Trigger message.
        if (enableTrigger)
        {
            if (triggerTimer > triggerInterval && previousOutput < triggerThreshold && level >= triggerThreshold)
            {
                if (broadcast)
                {
                    sendTo.BroadcastMessage (triggerMessage, true);
                }
                else
                {
                    sendTo.SendMessage (triggerMessage, true);
                }
                triggerTimer = 0.0f;
            }
        }
        
        // Input message.
        if (enableInput && level != previousOutput)
        {
            if (broadcast)
            {
                sendTo.BroadcastMessage (inputMessage, inputCurve.Evaluate (level));
            }
            else
            {
                sendTo.SendMessage (inputMessage, inputCurve.Evaluate (level));
            }
        }

        previousOutput = level;
        triggerTimer += Time.deltaTime;
    }

    #endregion
}
