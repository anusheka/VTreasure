using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Events;
using GameFramework.Core.Data;
using System.Text.RegularExpressions;
using Mapbox.Unity.Utilities;

public class DisplayHint3 : MonoBehaviour
{
    // Start is called before the first frame update
    int currentHuntID = EventPointer.currentEventID;
        
        [SerializeField] public TextMeshProUGUI _hint3Text;
        [SerializeField] public Button _button3;
        //[SerializeField] public TextMeshProUGUI _hint3Text;

        public static GeoCoordinate[] POIList;
        
        //GeoCoordinate[] POIList = SpawnPOIsOnMap.POIList;
        

        // Start is called before the first frame update
        void Start()
        {
            ReadFile();
            _button3.onClick.AddListener(RevealHint3);
            
            
            //_hint1Text.text = POIList[currentHuntID-1].Hint1;
            //_hint2Text.text = POIList[currentHuntID-1].Hint2;
            //_hint3Text.text = POIList[currentHuntID-1].Hint3;
            //currEventID = EventPointer.getEventID;
            Debug.Log("eventID: " + currentHuntID.ToString());
            Debug.Log("assignedHint1: " + POIList[currentHuntID].Hint1);
            //hint1 = POIList[currentHuntID-1].hint1;
            //hint2 = POIList[currentHuntID-1].hint2;
            //hint3 = POIList[currentHuntID-1].hint3;
            
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

                string hint1 = data[3];
                Debug.Log("hint1: " + hint1);
                string hint2 = data[4];
                string hint3 = data[5];
                string status = "";
                tempPOIList.Add(new GeoCoordinate(name, Conversions.StringToLatLon(latitude +", "+longitude), status, hint1, hint2, hint3) ); // creates a geocoord object and adds to list
            }

            POIList = tempPOIList.ToArray();
        }
        // Update is called once per frame
        void Update()
        {
            
        }
        public void RevealHint3() 
        {
            //EventManager.ActivateEvent();
            _hint3Text.text = POIList[currentHuntID-1].Hint3;
        }
}
