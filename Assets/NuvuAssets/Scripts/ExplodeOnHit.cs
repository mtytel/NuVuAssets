using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnHit : HitTarget
{
    public int explodeOnlyOnHit = 0;
    public bool destroyAfterExplode = false;
    public float explosionForce = 1.0f;
    public float explosionRadius = 1.0f;
    public float upwardsModifier = 0.0f;

    int numHits = 0;
    const float timeToWaitForDestroy = 10.0f;

    void Start()
    {
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    void Die()
    {
        transform.localScale = Vector3.zero;
        Invoke("DestroySelf", timeToWaitForDestroy);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody body = collider.GetComponent<Rigidbody>();
            if (body)
                body.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
        }

        if (destroyAfterExplode)
            Die();
    }

    override public void Hit(TouchHitInfo hitInfo)
    {
        numHits++;
        if (explodeOnlyOnHit == 0 || numHits == explodeOnlyOnHit)
            Explode();
    }
}
