using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class WeaponEffect : MonoBehaviour
{
    [SerializeField] protected WeaponAmmo weaponAmmo;

    public abstract void Execute();
}
