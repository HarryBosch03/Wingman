using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class AbilityManager : MonoBehaviour
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
        get => GetState<AbilityBase>((a) => a.UseState);
        set => GetState<AbilityBase>((a) => a.UseState = value);
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
        SetAbility(currentWeaponIndex);
    }

    private void SetAbility(int index)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(i == index);
        }
        currentWeaponIndex = index;
    }
}
