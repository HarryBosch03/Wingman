using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class WeaponEffect : MonoBehaviour
{
    [SerializeField] protected WeaponAmmo weaponAmmo;

    public abstract event System.Action ExecuteEvent;

    public abstract void Execute();
    public float LastExecuteTime { get; protected set; }

    protected virtual void Awake ()
    {
        ExecuteEvent += () => LastExecuteTime = Time.time;
    }
}
