using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class SingleFireTrigger : WeaponTrigger
{
    [Space]
    [SerializeField] float delay;

    float lastFireTime;

    public override bool UseState
    {
        get => base.UseState;
        set
        {
            base.UseState = value;
            if (value && Time.time > lastFireTime + delay)
            {
                weaponEffect.Execute();
                lastFireTime = Time.time;
            }
        }
    }
}
