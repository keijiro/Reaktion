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

public class ReaktorToActivation : MonoBehaviour
{
    public enum TargetType { GameObject, Component };
    public TargetType targetType;
    public GameObject[] targetGameObjects;
    public Component[] targetComponents;

    public float threshold = 0.9f;
    public float interval = 0.1f;
    public bool invert;
    
    Reaktor reaktor;
    bool previousState;
    float timer;

    void Awake()
    {
        reaktor = Reaktor.SearchAvailableFrom(gameObject);
        SwitchTargetState(false);
    }
	
    void Update()
    {
        if (timer <= 0.0f)
        {
            var state = (reaktor.Output >= threshold);
            if (state != previousState)
            {
                SwitchTargetState(state);
                previousState = state;
                timer = interval;
            }
        }
        timer -= Time.deltaTime;
    }

    void SwitchTargetState(bool state)
    {
        if (targetType == TargetType.GameObject)
        {
            foreach (var go in targetGameObjects)
                go.SetActive(state ^ invert);
        }
        else
        {
            foreach (var c in targetComponents)
                c.GetType().GetProperty("enabled").SetValue(c, state ^ invert, null);
        }
    }
}
