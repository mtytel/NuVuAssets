using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerToRaycast : MonoBehaviour
{
    public Transform triggerSource;
    public Transform playerTransform;

    TouchHitInfo currentHit;

    void Start()
    {
        if (triggerSource == null)
            triggerSource = transform;
        if (playerTransform == null)
            playerTransform = transform;
    }

    void Trigger()
    {
        Ray ray = new Ray(triggerSource.position, triggerSource.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Collider hitCollider = hit.collider;
            Rigidbody hitBody = hit.rigidbody;

            if (hitCollider)
            {
                HitTarget[] targets = hitCollider.GetComponents<HitTarget>();
                TouchHitInfo info = new TouchHitInfo();
                info.hitTargets = targets;
                info.player = playerTransform;
                info.touchSource = triggerSource;
                info.startDirection = ray.direction;
                info.lastDirection = ray.direction;
                info.curDirection = ray.direction;
                info.distance = hit.distance;

                foreach (HitTarget target in targets)
                    target.Hit(info);
                currentHit = info;
            }
        }
    }

    void Release()
    {
        foreach (HitTarget hit in currentHit.hitTargets)
            hit.Release(currentHit);

        currentHit = null;
    }
}
