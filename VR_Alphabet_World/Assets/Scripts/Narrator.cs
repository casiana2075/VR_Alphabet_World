using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;

public class NarratorSpeaking : MonoBehaviour
{
    public int contor = 0;
    public static int contorLetter = 0;
    public static int level = 1;
    public bool ok = false;
    private bool ok2 = false;
    [SerializeField] AudioSource introSource;
    [SerializeField] AudioSource forestAudio;
    [SerializeField] AudioSource grabAudio;
    [SerializeField] GameObject player;
    private AudioSource audioSource;

    [SerializeField] Vector3 teleportDestinationCameraForest;
    [SerializeField] Vector3 teleportDestinationPlayerForest;

    public GameObject playerCamera;
    private Vector3 cameraOffset;
    private Vector3 lastPosition;
    private Vector3 initialPosition;

    public Animator animator;
    public KeyCode triggerKey = KeyCode.G;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    private int fadeDirection;
    private Texture2D fadeTexture;
    private float alpha = 0f;
    private bool isFading = false;
    public Animator animatorLetter;
    public float flyForce = 12f;
    private Rigidbody rb;
    GameObject letter;
    [SerializeField] private GameObject xrRig;
    [SerializeField] private GameObject leftController;
    [SerializeField] private GameObject rightController;
    private bool isPlayerBlocked = false;
    [SerializeField] Trigger test;

