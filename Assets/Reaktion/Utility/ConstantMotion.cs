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

public class ConstantMotion : MonoBehaviour
{
    public enum VectorOption {
        X, Y, Z, Arbitral, Random
    };

    [System.Serializable]
    public class MotionElement
    {
        public VectorOption vectorOption;
        public Vector3 arbitralVector;
        public float velocity;
        public float velocityRandomness;

        Vector3 randomVector;
        float randomScalar;

        public void Initialize()
        {
            randomVector = Random.onUnitSphere;
            randomScalar = Random.value;
        }

        public Vector3 Vector {
            get {
                switch (vectorOption)
                {
                    case VectorOption.X:
                        return Vector3.right;
                    case VectorOption.Y:
                        return Vector3.up;
                    case VectorOption.Z:
                        return Vector3.forward;
                    case VectorOption.Arbitral:
                        return arbitralVector;
                    default:
                        return randomVector;
                }
            }
        }

        public float Delta {
            get {
                var scale = (1.0f - velocityRandomness * randomScalar);
                return velocity * scale * Time.deltaTime;
            }
        }
    }

    public MotionElement position = new MotionElement{
        vectorOption = VectorOption.X,
        arbitralVector = Vector3.right,
        velocity = 1.0f
    };

    public MotionElement rotation = new MotionElement{
        vectorOption = VectorOption.Y,
        arbitralVector = Vector3.up,
        velocity = 30.0f
    };

    public bool useLocalCoordinate = true;

    void Awake()
    {
        position.Initialize();
        rotation.Initialize();
    }

    void Update()
    {
        var deltaP = position.Vector * position.Delta;
        var deltaR = Quaternion.AngleAxis(rotation.Delta, rotation.Vector);

        if (useLocalCoordinate)
        {
            transform.localPosition += deltaP;
            transform.localRotation = deltaR * transform.localRotation;
        }
        else
        {
            transform.position += deltaP;
            transform.rotation = deltaR * transform.rotation;
        }
    }
}

} // namespace Reaktion
