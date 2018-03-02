// https://developers.google.com/maps/documentation/staticmaps/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoogleMap : MonoBehaviour
{
    public enum MapType
    {
        RoadMap,
        Satellite,
        Terrain,
        Hybrid
    }

    [System.Serializable]
    public class MapLocation
    {
        public string address;
        public float latitude;
        public float longitude;
    }

    [System.Serializable]
    public class MapFeatureStyle {
        public Color geometryFill = Color.white;
        public Color geometryStroke = Color.grey;
        public bool showIcon = false;
        public bool showText = false;
        public Color labelFill = Color.white;
        public Color labelStroke = Color.black;
    }

    [System.Serializable]
    public class MapStyle
    {
        public MapType mapType;
        public bool useDefaultStyle;
        public MapFeatureStyle roadStyle;
        public MapFeatureStyle administrativeStyle;
        public MapFeatureStyle landscapeStyle;
        public MapFeatureStyle poiStyle;
        public MapFeatureStyle waterStyle;
        public MapFeatureStyle transitStyle;
    }

    struct MapQuadCoordinate
    {
        public int latitudeIndex;
        public int longitudeIndex;

        public MapQuadCoordinate(int latIndex, int longIndex)
        {
            latitudeIndex = latIndex;
            longitudeIndex = longIndex;
        }
    }

    const float latitudeZeroZoom = 666.0f;
    const float longitudeZeroZoom = 900.0f;
    const float editorRefreshWait = 5.0f;

    public string googleApiKey;
    public bool useLocation = true;
    public MapLocation centerLocation;
    public int zoom = 17;
    public MapStyle mapStyle;
    public GoogleMapQuad quadModel;
    public int mapQuadRadius = 2;
    public Camera cameraComponent;

    float lastUpdate = -editorRefreshWait;
    float latitudeQuadWidth;
    float longitudeQuadWidth;
    Dictionary<MapQuadCoordinate, GoogleMapQuad> quadLookup;
    Transform mapContainer;

    void Awake()
    {
        latitudeQuadWidth = latitudeZeroZoom / Mathf.Pow(2.0f, zoom);
        longitudeQuadWidth = longitudeZeroZoom / Mathf.Pow(2.0f, zoom);
        quadLookup = new Dictionary<MapQuadCoordinate, GoogleMapQuad>();

        if (SystemInfo.supportsLocationService)
            Input.location.Start();

        lastUpdate = Time.time;

        mapContainer = new GameObject("MapContainer").transform;
        mapContainer.parent = transform;
        mapContainer.localScale = Vector3.one;

        RefreshSections();
    }

    void Update()
    {
        if (useLocation && Input.location.status == LocationServiceStatus.Running)
        {
            centerLocation.address = "";
            centerLocation.latitude = Input.location.lastData.latitude;
            centerLocation.longitude = Input.location.lastData.longitude;
        }

        UpdateLocation();

        if (Application.isEditor && Time.time - editorRefreshWait > lastUpdate)
        {
            lastUpdate = Time.time;
            RefreshSections();
        }
    }

    void UpdateLocation()
    {
        float latitudeIndex = centerLocation.latitude / latitudeQuadWidth;
        float longitudeIndex = centerLocation.longitude / longitudeQuadWidth;

        for (int i = -mapQuadRadius; i <= mapQuadRadius; ++i)
        {
            for (int j = -mapQuadRadius; j <= mapQuadRadius; ++j )
                MakeSureQuadExists(((int)latitudeIndex) + i, ((int)longitudeIndex) + j);
        }

        mapContainer.localPosition = new Vector3(-longitudeIndex, 0, -latitudeIndex);
    }

    void MakeSureQuadExists(int latitudeIndex, int longitudeIndex)
    {
        MapQuadCoordinate coordinates = new MapQuadCoordinate(latitudeIndex, longitudeIndex);
        if (quadLookup.ContainsKey(coordinates))
            return;

        GoogleMapQuad newQuad = Instantiate(quadModel, mapContainer) as GoogleMapQuad;
        newQuad.transform.localPosition = new Vector3(longitudeIndex, 0, latitudeIndex);
        newQuad.googleApiKey = googleApiKey;
        newQuad.centerLocation.latitude = latitudeIndex * latitudeQuadWidth;
        newQuad.centerLocation.longitude = longitudeIndex * longitudeQuadWidth;
        newQuad.zoom = zoom;
        newQuad.mapStyle = mapStyle;
        newQuad.LoadMap();
        quadLookup[coordinates] = newQuad;
    }

    void RefreshSections()
    {

    }

    public void PlaceObjectOnMap(Transform mapObject, float latitude, float longitude)
    {
        float latitudeIndex = latitude / latitudeQuadWidth;
        float longitudeIndex = longitude / longitudeQuadWidth;
        mapObject.parent = mapContainer;
        mapObject.localPosition = new Vector3(longitudeIndex, mapObject.localPosition.y, latitudeIndex);
    }
}
