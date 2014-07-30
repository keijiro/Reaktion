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

namespace Reaktion {

public class TurbulentMotion : MonoBehaviour
{
    // Amount of changes.
    public Vector3 maxDisplacement = Vector3.one;
    public Vector3 maxRotation     = new Vector3(30, 30, 0);
    public Vector3 minScale        = Vector3.one;

    // Flow settings.
    public Vector3 flowVector  = Vector3.up * 0.45f;
    public float   flowDensity = 1;

    // Coefficients (multiplier) for the elements.
    public float coeffDisplacement = 1;
    public float coeffRotation     = 1;
    public float coeffScale        = 1;

    // Misc settings.
    public bool   useLocalCoordinate = true;
    public bool   scaleByShader      = false;
    public string scalePropertyName  = "_Scale";

    // Initial states.
    Vector3    initialPosition;
    Quaternion initialRotation;
    Vector3    initialScale;

    void OnEnable()
    {
        // Store the initial states when it's enabled.
        if (useLocalCoordinate)
        {
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
        }
        else
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Noise position.
        var np = initialPosition * flowDensity + flowVector * Time.time;

        // Offset for the noise position.
        var offs = new Vector3(13, 17, 19);

        // Displacement.
        if (maxDisplacement != Vector3.zero)
        {
            // Noise position for the displacement.
            var npd = np * coeffDisplacement;

            // Get noise values.
            var vd = maxDisplacement;
            vd = new Vector3(
                vd.x == 0.0f ? 0.0f : vd.x * Perlin.Noise(npd),
                vd.y == 0.0f ? 0.0f : vd.y * Perlin.Noise(npd + offs),
                vd.z == 0.0f ? 0.0f : vd.z * Perlin.Noise(npd + offs * 2)
            );

            // Apply the displacement.
            if (useLocalCoordinate)
                transform.localPosition = initialPosition + vd;
            else
                transform.position = initialPosition + vd;
        }

        // Rotation.
        if (maxRotation != Vector3.zero)
        {
            // Noise position for the rotation.
            var npr = np * coeffRotation;

            // Get noise values.
            var vr = maxRotation;
            vr = new Vector3(
                vr.x == 0.0f ? 0.0f : vr.x * Perlin.Noise(npr + offs * 3),
                vr.y == 0.0f ? 0.0f : vr.y * Perlin.Noise(npr + offs * 4),
                vr.z == 0.0f ? 0.0f : vr.z * Perlin.Noise(npr + offs * 5)
            );

            // Apply the rotation.
            if (useLocalCoordinate)
                transform.localRotation = Quaternion.Euler(vr) * initialRotation;
            else
                transform.rotation = Quaternion.Euler(vr) * initialRotation;
        }

        // Scale.
        if (minScale != Vector3.one)
        {
            // Noise position for the scale.
            var nps = np * coeffScale;

            // Get noise values.
            var vs = minScale;
            vs = new Vector3(
                vs.x == 1.0f ? 1.0f : Mathf.Lerp((Perlin.Noise(nps + offs * 6) + 1) * 0.5f, 1.0f, vs.x),
                vs.y == 1.0f ? 1.0f : Mathf.Lerp((Perlin.Noise(nps + offs * 7) + 1) * 0.5f, 1.0f, vs.y),
                vs.z == 1.0f ? 1.0f : Mathf.Lerp((Perlin.Noise(nps + offs * 8) + 1) * 0.5f, 1.0f, vs.z)
            );

            // Apply the scale.
            if (scaleByShader)
                renderer.material.SetVector(scalePropertyName, vs);
            else
                transform.localScale = Vector3.Scale(initialScale, vs);
        }
    }
}

} // namespace Reaktion
