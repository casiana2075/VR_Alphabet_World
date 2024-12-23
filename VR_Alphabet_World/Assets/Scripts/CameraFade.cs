using UnityEngine;

public class CameraFade : MonoBehaviour
{
    public KeyCode key = KeyCode.X;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;

    private Texture2D fadeTexture;
    private float alpha = 0f;
    private int fadeDirection = 0;
    private bool isTransitioning = false;

    private void Start()
    {
        // Ini?ializare texturã
        fadeTexture = new Texture2D(1, 1);
        fadeTexture.SetPixel(0, 0, fadeColor);
        fadeTexture.Apply();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key) && !isTransitioning)
        {
            StartFadeOut();
        }
    }

    private void OnGUI()
    {
        if (isTransitioning)
        {
            GUI.color = new Color(1, 1, 1, alpha);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }

    private void StartFadeOut()
    {
        isTransitioning = true;
        fadeDirection = 1;
        alpha = 0f;
        Invoke("StartFadeIn", fadeDuration);
    }

    private void StartFadeIn()
    {
        fadeDirection = -1;
    }

    private void FixedUpdate()
    {
        if (isTransitioning)
        {
            alpha += fadeDirection * Time.deltaTime / fadeDuration;
            alpha = Mathf.Clamp01(alpha);

            if (fadeDirection == -1 && alpha <= 0f)
            {
                isTransitioning = false;
            }
        }
    }
}
