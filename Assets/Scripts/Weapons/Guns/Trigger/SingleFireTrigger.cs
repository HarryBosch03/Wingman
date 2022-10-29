using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class SingleFireTrigger : WeaponTrigger
{
    [Space]
    [SerializeField] float firerate;

    float lastFireTime;

    public override event Action UseEvent;

    public override bool UseState
    {
        get => base.UseState;
        set
        {
            base.UseState = value;
            if (value && Time.time > lastFireTime + 60.0f / firerate)
            {
                UseEvent?.Invoke();
                weaponEffect.Execute();
                lastFireTime = Time.time;
            }
        }
    }
}
