using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorSpeaking : MonoBehaviour
{
    [SerializeField] AudioSource introSource; 
    [SerializeField] AudioSource forestAudio;
    [SerializeField] AudioSource desertAudio;
    [SerializeField] AudioSource islandAudio;
    [SerializeField] AudioSource pyramidsAudio;
    [SerializeField] GameObject player;
    private AudioSource audioSource;

    [SerializeField] Vector3 teleportDestinationCameraForest;
    [SerializeField] Vector3 teleportDestinationCameraDesert;
    [SerializeField] Vector3 teleportDestinationCameraIsland;
    [SerializeField] Vector3 teleportDestinationCameraPyramids;
    [SerializeField] Vector3 teleportDestinationPlayerForest;
    [SerializeField] Vector3 teleportDestinationPlayerDesert;
    [SerializeField] Vector3 teleportDestinationPlayerIsland;
    [SerializeField] Vector3 teleportDestinationPlayerPyramids; 
    public GameObject playerCamera;
    private Vector3 cameraOffset;
    private Vector3 lastPosition;
    private Vector3 initialPosition;

    public Animator animator;

    // private float inactivityTimer = 0f;
    // private float inactivityThreshold = 10f;

    void Start()
    {
        animator=player.GetComponent<Animator>();
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsTalking", true);
        Initialization();
        audioSource = introSource;
        audioSource.Play();
        initialPosition = player.transform.position;
        lastPosition = initialPosition;
        introSource = this.GetComponent<AudioSource>();
        forestAudio = GameObject.Find("ForestAudio").GetComponent<AudioSource>();
        desertAudio = GameObject.Find("DesertAudio").GetComponent<AudioSource>();
        islandAudio = GameObject.Find("IslandAudio").GetComponent<AudioSource>();
        pyramidsAudio = GameObject.Find("PyramidAudio").GetComponent<AudioSource>();
        player = GameObject.Find("NarratorBear");
        playerCamera = GameObject.Find("XR Origin (XR Rig)");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (animator != null)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsTalking", true);
            }
            if (audioSource!=null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource = forestAudio;
            StartCoroutine(TeleportForest( audioSource));
            audioSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (animator != null)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsTalking", true);
            }
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource = desertAudio;
            StartCoroutine(TeleportDesert(audioSource));
            audioSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            if (animator != null)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsTalking", true);
            }
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource = islandAudio;
            StartCoroutine(TeleportIsland(audioSource));
            audioSource.Play();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (animator != null)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsTalking", true);
            }
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource = pyramidsAudio;
            StartCoroutine(TeleportPyramids(audioSource));
            audioSource.Play();

        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            animator.SetBool("IsIdle", true);
            animator.SetBool("IsTalking", false);
            StartCoroutine(TeleportBack());
        }
    }

    void Initialization()
    {
        teleportDestinationCameraForest=new Vector3(30.3400002f,-0.499999991f,53.4000015f);
        teleportDestinationCameraDesert=new Vector3(56.3400002f,-0.21999997f,-393.369995f);
        teleportDestinationCameraIsland=new Vector3(1171.46997f,16.8900009f,405.079987f);
        teleportDestinationCameraPyramids=new Vector3(-95.5999985f,21.4999996f,785.5f);
        teleportDestinationPlayerForest = new Vector3(28.3400002f, -0.50900000f, 50.400001f);
        teleportDestinationPlayerDesert =new Vector3(54.3400002f, -0.153999999f, -396.369995f);
        teleportDestinationPlayerIsland=new Vector3(1172.01599f, 16.9300003f, 400.720001f);
        teleportDestinationPlayerPyramids=new Vector3(-97.5999985f,21.7999996f,782.5f);
    }
    // void DetectInactivity()
    // {
    //     if (player.transform.position == lastPosition && !Input.anyKey)
    //     {
    //         inactivityTimer += Time.deltaTime;
    //         if (inactivityTimer >= inactivityThreshold) 
    //         {
    //             RestartIntroAudio();
    //             inactivityTimer = 0f; 
    //         }
    //     }
    //     else
    //     {
    //         inactivityTimer = 0f;
    //     }

    //     lastPosition = player.transform.position;
    // }

    // void RestartIntroAudio()
    // {
    //     if (!introSource.isPlaying)
    //     {
    //         introSource.Play();
    //     }
    // }

    void Introduction(AudioSource audioSource)
    {
        audioSource.Play();
    }

    IEnumerator TeleportForest(AudioSource audio) 
    {
        yield return new WaitForSeconds(1);
        player.transform.position = teleportDestinationPlayerForest;
        player.transform.rotation = Quaternion.Euler(0, 30, 0);
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position = teleportDestinationCameraForest + cameraOffset;
        }
        yield return new WaitForSeconds(3);
    }

    IEnumerator TeleportDesert(AudioSource audio)
    {
        yield return new WaitForSeconds(1);
        player.transform.position = teleportDestinationPlayerDesert;
        player.transform.rotation = Quaternion.Euler(0, 30, 0);
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position = teleportDestinationCameraDesert + cameraOffset;
        }
        yield return new WaitForSeconds(3);
    }

    IEnumerator TeleportIsland(AudioSource audio)
    {
        yield return new WaitForSeconds(1);
        player.transform.position = teleportDestinationPlayerIsland;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position = teleportDestinationCameraIsland + cameraOffset;
        }
        yield return new WaitForSeconds(3);
    }

    IEnumerator TeleportPyramids(AudioSource audio)
    {
        yield return new WaitForSeconds(1);
        player.transform.position = teleportDestinationPlayerPyramids;
        player.transform.rotation = Quaternion.Euler(0, 40, 0);
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position = teleportDestinationCameraPyramids + cameraOffset;
        }
        yield return new WaitForSeconds(3);
    }

    IEnumerator TeleportBack()
    {
        yield return new WaitForSeconds(1);
        player.transform.position = initialPosition;
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position =cameraOffset + (new Vector3(0, 1, 0));
        }
    }
}
