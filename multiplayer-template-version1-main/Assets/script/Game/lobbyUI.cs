using System;
using System.Collections;
using System.Collections.Generic;
using Game.Events;
using GameFramework.Core.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Game
{
    public class lobbyUI : MonoBehaviour
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
                _startButton.onClick.AddListener(OnStartButtonClicked);
                LobbyEvents.OnLobbyReady += OnLobbyReady;
            }
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
        }

        private async void OnStartButtonClicked()
        {
            _scoresButton.gameObject.SetActive(true);
            _startButton.gameObject.SetActive(false);
            _lobbyCodeText.gameObject.SetActive(false);
            _greyBackground.gameObject.SetActive(false);
            await GameLobbyManager.Instance.StartGame();
        }

        private void OnScoresButtonClicked()
        {
            _scoresButton.gameObject.SetActive(false);
            _leaderBoard.gameObject.SetActive(true);
            _backButton.gameObject.SetActive(true);
            _greyBackground.gameObject.SetActive(true);
            _startButton.gameObject.SetActive(false);

             _scoreText.text = $"{GameLobbyManager.Instance.GetScoreText()}";

        }

        private void OnBackButtonClicked()
        {
            _scoresButton.gameObject.SetActive(true);
            _leaderBoard.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _greyBackground.gameObject.SetActive(false);
            _startButton.gameObject.SetActive(false);
        }
    }
}