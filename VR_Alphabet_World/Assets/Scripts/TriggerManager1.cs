using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    public static TriggerManager Instance { get; private set; }

    [SerializeField] private List<Texture2D> letterTextures; // Assign PNGs in Inspector

    
    public Texture2D GetLetterTextureByIndex(int index)
    {
        Debug.Log($"AAAAAAAA  {letterTextures.Count}");
        if (index >= 0 && index < letterTextures.Count)
        {
            Debug.Log($"ABBBBBB  {letterTextures[index]}");
            return letterTextures[index];
        }

        Debug.LogError($"Letter texture at index {index} not found.");
        return null;
    }
}
