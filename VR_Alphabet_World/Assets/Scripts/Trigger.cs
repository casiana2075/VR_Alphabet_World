using System;
#if UNITY_EDITOR
using UnityEditor; // Required for AssetDatabase
#endif
using UnityEngine;
using System.Collections.Generic; // Required for List
using UnityEngine.Events;
using System.Collections;

public class Trigger : MonoBehaviour
{
    [SerializeField] bool uncheckOnEnter;

    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] string tagFilter;
    [SerializeField] UnityEvent onTriggerExit;

    [SerializeField] AudioSource rightAudioSource;
    [SerializeField] AudioSource wrongAudioSource;
    [SerializeField] AudioSource completionSound;

    [SerializeField] GameObject completionParticle; // Particle system to play on correct sequence completion

    [SerializeField] List<string> desiredSequence; // Desired order of collider hits (set in the inspector)
    private static List<string> currentSequence = new List<string>(); // Tracks the actual order of hits
    private static List<Trigger> allColliders = new List<Trigger>(); // Tracks all collider instances

    private bool isChecked = false; // Tracks whether this collider is currently "checked"
    private static int currentLetterIndex = 1; // Tracks the current letter index
    public static string currentLetter = "";
    public static GameObject currentColliderGroup;
    public static int completedLettersCount = 0; // Tracks the number of completed letters

    [SerializeField] Whiteboard whiteboard; // Reference to the Whiteboard script
    [SerializeField] NarratorSpeaking narratorSpeaking; // Reference to the NarratorSpeaking script
    [SerializeField] AudioSource forstAudio; // Reference to the AudioSource component

    [SerializeField] GameObject activeColliderGroup; // Current active group of colliders
    [SerializeField] GameObject nextColliderGroup; // Next group of colliders to activate
    [SerializeField] private List<GameObject> allColliderGroups; // List of all collider groups


    void Awake()
    {
        // Register this collider in the static list
        allColliders.Add(this);
    }

    void OnDestroy()
    {
        // Remove this collider from the static list when destroyed
        allColliders.Remove(this);
    }

    void OnTriggerEnter(Collider other)
    {
        // Filter by tag if needed
        if (!string.IsNullOrEmpty(tagFilter) && !other.CompareTag(tagFilter))
        {
            return;
        }

        // If this collider is already checked, do nothing
        if (isChecked)
        {
            return;
        }

        // Add this collider's name to the sequence
        if (!currentSequence.Contains(gameObject.name)) // Avoid duplicate entries
        {
            currentSequence.Add(gameObject.name);
            Debug.Log($"Collider {gameObject.name} hit. Current sequence: {string.Join(", ", currentSequence)}");
        }

        // Check if the sequence is invalid
        if (!IsSequenceValid())
        {
            Debug.LogWarning("Incorrect sequence detected! Resetting and rechecking all colliders.");
            wrongAudioSource.Play();

            // Regenerate the whiteboard material
            RegenerateWhiteboardMaterial();

            // Reset all colliders
            ResetAllColliders();
            return;
        }
        else
        {
            Debug.Log("Correct sequence detected!");
            rightAudioSource.Play();
            isChecked = true; // Mark this collider as "checked"
        }

        if (IsSequenceComplete())
        {
            Debug.Log("All colliders hit in the correct order!");
            completionSound.Play();
            PlayCompletionEffect();
            LevelDone();
            Test();
            RegenerateWhiteboardMaterial();
            AsignNextLetterWhiteboardMaterial();
            SwitchColliderGroup();
        }

        // Trigger event
        onTriggerEnter.Invoke();

        if (uncheckOnEnter)
        {
            gameObject.SetActive(false);
        }
    }

    private void LevelDone()
    {
        Debug.Log("Level done");
        completedLettersCount++;

        if (completedLettersCount == 6)
        {
            Debug.Log("All 6 letters have been drawn! Level complete!");
            currentLetter = "gata";
            completedLettersCount = 0;
        }
        else
        {
            Debug.Log($"Letter {completedLettersCount} completed! {6 - completedLettersCount} letters remaining.");
        }
    }

    public void Reset(string a)
    {
        currentLetter = a;
    }

    public bool Test()
    {
        Debug.Log("Tets"); 
        if (currentLetter == "gata")
        {
            return true;
        }
        return false;
    }

    private void AsignNextLetterWhiteboardMaterial()
    {
        if (whiteboard == null)
        {
            Debug.LogWarning("No Whiteboard reference assigned. Cannot assign next letter material.");
            return;
        }

        // Determine the next letter based on the sequence progression
        string nextLetterPath = $"Assets/Materials/letters_guidelines/{currentLetterIndex + 1}.png"; // For B: 2.png, for C: 3.png, etc.

        // Load the texture using AssetDatabase
        Texture2D nextLetterTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(nextLetterPath);

        if (nextLetterTexture == null)
        {
            Debug.LogError($"Failed to load texture at path '{nextLetterPath}'. Check the file name and path.");
            return;
        }

        // Get the Renderer of the whiteboard and assign the texture to the material's _BaseTexture property
        Renderer renderer = whiteboard.GetComponent<Renderer>();
        if (renderer != null)
        {
            Debug.Log($"Assigning texture '{nextLetterPath}' to the whiteboard material.");
            renderer.material.SetTexture("_BaseTexture", nextLetterTexture); // Update the _BaseTexture
        }
        else
        {
            Debug.LogError("Whiteboard renderer is missing. Cannot assign material.");
        }
        currentLetterIndex++; // Increment the letter index for the next letter
    }


    private void SwitchColliderGroup()
    {
        if (activeColliderGroup != null)
        {
            // Deactivate the current group of colliders
            activeColliderGroup.SetActive(false);
        }

        if (nextColliderGroup != null)
        {
            // Activate the next group of colliders
            nextColliderGroup.SetActive(true);
            currentColliderGroup = nextColliderGroup;
            Debug.Log($"Switched to the next collider group: {nextColliderGroup.name}");
        }
        else
        {
            Debug.LogWarning("No next collider group assigned.");
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(tagFilter) && !other.CompareTag(tagFilter))
        {
            return;
        }
        onTriggerExit.Invoke();
    }

    // Method to check if the current sequence matches the desired one (in order so far)
    private bool IsSequenceValid()
    {
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (i >= desiredSequence.Count || currentSequence[i] != desiredSequence[i])
            {
                return false;
            }
        }
        return true;
    }

    // Method to check if the sequence is complete (all colliders hit in correct order)
    private bool IsSequenceComplete()
    {
        return currentSequence.Count == desiredSequence.Count && IsSequenceValid();
    }

    // Method to reset the sequence
    private void ResetSequence()
    {
        currentSequence.Clear();
    }

    // Method to reset all colliders and recheck the sequence
    private void ResetAllColliders()
    {
        // Clear the sequence
        ResetSequence();

        // Reset all colliders in the scene
        foreach (Trigger trigger in allColliders)
        {
            trigger.ResetColliderState();
        }

        Debug.Log("All colliders have been reset. Sequence checking restarted.");
    }

    // Method to reset this collider's state
    private void ResetColliderState()
    {
        isChecked = false; // Uncheck this collider
        if (uncheckOnEnter)
        {
            gameObject.SetActive(true); // Reactivate if necessary
        }
    }

    // Method to play the particle effect when the sequence is complete
    private void PlayCompletionEffect()
    {
       Debug.Log("Playing completion particle effect...");
        if (completionParticle != null)
        {
            // Ensure the GameObject is active
            if (!completionParticle.activeSelf)
            {
                completionParticle.SetActive(true);
                Debug.Log("Reactivating completion particle GameObject.");
            }

            ParticleSystem particleSystem = completionParticle.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Stop();
                particleSystem.Play();  // Play the particle effect
                Debug.Log("Playing completion particle effect!");
            }
            else
            {
                Debug.LogWarning("No ParticleSystem component found on the completionParticle GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("No completion particle assigned.");
        }
    }


    // Method to regenerate the whiteboard material
    private void RegenerateWhiteboardMaterial()
    {
        if (whiteboard != null)
        {
            Debug.Log("Regenerating whiteboard material...");

            // Create a new texture and assign it to the whiteboard
            whiteboard.texture = new Texture2D((int)whiteboard.textureSize.x, (int)whiteboard.textureSize.y, TextureFormat.RGBA32, false);
            whiteboard.texture.Apply();

            Renderer renderer = whiteboard.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.SetTexture("_OverlayTexture", whiteboard.texture);
            }

            // Overlay guideline if available
            if (whiteboard.guidelineTexture != null)
            {
                whiteboard.texture.SetPixels(whiteboard.guidelineTexture.GetPixels());
                whiteboard.texture.Apply();
            }

            Debug.Log("Whiteboard material regenerated.");
        }
        else
        {
            Debug.LogWarning("No Whiteboard reference assigned. Cannot regenerate material.");
        }
    }

    // Optional: Reset sequence when the game starts or upon manual reset
    private void Start()
    {
        ResetSequence();
        int firstLetterIndex = 1;
        completionParticle.SetActive(true);

        // Deactivate all groups except the active one
        foreach (GameObject group in allColliderGroups)
        {
            if (group != null && firstLetterIndex != 1 && group != currentColliderGroup )
            {
                firstLetterIndex++;
                group.SetActive(false); // Deactivate all groups
            }
            firstLetterIndex++;
        }
    }

    private void Update()
    {
        if (completionSound.isPlaying) 
        {
            wrongAudioSource.Stop();   
        }
    }
}