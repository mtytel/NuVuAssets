using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePrefabOnHit : HitTarget
{
    public Transform prefab;
    public int createOnlyOnHit = 0;

    int numHits = 0;

    void Start()
    {
    }

    bool ShouldCreate()
    {
        return createOnlyOnHit == 0 || numHits == createOnlyOnHit;
    }

    void Create()
    {
        Instantiate(prefab, transform.position, transform.rotation);
    }

    override public void Hit(TouchHitInfo hitInfo)
    {
        numHits++;

        if (prefab != null && ShouldCreate())
            Create();
    }
}
