using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : HitTarget
{
    public int destroyOnlyOnHit = 0;
    int numHits = 0;

    void Start()
    {
    }

    override public void Hit(TouchHitInfo hitInfo)
    {
        numHits++;
        if (destroyOnlyOnHit == 0 || numHits == destroyOnlyOnHit)
            Destroy(this.gameObject);
    }
}
