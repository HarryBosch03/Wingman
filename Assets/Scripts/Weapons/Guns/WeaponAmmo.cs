using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class WeaponAmmo : MonoBehaviour
{
    public virtual bool Reload { get; set; }

    public abstract bool TryFire();
}