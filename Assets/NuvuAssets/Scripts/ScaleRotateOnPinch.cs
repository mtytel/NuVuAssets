using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRotateOnPinch : HitTarget
{
    public bool scale = true;
    public bool rotate = false;

    void Start()
    {
    }

    override public void PinchMove(TouchHitInfo hitInfoMoved, TouchHitInfo hitInfoStill)
    {
        float distance = Mathf.Max(hitInfoMoved.distance, hitInfoStill.distance);
        Vector3 lastDelta = distance * (hitInfoMoved.lastDirection - hitInfoStill.curDirection);
        Vector3 curDelta = distance * (hitInfoMoved.curDirection - hitInfoStill.curDirection);

        if (scale)
            transform.localScale = (curDelta.magnitude / lastDelta.magnitude) * transform.localScale;
        if (rotate)
            transform.rotation = Quaternion.FromToRotation(lastDelta, curDelta) * transform.rotation;
    }
}
