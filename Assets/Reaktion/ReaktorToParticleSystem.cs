using UnityEngine;
using System.Collections;

public class ReaktorToParticleSystem : MonoBehaviour
{
    #region Public properties

    // Burst options.
    public bool enableBurst;
    public float burstThreshold = 0.9f;
    public int burstEmissionNumber = 10;

    // Emission rate options.
    public bool enableEmission;
    public float maxEmissionRate = 30.0f;
    public AnimationCurve emissionRateCurve;

    #endregion

    #region Private variables

    Reaktor reaktor;
    float previousOutput;

    #endregion

    void Start ()
    {
        reaktor = Reaktor.SearchAvailableFrom (gameObject);
    }

    void Update ()
    {
        if (enableBurst)
        {
            if (previousOutput < burstThreshold && reaktor.Output >= burstThreshold)
            {
                particleSystem.Emit (burstEmissionNumber);
                particleSystem.Play ();
            }
            previousOutput = reaktor.Output;
        }

        if (enableEmission)
        {
            particleSystem.emissionRate = maxEmissionRate * emissionRateCurve.Evaluate(reaktor.Output);
        }
    }
}
