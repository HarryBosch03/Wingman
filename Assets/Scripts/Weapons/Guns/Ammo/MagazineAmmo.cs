using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class MagazineAmmo : WeaponAmmo
{
    [SerializeField] int magazineSize;
    [SerializeField] int currentMagazine;
    [SerializeField] float reloadTime;

    public override event Action ReloadEvent;

    public int CurrentMagazine => currentMagazine;

    public override bool Reload
    {
        get => base.Reload;
        set
        {
            base.Reload = value;
            if (value) StartCoroutine(ReloadRoutine());
        }
    }

    private IEnumerator ReloadRoutine()
    {
        if (Reloading) yield break;
        if (currentMagazine >= magazineSize) yield break;

        Reloading = true;
        currentMagazine = 0;

        ReloadEvent?.Invoke();

        yield return new WaitForSeconds(reloadTime);

        Reloading = false;
        currentMagazine = magazineSize;
    }

    public override bool TryFire()
    {
        if (Reloading) return false;
        if (currentMagazine <= 0) return false;

        currentMagazine--;
        return true;
    }
}
