using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaceOnMap : MonoBehaviour
{
    public GoogleMap map;
    public float latitude;
    public float longitude;

    void Start()
    {
        if (map)
            map.PlaceObjectOnMap(transform, latitude, longitude);
    }
}
