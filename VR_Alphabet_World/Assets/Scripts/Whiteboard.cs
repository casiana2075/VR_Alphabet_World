using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Vector2 textureSize = new Vector2(2048, 1024);


    [HideInInspector]
    public Texture2D texture;
    [HideInInspector]
    public Texture2D guidelineTexture;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        texture = new Texture2D((int)textureSize.x, (int)textureSize.y, TextureFormat.RGBA32, false);
        texture.Apply();

        _renderer.material.SetTexture("_OverlayTexture", texture);

        if (guidelineTexture != null)
        {
            OverlayGuideline();
        }
    }

    private void OverlayGuideline()
    {
        Color[] guidelinePixels = guidelineTexture.GetPixels();
        texture.SetPixels(guidelinePixels);
        texture.Apply();
    }
}
