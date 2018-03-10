using UnityEngine;

public class MoveObjectOnHit : HitTarget
{
    public Transform objectToMove;

    override public void Hit(TouchHitInfo hitInfo)
    {
        MoveTo(hitInfo.hitPoint);
    }

    override public void Move(TouchHitInfo hitInfo)
    {
        Ray ray = new Ray(hitInfo.touchSource.position, hitInfo.curDirection);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            MoveTo(hit.point);
    }

    override public void Release(TouchHitInfo hitInfo)
    {
    }

    void MoveTo(Vector3 position) {
        if (objectToMove)
            objectToMove.position = position;
    }
}
