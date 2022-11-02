using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class GrappleAbility : AbilityBase
{
    [SerializeField] float maxRange;
    [SerializeField] LineRenderer lines;

    new Rigidbody rigidbody;

    Rigidbody hitRigidbody;
    Transform hitObject;
    Vector3 localHitPoint;

    public Vector3? HitPoint
    {
        get
        {
            if (hitObject)
            {
                return hitObject.TransformPoint(localHitPoint);
            }
            else return null;
        }
    }

    private void Awake()
    {
        rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var point = HitPoint;
        lines.enabled = point.HasValue;

        if (lines.enabled)
        {
            lines.SetPosition(0, transform.position);
            lines.SetPosition(1, point.Value);

            Vector3 vector = point.Value - rigidbody.position;
            float dist = vector.magnitude;
            Vector3 direction = dist != 0.0f ? vector / dist : Vector3.zero;

            if (dist > maxRange)
            {
                Vector3 force = Vector3.zero;

                float opposingForce = Mathf.Max(Vector3.Dot(-direction, rigidbody.velocity), 0.0f);
                force += direction * opposingForce;

                float tension = dist - maxRange;
                force += direction * tension / Time.deltaTime;

                if (hitRigidbody ? !hitRigidbody.isKinematic : false)
                {
                    DistributeForce(force, rigidbody, hitRigidbody);
                }
                else
                {
                    rigidbody.velocity += force;
                }
            }
        }
    }

    private static void DistributeForce(Vector3 force, Rigidbody a, Rigidbody b)
    {
        Vector3 aForceDir = force.normalized * Mathf.Sign(Vector3.Dot(force, b.position - a.position));
        Vector3 bForceDir = -aForceDir;

        float fMag = force.magnitude;

        float tMass = a.mass + b.mass;
        float aFMag = fMag * (b.mass / tMass);
        float bFMag = fMag * (a.mass / tMass);

        a.velocity += aForceDir * aFMag;
        b.velocity += bForceDir * bFMag;
    }

    public override bool UseState
    {
        get => base.UseState;
        set
        {
            if (value && !base.UseState)
            {
                ToggleGrapple();
            }

            base.UseState = value;
        }
    }

    private void ToggleGrapple()
    {
        if (HitPoint.HasValue)
        {
            hitObject = null;
        }
        else
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out var hit, maxRange))
            {
                hitObject = hit.transform;
                hitRigidbody = hit.rigidbody;
                localHitPoint = hitObject.InverseTransformPoint(hit.point);
            }
        }
    }
}
