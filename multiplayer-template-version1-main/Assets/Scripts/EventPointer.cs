using System.Collections;
using System.Collections.Generic;

// Mapbox library
using UnityEngine;
using Mapbox.Examples; // this is for calculating the distance between player and POI
using Mapbox.Utils; // this is for calculating the distance between player and POI


public class EventPointer : MonoBehaviour
{
    //  For the document-wide formatting: Ctrl + K , Ctrl + D

    [SerializeField] float rotationSpeed = 50.0f;  // making it seialized allows the developer to access private variables through the inspector
    [SerializeField] float amplitude = 2.0f;
    [SerializeField] float frequency = 0.50f;

    // these two variables will be used for calculating the distance between the player and POI
    LocationStatus playerLocation;
    public Vector2d eventPos; // stores the Location of this POI, this is configured by SpawnOnMap component of EventSpawner

    MenuUIManager menuUIManager;

    EventManager eventManager; // gives us access to the EventManager for the game

    public int eventID;


    // Start is called before the first frame update
    void Start()
    {
        menuUIManager = GameObject.Find("Canvas").GetComponent<MenuUIManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FloatAndRotatePointer();
    }

    void FloatAndRotatePointer()
    {
        // the Math functions handle the rotation on the y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude) + 15, transform.position.z);
    }


    private void OnMouseDown()
    {
        // the LocationStatus script retrieves the player's/user's location
        playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();

        var playerLocationLat = playerLocation.GetLocationLat();
        var playerLocationLong = playerLocation.GetLocationLong();

        // retrieve location of spawned event pointers
        var eventLocationLat = eventPos[0];
        var eventLocationLong = eventPos[1];

        var distance = DistanceBetweenPlaces(playerLocationLat, playerLocationLong, eventLocationLat, eventLocationLong);
        
        var bearing = Bearing(playerLocationLat, playerLocationLong, eventLocationLat, eventLocationLong);
        Debug.Log("\n\n\n");
        Debug.Log("clicked distance:" + distance);
        Debug.Log("bearing :" + bearing);
        Debug.Log("player loc :" + playerLocationLat + ", " + playerLocationLong);
        Debug.Log("event loc :" + eventLocationLat + ", " + eventLocationLong);

        if (distance < eventManager.getMinimumDistanceToAccess())
        {
            menuUIManager.HideUserNotInRangePanel();
            menuUIManager.DisplayEventPanel(eventID); // pass the eventID assigned to this event
            
        }
        else {
            menuUIManager.DisplayUserNotInRangePanel();
            menuUIManager.HideStartEventPanel();
        }
        Debug.Log("\n\n\n");
        Debug.Log("clicked distance:" + distance);


    }



    // haversine formula, see code below, note there is an error in the C# version
    // https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula/21623206#21623206
    const double PIx = 3.141592653589793;
    const double RADIUS = 6378.16;  // this is km so the function returns km

    /*
     * if you want to check your math you can try these coordinates and compare your results against a distance calculator, 
     * 
     * example coordinate pair, 
     *   From: 37.2446764423826, -80.428481768204
     *   To: 37.244866, -80.408273
     *   
     *   
     *   The difference betweek these two coordinates is 1.79 kilometers 
     * 
     * Distance calculator
     * https://boulter.com/gps/distance/
     * 
     * 
     */

    /*
     *  Calculates and returns the distance between lat1, long1 to lat2, long2
     * 
     */
    public static double DistanceBetweenPlaces(
        double lat1,
        double long1,
        double lat2,
        double long2)
    {
        double dlong = ConvertDegToRad(long2 - long1);
        double dlat = ConvertDegToRad(lat2 - lat1);

        double a = (Mathd.Sin(dlat / 2) * Mathd.Sin(dlat / 2)) + Mathd.Cos(ConvertDegToRad(lat1))
            * Mathd.Cos(ConvertDegToRad(lat2)) * (Mathd.Sin(dlong / 2) * Mathd.Sin(dlong / 2));
        double angle = 2 * Mathd.Atan2(Mathd.Sqrt(a), Mathd.Sqrt(1 - a));
        
        return angle * RADIUS;
    }


    /*
     *  Calculates and returns the bearing from lat1, long1 to lat2, long2
     * 
     */
    public static double Bearing(double lat1, double long1, double lat2, double long2)
    {
        double diff = Mathd.Abs(Mathd.Abs(long1) - Mathd.Abs(long2));
        double x = Mathd.Cos(lat2) * Mathd.Sin(diff);
        double y = Mathd.Cos(lat1) * Mathd.Sin(lat2) - Mathd.Sin(lat1) * Mathd.Cos(lat2) * Mathd.Cos(diff);
        double result = Mathd.Atan2(x, y);

        return ConvertRadToDeg(result);
    }



    /*
     *  Convert Degrees to Radians
     * 
     */
    public static double ConvertDegToRad(double x)
    {
        return x * PIx / 180;
    }

    /*
     *  Convert Radians to Degrees
     * 
     */
    public static double ConvertRadToDeg(double x)
    {
        return 180.0 * x / PIx;
    }

    public double ConvertMilesToKilometers(double miles)
    {
        return miles * 1.609344;
    }

    public double ConvertKilometersToMiles(double kilometers)
    {
        return kilometers * 0.621371192;
    }


}
