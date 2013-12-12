using UnityEngine;
using System.Collections;

public class ReaktorToRotation : MonoBehaviour
{
	public Vector3 axis = Vector3.up;
	public float minAngle = -90.0f;
	public float maxAngle = 90.0f;
	Reaktor reaktor;
    Quaternion initial;

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
        initial = transform.localRotation;
    }

    void Update ()
    {
		var angle = Mathf.Lerp (minAngle, maxAngle, reaktor.Output);
		transform.localRotation = Quaternion.AngleAxis (angle, axis) * initial;
    }
}
