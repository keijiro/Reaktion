// Reaktor - Controller for Audio Reaction
using UnityEngine;
using System.Collections;

public class Reaktor : MonoBehaviour
{
    #region Public properties

    public int bandIndex = 1;
    public float dynamicRange = 20.0f;
    public float headroom = 6.0f;
    public float falldown = 0.4f;
    public float lowerBound = -60.0f;
    public float powerFactor = 1.0f;
    public float sensibility = 18.0f;
	public bool showOptions;

    public float Output {
        get { return output; }
    }

	public float Peak {
		get { return peak; }
	}

    #endregion

    #region Private variables
    
    float output;
    float peak;

    #endregion

    #region MonoBehaviour functions

    void Start ()
    {
        // Begins with the lowest level.
        peak = lowerBound + dynamicRange + headroom;
    }

    void Update ()
    {
        // Audio input.
        var input = bandIndex < 0 ? -1e6f : AudioJack.instance.BandLevels [bandIndex];

        // Check the peak level.
        peak -= Time.deltaTime * falldown;
        peak = Mathf.Max (peak, Mathf.Max (input, lowerBound + dynamicRange + headroom));

        // Normalize the input level.
        input = (input - peak + headroom + dynamicRange) / dynamicRange;
        input = Mathf.Pow (Mathf.Clamp01 (input), powerFactor);

        // Interpolation.
        output = input - (input - output) * Mathf.Exp (-sensibility * Time.deltaTime);
    }

    // Search an available Reaktor placed close to the game object.
    public static Reaktor SearchAvailableFrom (GameObject go)
    {
        // 1. Look for a component from the parent.
        // 2. From the grandparent.
        // 3. From the childlen.
        // 4. Give up.
        var r = go.GetComponent<Reaktor> ();
        if (r == null)
        {
            r = go.transform.parent.GetComponent<Reaktor> ();
            if (r == null)
            {
                r = go.transform.parent.GetComponent<Reaktor> ();
                if (r == null)
                {
                    r = go.GetComponentInChildren<Reaktor> ();
                }
            }
        }
        return r;
    }

    #endregion
}
