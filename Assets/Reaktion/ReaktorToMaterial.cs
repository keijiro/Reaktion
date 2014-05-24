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

public class ReaktorToMaterial : MonoBehaviour
{
    public string colorName = "_Color";
    public Color colorFrom = Color.black;
    public Color colorTo = Color.white;

    public string floatName;
    public float floatFrom = 0.0f;
    public float floatTo = 1.0f;

    public string vectorName;
    public Vector4 vectorFrom = Vector4.zero;
    public Vector4 vectorTo = Vector4.one;

    Reaktor reaktor;
    Material material;

    void Start()
    {
        reaktor = Reaktor.SearchAvailableFrom(gameObject);
        material = renderer.material;
        UpdateMaterial(0);
    }

    void Update()
    {
        UpdateMaterial(reaktor.Output);
    }

    void UpdateMaterial(float param)
    {
        if (colorName.Length > 0)
            material.SetColor(colorName, Color.Lerp(colorFrom, colorTo, param));
        if (floatName.Length > 0)
            material.SetFloat(floatName, Mathf.Lerp(floatFrom, floatTo, param));
        if (vectorName.Length > 0)
            material.SetVector(vectorName, Vector4.Lerp(vectorFrom, vectorTo, param));
    }
}
