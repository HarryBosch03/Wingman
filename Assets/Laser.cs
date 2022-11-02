using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
[DefaultExecutionOrder(100)]
public sealed class Laser : MonoBehaviour
{
    [SerializeField] Transform reference;
    [SerializeField] Vector3 direction;
    [SerializeField] float range;
    [SerializeField] ParticleSystem laserEnd;

    [Space]
    [SerializeField] FPCameraController cameraController;

    Vector3? point;

    private void FixedUpdate()
    {
        Ray ray;
        if (reference) ray = new Ray(reference.position, transform.rotation * direction);
        else ray = new Ray(transform.position, transform.rotation * direction);

        if (Physics.Raycast(ray, out var hit, range))
        {
            point = hit.point;
        }
        else
        {
            point = null;
        }
    }

    private void Update()
    {
        var endEmmision = laserEnd.emission;

        if (point.HasValue)
        {
            if (laserEnd)
            {
                endEmmision.rateOverTime = 1000.0f;
                laserEnd.transform.position = point.Value;
            }
        }
        else
        {
            if (laserEnd)
            {
                endEmmision.rateOverTime = 0.0f;
            }
        }

        laserEnd.Simulate(Time.deltaTime, true, false, false);
    }
}
