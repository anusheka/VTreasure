using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // this allows us to load the scene in ActivateEvent
using System;
using Game.Events;
using GameFramework.Core.Data;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Mapbox.Unity.Utilities;
// Mapbox library
using Mapbox.Examples; // this is for calculating the distance between player and POI
using Mapbox.Examples.Scripts; // this is for calculating the distance between player and POI
using Mapbox.Utils; // this is for calculating the distance between player and POI


public class EventManager : MonoBehaviour
{
    double minimumDistanceToAccess = 100000000000;
    [SerializeField] GameObject HintsFolder;
    [SerializeField] Button back;
    private int eventID;

    // Start is called before the first frame update
    void Start()
    {
        back.onClick.AddListener(OnBackButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setEventID(int e) {
        eventID = e;
    }

    public int getEventID() {
        return eventID;
    }

    public GameObject getHintsFolder()
    {
        return HintsFolder;
    }

    public void setPageActive(bool b) {
        HintsFolder.SetActive(b);
    }

    public void OnBackButtonClicked() {
        setPageActive(false);
    }

    // Activates event and launches scene if the event is within range and the user selects "join"
    public void ActivateEvent(int eventID)
    {
        if(eventID == 1)
        {
        //    SceneManager.LoadScene("SceneForEvent1");
            SceneManager.LoadScene("MossArts");

        }
    }


    public double getMinimumDistanceToAccess() {
        return minimumDistanceToAccess;
    }

}
