using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectOnDrag : HitTarget
{
    public Transform toRotate;
    public float multiplier = 1.0f;
    public bool xAxis = true;
    public bool yAxis = true;
    public bool zAxis = true;
    public float smoothing = 0.9f;

    Vector3 lastRotation;

    void Start()
    {
    }

    override public void Move(TouchHitInfo hitInfo)
    {
        Quaternion rotation = Quaternion.LookRotation(hitInfo.curDirection) *
                              Quaternion.Inverse(Quaternion.LookRotation(hitInfo.lastDirection));
        Vector3 euler = rotation.eulerAngles;

        if (!xAxis)
            euler.x = 0.0f;
        if (!yAxis)
            euler.y = 0.0f;
        if (!zAxis)
            euler.z = 0.0f;
        toRotate.rotation = Quaternion.Euler(euler) * transform.rotation;
    }
}
