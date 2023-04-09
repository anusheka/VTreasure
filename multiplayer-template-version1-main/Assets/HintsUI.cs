using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game.Events;
using GameFramework.Core.Data;
using System.Text.RegularExpressions;
using Mapbox.Unity.Utilities;


namespace Game
{
    public class HintsUI : MonoBehaviour
    {
        
        int currentHuntID = EventPointer.currentEventID;
        [SerializeField] public Button _backButton;
        [SerializeField] public Button _openCamera;
        [SerializeField] public GameObject _cameraFolder;
        [SerializeField] public GameObject _hintsFolder;

        private lobbyUI lobby;

        // Start is called before the first frame update
        void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _openCamera.onClick.AddListener(OpenCamera);
            
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void OpenCamera() {
            _cameraFolder.SetActive(true);
            _hintsFolder.SetActive(false);
        }

        private void OnBackButtonClicked()
        {
            // lobby.OnBackButtonClicked();
            // SceneManager.LoadSceneAsync("Location-basedGame_backup");
            _cameraFolder.SetActive(false);
            _hintsFolder.SetActive(true);
        }

    }

}