    void Start()
    {
        fadeDirection = 0;
        animator = player.GetComponent<Animator>();
        letter = GameObject.Find("letterCharactersForest/A");
        Initialization();
        initialPosition = new Vector3(-1.86000001f, 0.588999987f, -3.30999994f);
        lastPosition = initialPosition;
        cameraOffset = new Vector3(1.33000004f, 1.01300001f, 0f);
        introSource = this.GetComponent<AudioSource>();
        forestAudio = GameObject.Find("ForestAudio").GetComponent<AudioSource>();
        player = GameObject.Find("NarratorBear");
        playerCamera = GameObject.Find("XR Origin (XR Rig)");
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, fadeColor);
        fadeTexture.Apply();
        audioSource = introSource;
        StartCoroutine(PlayIntroThenDoStuff(audioSource));
        Debug.Log("OK = False");
        Debug.Log(ok);
        ok = false;
    }

    public bool Test()
    {
        Debug.Log("Test called");
        return ok;
    }

    IEnumerator PlayIntroThenDoStuff(AudioSource introSource)
    {
        Debug.Log("Playing intro...");
        BlockPlayerMovement();
        introSource.Play();
        Debug.Log("Intro played. OK = True");
        ok = true;
        yield return new WaitForSeconds(20);
        UnblockPlayerMovement();
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
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource = forestAudio;
            StartCoroutine(TeleportForest(audioSource));
        }
        else if (test.Test() == true && contor == 0)
        {
            test.Reset("");
            contor++;
            Debug.Log("Test passed");
            if (animator != null)
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsTalking", true);
            }
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource = forestAudio;
            StartCoroutine(TeleportForest(audioSource));
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
        teleportDestinationCameraForest = new Vector3(30.3400002f, -0.499999991f, 53.4000015f);
        teleportDestinationPlayerForest = new Vector3(28.3400002f, -0.50900000f, 50.400001f);
    }
    void Introduction(AudioSource audioSource)
    {
        audioSource.Play();
    }

    void TeleportLetters()
    {
        if (level == 1)
        {
            char litera = (char)('A' + contorLetter);
            contorLetter++;

            GameObject allLetters = GameObject.Find("letterCharactersForest");

            if (allLetters != null)
            {
                // A
                if (litera == 'A')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(2.2249999f, -0.167999998f, -11.7019997f));
                        letter.localScale = new Vector3(0.0027f, 0.0027f, 0.0027f);
                        letter.rotation = Quaternion.Euler(0, 60f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'A'.");
                    }
                }

                // B
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'B')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(4.7420001f, -0.825999975f, -11.2519999f));
                        letter.localScale = new Vector3(0.0027f, 0.0027f, 0.0027f);
                        letter.rotation = Quaternion.Euler(0, 60f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'B'.");
                    }
                }

                // C
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'C')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(3.5940001f, -0.841000021f, -11.0959997f));
                        letter.localScale = new Vector3(0.0027f, 0.0027f, 0.0027f);
                        letter.rotation = Quaternion.Euler(-93.3f, 60f, 30.9f);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'C'.");
                    }
                }

                // D
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'D')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(3.98000002f, -0.739000022f, -10.6420002f));
                        letter.localScale = new Vector3(0.0027f, 0.0027f, 0.0027f);
                        letter.rotation = Quaternion.Euler(-90, 0, 104.94f);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'D'.");
                    }
                }

                // E
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'E')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(4.16800022f, -0.851999998f, -9.09399986f));
                        letter.localScale = new Vector3(0.0003f, 0.0003f, 0.0003f);
                        letter.rotation = Quaternion.Euler(-90f, 0f, -266.326f);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'E'.");
                    }
                }

                // F
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'F')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(4.04699993f, -0.763000011f, -10.0880003f));
                        letter.localScale = new Vector3(0.0027f, 0.0027f, 0.0027f);
                        letter.rotation = Quaternion.Euler(-90f, 0, 133.089f);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'F'.");
                    }
                }

            }
        }
        else if (level == 2)
        {

            Debug.Log("A intrat in level 2");
            char litera = (char)('A' + contorLetter);
            contorLetter++;

            GameObject allLetters = GameObject.Find("letterCharactersForest");

            if (allLetters != null)
            {
                // G
                if (litera == 'G')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(2.53900003f, -0.151999995f, -15.7279997f));
                        letter.localScale = new Vector3(0.00499999989f, 0.00499999989f, 0.00499999989f);
                        letter.rotation = Quaternion.Euler(270, 83.109581f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'G'.");
                    }
                }

                // H
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'H')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(3.1400001f, -0.722000003f, -8.02000046f));
                        letter.localScale = new Vector3(-0.00379542308f, -0.0018935001f, -0.00429068785f);
                        letter.rotation = Quaternion.Euler(270, 62.0534859f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'H'.");
                    }
                }

                // I
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'I')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(2.64249992f, -0.81595999f, -13.7959995f));
                        letter.localScale = new Vector3(0.00327045238f, 0.00162805303f, 0.00312569994f);
                        letter.rotation = Quaternion.Euler(270, 105.555893f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'I'.");
                    }
                }

                // J
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'J')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(3.49600005f, -0.840120018f, -12.5170002f));
                        letter.localScale = new Vector3(0.00251711998f, 0.00213634386f, 0.0033585832f);
                        letter.rotation = Quaternion.Euler(270, 106.261398f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'J'.");
                    }
                }

                // K
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'K')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(2.21429992f, -0.852469981f, -14.4209995f));
                        letter.localScale = new Vector3(0.00518480875f, 0.002937451f, 0.00386945321f);
                        letter.rotation = Quaternion.Euler(270, 107.072044f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'K'.");
                    }
                }

                // L
                litera = (char)('A' + contorLetter);
                contorLetter++;
                if (litera == 'L')
                {
                    Transform letter = allLetters.transform.Find(litera.ToString());
                    if (letter != null)
                    {
                        letter.position = allLetters.transform.TransformPoint(new Vector3(3.94930005f, -0.748369992f, -10.2410002f));
                        letter.localScale = new Vector3(0.000124127269f, 0.000387811247f, 0.00026146209f);
                        letter.rotation = Quaternion.Euler(270.019775f, 130.912567f, 0);
                    }
                    else
                    {
                        Debug.LogError("Nu s-a gasit litera 'L'.");
                    }
                }
            }
        }
        else if (level == 3)
        {
            Debug.Log("Level 3");
        }
    }


    public IEnumerator TeleportForest(AudioSource audio)
    {
        Debug.Log("Teleporting to forest scene...");
        yield return StartCoroutine(FadeOut());
        TeleportLetters();
        player.transform.position = teleportDestinationPlayerForest;
        player.transform.rotation = Quaternion.Euler(0, 30, 0);
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position = teleportDestinationCameraForest;
        }
        yield return StartCoroutine(FadeIn());
        audio.Play();
        BlockPlayerMovement();
        yield return new WaitForSeconds(21);
        UnblockPlayerMovement();
        Debug.Log("Teleporting to forest scene...");
    }

    void BlockPlayerMovement()
    {
        if (!isPlayerBlocked)
        {
            Debug.Log("Blocking player movement. Disabling controllers.");
            leftController.SetActive(false);
            rightController.SetActive(false);

            xrRig.transform.position = xrRig.transform.position;

            isPlayerBlocked = true;
        }
    }

    void UnblockPlayerMovement()
    {
        if (isPlayerBlocked)
        {
            Debug.Log("Unblocking player movement. Enabling controllers.");
            leftController.SetActive(true);
            rightController.SetActive(true);

            isPlayerBlocked = false;
        }
    }

    public IEnumerator TeleportBack()
    {
        level++;
        yield return StartCoroutine(FadeOut());
        player.transform.position = initialPosition;
        player.transform.rotation = Quaternion.Euler(0, 40, 0);
        if (playerCamera != null && player != null)
        {
            playerCamera.transform.position = new Vector3(3.03514242f, 0.544080138f, 0.912534654f);
        }
        yield return StartCoroutine(FadeIn());
        contor=0;
    }

    public IEnumerator FadeOut()
    {
        isFading = true;
        fadeDirection = 1;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / fadeDuration;
            alpha = Mathf.Clamp01(alpha);
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        fadeDirection = -1;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime / fadeDuration;
            alpha = Mathf.Clamp01(alpha);
            yield return null;
        }
        isFading = false;
    }

    void OnGUI()
    {
        if (isFading)
        {
            GUI.color = new Color(1, 1, 1, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }

    public void StartNarration()
    {
        Start();
        Update();
    }
}