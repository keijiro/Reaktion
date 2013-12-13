using UnityEngine;
using System.Collections;

public class ReaktorToTransform : MonoBehaviour
{
    #region Public properties

    //
    public bool enablePosition;
    public Vector3 position = Vector3.up;

    //
    public bool enableRotation;
    public Vector3 rotationAxis = Vector3.up;
    public float minAngle = -90.0f;
    public float maxAngle = 90.0f;

    //
    public bool enableScale;
    public Vector3 scale = Vector3.one;

    #endregion

    #region Private variables

    Reaktor reaktor;
    Vector3 initialPosition;
    Quaternion initialRotation;
    Vector3 initialScale;

    #endregion

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        initialScale = transform.localScale;
    }

    void Update ()
    {
        if (enablePosition)
            transform.localPosition = initialPosition + position * reaktor.Output;

        if (enableRotation)
        {
            var angle = Mathf.Lerp (minAngle, maxAngle, reaktor.Output);
            transform.localRotation = Quaternion.AngleAxis (angle, rotationAxis) * initialRotation;
        }

        if (enableScale)
            transform.localScale = initialScale + scale * reaktor.Output;
    }
}
