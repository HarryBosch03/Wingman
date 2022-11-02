using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public abstract class WeaponEffect : MonoBehaviour
{
    [SerializeField] protected WeaponAmmo weaponAmmo;

    public abstract event System.Action PreExecuteEvent;
    public abstract event System.Action PostExecuteEvent;

    public abstract void Execute();
    public float LastExecuteTime { get; protected set; }

    protected virtual void Awake ()
    {
        PostExecuteEvent += () => LastExecuteTime = Time.time;
    }
}
