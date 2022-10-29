using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class WeaponTrigger : MonoBehaviour
{
    [SerializeField] protected WeaponEffect weaponEffect;

    public abstract event System.Action UseEvent;

    public virtual bool UseState { get; set; }
}
