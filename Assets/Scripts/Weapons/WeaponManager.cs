using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        get
        {
            if (!CurrentWeapon) return false;

            if (CurrentWeapon.TryGetComponent(out WeaponTrigger trigger))
            {
                return trigger.UseState;
            }
            else return false;
        }
        set
        {
            if (!CurrentWeapon) return;

            if (CurrentWeapon.TryGetComponent(out WeaponTrigger trigger))
            {
                trigger.UseState = value;
            }
        }
    }

    public bool ReloadState
    {
        get
        {
            if (!CurrentWeapon) return false;

            if (CurrentWeapon.TryGetComponent(out WeaponAmmo trigger))
            {
                return trigger.Reload;
            }
            else return false;
        }
        set
        {
            if (!CurrentWeapon) return;

            if (CurrentWeapon.TryGetComponent(out WeaponAmmo trigger))
            {
                trigger.Reload = value;
            }
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
