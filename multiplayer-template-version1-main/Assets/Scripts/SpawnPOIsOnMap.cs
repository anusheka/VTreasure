namespace Mapbox.Examples
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.MeshGeneration.Factories;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;


    using System.Text.RegularExpressions; // for the regular expression split

    public class SpawnPOIsOnMap : MonoBehaviour
    {
        /**
         * 
         * SpawnOnMap provides an outline of placing custom markers. 
         * It uses the Start() function to calculate initial marker positions, 
         * then uses Update() to update those positions every time the map changes 
         * (either from zooming or panning).
         * See https://docs.mapbox.com/unity/maps/guides/add-markers/
         * 
         * */

        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        [Geocode]
        string[] _locationStrings;
        Vector2d[] _locations;

        [SerializeField]
        float _spawnScale = 100f;

        [SerializeField]
        GameObject _markerPrefab;

        List<GameObject> _spawnedObjects;

        GeoCoordinate[] POIList;

        void Start()
        {
             ReadFile();

            _spawnedObjects = new List<GameObject>();

            for (int i = 0; i < POIList.Length; i++)
            {

                var instance = Instantiate(_markerPrefab); // this instantiates a new item for spawning

                // added this line of code below to set the eventPose of each instance of a spawned object like an eventPointer
                instance.GetComponent<EventPointer>().eventPos = POIList[i].Location;  // using the Location Property of GEOCordinate

                // added this line of code below to set the eventID of each instance of a spawned object like an eventPointer
                // the eventID will simply be the index within the List plus 1, i + 1.
                // our events will therefore be numbered, 1, 2, etc.
                instance.GetComponent<EventPointer>().eventID = i + 1;

                instance.transform.localPosition = _map.GeoToWorldPosition(POIList[i].Location, true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                _spawnedObjects.Add(instance);
            }
        }

        private void Update()
        {
            int count = _spawnedObjects.Count;
            for (int i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                // var location = _locations[i];
                var location = POIList[i].Location;
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            }
        }

        private void ReadFile()
        {
            List<GeoCoordinate> tempPOIList = new List<GeoCoordinate>();

            // Read each line of the file into a string array. Each element
            // of the array is one line of the file.
            string[] lines = System.IO.File.ReadAllLines(@"Assets/Resources/PointsOfInterest.txt");

            // Display the file contents by using a foreach loop.
     
       
            foreach (string line in lines)
            {
                string pattern = @",\s*";
                string[] data = Regex.Split(line, pattern);
                string name = data[0];
            
                string latitude = data[1];
                string longitude = data[2];
                string status = "";
                tempPOIList.Add(new GeoCoordinate(name, Conversions.StringToLatLon(latitude +", "+longitude), status) ); // creates a geocoord object and adds to list
            }

            POIList = tempPOIList.ToArray();
        }




    }
}