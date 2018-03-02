using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportOnHit : HitTarget
{
    void Start()
    {
    }

    override public void Hit(TouchHitInfo hitInfo)
    {
        hitInfo.player.position = transform.position;
    }
}
