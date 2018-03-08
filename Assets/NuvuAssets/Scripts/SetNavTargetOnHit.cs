using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class SetNavTargetOnHit : HitTarget
{
    public NavMeshAgent navAgent;

    override public void Hit(TouchHitInfo hitInfo)
    {
        if (navAgent)
            navAgent.SetDestination(hitInfo.hitPoint);
    }

    override public void Release(TouchHitInfo hitInfo)
    {
    }
}
