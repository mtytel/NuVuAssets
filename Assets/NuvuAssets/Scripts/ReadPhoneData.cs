using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadPhoneData : HitTarget
{
    public bool useGps = false;
    public Animator[] animators;

    [Space(10)]
    [Header("GPS Output")]
    public float outputAltitude = 0.0f;
    public float outputLatitude = 0.0f;
    public float outputLongitude = 0.0f;

    [Space(10)]
    [Header("Gyro Output")]
    public Vector3 outputGyro;

    [Space(10)]
    [Header("Accelerometer Output")]
    public Vector3 outputAcceleration;
    public float outputAccelerationMagnitude;

    void Start()
    {
        if (useGps && SystemInfo.supportsLocationService)
            Input.location.Start();

        Input.gyro.enabled = true;
    }

    void Update()
    {
        UpdateInput();
        UpdateAnimators();
    }

    void UpdateInput()
    {
        if (useGps && Input.location.status == LocationServiceStatus.Running)
        {
            outputAltitude = Input.location.lastData.altitude;
            outputLatitude = Input.location.lastData.latitude;
            outputLongitude = Input.location.lastData.longitude;
        }

        if (SystemInfo.supportsGyroscope)
            outputGyro = Input.gyro.attitude.eulerAngles;

        if (SystemInfo.supportsAccelerometer)
        {
            outputAcceleration = Input.acceleration;
            outputAccelerationMagnitude = outputAcceleration.magnitude;
        }
    }

    void UpdateAnimators()
    {
        foreach (Animator animator in animators)
        {
            animator.SetFloat("Altitude", outputAltitude);
            animator.SetFloat("Latitude", outputLatitude);
            animator.SetFloat("Longitude", outputLongitude);
            animator.SetFloat("Gyro X", outputGyro.x);
            animator.SetFloat("Gyro Y", outputGyro.y);
            animator.SetFloat("Gyro Z", outputGyro.z);
            animator.SetFloat("Accelerometer Magnitude", outputAccelerationMagnitude);
        }
    }

    void OnDestroy()
    {
        Input.location.Stop();
    }
}
