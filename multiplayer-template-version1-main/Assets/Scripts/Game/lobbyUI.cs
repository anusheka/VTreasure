using System;
using System.Collections;
using System.Collections.Generic;
using Game.Events;
using GameFramework.Core.Data;
using TMPro;
using UnityEngine;
// using UnityEngine.CommandAttribute;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cinemachine;
using Game;
using Unity.Netcode;

namespace Game
{
    public class lobbyUI : NetworkBehaviour
    {
        [SerializeField] private TextMeshProUGUI _lobbyCodeText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _leaderBoard;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _readyButton;
        [SerializeField] private Button _scoresButton;
        [SerializeField] private Button _backButton;
        [SerializeField] private Image _mapImage;
        [SerializeField] private Image _greyBackground;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private TextMeshProUGUI _mapName;
        [SerializeField] private MapSelectionData _mapSelectionData;

        private NetworkVariable<bool> gameStarted = new NetworkVariable<bool>();

        // public delegate void SetActive(bool b);

        // [SerializeField] private NetworkVariable<TextMeshProUGUI> _lobbyCodeText;
        // [SerializeField] private NetworkVariable<TextMeshProUGUI> _scoreText;
        // [SerializeField] private NetworkVariable<TextMeshProUGUI> _leaderBoard;
        // [SerializeField] private NetworkVariable<Button> _startButton;
        // [SerializeField] private NetworkVariable<Button> _readyButton;
        // [SerializeField] private NetworkVariable<Button> _scoresButton;
        // [SerializeField] private NetworkVariable<Button> _backButton;
        // [SerializeField] private NetworkVariable<Image> _mapImage;
        // [SerializeField] private NetworkVariable<Image> _greyBackground;
        // [SerializeField] private NetworkVariable<Button> _leftButton;
        // [SerializeField] private NetworkVariable<Button> _rightButton;
        // [SerializeField] private NetworkVariable<TextMeshProUGUI> _mapName;
        // [SerializeField] private NetworkVariable<MapSelectionData> _mapSelectionData;


        private int _currentMapIndex = 0;

        private void OnEnable()
        {
            _readyButton.onClick.AddListener(OnReadyPressed);
            _scoresButton.onClick.AddListener(OnScoresButtonClicked);
            _backButton.onClick.AddListener(OnBackButtonClicked);

            if (GameLobbyManager.Instance.IsHost)
            {
                _leftButton.onClick.AddListener(OnLeftButtonClicked);
                _rightButton.onClick.AddListener(OnRightButtonClicked);
                LobbyEvents.OnLobbyReady += OnLobbyReady;
            }
            _startButton.onClick.AddListener(OnStartButtonClicked);
            LobbyEvents.OnLobbyUpdated += OnLobbyUpdated;
        }

        private void OnDisable()
        {
            _readyButton.onClick.RemoveAllListeners();
            _leftButton.onClick.RemoveAllListeners();
            _rightButton.onClick.RemoveAllListeners();
            _startButton.onClick.RemoveAllListeners();
            _scoresButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            LobbyEvents.OnLobbyUpdated -= OnLobbyUpdated;
            LobbyEvents.OnLobbyReady -= OnLobbyReady;
        }

        // Start is called before the first frame update
        void Start()
        {
            _lobbyCodeText.text = $"Lobby code: {GameLobbyManager.Instance.GetLobbyCode()}";
            // _scoreText.text = $"{GameLobbyManager.Instance.GetScoreText()}";
            if (!GameLobbyManager.Instance.IsHost)
            {
                _leftButton.gameObject.SetActive(false);
                _rightButton.gameObject.SetActive(false);
            }
            else
            {
                GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapSelectionData.Maps[_currentMapIndex].SceneName);
            }
        }

                // Update is called once per frame
        void Update()
        {
            if(gameStarted.Value) {
                Debug.Log("Game Started?");
                StartGame();
            }

            GameStartedClientRpc();
        }

