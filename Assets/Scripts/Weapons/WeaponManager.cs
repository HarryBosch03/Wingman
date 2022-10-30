using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;

    int currentWeaponIndex;

    public GameObject CurrentWeapon
    {
        get
        {
            if (currentWeaponIndex < 0) return null;
            if (currentWeaponIndex >= weapons.Length) return null;

            return weapons[currentWeaponIndex];
        }
    }

    public bool UseState
    {
        get => GetState<WeaponTrigger>((t) => t.UseState);
        set => GetState<WeaponTrigger>((t) => t.UseState = value);
    }

    public bool ReloadState
    {
        get => GetState<WeaponAmmo>((a) => a.Reload);
        set => GetState<WeaponAmmo>((a) => a.Reload = value);
    }

    public bool AimState
    {
        get => GetState<WeaponADS>((a) => a.ADSState);
        set => GetState<WeaponADS>((a) => a.ADSState = value);
    }

    public bool GetState<T>(System.Func<T, bool> method)
    {
        if (!CurrentWeapon) return false;

        if (CurrentWeapon.TryGetComponent(out T component))
        {
            return method(component);
        }
        else return false;
    }

    public void SetState<T>(System.Action<T> method)
    {
        if (!CurrentWeapon) return;

        if (CurrentWeapon.TryGetComponent(out T component))
        {
            method(component);
        }
    }

    private void OnEnable()
    {
        SetWeapon(currentWeaponIndex);
    }

    private void SetWeapon(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }
        currentWeaponIndex = index;
    }
}
