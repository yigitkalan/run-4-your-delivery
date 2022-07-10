using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailHandler : MonoBehaviour
{
    private DriverControl dc;
    private TrailRenderer tr;
    // Start is called before the first frame update
    void Start()
    {
        dc = GetComponentInParent<DriverControl>();
        tr = GetComponent<TrailRenderer>();
        tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(dc.isBreaking())
            tr.emitting = true;
        else
            tr.emitting = false;

    }
}
