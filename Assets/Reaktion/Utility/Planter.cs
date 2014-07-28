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

[AddComponentMenu("Reaktion/Utility/Planter")]
public class Planter : MonoBehaviour
{
    // General parameters.
    public GameObject[] prefabs;
    public int maxObjects = 100;

    // Distribution settings.
    public enum DistributionMode { Single, Random, Grid }
    public DistributionMode distributionMode;
    public Vector2 distributionRange = new Vector2(3, 0);
    public float gridSpace = 1;

    // Rotation setting.
    public enum RotationMode { Keep, Planter, Random }
    public RotationMode rotationMode;

    // Interval settings.
    public enum IntervalMode { Distance, Time }
    public IntervalMode intervalMode;
    public float interval = 1;

    // Object pool.
    GameObject[] objectPool;
    int objectPoolIndex;

    // Variables for managing interval.
    float intervalCounter;
    Vector3 previousPosition;
    Quaternion previousRotation;

    // Put an instance at the position; Make a new one or reuse the oldest.
    void PutInstance(Vector3 position, Quaternion rotation)
    {
        // Randomize rotation if needed.
        if (rotationMode == RotationMode.Random)
            rotation = Random.rotation;

        // Pick up the oldest object.
        var go = objectPool[objectPoolIndex];

        if (go == null)
        {
            // Make a new instance and push it to the pool.
            var prefab = prefabs[Random.Range(0, prefabs.Length)];
            if (rotationMode == RotationMode.Keep)
                rotation = prefab.transform.rotation;
            go = Instantiate(prefab, position, rotation) as GameObject;
            objectPool[objectPoolIndex] = go;
        }
        else
        {
            // Reuse the oldest object in the pool.
            go.transform.position = position;
            if (rotationMode != RotationMode.Keep)
                go.transform.rotation = rotation;
        }

        // Increment the pool index.
        objectPoolIndex = (objectPoolIndex + 1) % maxObjects;
    }

    // Plant a set of objects along the grid.
    void PlantAlongGrid(Vector3 position, Quaternion rotation)
    {
        // Local direction vectors.
        var lx = rotation * Vector3.right;
        var ly = rotation * Vector3.up;

        // Number of columns and rows.
        var nx = Mathf.Max(Mathf.FloorToInt(distributionRange.x / gridSpace), 0);
        var ny = Mathf.Max(Mathf.FloorToInt(distributionRange.y / gridSpace), 0);

        // Put instances on each point of the grid.
        for (var y = 0; y <= ny; y++)
        {
            var dy = gridSpace * ((float)y - 0.5f * ny);
            for (var x = 0; x <= nx; x++)
            {
                var dx = gridSpace * ((float)x - 0.5f * nx);
                PutInstance(position + lx * dx + ly * dy, rotation);
            }
        }
    }

    // Plant a object at random.
    void PlantRandom(Vector3 position, Quaternion rotation)
    {
        // Local direction vectors.
        var lx = rotation * Vector3.right;
        var ly = rotation * Vector3.up;

        // Get random value.
        var dx = (Random.value - 0.5f) * distributionRange.x;
        var dy = (Random.value - 0.5f) * distributionRange.y;

        // Put an instance on the point.
        PutInstance(position + lx * dx + ly * dy, rotation);
    }

    void Awake()
    {
        objectPool = new GameObject[maxObjects];
    }

    void Start()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    void Update()
    {
        // Get delta value on the interval parameter.
        var delta = intervalMode == IntervalMode.Distance ?
            Vector3.Distance(transform.position, previousPosition) : Time.deltaTime;

        // Look for the next plant position between frames.
        for (var t = interval; t < intervalCounter + delta ; t += interval)
        {
            // Interpolate the position and the rotation.
            var p = (t - intervalCounter) / delta;
            var position = Vector3.Lerp(previousPosition, transform.position, p);
            var rotation = Quaternion.Slerp(previousRotation, transform.rotation, p);

            // Plant!
            if (distributionMode == DistributionMode.Grid)
                PlantAlongGrid(position, rotation);
            else if (distributionMode == DistributionMode.Random)
                PlantRandom(position, rotation);
            else
                PutInstance(position, rotation);
        }

        // Update the counter and the position history.
        intervalCounter = (intervalCounter + delta) % interval;
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }
}

} // namespace Reaktion
