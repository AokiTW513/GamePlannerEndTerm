using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongLight : MonoBehaviour
{
    Light L;
    public float increment = 0.2f;
    public float minIntensity = 2, maxIntensity = 12;
    void Start()
    {
        L = GetComponent<Light>();
    }

    void Update()
    {
        if (L.intensity + increment > maxIntensity)
        {
            L.intensity = maxIntensity;
            increment *= -1;
        }
        else if (L.intensity + increment < minIntensity)
        {
            L.intensity = minIntensity;
            increment *= -1;
        }
        else
            L.intensity += increment;
    }

}