using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeHandler : MonoBehaviour
{
    private DriverControl dc;
    private ParticleSystem particles;
    private ParticleSystem.EmissionModule particleEmission;

    // Start is called before the first frame update
    void Start()
    {
        dc = GetComponentInParent<DriverControl>();
        particles = GetComponent<ParticleSystem>();
        particleEmission = particles.emission;

    }

    // Update is called once per frame
    void Update()
    {
        if(dc.GetAccInput() > 0.001 || dc.isBreaking()){
            particleEmission.rateOverTime = 10;
        }
        else
            particleEmission.rateOverTime =0;

    }
}
