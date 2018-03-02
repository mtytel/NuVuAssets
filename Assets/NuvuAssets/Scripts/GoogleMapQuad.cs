// https://developers.google.com/maps/documentation/staticmaps/

using UnityEngine;
using System.Collections;

public class GoogleMapQuad : MonoBehaviour {
    const string url = "http://maps.googleapis.com/maps/api/staticmap";
    const int size = 640;

    public string googleApiKey;
    public GoogleMap.MapLocation centerLocation;
    public int zoom = 17;
    public GoogleMap.MapStyle mapStyle;

    void Start()
    {
        GetComponent<Renderer>().material.mainTexture = new Texture2D(size, size, TextureFormat.DXT1, false);
    }

    public void LoadMap()
    {
        StartCoroutine(RefreshMap());
    }

    string GetColorParameters(GoogleMap.MapFeatureStyle style, string feature)
    {
        string parameters = "";
        parameters += "&style=feature:" + feature;
        parameters += "|element:geometry.fill|color:0x" + ColorUtility.ToHtmlStringRGB(style.geometryFill);
        parameters += "&style=feature:" + feature;
        parameters += "|element:geometry.stroke|color:0x" + ColorUtility.ToHtmlStringRGB(style.geometryStroke);
        parameters += "&style=feature:" + feature;
        parameters += "|element:labels.icon|visibility:" + (style.showIcon ? "on" : "off");
        parameters += "&style=feature:" + feature;
        parameters += "|element:labels.text.fill|color:0x" + ColorUtility.ToHtmlStringRGB(style.labelFill);
        parameters += "|visibility:" + (style.showText ? "on" : "off");
        parameters += "&style=feature:" + feature;
        parameters += "|element:labels.text.stroke|color:0x" + ColorUtility.ToHtmlStringRGB(style.labelStroke);
        parameters += "|visibility:" + (style.showText ? "on" : "off");

        return parameters;
    }

    string GetParameters()
    {
        string parameters = "";
        if (centerLocation.address != "")
            parameters += "center=" + WWW.UnEscapeURL(centerLocation.address);
        else
        {
            string location = string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude);
            parameters += "center=" + WWW.UnEscapeURL(location);
        }

        parameters += "&zoom=" + zoom.ToString();
        parameters += "&size=" + WWW.UnEscapeURL(string.Format("{0}x{0}", size));
        parameters += "&scale=2";
        parameters += "&maptype=" + mapStyle.mapType.ToString().ToLower();

        if (!mapStyle.useDefaultStyle)
        {
            parameters += GetColorParameters(mapStyle.roadStyle, "road");
            parameters += GetColorParameters(mapStyle.administrativeStyle, "administrative");
            parameters += GetColorParameters(mapStyle.landscapeStyle, "landscape");
            parameters += GetColorParameters(mapStyle.poiStyle, "poi");
            parameters += GetColorParameters(mapStyle.waterStyle, "water");
            parameters += GetColorParameters(mapStyle.transitStyle, "transit");
        }

        parameters += "&key=" + WWW.UnEscapeURL(googleApiKey);

        return parameters;
    }

    IEnumerator RefreshMap()
    {
        string parameters = GetParameters();
        WWW request = new WWW(url + "?" + parameters);

        while (!request.isDone)
            yield return null;

        if (string.IsNullOrEmpty(request.error))
            request.LoadImageIntoTexture((Texture2D)GetComponent<Renderer>().material.mainTexture);
        else
        {
            Debug.LogWarning(url + "?" + parameters); 
            Debug.LogWarning(request.error);
        }
    }
}
