using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectOnDrag : HitTarget
{
    public bool rotate = false;

    void Start()
    {
    }

    override public void Move(TouchHitInfo hitInfo)
    {
        Vector3 delta = hitInfo.distance * (hitInfo.curDirection - hitInfo.lastDirection);
        transform.position += delta;

        Vector3 offset = transform.position - hitInfo.touchSource.position;
        offset = hitInfo.distance * offset.normalized;
        transform.position = hitInfo.touchSource.position + offset;

        if (rotate)
        {
            Quaternion rotation = Quaternion.LookRotation(hitInfo.curDirection) *
                                  Quaternion.Inverse(Quaternion.LookRotation(hitInfo.lastDirection));
            transform.rotation = rotation * transform.rotation;
        }
    }
}
