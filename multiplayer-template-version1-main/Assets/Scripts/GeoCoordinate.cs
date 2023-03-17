using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils; // this is for using Vector2d to store the locations

public class GeoCoordinate
{
    // string Name;
    // Vector2d Location;
    // string Status;

    public GeoCoordinate(string name, Vector2d location, string status)
    {
        Name = name;
        Location = location;
        Status = status;
    }

    public string Name { get; set; }
    public Vector2d Location { get; set; }
    public string Status { get; set; }
    public double getLatitude() { return Location[0]; }
    public double getLongitude() { return Location[1]; }


}
