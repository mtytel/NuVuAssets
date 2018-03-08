using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyAnimatorOnHit : HitTarget
{
    public Animator animator;
    int numHits = 0;
    int numHolding = 0;

    void Start()
    {
    }

    override public void Hit(TouchHitInfo hitInfo)
    {
        numHits++;
        numHolding++;
        animator.SetTrigger("Hit");
        animator.SetInteger("NumHits", numHits);
        animator.SetBool("Holding", true);
    }

    override public void Release(TouchHitInfo hitInfo)
    {
        numHolding--;
        animator.SetTrigger("Release");
        animator.SetBool("Holding", numHolding != 0);
    }
}
