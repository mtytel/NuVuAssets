using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadWebCam : MonoBehaviour
{
    public bool preferFrontFacing = false;

    WebCamTexture webCamTexture;

    void LoadCamera(string deviceName)
    {
        RawImage rawImage = GetComponent<RawImage>();
        webCamTexture = new WebCamTexture(deviceName, Screen.width, Screen.height, 60);
        webCamTexture.filterMode = FilterMode.Trilinear;
        rawImage.texture = webCamTexture;
        webCamTexture.Play();
    }

    void Start()
    {
        foreach (WebCamDevice cam in WebCamTexture.devices)
        {
            if (cam.isFrontFacing == preferFrontFacing)
            {
                LoadCamera(cam.name);
                return;
            }
        }

        foreach (WebCamDevice cam in WebCamTexture.devices)
        {
            LoadCamera(cam.name);
            return;
        }
    }

    void Update()
    {
        if (webCamTexture == null || webCamTexture.width < 100)
            return;

        RawImage image = GetComponent<RawImage>();
        Vector3 rotationVector = Vector3.zero;
        rotationVector.z = -webCamTexture.videoRotationAngle;
        GetComponent<RawImage>().rectTransform.localEulerAngles = rotationVector;

        float videoRatio = (float)webCamTexture.width / (float)webCamTexture.height;
        GetComponent<AspectRatioFitter>().aspectRatio = videoRatio;

        float x = 0.0f;
        float y = 0.0f;
        float width = 1.0f;
        float height = 1.0f;
        if (webCamTexture.videoVerticallyMirrored)
        {
            y = 1.0f;
            height = -1.0f;
        }

        if (preferFrontFacing)
        {
            x = 1.0f;
            width = -1.0f;
        }

        image.uvRect = new Rect(x, y, width, height);
    }
}
