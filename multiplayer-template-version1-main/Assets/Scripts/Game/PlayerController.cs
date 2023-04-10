using System.Collections.Generic;
using Cinemachine;
using Game;
using Unity.Netcode;
using UnityEngine;

namespace Mapbox.Examples
{
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
[RequireComponent(typeof(CharacterController))]

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Vector2 _minMaxRotationX;
    [SerializeField] private Transform _camTransform;
    private CharacterController _cc;
    private PlayerControll _playerControl;
    private float _cameraAngle;
    [SerializeField] private Transform spwanedObjPrefab;
    private Transform spwanedObjTransform;


    bool _isInitialized;

	ILocationProvider _locationProvider;
    ILocationProvider LocationProvider
		{
			get
			{
				if (_locationProvider == null)
				{
					_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
				}

				return _locationProvider;
			}
		}
    // public override void OnNetworkSpawn()
    // {
    //     CinemachineVirtualCamera cvm = _camTransform.gameObject.GetComponent<CinemachineVirtualCamera>();
    //     if (IsOwner)
    //     {
    //         cvm.Priority = 1;
    //     }
    //     else
    //     {
    //         cvm.Priority = 0;
    //     }
    // }
    
    // Start is called before the first frame update
    void Start()
    {
        LocationProviderFactory.Instance.mapManager.OnInitialized += () => _isInitialized = true;
        // _cc = GetComponent<CharacterController>();
        // _playerControl = new PlayerControll();
        // _playerControl.Enable();
        // Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (_isInitialized)
			{
				var map = LocationProviderFactory.Instance.mapManager;
				transform.localPosition = map.GeoToWorldPosition(LocationProvider.CurrentLocation.LatitudeLongitude);
			}
            // if (_playerControl.Player.Move.inProgress)
            // {
            //     Vector2 movementInput = _playerControl.Player.Move.ReadValue<Vector2>();
            //     Vector3 movement = movementInput.x * _camTransform.right + movementInput.y * _camTransform.forward;
            //     movement.y = 0;
            //     _cc.Move(movement * _speed * Time.deltaTime);
            // }
            // if (_playerControl.Player.Look.inProgress)
            // {
            //     Vector2 lookInput = _playerControl.Player.Look.ReadValue<Vector2>();
            //     transform.RotateAround(transform.position, transform.up, lookInput.x * _turnSpeed * Time.deltaTime);
            //     RotateCamera(lookInput.y);
            // }
            // if (Input.GetKey(KeyCode.T)) {
            // spawnBallServerRPC();
            // }
        }
        
        //if (!IsOwner) return;
        ///*if (Input.GetKey(KeyCode.T)) {  //click key code T -> it might do something 
        //    randomNumber.Value = Random.Range(0, 100);
        //}*/
        //Vector3 moveDir = new Vector3(0, 0, 0); //figure out which direction it i sgoing move
        //if (Input.GetKey(KeyCode.W)) moveDir.z = +1f; //this WASD listener 
        //if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        //if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        //if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        //float moveSpeed = 3f;
        //transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void RotateCamera(float lookInputY)
    {
        _cameraAngle = Vector3.SignedAngle(transform.forward, _camTransform.forward, _camTransform.right);
        float cameraRotationAmount = lookInputY * _turnSpeed * Time.deltaTime;
        float newCameraAngle = _cameraAngle - cameraRotationAmount;
        if (newCameraAngle <= _minMaxRotationX.x && newCameraAngle >= _minMaxRotationX.y)
        {
            _camTransform.RotateAround(_camTransform.position, _camTransform.right, -lookInputY * _turnSpeed * Time.deltaTime);
        }
    }
    private void spawnBall(){
        spwanedObjTransform = Instantiate(spwanedObjPrefab);
        spwanedObjTransform.position = transform.position;
        spwanedObjTransform.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void spawnBallServerRPC(){
        spawnBall();
    }
}
}