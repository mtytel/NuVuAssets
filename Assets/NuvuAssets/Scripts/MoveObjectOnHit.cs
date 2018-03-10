using UnityEngine;

public class MoveObjectOnHit : HitTarget
{
    public Transform objectToMove;

    override public void Hit(TouchHitInfo hitInfo)
    {
        if (objectToMove)
            objectToMove.position = hitInfo.hitPoint;
    }

    override public void Release(TouchHitInfo hitInfo)
    {
    }
}
