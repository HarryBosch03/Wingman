using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponProjectileEffect : WeaponEffect
{
    [SerializeField] GameObject projectile;
    [SerializeField] Transform muzzle;
    [SerializeField] int projectileCount;
    [SerializeField] float spray;

    public override void Execute()
    {
        if (weaponAmmo.TryFire())
        {
            for (int i = 0; i < projectileCount; i++)
            {
                Quaternion rotation = Quaternion.Euler(Random.insideUnitSphere * spray);
                Instantiate(projectile, muzzle.position, muzzle.rotation * rotation);
            }
        }
    }
}
