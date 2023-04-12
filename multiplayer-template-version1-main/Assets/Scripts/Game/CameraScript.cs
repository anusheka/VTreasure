using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Camera;
#if UNITY_ANDROID || UNITY_IOS
using NativeCameraNamespace;
#endif

using System;

using Game.Events;
using GameFramework.Core.Data;
using TMPro;
using NativeCameraNamespace;

// using UnityEngine.CommandAttribute;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cinemachine;
using Game;
using Unity.Netcode;

public class CameraScript : MonoBehaviour
{
    static WebCamTexture backCam;
    // Start is called before the first frame update
    [SerializeField] private Button capture;
    [SerializeField] public RawImage rawimage;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button _scoresButton;
    // private CameraCallback callback;
    private static NCCameraCallbackiOS instance;
	private NativeCamera.CameraCallback callback;

    private int score;


    void Start()
    {
        capture.onClick.AddListener(OnCaptureClicked);
         _scoresButton.onClick.AddListener(OnScoreClicked);
        score = 0;


        if (backCam == null)
            backCam = new WebCamTexture();

        rawimage.texture = backCam;
        

        if (!backCam.isPlaying)
            backCam.Play();

        // CameraCallback callback = new CameraCallback("C:\Users");
        // private readonly NativeCamera.CameraCallback callback;

        
        // NativeCamera.TakePicture( callback, 512, true, -1 );

        // WebCamTexture webcamTexture = new WebCamTexture(WebCamTexture.devices[WebCamTexture.devices.Length-1].name);
        //  rawimage.texture = new WebCamTexture(WebCamTexture.devices[WebCamTexture.devices.Length-1].name); //webcamTexture;
        //  rawimage.material.mainTexture = webcamTexture;
        //  webcamTexture.Play();

        // var devices: WebCamDevice[] = WebCamTexture.devices;
        // for (var i: int = 0; i < devices.Length; i++)
        //     Debug.Log(devices[i].name);
    
        // var deviceCamera: GameObject = GameObject.FindWithTag("Player");
        // var webcamTexture: WebCamTexture = new WebCamTexture();
        // deviceCamera.GetComponent.<Renderer>().material.mainTexture = webcamTexture;
        // webcamTexture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        rawimage.SetNativeSize();
    }


    public void OnScoreClicked() {
        scoreText.gameObject.SetActive(true);
        scoreText.text = $"My Score: {score.ToString()}";
         Debug.Log(score);
    }


    //https://github.com/yasirkula/UnityNativeCamera
    public void OnCaptureClicked() {
        int maxSize = 512;
        score++;

        Debug.Log(score);

        // NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
        // {
        //     Debug.Log( "Image path: " + path );
        //     if( path != null )
        //     {
        //         // Create a Texture2D from the captured image
        //         Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );
        //         if( texture == null )
        //         {
        //             Debug.Log( "Couldn't load texture from " + path );
        //             return;
        //         }

        //         // Assign texture to a temporary quad and destroy it after 5 seconds
        //         GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
        //         quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
        //         quad.transform.forward = Camera.main.transform.forward;
        //         quad.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );
                
        //         Material material = quad.GetComponent<Renderer>().material;
        //         if( !material.shader.isSupported ) // happens when Standard shader is not included in the build
        //             material.shader = Shader.Find( "Legacy Shaders/Diffuse" );

        //         material.mainTexture = texture;
                    
        //         Destroy( quad, 5f );

        //         // If a procedural texture is not destroyed manually, 
        //         // it will only be freed after a scene change
        //         Destroy( texture, 5f );
        //     }
        // }, maxSize );

    }
}

