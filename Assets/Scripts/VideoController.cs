using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public static VideoController instance;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private RenderTexture videoStreamRenderTexture;

    public string video1endpoint = "https://pallystorageacc.blob.core.windows.net/asset-6127d81b-7a64-40b5-8964-a45b3a800929/Ayutthaya - Easy Tripod Paint _ _1920x960_AACAudio_4180.mp4?sv=2017-04-17&sr=c&si=6534c20d-0926-46c7-8b61-1a96a625f6be&sig=a09SmyRuonWUnximkp9I8OSjBz56NO%2BLOTMu6LjISy0%3D&st=2019-03-19T11%3A12%3A17Z&se=2119-03-19T11%3A12%3A17Z";

    public Coroutine currentCoroutine = null;
    public bool initiate_player = false;
    public GameObject cursor;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Application.runInBackground = true;
    }

    private IEnumerator PlayVideo()
    {
        videoStreamRenderTexture = new RenderTexture(1920, 960, 32, RenderTextureFormat.ARGB32);
        videoStreamRenderTexture.Create();

        // assign the render texture to the object material 
        Material sphereMaterial = gameObject.GetComponent<Renderer>().sharedMaterial;

        //create a VideoPlayer component 
        videoPlayer = gameObject.GetComponent<VideoPlayer>();

        // Set the video to loop. 
        videoPlayer.isLooping = true;

        // Set the VideoPlayer component to play the video from the texture 
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = videoStreamRenderTexture;

        audioSource = gameObject.GetComponent<AudioSource>();
        // Pause Audio play on Awake 
        audioSource.Pause();

        // Set Audio Output to AudioSource 
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.source = VideoSource.Url;

        // Assign the Audio from Video to AudioSource to be played 
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.url = video1endpoint;

        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }
        sphereMaterial.mainTexture = videoStreamRenderTexture;

        //Play Video 
        videoPlayer.Play();
        //Play Sound 
        audioSource.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (initiate_player) {
            Debug.Log("Start: " + SceneManager.GetActiveScene().name);
            currentCoroutine = StartCoroutine(PlayVideo());
            initiate_player = false;
        }
    }
}
