using UnityEngine;
using System.Collections;

public class ReaktorToScale : MonoBehaviour
{
    public Vector3 vector = Vector3.one;
    Reaktor reaktor;
    Vector3 initial;

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
        initial = transform.localScale;
    }

    void Update ()
    {
        transform.localScale = initial + vector * reaktor.Output;
    }
}
