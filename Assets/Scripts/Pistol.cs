using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Pistol : Weapon
{
    [SerializeField] private Projectile bulletPrefab;
    protected override void StartShooting(ActivateEventArgs args)
    {
        base.StartShooting(args);
        Shoot();
    }

    protected override void Shoot()
    {
        base.Shoot();
        Projectile projectileInstance = Instantiate(bulletPrefab , bulletSpawn.position , bulletPrefab.transform.rotation);
        projectileInstance.Init(this);
        projectileInstance.Launch(transform);
    }

    protected override void StopShooting(DeactivateEventArgs args)
    {
        base.StopShooting(args);

    }

   

}
