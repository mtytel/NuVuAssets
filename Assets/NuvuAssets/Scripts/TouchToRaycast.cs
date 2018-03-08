using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHitInfo {
    public HitTarget[] hitTargets;
    public Transform player;
    public Transform touchSource;
    public Vector3 hitPoint;
    public Vector3 startDirection;
    public Vector3 lastDirection;
    public Vector3 curDirection;
    public float distance;
}

public class TouchToRaycast : MonoBehaviour
{
    public Transform playerTransform;
    Camera cameraComponent;

    Dictionary<int, TouchHitInfo> currentHits;
    List<TouchHitInfo> hitOrder;

    Ray GetCameraRay(Vector2 screenPosition)
    {
        return cameraComponent.ScreenPointToRay(screenPosition);
    }

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        Input.multiTouchEnabled = true;
        currentHits = new Dictionary<int, TouchHitInfo>();
        hitOrder = new List<TouchHitInfo>();

        if (playerTransform == null)
            playerTransform = transform;
    }

    void ReceiveTouchBegin(Vector2 position, int id)
    {
        Ray ray = GetCameraRay(position);
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
                info.touchSource = cameraComponent.transform;
                info.hitPoint = hit.point;
                info.startDirection = ray.direction;
                info.lastDirection = ray.direction;
                info.curDirection = ray.direction;
                info.distance = (hitCollider.transform.position - cameraComponent.transform.position).magnitude;
                currentHits[id] = info;
                hitOrder.Add(info);

                foreach (HitTarget target in targets)
                    target.Hit(info);
            }
        }
    }

    void ReceiveTouchEnd(Vector2 position, int id)
    {
        if (!currentHits.ContainsKey(id))
            return;

        HitTarget[] targets = currentHits[id].hitTargets;
        foreach (HitTarget target in targets)
        {
            if (target)
                target.Release(currentHits[id]);
        }

        hitOrder.Remove(currentHits[id]);
        currentHits.Remove(id);
    }

    void ReceiveTouchMove(Vector2 position, int id)
    {
        if (!currentHits.ContainsKey(id))
            return;

        Ray ray = GetCameraRay(position);
        TouchHitInfo info = currentHits[id];
        info.lastDirection = info.curDirection;
        info.curDirection = ray.direction;

        HitTarget[] targets = info.hitTargets;
        foreach (HitTarget target in targets)
        {
            if (target)
                target.Move(info);
        }

        foreach (TouchHitInfo otherHit in hitOrder)
        {
            if (otherHit != info && otherHit.hitTargets[0] == info.hitTargets[0])
            {
                foreach (HitTarget target in targets)
                {
                    if (target)
                        target.PinchMove(info, otherHit);
                }
            }
        }

        currentHits[id] = info;
    }

    void ReceiveTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
            ReceiveTouchBegin(touch.position, touch.fingerId);
        else if (touch.phase == TouchPhase.Ended)
            ReceiveTouchEnd(touch.position, touch.fingerId);
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            ReceiveTouchMove(touch.position, touch.fingerId);
    }

    void UpdateTouches()
    {
        for (int i = 0; i < Input.touchCount; ++i)
            ReceiveTouch(Input.touches[i]);
    }

    void UpdateMouse()
    {
        Vector2 position = new Vector2(cameraComponent.pixelWidth / 2.0f, cameraComponent.pixelHeight / 2.0f);
        if (Cursor.lockState != CursorLockMode.Locked)
            position = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
            ReceiveTouchBegin(position, 0);
        if (Input.GetMouseButtonUp(0))
            ReceiveTouchEnd(position, 0);
        if (Input.GetMouseButton(0))
            ReceiveTouchMove(position, 0);
    }

    void Update()
    {
        if (cameraComponent == null)
            return;

        if (Input.touchSupported)
            UpdateTouches();
        else
            UpdateMouse();
    }
}
