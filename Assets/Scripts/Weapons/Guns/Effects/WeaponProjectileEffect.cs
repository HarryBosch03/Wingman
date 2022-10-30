using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[DisallowMultipleComponent]
public sealed class WeaponProjectileEffect : WeaponEffect
{
    [SerializeField] Projectile projectile;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject shooter;
    [SerializeField] int projectileCount;
    [SerializeField] float spray;

    public override event System.Action ExecuteEvent;

    public override void Execute()
    {
        if (weaponAmmo.TryFire())
        {
            ExecuteEvent?.Invoke();

            for (int i = 0; i < projectileCount; i++)
            {
                Quaternion rotation = Quaternion.Euler(Random.insideUnitSphere * spray);
                Projectile instance = Instantiate(projectile, muzzle.position, muzzle.rotation * rotation);
                instance.Shooter = shooter;
            }
        }
    }
}
