using UnityEngine;
using System.Collections;

public class ReaktorToPosition : MonoBehaviour
{
    public Vector3 vector = Vector3.up;
    Reaktor reaktor;
    Vector3 initial;

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
        initial = transform.localPosition;
    }

    void Update ()
    {
        transform.localPosition = initial + vector * reaktor.Output;
    }
}
