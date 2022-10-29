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

    bool reloading;

    public override event Action ReloadEvent;

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
        if (reloading) yield break;
        if (currentMagazine >= magazineSize) yield break;

        reloading = true;
        currentMagazine = 0;

        ReloadEvent?.Invoke();

        yield return new WaitForSeconds(reloadTime);

        reloading = false;
        currentMagazine = magazineSize;
    }

    public override bool TryFire()
    {
        if (reloading) return false;
        if (currentMagazine <= 0) return false;

        currentMagazine--;
        return true;
    }
}