        private async void OnLeftButtonClicked()
        {
            if (_currentMapIndex - 1 > 0)
            {
                _currentMapIndex--;
            }
            else
            {
                _currentMapIndex = 0;
            }
            UpdateMap();
            await GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapSelectionData.Maps[_currentMapIndex].SceneName);
        }

        private async void OnRightButtonClicked()
        {
            if (_currentMapIndex + 1 < _mapSelectionData.Maps.Count - 1)
            {
                _currentMapIndex++;
            }
            else
            {
                _currentMapIndex = _mapSelectionData.Maps.Count - 1;
            }
            UpdateMap();
            await GameLobbyManager.Instance.SetSelectedMap(_currentMapIndex, _mapSelectionData.Maps[_currentMapIndex].SceneName);
        }

        private async void OnReadyPressed()
        {
            bool succeed = await GameLobbyManager.Instance.SetPlayerReady();
            if (succeed)
            {
                _readyButton.gameObject.SetActive(false);
                _startButton.gameObject.SetActive(true);
            }
        }

        private void UpdateMap()
        {
            _mapImage.color = _mapSelectionData.Maps[_currentMapIndex].MapThumbnail;
            _mapName.text = _mapSelectionData.Maps[_currentMapIndex].MapName;
        }

        private void OnLobbyUpdated()
        {
            _currentMapIndex = GameLobbyManager.Instance.GetMapIndex();
            UpdateMap();
        }

        private void OnLobbyReady()
        {
            _startButton.gameObject.SetActive(true);
            //  CmdSetActive(_startButton, true);
        }



        // [ServerRpc]
        private void OnStartButtonClicked()
        {
            _scoresButton.gameObject.SetActive(true);
            _startButton.gameObject.SetActive(false);
            _lobbyCodeText.gameObject.SetActive(false);
            _greyBackground.gameObject.SetActive(false);

            // RpcSetObjectActive(_scoresButton, true);
            // RpcSetObjectActive(_lobbyCodeText, true);
            // RpcSetObjectActive(_greyBackground, false);
            // RpcSetObjectActive(_startButton, false);

            GameStartedServerRpc(true);
            
            StartGame();
        }

        private async void StartGame() {
            await GameLobbyManager.Instance.StartGame();
        }

        // [ServerRpc]
        private void OnScoresButtonClicked()
        {
            _scoresButton.gameObject.SetActive(false);
            _leaderBoard.gameObject.SetActive(true);
            _backButton.gameObject.SetActive(true);
            _greyBackground.gameObject.SetActive(true);
            _startButton.gameObject.SetActive(false);
            // _scoreText.gameObject.SetActive(true);

            // RpcSetObjectActive(_scoresButton, false);
            // RpcSetObjectActive(_leaderBoard, true);
            // RpcSetObjectActive(_backButton, true);
            // RpcSetObjectActive(_greyBackground, true);
            // RpcSetObjectActive(_startButton, false);

            // _scoreText.text = $"{GameLobbyManager.Instance.GetScoreText()}";

        }

        // [ServerRpc]
        public void OnBackButtonClicked()
        {
            _scoresButton.gameObject.SetActive(true);
            _leaderBoard.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _greyBackground.gameObject.SetActive(false);
            _startButton.gameObject.SetActive(false);
            _scoreText.gameObject.SetActive(false);


            // RpcSetObjectActive(_scoresButton, true);
            // RpcSetObjectActive(_leaderBoard, false);
            // RpcSetObjectActive(_backButton, false);
            // RpcSetObjectActive(_greyBackground, false);
            // RpcSetObjectActive(_startButton, false);
        }

        // private void onValueChanged(bool gameStarted, bool gameNotStarted) {
        //     Debug.Log("START?????????????");
        // }

        // public override void OnNetworkSpawn() {
        //     base.OnNetworkSpawn();
        //     // bool temp = gameStarted;
        //     gameStarted.OnValueChanged(true, false);
        // }

        [ServerRpc]
        private void GameStartedServerRpc(bool b) {
            gameStarted.Value = b;
        }

        [ClientRpc]
        private void GameStartedClientRpc() {
            if(gameStarted.Value) {
                 StartGame();
            }
        }

        // [ServerRpc]
        // private void RpcSetObjectActive(GameObject g, bool b) {
        //     g.SetActive(b);
        // }

        // [SyncEvent]
        // public event SetActive EventSetActive

        // [Command]
        // public void CmdSetActive(GameObject g, bool b) {
        //     g.SetActive(b);
        // }
    }
}