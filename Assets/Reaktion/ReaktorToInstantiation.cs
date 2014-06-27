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

public class ReaktorToInstantiation : MonoBehaviour
{
    // General options.
    public GameObject[] prefabs;
    public bool randomRotation = true;
    public Transform parent;

    // Options for burst instantiation.
    public bool enableBurst;
    public float burstThreshold = 0.9f;
    public float burstInterval = 0.1f;
    public int burstNumber = 5;

    // Options for time-based instantiation
    public bool enableTimer;
    public AnimationCurve rateCurve = AnimationCurve.Linear(0, 0, 1, 1);

    // Instantiation range information
    public enum RangeType { Sphere, Box };
    public RangeType rangeType = RangeType.Sphere;
    public float rangeRadius = 1.0f;
    public Vector3 rangeVector = Vector3.one;

    // Private variables
    Reaktor reaktor;
    float previousOutput;
    float burstIntervalTimer;
    float timer;

    void Start()
    {
        reaktor = Reaktor.SearchAvailableFrom(gameObject);
    }

    void Update()
    {
        // Burst instantiation
        if (enableBurst && burstIntervalTimer <= 0.0f)
        {
            if (previousOutput < burstThreshold && reaktor.Output >= burstThreshold)
            {
                for (var i = 0; i < burstNumber; i++) Spawn();
                burstIntervalTimer = burstInterval;
            }
        }

        burstIntervalTimer -= Time.deltaTime;

        // Time-based instantiation
        if (enableTimer)
        {
            timer += rateCurve.Evaluate(reaktor.Output) * Time.deltaTime;
            for (; timer > 1.0f; timer -= 1.0f) Spawn();
        }

        previousOutput = reaktor.Output;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        if (rangeType == RangeType.Sphere)
        {
            Gizmos.DrawWireSphere(Vector3.zero, rangeRadius);
        }
        else
        {
            Gizmos.DrawWireCube(Vector3.zero, rangeVector);
        }
    }

    Vector3 GetRandomPositionInRange()
    {
        if (rangeType == RangeType.Sphere)
        {
            return Random.insideUnitSphere * rangeRadius;
        }
        else
        {
            var rv = new Vector3(Random.value, Random.value, Random.value);
            return Vector3.Scale(rv - Vector3.one * 0.5f, rangeVector);
        }
    }

    void Spawn()
    {
        var prefab = prefabs[Random.Range(0, prefabs.Length)];
        var position = transform.TransformPoint(GetRandomPositionInRange());
        var rotation = randomRotation ? Random.rotation : prefab.transform.rotation * transform.rotation;
        var instance = Instantiate(prefab, position, rotation) as GameObject;
        if (parent != null) instance.transform.parent = parent;
    }
}
