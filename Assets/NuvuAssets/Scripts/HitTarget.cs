using UnityEngine;

public class HitTarget : MonoBehaviour
{
    void Start()
    {
    }

    virtual public void Hit(TouchHitInfo hitInfo)
    {
    }

    virtual public void Release(TouchHitInfo hitInfo)
    {
    }

    virtual public void Move(TouchHitInfo hitInfo)
    {
    }

    virtual public void PinchMove(TouchHitInfo hitInfoMoved, TouchHitInfo hitInfoStill)
    {
    }
}
