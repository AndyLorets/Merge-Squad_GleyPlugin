using D2D;
using D2D.Gameplay;
using UnityEngine;

public class Shield : Unit, IHittable
{
    private Rigidbody[] rb;

    private Health health;

    private void Awake()
    {
        rb = ChildrenGets<Rigidbody>();
        health = Get<Health>();

        transform.parent.GetComponentInParent<Health>().Died += DestroyShield;
        Get<Health>().Died += DestroyShield;
    }

    public void GetHit(float damage)
    {
        health.ApplyDamage(null, damage);
    }

    private void DestroyShield()
    {
        foreach (var rigid in rb)
        {
            rigid.isKinematic = false;
        }

        Get<Collider>().enabled = false;
    }
}