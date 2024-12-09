using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PhysicsProjectile : Projectile
{
    [SerializeField] private float lifeTime;
    private Rigidbody rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public override void Init(Weapon weapon)
    {
        base.Init(weapon); 
        Destroy(gameObject , lifeTime);

    }

    public override void Launch(Transform _transform)
    {
        base.Launch(_transform);
        rigidBody.velocity = _transform.forward * weapon.GetShootingForce();
    }
}
